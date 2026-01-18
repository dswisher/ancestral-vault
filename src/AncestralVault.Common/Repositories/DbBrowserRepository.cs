// Copyright (c) Doug Swisher. All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using AncestralVault.Common.Database;
using AncestralVault.Common.Models.ViewModels.DbBrowser;
using Microsoft.EntityFrameworkCore;

namespace AncestralVault.Common.Repositories
{
    /// <summary>
    /// Service for browsing database entities using reflection.
    /// </summary>
    public class DbBrowserRepository : IDbBrowserRepository
    {
        private readonly Dictionary<string, Type> entityTypeCache;

        public DbBrowserRepository()
        {
            // Build a cache of entity types for fast case-insensitive lookup
            entityTypeCache = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

            var dbSetProperties = typeof(AncestralVaultDbContext)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>));

            foreach (var prop in dbSetProperties)
            {
                var entityType = prop.PropertyType.GetGenericArguments()[0];
                var typeName = entityType.Name;

                // Add both singular and plural forms
                entityTypeCache[typeName] = entityType;
                entityTypeCache[prop.Name] = entityType;
            }
        }


        public List<EntityTypeInfo> GetAllEntityTypes()
        {
            var dbSetProperties = typeof(AncestralVaultDbContext)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType.IsGenericType &&
                            p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                .OrderBy(p => p.Name);

            var result = new List<EntityTypeInfo>();
            foreach (var prop in dbSetProperties)
            {
                var entityType = prop.PropertyType.GetGenericArguments()[0];
                result.Add(new EntityTypeInfo
                {
                    TypeName = entityType.Name,
                    UrlName = entityType.Name.ToLowerInvariant()
                });
            }

            return result;
        }


        public bool TryResolveEntityType(string typeName, out Type? entityType)
        {
            return entityTypeCache.TryGetValue(typeName, out entityType);
        }


        public EntityListViewModel BuildListViewModel(AncestralVaultDbContext dbContext, Type entityType)
        {
            var urlName = entityType.Name.ToLowerInvariant();
            var primaryKeyProperty = GetPrimaryKeyProperty(entityType);
            var primaryKeyName = primaryKeyProperty?.Name ?? "Id";

            // Get the DbSet<T> property from the context
            var dbSetProperty = typeof(AncestralVaultDbContext)
                .GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                     p.PropertyType.GetGenericArguments()[0] == entityType);

            if (dbSetProperty == null)
            {
                throw new InvalidOperationException($"DbSet for type {entityType.Name} not found.");
            }

            // Get the DbSet and query it
            var dbSet = dbSetProperty.GetValue(dbContext);
            if (dbSet == null)
            {
                throw new InvalidOperationException($"DbSet for type {entityType.Name} is null.");
            }

            // Use reflection to call Take(100).ToList()
            var queryable = (IQueryable)dbSet;
            var takeMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Take" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType);
            var limitedQuery = takeMethod.Invoke(null, [queryable, 100]);

            var toListMethod = typeof(Enumerable)
                .GetMethods()
                .First(m => m.Name == "ToList" && m.GetParameters().Length == 1)
                .MakeGenericMethod(entityType);
            var entities = (IList)toListMethod.Invoke(null, [limitedQuery])!;

            // Build the view model
            var entityItems = new List<EntityListItem>();

            // Get navigation property names to filter them out
            var navigationPropertyNames = GetNavigationPropertyNames(dbContext, entityType);

            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => !navigationPropertyNames.Contains(p.Name))
                .Take(5) // Show up to 5 properties in the list view
                .ToList();

            foreach (var entity in entities)
            {
                var id = primaryKeyProperty?.GetValue(entity)?.ToString() ?? "N/A";
                var propertyDict = new Dictionary<string, object?>();

                foreach (var prop in properties)
                {
                    propertyDict[prop.Name] = prop.GetValue(entity);
                }

                entityItems.Add(new EntityListItem
                {
                    Id = id,
                    Properties = propertyDict
                });
            }

            return new EntityListViewModel
            {
                TypeName = entityType.Name,
                UrlName = urlName,
                PrimaryKeyName = primaryKeyName,
                Entities = entityItems
            };
        }


        public EntityDetailViewModel? BuildDetailViewModel(AncestralVaultDbContext dbContext, Type entityType, string id)
        {
            var urlName = entityType.Name.ToLowerInvariant();
            var primaryKeyProperty = GetPrimaryKeyProperty(entityType);

            if (primaryKeyProperty == null)
            {
                return null;
            }

            // Find the entity by primary key
            var entity = FindEntityById(dbContext, entityType, primaryKeyProperty, id);
            if (entity == null)
            {
                return null;
            }

            // Eagerly load all navigation properties
            var entry = dbContext.Entry(entity);
            foreach (var navigation in entry.Navigations)
            {
                navigation.Load();
            }

            // Categorize and build property values
            var propertyValues = new List<PropertyValue>();
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                var propertyValue = BuildPropertyValue(prop, entity);
                propertyValues.Add(propertyValue);
            }

            return new EntityDetailViewModel
            {
                TypeName = entityType.Name,
                UrlName = urlName,
                Id = id,
                Properties = propertyValues
            };
        }


        private static PropertyInfo? GetPrimaryKeyProperty(Type entityType)
        {
            var properties = entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // First, look for [Key] attribute
            var keyProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
            if (keyProperty != null)
            {
                return keyProperty;
            }

            // Fall back to convention: "Id", "{TypeName}Id", "{TypeName}Key"
            var typeName = entityType.Name;
            return properties.FirstOrDefault(p =>
                p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase) ||
                p.Name.Equals($"{typeName}Id", StringComparison.OrdinalIgnoreCase) ||
                p.Name.Equals($"{typeName}Key", StringComparison.OrdinalIgnoreCase));
        }


        private static object? FindEntityById(AncestralVaultDbContext dbContext, Type entityType, PropertyInfo primaryKeyProperty, string id)
        {
            // Get the DbSet
            var dbSetProperty = typeof(AncestralVaultDbContext)
                .GetProperties()
                .FirstOrDefault(p => p.PropertyType.IsGenericType &&
                                     p.PropertyType.GetGenericArguments()[0] == entityType);

            if (dbSetProperty == null)
            {
                return null;
            }

            var dbSet = dbSetProperty.GetValue(dbContext) as IQueryable;
            if (dbSet == null)
            {
                return null;
            }

            // Convert the ID string to the appropriate type
            object convertedId;
            var keyType = primaryKeyProperty.PropertyType;

            if (keyType == typeof(long))
            {
                if (!long.TryParse(id, out var longId))
                {
                    return null;
                }

                convertedId = longId;
            }
            else if (keyType == typeof(string))
            {
                convertedId = id;
            }
            else
            {
                // Unsupported key type
                return null;
            }

            // Use FirstOrDefault with a lambda to find the entity
            var parameter = System.Linq.Expressions.Expression.Parameter(entityType, "e");
            var property = System.Linq.Expressions.Expression.Property(parameter, primaryKeyProperty);
            var constant = System.Linq.Expressions.Expression.Constant(convertedId);
            var equals = System.Linq.Expressions.Expression.Equal(property, constant);
            var lambda = System.Linq.Expressions.Expression.Lambda(equals, parameter);

            var whereMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "Where" && m.GetParameters().Length == 2)
                .MakeGenericMethod(entityType);
            var filteredQuery = whereMethod.Invoke(null, [dbSet, lambda]);

            var firstOrDefaultMethod = typeof(Queryable)
                .GetMethods()
                .First(m => m.Name == "FirstOrDefault" && m.GetParameters().Length == 1)
                .MakeGenericMethod(entityType);
            var result = firstOrDefaultMethod.Invoke(null, [filteredQuery]);

            return result;
        }


        private static bool IsCollectionProperty(PropertyInfo prop)
        {
            return prop.PropertyType.IsGenericType &&
                   prop.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>);
        }


        /// <summary>
        /// Gets the names of all navigation properties (both collections and references) for an entity type.
        /// </summary>
        /// <param name="dbContext">The database context containing the EF Core model metadata.</param>
        /// <param name="entityClrType">The CLR type of the entity.</param>
        /// <returns>A set of navigation property names for efficient lookup.</returns>
        private static HashSet<string> GetNavigationPropertyNames(AncestralVaultDbContext dbContext, Type entityClrType)
        {
            var entityType = dbContext.Model.FindEntityType(entityClrType);
            if (entityType == null)
            {
                // Shouldn't happen for valid entity types, but return empty set for safety
                return new HashSet<string>();
            }

            return entityType.GetNavigations()
                .Select(n => n.Name)
                .ToHashSet();
        }


        private static PropertyValue BuildPropertyValue(PropertyInfo prop, object entity)
        {
            var value = prop.GetValue(entity);

            // Check if this is a collection
            if (IsCollectionProperty(prop))
            {
                var collectionType = prop.PropertyType.GetGenericArguments()[0];
                var collectionUrlName = collectionType.Name.ToLowerInvariant();
                var collectionKeyProperty = GetPrimaryKeyProperty(collectionType);

                var relatedIds = new List<string>();
                if (value is IEnumerable collection && collectionKeyProperty != null)
                {
                    foreach (var item in collection)
                    {
                        var itemId = collectionKeyProperty.GetValue(item)?.ToString();
                        if (itemId != null)
                        {
                            relatedIds.Add(itemId);
                        }
                    }
                }

                return new PropertyValue
                {
                    Name = prop.Name,
                    Value = null,
                    Type = PropertyType.Collection,
                    RelatedEntityType = collectionUrlName,
                    RelatedEntityIds = relatedIds
                };
            }

            // Check if this is a foreign key navigation property
            var foreignKeyAttr = prop.GetCustomAttribute<ForeignKeyAttribute>();
            if (foreignKeyAttr != null && !prop.PropertyType.IsValueType && prop.PropertyType != typeof(string))
            {
                var relatedType = prop.PropertyType;
                var relatedUrlName = relatedType.Name.ToLowerInvariant();
                var relatedKeyProperty = GetPrimaryKeyProperty(relatedType);

                string? relatedId = null;
                if (value != null && relatedKeyProperty != null)
                {
                    relatedId = relatedKeyProperty.GetValue(value)?.ToString();
                }

                return new PropertyValue
                {
                    Name = prop.Name,
                    Value = relatedId,
                    Type = PropertyType.ForeignKeyNavigation,
                    RelatedEntityType = relatedUrlName
                };
            }

            // Check if this is a foreign key property
            if (foreignKeyAttr != null)
            {
                // This is the FK property itself, not the navigation
                // We need to find the navigation property it points to
                var navPropName = foreignKeyAttr.Name;
                var navProp = entity.GetType().GetProperty(navPropName);
                if (navProp != null)
                {
                    var relatedType = navProp.PropertyType;
                    var relatedUrlName = relatedType.Name.ToLowerInvariant();

                    return new PropertyValue
                    {
                        Name = prop.Name,
                        Value = value,
                        Type = PropertyType.ForeignKey,
                        RelatedEntityType = relatedUrlName
                    };
                }
            }

            // Check if another property has a ForeignKey attribute pointing to this property
            var referencingProperty = entity.GetType()
                .GetProperties()
                .FirstOrDefault(p =>
                {
                    var fkAttr = p.GetCustomAttribute<ForeignKeyAttribute>();
                    return fkAttr != null && fkAttr.Name.Equals(prop.Name, StringComparison.OrdinalIgnoreCase);
                });

            if (referencingProperty != null && !referencingProperty.PropertyType.IsValueType && referencingProperty.PropertyType != typeof(string))
            {
                var relatedType = referencingProperty.PropertyType;
                var relatedUrlName = relatedType.Name.ToLowerInvariant();

                return new PropertyValue
                {
                    Name = prop.Name,
                    Value = value,
                    Type = PropertyType.ForeignKey,
                    RelatedEntityType = relatedUrlName
                };
            }

            // Simple property
            return new PropertyValue
            {
                Name = prop.Name,
                Value = value,
                Type = PropertyType.Simple
            };
        }
    }
}

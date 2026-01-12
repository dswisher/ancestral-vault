# Notes on storing GDM data in JSON

The sample GDMXML file, [fugl.xml](fugl.xml) is appealing, as the file is just a list of GDM objects.
I would like to use JSON, however, and I think this approach could work.
Here is a snippet of JSON, representing the first three elements of the sample file:

```
[
  {
    "project": {
      "id": "project1",
      "name": "Andreas Fugl line",
      "description": "The beginnings of our Fugl line"
    }
  },
  {
    "researcher": {
      "id": "researcher1",
      "name": "John Researcher"
    }
  },
  {
    "activity": {
      "id": "activity1",
      "researcher_ref": "researcher1"
    }
  }
]
```

To deserialize this in C#, we could use a custom converter.

```
// Base interface for all GDM entities
public interface IGdmEntity
{
    string Id { get; }
}

// Your concrete model classes
public class Project : IGdmEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    // ... other properties
}

public class Researcher : IGdmEntity
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    // ... other properties
}

public class Activity : IGdmEntity
{
    public string Id { get; set; }
    public string ResearcherRef { get; set; }
    public string Status { get; set; }
    // ... other properties
}


// Custom converter for the wrapper pattern
public class GdmEntityConverter : JsonConverter<IGdmEntity>
{
    private static readonly Dictionary<string, Type> _typeMap = new()
    {
        ["project"] = typeof(Project),
        ["researcher"] = typeof(Researcher),
        ["activity"] = typeof(Activity),
        ["persona"] = typeof(Persona),
        ["event"] = typeof(Event)
        // ... register all types
    };

    private static readonly Dictionary<Type, string> _nameMap =
        _typeMap.ToDictionary(kvp => kvp.Value, kvp => kvp.Key);

    public override IGdmEntity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        var property = root.EnumerateObject().First();

        if (!_typeMap.TryGetValue(property.Name, out var targetType))
            throw new JsonException($"Unknown entity type: {property.Name}");

        var content = property.Value.GetRawText();
        return (IGdmEntity)JsonSerializer.Deserialize(content, targetType, options);
    }

    public override void Write(Utf8JsonWriter writer, IGdmEntity value, JsonSerializerOptions options)
    {
        if (!_nameMap.TryGetValue(value.GetType(), out var typeName))
            throw new JsonException($"Unregistered entity type: {value.GetType().Name}");

        writer.WriteStartObject();
        writer.WritePropertyName(typeName);
        JsonSerializer.Serialize(writer, value, value.GetType(), options);
        writer.WriteEndObject();
    }
}


// Usage
var options = new JsonSerializerOptions();
options.Converters.Add(new GdmEntityConverter());

var entities = JsonSerializer.Deserialize<List<IGdmEntity>>(jsonString, options);
```


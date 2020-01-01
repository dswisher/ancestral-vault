
using System.Collections.Generic;

using Microsoft.Extensions.Logging;

using AncestralVault.Models.Abstracts;
using AncestralVault.Models.InMemory;


namespace AncestralVault.Models.Services
{
    public class AbstractLoader
    {
        private readonly IMemoryRepository repo;
        private readonly ILogger logger;

        public AbstractLoader(IMemoryRepository repo, ILogger<AbstractLoader> logger)
        {
            this.repo = repo;
            this.logger = logger;
        }


        public void Load(Abstract data)
        {
            foreach (var abstractSource in data.Sources)
            {
                logger.LogInformation("    Citation.Title: '{0}'", abstractSource.Citation.Title);

                var memorySource = CreateSourceFromCitation(abstractSource.Citation);

                foreach (var e in abstractSource.Events)
                {
                    logger.LogInformation("    Event.Name: '{0}'", e.Name);

                    // TODO - process the data!
                }
            }
        }


        private InMemory.Source CreateSourceFromCitation(Citation citation)
        {
            // TODO - look for a higher-level source
            // TODO - subject place and jurisdiction place
            var source = repo.CreateSource();

            // TODO - for now, set the source comments equal to the citation text
            source.Comments = citation.Text;

            // Add citation parts
            AddCitationPart(source, "Title", citation.Title);
            // TODO - add remaining parts

            // Return what we've built
            return source;
        }


        private static void AddCitationPart(InMemory.Source source, string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            // TODO - do a lookup for citation type, rather than creating a new one each time
            var type = new CitationPartType
            {
                Name = name
            };

            var part = new CitationPart
            {
                Type = type,
                Value = value
            };

            source.CitationParts.Add(part);
        }
    }
}


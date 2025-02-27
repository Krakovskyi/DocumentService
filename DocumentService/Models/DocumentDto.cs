using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DocumentService.Models
{
    /// <summary>
    /// Data Transfer Object for document operations
    /// </summary>
    public class DocumentDto
    {
        /// <summary>
        /// Unique identifier for the document
        /// If not provided, a new ID will be generated
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// List of tags for categorization and searching
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// Dynamic document data
        /// Can be any valid JSON structure
        /// </summary>
        public object Data { get; set; }
    }

    /// <summary>
    /// Example provider for DocumentDto
    /// </summary>
    public class DocumentDtoExample : IExamplesProvider<DocumentDto>
    {
        /// <summary>
        /// Provides example data for Swagger UI
        /// </summary>
        /// <returns>Example document</returns>
        public DocumentDto GetExamples()
        {
            return new DocumentDto
            {
                Id = "test-doc-1",
                Tags = new List<string> { "test", "document" },
                Data = new 
                {
                    key1 = "value1",
                    key2 = 42
                }
            };
        }
    }
} 
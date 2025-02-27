using System.Text.Json;
using DocumentService.Models;

namespace DocumentService.Serializers
{
    /// <summary>
    /// Provides JSON serialization and deserialization for documents
    /// </summary>
    public class JsonDocumentSerializer : IDocumentSerializer
    {
        /// <summary>
        /// Content type for JSON serialization
        /// </summary>
        public string ContentType => "application/json";

        /// <summary>
        /// Serializes a document to JSON format
        /// </summary>
        /// <param name="document">Document to serialize</param>
        /// <returns>JSON string representation of the document</returns>
        public string Serialize(Document document)
        {
            return JsonSerializer.Serialize(document);
        }

        /// <summary>
        /// Deserializes JSON string to Document object
        /// </summary>
        /// <param name="data">JSON string to deserialize</param>
        /// <returns>Document object</returns>
        public Document Deserialize(string data)
        {
            return JsonSerializer.Deserialize<Document>(data);
        }
    }
} 
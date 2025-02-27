using DocumentService.Models;

namespace DocumentService.Serializers
{
    /// <summary>
    /// Interface for document serialization and deserialization
    /// </summary>
    public interface IDocumentSerializer
    {
        /// <summary>
        /// Content type for the serialization format
        /// </summary>
        string ContentType { get; }

        /// <summary>
        /// Serializes a document to string
        /// </summary>
        /// <param name="document">Document to serialize</param>
        /// <returns>Serialized document string</returns>
        string Serialize(Document document);

        /// <summary>
        /// Deserializes string to Document object
        /// </summary>
        /// <param name="data">String to deserialize</param>
        /// <returns>Document object</returns>
        Document Deserialize(string data);
    }
} 
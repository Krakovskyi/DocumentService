using System.Xml;
using System.Xml.Serialization;
using System.Text.Json;
using DocumentService.Models;

namespace DocumentService.Serializers
{
    /// <summary>
    /// Provides XML serialization and deserialization for documents
    /// 
    /// Key features:
    /// - Converts Document objects to XML format
    /// - Supports custom XML mapping
    /// - Handles complex document structures
    /// - Thread-safe serialization
    /// 
    /// Usage example:
    /// var serializer = new XmlDocumentSerializer();
    /// string xmlDocument = serializer.Serialize(myDocument);
    /// Document reconstructedDocument = serializer.Deserialize(xmlDocument);
    /// </summary>
    public class XmlDocumentSerializer : IDocumentSerializer
    {
        /// <summary>
        /// Defines the MIME content type for XML serialization
        /// Follows standard XML content type specification
        /// </summary>
        public string ContentType => "application/xml";

        /// <summary>
        /// Serializes a document to XML format with enhanced error handling
        /// 
        /// Transformation process:
        /// 1. Validates input document
        /// 2. Creates XML wrapper with document data
        /// 3. Generates formatted XML string
        /// 
        /// Performance considerations:
        /// - Uses XmlSerializer for efficient conversion
        /// - Supports indentation for readability
        /// - Provides detailed error information
        /// </summary>
        /// <param name="document">Document to serialize</param>
        /// <returns>Formatted XML string representation</returns>
        /// <exception cref="ArgumentNullException">Thrown when document is null</exception>
        /// <exception cref="SerializationException">Thrown on serialization errors</exception>
        public string Serialize(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document), "Document cannot be null");

            try 
            {
                // Create specialized XML serializer for wrapper class
                var serializer = new XmlSerializer(typeof(DocumentXmlWrapper));
                
                // Transform Document to XML-compatible wrapper
                var wrapper = new DocumentXmlWrapper
                {
                    Id = document.Id,
                    Tags = document.Tags,
                    Data = document.Data.RootElement.GetRawText()
                };

                using var stringWriter = new StringWriter();
                // Configure XML writer with proper formatting
                using var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true });
                
                // Perform actual XML serialization
                serializer.Serialize(xmlWriter, wrapper);
                return stringWriter.ToString();
            }
            catch (Exception ex)
            {
                // Wrap and rethrow serialization errors with context
                throw new SerializationException($"XML serialization error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deserializes XML string to Document object
        /// </summary>
        /// <param name="data">XML string to deserialize</param>
        /// <returns>Document object</returns>
        public Document Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("XML data cannot be null or empty", nameof(data));

            try 
            {
                // Create XML serializer for the wrapper class
                var serializer = new XmlSerializer(typeof(DocumentXmlWrapper));
                
                // Deserialize XML to wrapper
                using var stringReader = new StringReader(data);
                var wrapper = (DocumentXmlWrapper)serializer.Deserialize(stringReader);

                // Convert wrapper to Document
                return new Document
                {
                    Id = wrapper.Id,
                    Tags = wrapper.Tags ?? new List<string>(),
                    Data = JsonDocument.Parse(wrapper.Data ?? "{}")
                };
            }
            catch (Exception ex)
            {
                // Wrap deserialization errors in custom exception
                throw new SerializationException($"XML deserialization error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Asynchronously serializes a document to XML format
        /// </summary>
        /// <param name="document">Document to serialize</param>
        /// <returns>Task with XML string representation</returns>
        public async Task<string> SerializeAsync(Document document)
        {
            // Run synchronous serialization on a background thread
            return await Task.Run(() => Serialize(document));
        }

        /// <summary>
        /// Asynchronously deserializes XML string to Document object
        /// </summary>
        /// <param name="data">XML string to deserialize</param>
        /// <returns>Task with Document object</returns>
        public async Task<Document> DeserializeAsync(string data)
        {
            // Run synchronous deserialization on a background thread
            return await Task.Run(() => Deserialize(data));
        }
    }

    /// <summary>
    /// Helper class for XML serialization with annotations
    /// </summary>
    [XmlRoot("document")]
    public class DocumentXmlWrapper
    {
        /// <summary>
        /// Document identifier
        /// </summary>
        [XmlElement("id")]
        public string Id { get; set; }

        /// <summary>
        /// List of document tags
        /// </summary>
        [XmlArray("tags")]
        [XmlArrayItem("tag")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Document data as JSON string
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }
    }

    /// <summary>
    /// Custom exception for serialization errors
    /// </summary>
    public class SerializationException : Exception
    {
        /// <summary>
        /// Creates a new serialization exception with message
        /// </summary>
        public SerializationException(string message) : base(message) { }
        
        /// <summary>
        /// Creates a new serialization exception with message and inner exception
        /// </summary>
        public SerializationException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}
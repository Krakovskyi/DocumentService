using MessagePack;
using DocumentService.Models;
using System.Text.Json;

namespace DocumentService.Serializers
{
    /// <summary>
    /// Provides MessagePack serialization and deserialization for documents
    /// </summary>
    public class MessagePackDocumentSerializer : IDocumentSerializer
    {
        /// <summary>
        /// Content type for MessagePack serialization
        /// </summary>
        public string ContentType => "application/x-msgpack";

        /// <summary>
        /// Serializes a document to MessagePack format
        /// </summary>
        /// <param name="document">Document to serialize</param>
        /// <returns>Base64 encoded MessagePack data</returns>
        public string Serialize(Document document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document), "Document cannot be null");

            try 
            {
                // Create serializable document for MessagePack
                var serializableDocument = new SerializableDocument
                {
                    Id = document.Id,
                    Tags = document.Tags,
                    Data = document.Data.RootElement.GetRawText()
                };

                // Serialize and convert to Base64 string
                return Convert.ToBase64String(MessagePackSerializer.Serialize(serializableDocument));
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack serialization error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deserializes MessagePack data to Document object
        /// </summary>
        /// <param name="data">Base64 encoded MessagePack data</param>
        /// <returns>Document object</returns>
        public Document Deserialize(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("MessagePack data cannot be null or empty", nameof(data));

            try 
            {
                // Convert from Base64 and deserialize
                var bytes = Convert.FromBase64String(data);
                var serializableDocument = MessagePackSerializer.Deserialize<SerializableDocument>(bytes);

                // Convert to Document
                return new Document
                {
                    Id = serializableDocument.Id,
                    Tags = serializableDocument.Tags ?? new List<string>(),
                    Data = JsonDocument.Parse(serializableDocument.Data ?? "{}")
                };
            }
            catch (Exception ex)
            {
                throw new SerializationException($"MessagePack deserialization error: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Asynchronously serializes a document to MessagePack format
        /// </summary>
        public async Task<string> SerializeAsync(Document document)
        {
            return await Task.Run(() => Serialize(document));
        }

        /// <summary>
        /// Asynchronously deserializes MessagePack data to Document object
        /// </summary>
        public async Task<Document> DeserializeAsync(string data)
        {
            return await Task.Run(() => Deserialize(data));
        }

        /// <summary>
        /// Class for MessagePack serialization
        /// </summary>
        [MessagePackObject]
        public class SerializableDocument
        {
            /// <summary>
            /// Document identifier
            /// </summary>
            [Key(0)]
            public string Id { get; set; }

            /// <summary>
            /// List of document tags
            /// </summary>
            [Key(1)]
            public List<string> Tags { get; set; }

            /// <summary>
            /// Document data as JSON string
            /// </summary>
            [Key(2)]
            public string Data { get; set; }
        }
    }
} 
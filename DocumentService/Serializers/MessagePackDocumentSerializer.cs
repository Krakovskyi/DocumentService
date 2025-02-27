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
            var dto = new
            {
                document.Id,
                document.Tags,
                Data = document.Data.RootElement.GetRawText()
            };

            var bytes = MessagePackSerializer.Serialize(dto);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Deserializes MessagePack data to Document object
        /// </summary>
        /// <param name="data">Base64 encoded MessagePack data</param>
        /// <returns>Document object</returns>
        public Document Deserialize(string data)
        {
            var bytes = Convert.FromBase64String(data);
            var dto = MessagePackSerializer.Deserialize<dynamic>(bytes);
            
            return new Document
            {
                Id = dto.Id,
                Tags = dto.Tags,
                Data = JsonDocument.Parse(dto.Data)
            };
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
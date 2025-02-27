namespace DocumentService.Serializers
{
    /// <summary>
    /// Interface for serializer factory
    /// </summary>
    public interface ISerializerFactory
    {
        /// <summary>
        /// Gets a serializer for the specified content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>Document serializer</returns>
        IDocumentSerializer GetSerializer(string contentType);
    }

    /// <summary>
    /// Factory for creating document serializers based on content type
    /// </summary>
    public class SerializerFactory : ISerializerFactory
    {
        private readonly JsonDocumentSerializer _jsonSerializer;
        private readonly XmlDocumentSerializer _xmlSerializer;
        private readonly MessagePackDocumentSerializer _messagePackSerializer;

        /// <summary>
        /// Creates a new serializer factory
        /// </summary>
        /// <param name="jsonSerializer">JSON serializer</param>
        /// <param name="xmlSerializer">XML serializer</param>
        /// <param name="messagePackSerializer">MessagePack serializer</param>
        public SerializerFactory(
            JsonDocumentSerializer jsonSerializer,
            XmlDocumentSerializer xmlSerializer,
            MessagePackDocumentSerializer messagePackSerializer)
        {
            _jsonSerializer = jsonSerializer;
            _xmlSerializer = xmlSerializer;
            _messagePackSerializer = messagePackSerializer;
        }

        /// <summary>
        /// Gets a serializer for the specified content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>Document serializer</returns>
        public IDocumentSerializer GetSerializer(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return _jsonSerializer;

            if (contentType.Contains("application/xml"))
                return _xmlSerializer;

            if (contentType.Contains("application/x-msgpack"))
                return _messagePackSerializer;

            return _jsonSerializer;
        }
    }
} 
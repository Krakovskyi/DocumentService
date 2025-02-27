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
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, Type> _serializers;

        /// <summary>
        /// Creates a new serializer factory
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving serializers</param>
        public SerializerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _serializers = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase)
            {
                { "application/json", typeof(JsonDocumentSerializer) },
                { "application/xml", typeof(XmlDocumentSerializer) },
                { "application/x-msgpack", typeof(MessagePackDocumentSerializer) }
            };
        }

        /// <summary>
        /// Gets a serializer for the specified content type
        /// </summary>
        /// <param name="contentType">Content type</param>
        /// <returns>Document serializer</returns>
        public IDocumentSerializer GetSerializer(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return _serviceProvider.GetRequiredService<JsonDocumentSerializer>();

            // Remove parameters after semicolon if present
            contentType = contentType.Split(';')[0].Trim();

            if (_serializers.TryGetValue(contentType, out var serializerType))
            {
                return (IDocumentSerializer)_serviceProvider.GetRequiredService(serializerType);
            }

            // Default to JSON serializer
            return _serviceProvider.GetRequiredService<JsonDocumentSerializer>();
        }
    }
} 
using DocumentService.Models;
using DocumentService.Repositories;
using DocumentService.Serializers;
using System.Text.Json;

namespace DocumentService.Services
{
    /// <summary>
    /// Service for managing document operations
    /// </summary>
    public class DocumentServiceManager
    {
        private readonly IDocumentRepository _repository;
        private readonly IDocumentSerializer _jsonSerializer;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="repository">Document repository</param>
        /// <param name="jsonSerializer">JSON serializer for documents</param>
        public DocumentServiceManager(
            IDocumentRepository repository, 
            JsonDocumentSerializer jsonSerializer)
        {
            _repository = repository;
            _jsonSerializer = jsonSerializer;
        }

        /// <summary>
        /// Creates a new document
        /// </summary>
        /// <param name="documentDto">Document data</param>
        /// <returns>Created document</returns>
        public async Task<Document> CreateDocumentAsync(DocumentDto documentDto)
        {
            var document = new Document
            {
                // Generate new GUID if ID not provided
                Id = documentDto.Id ?? Guid.NewGuid().ToString(),
                Tags = documentDto.Tags ?? new List<string>(),
                Data = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(documentDto.Data))
            };

            return await _repository.AddAsync(document);
        }

        /// <summary>
        /// Gets a document by ID
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Document if found, null otherwise</returns>
        public async Task<Document> GetDocumentAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        /// <summary>
        /// Updates an existing document
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="documentDto">Updated document data</param>
        /// <returns>Updated document if found, null otherwise</returns>
        public async Task<Document> UpdateDocumentAsync(string id, DocumentDto documentDto)
        {
            var existingDocument = await _repository.GetByIdAsync(id);
            if (existingDocument == null)
                return null;

            existingDocument.Tags = documentDto.Tags ?? existingDocument.Tags;
            existingDocument.Data = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(documentDto.Data));

            return await _repository.UpdateAsync(existingDocument);
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public async Task DeleteDocumentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
} 
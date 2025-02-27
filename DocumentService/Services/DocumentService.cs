using DocumentService.Models;
using DocumentService.Repositories;
using DocumentService.Serializers;
using System.Text.Json;

namespace DocumentService.Services
{
    public class DocumentService
    {
        private readonly IDocumentRepository _repository;
        private readonly IDocumentSerializer _serializer;

        public DocumentService(
            IDocumentRepository repository, 
            IDocumentSerializer serializer)
        {
            _repository = repository;
            _serializer = serializer;
        }

        public async Task<Document> CreateDocumentAsync(DocumentDto documentDto)
        {
            var document = new Document
            {
                Id = documentDto.Id ?? Guid.NewGuid().ToString(),
                Tags = documentDto.Tags ?? new List<string>(),
                Data = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(documentDto.Data))
            };

            return await _repository.AddAsync(document);
        }

        public async Task<Document> GetDocumentAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Document> UpdateDocumentAsync(string id, DocumentDto documentDto)
        {
            var existingDocument = await _repository.GetByIdAsync(id);
            if (existingDocument == null)
                return null;

            existingDocument.Tags = documentDto.Tags ?? existingDocument.Tags;
            existingDocument.Data = JsonDocument.Parse(System.Text.Json.JsonSerializer.Serialize(documentDto.Data));

            return await _repository.UpdateAsync(existingDocument);
        }

        public async Task DeleteDocumentAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
} 
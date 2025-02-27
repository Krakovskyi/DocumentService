using DocumentService.Models;
using System.Collections.Concurrent;

namespace DocumentService.Repositories
{
    /// <summary>
    /// In-memory implementation of document repository
    /// Suitable for testing and development
    /// </summary>
    public class InMemoryDocumentRepository : IDocumentRepository
    {
        // Thread-safe dictionary for storing documents
        private readonly ConcurrentDictionary<string, Document> _documents = new();

        /// <summary>
        /// Gets a document by its ID
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Document if found, null otherwise</returns>
        public Task<Document> GetByIdAsync(string id)
        {
            _documents.TryGetValue(id, out var document);
            return Task.FromResult(document);
        }

        /// <summary>
        /// Adds a new document to the repository
        /// </summary>
        /// <param name="document">Document to add</param>
        /// <returns>Added document</returns>
        /// <exception cref="ArgumentException">Thrown when document with same ID already exists</exception>
        public Task<Document> AddAsync(Document document)
        {
            if (!_documents.TryAdd(document.Id, document))
            {
                throw new ArgumentException($"Document with ID {document.Id} already exists");
            }

            return Task.FromResult(document);
        }

        /// <summary>
        /// Updates an existing document
        /// </summary>
        /// <param name="document">Document with updated data</param>
        /// <returns>Updated document</returns>
        /// <exception cref="KeyNotFoundException">Thrown when document not found</exception>
        public Task<Document> UpdateAsync(Document document)
        {
            if (!_documents.TryGetValue(document.Id, out _))
            {
                throw new KeyNotFoundException($"Document with ID {document.Id} not found");
            }

            _documents[document.Id] = document;
            return Task.FromResult(document);
        }

        /// <summary>
        /// Deletes a document by its ID
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Task representing the asynchronous operation</returns>
        public Task DeleteAsync(string id)
        {
            _documents.TryRemove(id, out _);
            return Task.CompletedTask;
        }
    }
} 
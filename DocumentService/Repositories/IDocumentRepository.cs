using DocumentService.Models;

namespace DocumentService.Repositories
{
    /// <summary>
    /// Interface for document repository operations
    /// </summary>
    public interface IDocumentRepository
    {
        /// <summary>
        /// Gets a document by its ID
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Document if found, null otherwise</returns>
        Task<Document> GetByIdAsync(string id);

        /// <summary>
        /// Adds a new document to the repository
        /// </summary>
        /// <param name="document">Document to add</param>
        /// <returns>Added document</returns>
        Task<Document> AddAsync(Document document);

        /// <summary>
        /// Updates an existing document
        /// </summary>
        /// <param name="document">Document with updated data</param>
        /// <returns>Updated document</returns>
        Task<Document> UpdateAsync(Document document);

        /// <summary>
        /// Deletes a document by its ID
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Task representing the asynchronous operation</returns>
        Task DeleteAsync(string id);
    }
} 
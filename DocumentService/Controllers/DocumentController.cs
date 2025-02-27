using Microsoft.AspNetCore.Mvc;
using DocumentService.Models;
using DocumentService.Services;
using System.Threading.Tasks;
using System.Linq;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace DocumentService.Controllers
{
    /// <summary>
    /// Controller for document operations
    /// </summary>
    [ApiController]
    [Route("documents")]
    public class DocumentController : ControllerBase
    {
        private readonly DocumentServiceManager _documentService;
        private readonly ISerializerFactory _serializerFactory;

        /// <summary>
        /// Constructor with dependency injection
        /// </summary>
        /// <param name="documentService">Service for document operations</param>
        /// <param name="serializerFactory">Factory for document serializers</param>
        public DocumentController(DocumentServiceManager documentService, ISerializerFactory serializerFactory)
        {
            _documentService = documentService;
            _serializerFactory = serializerFactory;
        }

        /// <summary>
        /// Creates a new document
        /// </summary>
        /// <param name="documentDto">Document data</param>
        /// <returns>Created document</returns>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Create a new document",
            Description = "Creates a document with the specified data"
        )]
        [SwaggerResponse(201, "Document created successfully")]
        [SwaggerResponse(400, "Invalid document data")]
        [SwaggerRequestExample(typeof(DocumentDto), typeof(DocumentDtoExample))]
        public async Task<IActionResult> CreateDocument([FromBody] DocumentDto documentDto)
        {
            var document = await _documentService.CreateDocumentAsync(documentDto);
            return CreatedAtAction(nameof(GetDocument), new { id = document.Id }, document);
        }

        /// <summary>
        /// Gets a document by ID
        /// Supports content negotiation for different formats
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>Document in requested format</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocument(string id)
        {
            var document = await _documentService.GetDocumentAsync(id);
            if (document == null)
                return NotFound();

            // Определяем сериализатор на основе Accept заголовка
            var acceptHeader = Request.Headers.Accept.FirstOrDefault();
            var serializer = _serializerFactory.GetSerializer(acceptHeader);
            
            var serializedDocument = serializer.Serialize(document);
            return Content(serializedDocument, serializer.ContentType);
        }

        /// <summary>
        /// Updates an existing document
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <param name="documentDto">Updated document data</param>
        /// <returns>Updated document</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(string id, [FromBody] DocumentDto documentDto)
        {
            var document = await _documentService.UpdateDocumentAsync(id, documentDto);
            if (document == null)
                return NotFound();

            return Ok(document);
        }

        /// <summary>
        /// Deletes a document
        /// </summary>
        /// <param name="id">Document ID</param>
        /// <returns>No content</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(string id)
        {
            await _documentService.DeleteDocumentAsync(id);
            return NoContent();
        }
    }
} 
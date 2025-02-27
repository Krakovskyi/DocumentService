using System.Text.Json;
using System.ComponentModel.DataAnnotations;

namespace DocumentService.Models
{
    /// <summary>
    /// Represents a document in the system
    /// </summary>
    public class Document
    {
        /// <summary>
        /// Unique identifier for the document
        /// </summary>
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Id { get; set; }

        /// <summary>
        /// List of tags for categorization and searching
        /// </summary>
        [Required]
        public List<string> Tags { get; set; } = new List<string>();

        /// <summary>
        /// Dynamic document data stored as JSON
        /// </summary>
        [Required]
        public JsonDocument Data { get; set; }

        /// <summary>
        /// Проверяет валидность документа
        /// </summary>
        /// <returns>True, если документ корректен</returns>
        public bool IsValid()
        {
            // Проверяем, что идентификатор не пустой
            // Теги не null
            // Данные не null
            return !string.IsNullOrWhiteSpace(Id) && 
                   Tags != null && 
                   Data != null;
        }

        // Конструктор с параметрами для создания документа
        public Document(string id, List<string> tags, JsonDocument data)
        {
            Id = id;
            Tags = tags;
            Data = data;
        }

        // Пустой конструктор для десериализации
        public Document() { }
    }
} 
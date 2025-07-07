#nullable enable

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskTracker
{
    public class Tarea
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; } = "todo";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [JsonConstructor]
        public Tarea(int id, string? description, string? status = null, 
            DateTime? createdAt = null, DateTime? updatedAt = null)
        {
            Id = id;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Status = status ?? "todo";
            CreatedAt = createdAt ?? DateTime.Now;
            UpdatedAt = updatedAt ?? DateTime.Now;
        }

        public Tarea(int id, string? description) : this(id, description, "todo", null, null) {}

        public Dictionary<string, object> ToDictionary() => new()
        {
            ["id"] = Id,
            ["description"] = Description ?? string.Empty,
            ["status"] = Status,
            ["createdAt"] = CreatedAt,
            ["updatedAt"] = UpdatedAt
        };

        public static Tarea? FromDictionary(Dictionary<string, object> d)
        {
            try
            {
                if (!d.ContainsKey("id") || !d.ContainsKey("description") || 
                    !d.ContainsKey("status") || !d.ContainsKey("createdAt") || 
                    !d.ContainsKey("updatedAt"))
                {
                    throw new ArgumentException("El diccionario no contiene todos los campos requeridos.");
                }

                var id = Convert.ToInt32(d["id"]);
                var description = Convert.ToString(d["description"]);
                var status = Convert.ToString(d["status"]);
                
                if (!DateTime.TryParse(Convert.ToString(d["createdAt"]), out var createdAt) ||
                    !DateTime.TryParse(Convert.ToString(d["updatedAt"]), out var updatedAt))
                {
                    throw new ArgumentException("Formato de fecha inválido");
                }

                return new Tarea(id, description, status, createdAt, updatedAt);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear Tarea desde diccionario: {ex.Message}");
                return null;
            }
        }
    }
}

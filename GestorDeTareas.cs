using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace TaskTracker
{
    public class GestorDeTareas
    {
        private List<Tarea> tasks = new();
        private readonly string filePath;
     
        public GestorDeTareas(string filePath)
        {
            this.filePath = filePath;
            LoadTasks();
        }

        private void LoadTasks()
        {
            if (!File.Exists(filePath)) 
            {
                tasks = new();
                return;
            }

            try 
            {
                string json = File.ReadAllText(filePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    tasks = new();
                    return;
                }

                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                };
                
                var listas = JsonSerializer.Deserialize<List<Tarea>>(json, options) ?? new();
                tasks = listas;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Error en el formato del archivo de tareas: {ex.Message}");
                tasks = new();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inesperado al cargar tareas: {ex.Message}");
                tasks = new();
            }
        }
    
        public void SaveTasks()
        {
            try
            {
                var taskDicts = tasks.ConvertAll(t => t.ToDictionary());
                var options = new JsonSerializerOptions 
                { 
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string json = JsonSerializer.Serialize(taskDicts, options);
                string? directory = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar las tareas: {ex.Message}");
                throw;
            }
        }
    
        public Tarea AddTask(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

            int newId = tasks.Count > 0 ? tasks.Max(t => t.Id) + 1 : 1;
            var tarea = new Tarea(newId, description.Trim());
            
            try
            {
                tasks.Add(tarea);
                SaveTasks();
                return tarea;
            }
            catch
            {
                tasks.Remove(tarea); // Revertir en caso de error
                throw;
            }
        }

        private Tarea? FindTask(int id) => tasks.FirstOrDefault(t => t.Id == id);

        public bool UpdateTask(int id, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La descripción no puede estar vacía", nameof(description));

            var tarea = FindTask(id);
            if (tarea == null) return false;
            
            try
            {
                tarea.Description = description.Trim();
                tarea.UpdatedAt = DateTime.Now;
                SaveTasks();
                return true;
            }
            catch
            {
                // Revertir cambios en caso de error
                tarea.UpdatedAt = DateTime.Now.AddSeconds(-1); // Marcar como no guardado
                return false;
            }
        }

        public bool DeleteTask(int id)
        {
            var tarea = FindTask(id);
            if (tarea == null) return false;
            
            tasks.Remove(tarea);
            SaveTasks();
            return true;
        }

        public bool MarkInProgress(int id)
        {
            var tarea = FindTask(id);
            if (tarea == null || tarea.Status == "done") return false;
            
            tarea.Status = "in-progress";
            tarea.UpdatedAt = DateTime.Now;
            SaveTasks();
            return true;
        }

        public bool MarkDone(int id)
        {
            var tarea = FindTask(id);
            if (tarea == null) return false;
            
            tarea.Status = "done";
            tarea.UpdatedAt = DateTime.Now;
            SaveTasks();
            return true;
        }

        public List<Tarea> ListTasks() => new(tasks);

        public List<Tarea> ListByStatus(string status) => 
            tasks.Where(t => t.Status == status).ToList();
    }
}
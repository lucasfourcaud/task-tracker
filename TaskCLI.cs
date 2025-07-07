using System;
using System.Linq;

namespace TaskTracker
{
    public class TaskCLI
    {
        private readonly GestorDeTareas manager;

        public TaskCLI(GestorDeTareas manager) => this.manager = manager;

        public void ParseArgs(string[] args)
        {
            if (args.Length == 0)
            {
                ShowHelp();
                return;
            }

            string command = args[0].ToLower();
            string[] commandArgs = args[1..];

            try
            {
                RunCommand(command, commandArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ShowHelp();
            }
        }

        private void RunCommand(string command, string[] args)
        {
            switch (command)
            {
                case "add" when args.Length == 0:
                    throw new ArgumentException("Falta la descripción de la tarea");
                case "add":
                    HandleAdd(string.Join(" ", args));
                    break;

                case "update":
                    if (args.Length < 2) 
                        throw new ArgumentException("Uso: update <id> <nueva_descripción>");
                    if (!int.TryParse(args[0], out int updateId))
                        throw new ArgumentException("ID inválido");
                    HandleUpdate(updateId, string.Join(" ", args[1..]));
                    break;

                case "delete":
                    if (args.Length == 0)
                        throw new ArgumentException("Uso: delete <id>");
                    if (!int.TryParse(args[0], out int deleteId))
                        throw new ArgumentException("ID inválido");
                    HandleDelete(deleteId);
                    break;

                case "mark-in-progress":
                    if (args.Length == 0)
                        throw new ArgumentException("Uso: mark-in-progress <id>");
                    if (!int.TryParse(args[0], out int progressId))
                        throw new ArgumentException("ID inválido");
                    HandleMarkInProgress(progressId);
                    break;

                case "mark-done":
                    if (args.Length == 0)
                        throw new ArgumentException("Uso: mark-done <id>");
                    if (!int.TryParse(args[0], out int doneId))
                        throw new ArgumentException("ID inválido");
                    HandleMarkDone(doneId);
                    break;

                case "list":
                    HandleList();
                    break;

                default:
                    Console.WriteLine("Comando no reconocido");
                    ShowHelp();
                    break;
            }
        }

        private static void ShowHelp()
        {
            const string helpText = """
            Uso: task-cli <comando> [argumentos]
            Comandos disponibles:
              add <descripción>         - Añade una nueva tarea
              update <id> <descripción> - Actualiza una tarea existente
              delete <id>              - Elimina una tarea
              mark-in-progress <id>    - Marca una tarea como en progreso
              mark-done <id>           - Marca una tarea como completada
              list                     - Muestra todas las tareas
            """;
            Console.WriteLine(helpText);
        }

        private void HandleAdd(string description)
        {
            var tarea = manager.AddTask(description);
            Console.WriteLine($"Tarea añadida con ID: {tarea.Id}");
        }

        private void HandleUpdate(int id, string description)
        {
            Console.WriteLine(manager.UpdateTask(id, description)
                ? "Tarea actualizada con éxito"
                : "Error: Tarea no encontrada");
        }

        private void HandleDelete(int id)
        {
            Console.WriteLine(manager.DeleteTask(id)
                ? "Tarea eliminada con éxito"
                : "Error: Tarea no encontrada");
        }

        private void HandleMarkInProgress(int id)
        {
            Console.WriteLine(manager.MarkInProgress(id)
                ? "Tarea marcada como en progreso"
                : "Error: Tarea no encontrada o ya completada");
        }

        private void HandleMarkDone(int id)
        {
            Console.WriteLine(manager.MarkDone(id)
                ? "Tarea marcada como completada"
                : "Error: Tarea no encontrada");
        }

        private void HandleList()
        {
            var tasks = manager.ListTasks();
            if (tasks.Count == 0)
            {
                Console.WriteLine("No hay tareas.");
                return;
            }

            Console.WriteLine("ID | Estado        | Descripción");
            Console.WriteLine("-------------------------------");
            
            foreach (var tarea in tasks)
            {
                string estado = tarea.Status switch
                {
                    "todo" => "Pendiente    ",
                    "in-progress" => "En progreso ",
                    "done" => "Completada   ",
                    _ => tarea.Status ?? "Desconocido  "
                };
                Console.WriteLine($"{tarea.Id,2} | {estado} | {tarea.Description}");
            }
        }
    }
}
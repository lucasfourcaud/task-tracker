using System;
using System.Collections.Generic;

using System;

namespace TaskTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Crea el gerente con la ruta del archivo tasks.json
            var manager = new GestorDeTareas("tasks.json");

            // Crea la interfaz de comandos con el gerente
            var cli = new TaskCLI(manager);

            // Procesa los argumentos de la terminal
            cli.ParseArgs(args);
        }
    }
}
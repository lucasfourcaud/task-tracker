
https://github.com/lucasfourcaud/task-tracker
 
 Task Tracker
Solución de muestra para el desafío de seguimiento de tareas de roadmap.sh .

Una aplicación de línea de comandos para gestionar tareas, desarrollada en C# con .NET Core.
 


✨ Características
✅ Crear nuevas tareas con descripción
📝 Actualizar tareas existentes
❌ Eliminar tareas
🔄 Cambiar estado de tareas (Pendiente/En progreso/Completada)
📋 Listar todas las tareas con su estado actual
💾 Guardado automático en archivo JSON
🛠️ Validación de entradas
🚨 Manejo robusto de errores
📋 Requisitos
.NET 6.0 SDK o superior
🚀 Instalación
Clona el repositorio:

git clone [URL_DEL_REPOSITORIO]
cd Task-Tracker
Compila el proyecto:

dotnet build
 Uso
Agregar una tarea
dotnet run -- add "Descripción de la tarea"
Listar todas las tareas
dotnet run -- list
Actualizar una tarea
dotnet run -- update [ID] "Nueva descripción"
Marcar tarea como en progreso
dotnet run -- mark-in-progress [ID]
Marcar tarea como completada
dotnet run -- mark-done [ID]
Eliminar una tarea
dotnet run -- delete [ID]
 Estructura del proyecto
Program.cs - Punto de entrada de la aplicación
TaskCLI.cs - Lógica de la interfaz de línea de comandos
GestorDeTareas.cs - Lógica principal del gestor de tareas
Tarea.cs - Modelo de datos para las tareas
tasks.json - Archivo donde se guardan las tareas (se crea automáticamente)
 Ejemplo de uso
# Agregar tareas
dotnet run -- add "Comprar leche"
dotnet run -- add "Hacer ejercicio"

# Ver tareas
dotnet run -- list
# Salida:
# ID | Estado        | Descripción
# -------------------------------
#  1 | Pendiente    | Comprar leche
#  2 | Pendiente    | Hacer ejercicio

# Marcar tarea como en progreso
dotnet run -- mark-in-progress 1

# Completar tarea
dotnet run -- mark-done 2

# Ver tareas actualizadas
dotnet run -- list
# Salida:
# ID | Estado        | Descripción
# -------------------------------
#  1 | En progreso  | Comprar leche
#  2 | Completada   | Hacer ejercicio

Características Técnicas
Persistencia de datos en formato JSON
Validación de entradas del usuario
Manejo de errores robusto
Código limpio y mantenible
Compatible con .NET 6.0+

Licencia
Este proyecto está bajo la Licencia MIT. Ver el archivo LICENSE para más detalles.

Desarrollado con ❤️ por Lucas Fourcaud
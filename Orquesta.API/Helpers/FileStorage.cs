using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;
using System;

namespace Orquesta.API.Helpers
{
    public class FileStorage : IFileStorage
    {
        private readonly string localDirectoryPath;

        public FileStorage(IConfiguration configuration)
        {
            // Define la carpeta donde se guardarán las imágenes localmente.
            localDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagenes");

            // Asegúrate de que la carpeta existe.
            if (!Directory.Exists(localDirectoryPath))
            {
                Directory.CreateDirectory(localDirectoryPath);
            }
        }

        public async Task<string> SaveFileAsync(byte[] content, string extension, string containerName)
        {
            // Genera un nombre único para el archivo
            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(localDirectoryPath, containerName, fileName);

            // Crea la carpeta del contenedor si no existe
            var containerPath = Path.Combine(localDirectoryPath, containerName);
            if (!Directory.Exists(containerPath))
            {
                Directory.CreateDirectory(containerPath);
            }

            // Guarda el archivo en la ruta local
            await File.WriteAllBytesAsync(fullPath, content);

            // Devuelve la URL relativa
            return $"/imagenes/{containerName}/{fileName}";
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var fileName = Path.GetFileName(path);
            var fullPath = Path.Combine(localDirectoryPath, containerName, fileName);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            await Task.CompletedTask;
        }

        public async Task<string> EditFileAsync(byte[] content, string extension, string containerName, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                await RemoveFileAsync(path, containerName);
            }

            return await SaveFileAsync(content, extension, containerName);
        }
    }
}

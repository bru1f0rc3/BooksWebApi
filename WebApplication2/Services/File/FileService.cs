using System.IO;
using Microsoft.AspNetCore.Http;

namespace WebApplication2.Services.File
{
    public class FileService
    {
        private readonly string _uploadDirectory;

        public FileService(IWebHostEnvironment environment)
        {
            _uploadDirectory = Path.Combine(environment.ContentRootPath, "coverlink");
            if (!Directory.Exists(_uploadDirectory))
            {
                Directory.CreateDirectory(_uploadDirectory);
            }
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл не выбран");

            if (!file.ContentType.StartsWith("image/"))
                throw new ArgumentException("Файл должен быть изображением");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }


            return fileName;
        }

        public void DeleteImage(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return;

            if (fileName.StartsWith("http://") || fileName.StartsWith("https://"))
                return;

            var filePath = Path.Combine(_uploadDirectory, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
} 
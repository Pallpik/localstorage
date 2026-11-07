using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using HomeLocalStorage.DataAccess.Managers;
using Microsoft.AspNetCore.Http;

namespace HomeLocalStorage.DataAccess.Managers
{
    public class FileManager : IFileManager
    {

        public async Task<string> CreateFile(IFormFile file, string path, string fileName, string type)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                await using var fileStream = new FileStream(Path.Combine(path, fileName + type), FileMode.CreateNew);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                return $"Error: {ex}";
            }

            return "";
        }
        public async Task<string> RewriteFile(IFormFile file, string path, string fileName, string type)
        {
            var error = string.Empty;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    error += "Warning: File with this path not found, will be created...";
                }
                await using var fileStream = new FileStream(Path.Combine(path, fileName + type), FileMode.Create);
                await file.CopyToAsync(fileStream);
            }
            catch (Exception ex)
            {
                return $"Error: {ex}/n{error}";
            }
            return $"{error}";
        }
        public async Task<string> DeleteFile(string path, string fileName, string type)
        {
            try
            {
                if (!System.IO.File.Exists(Path.Combine(path, fileName + type)))
                    return $"Error: File {fileName} not found";

                try
                {
                    using var stream = System.IO.File.Open(Path.Combine(path, fileName + type), FileMode.Open, FileAccess.Read, FileShare.None);
                }
                catch (IOException ex)
                {
                    return $"Warning: File {fileName} currently used by another process\nError: {ex}";
                }

                System.IO.File.Delete(Path.Combine(path, fileName+type));

                return "";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

        }

        public async Task<string> RedirectFile(string path, string fileName, string newPath, string type)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(newPath))
                {
                    return "Path, filename and new path are required";
                }

                var sourcePath = Path.Combine(path, fileName+type);
                var destinationPath = Path.Combine(newPath, fileName+type);

                if (!File.Exists(sourcePath))
                {
                    return $"Source file not found: {fileName}";
                }

                string? destinationDir = Path.GetDirectoryName(destinationPath);
                if (!Directory.Exists(destinationDir))
                {
                    Directory.CreateDirectory(destinationDir!);
                }

                if (File.Exists(destinationPath))
                {
                    return $"File already exists in destination: {fileName}";
                }

                await Task.Run(() => File.Move(sourcePath, destinationPath));

                return "";
            }
            catch (Exception ex)
            {
                return $"Error moving file: {ex.Message}";
            }
        }
        public async Task<string> RenameFile(string path, string fileName, string newName, string type)
        {
            try
            {
                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(newName))
                {
                    return "Path, filename and new name are required";
                }

                var safeNewName = Path.GetFileName(newName);

                var sourcePath = Path.Combine(path, fileName + type);
                var destinationPath = Path.Combine(path, safeNewName + type);

                if (!File.Exists(sourcePath))
                {
                    return $"File not found: {fileName}";
                }

                if (File.Exists(destinationPath))
                {
                    return $"File with name {safeNewName} already exists";
                }

                await Task.Run(() => File.Move(sourcePath, destinationPath));

                return "";
            }
            catch (Exception ex)
            {
                return $"Error renaming file: {ex.Message}";
            }
        }

        public async Task<FileStream> GetFileStream(string path, string name, string type)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Path and name are required");
            }

            var fullPath = Path.Combine(path, name + type);

            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found: {fullPath}");
            }


            return System.IO.File.OpenRead(fullPath);

        }
    }
}

using System;
using System.IO;
using System.Threading.Tasks;

namespace HomeLocalStorage.DataAccess.Managers
{
    public class FolderManager : IFolderManager
    {
        public async Task<string> CreateFolder(string path, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Имя папки не может быть пустым", nameof(name));

                string fullPath = Path.Combine(path, name);

                if (Directory.Exists(fullPath))
                    throw new IOException($"Папка '{name}' уже существует");

                Directory.CreateDirectory(fullPath);

                return "";
            }
            catch (Exception ex)
            {
               return $"Ошибка при создании папки: {ex.Message}";
            }
        }

        public async Task<string> DeleteFolder(string path, string name)
        {
            try
            {
                
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentException("Имя папки не может быть пустым", nameof(name));

                string fullPath = Path.Combine(path, name);

                if (!Directory.Exists(fullPath))
                    throw new DirectoryNotFoundException($"Папка '{fullPath}' не найдена");

                Directory.Delete(fullPath, recursive: true);

                return "";
            }
            catch (Exception ex)
            {
                return $"Ошибка при удалении папки: {ex.Message}";
            }
        }

        public async Task<string> UpdateFolderName(string path, string folderName, string newName)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(folderName))
                    throw new ArgumentException("Имя папки не может быть пустым", nameof(folderName));

                if (string.IsNullOrWhiteSpace(newName))
                    throw new ArgumentException("Новое имя папки не может быть пустым", nameof(newName));

                string oldPath = Path.Combine(path, folderName);
                string newPath = Path.Combine(path, newName);

                if (!Directory.Exists(oldPath))
                    throw new DirectoryNotFoundException($"Папка '{oldPath}' не найдена");

                if (Directory.Exists(newPath))
                    throw new IOException($"Папка с именем '{newName}' уже существует в указанном пути");

                Directory.Move(oldPath, newPath);

                return "";
            }
            catch (Exception ex)
            {
                return $"Ошибка при переименовании папки: {ex.Message}";
            }
        }

        public async Task<string> RedirectFolder(string currentPath, string folderName, string newPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(folderName))
                    throw new ArgumentException("Имя папки не может быть пустым", nameof(folderName));

                string oldFullPath = Path.Combine(currentPath, folderName);
                string newFullPath = Path.Combine(newPath, folderName);

                if (!Directory.Exists(oldFullPath))
                    throw new DirectoryNotFoundException($"Папка '{oldFullPath}' не найдена");

                if (!Directory.Exists(newPath))
                    throw new DirectoryNotFoundException($"Целевая директория '{newPath}' не найдена");

                if (Directory.Exists(newFullPath))
                    throw new IOException($"Папка с именем '{folderName}' уже существует в целевой директории");

                Directory.Move(oldFullPath, newFullPath);

                return await Task.FromResult(newFullPath);
            }
            catch (Exception ex)
            {
                return $"Ошибка при перемещении папки: {ex.Message}";
            }
        }
    }
}
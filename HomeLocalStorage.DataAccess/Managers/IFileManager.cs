using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Managers
{
    public interface IFileManager
    {
        Task<FileStream> GetFileStream(string path, string name, string type);
        Task<string> CreateFile(IFormFile file, string path, string fileName, string type);
        Task<string> RewriteFile(IFormFile file, string path, string fileName, string type);
        Task<string> DeleteFile(string path, string fileName, string type);
        Task<string> RedirectFile(string path, string fileName, string newPath, string type);
        Task<string> RenameFile(string path, string fileName, string newName, string type);

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HomeLocalStorage.DataAccess.Managers
{
    public interface IFolderManager
    {
        Task<string> CreateFolder(string path, string name);
        Task<string> DeleteFolder(string path, string name);
        Task<string> UpdateFolderName(string path, string folderName, string newName);
        Task<string> RedirectFolder(string currentPath, string folderName, string newPath);
    }
}

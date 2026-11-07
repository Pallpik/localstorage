using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeLocalStorage.Core.Models
{
    public enum AccessCodes
    {
        REALREAD,
        REALNONE,
        PSEUDOREAD,
        PSEUDONONE,
    }

    public class Material
    {
        public const int MAX_NAME_LENGTH = 200;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Path { get; set; } = string.Empty;
        public AccessCodes Code { get; set; }


        public Material() {
            Id = Guid.Empty;
            Name = string.Empty;
            Description = string.Empty;
            Type = string.Empty;
            Author = string.Empty;
            Code = AccessCodes.REALREAD;
        }
        public Material(Guid Id, string Name, string Description, string Path, string Type, string Author, AccessCodes Code) {
            this.Id = Id;
            this.Name = Name;
            this.Description = Description;
            this.Type = Type;
            this.Path = Path;
            this.Author = Author;
            this.Code = Code;
        }

        public static (bool isValid, string error) ValidateName(string Name)
        {
            if (string.IsNullOrEmpty(Name) || Name.Length > MAX_NAME_LENGTH)
            {
                return (false, $"Name can not be empty or longer than {MAX_NAME_LENGTH} symbols!");
            }
            return (true, "");
        }

        public static (bool isValid, string error) ValidatePath(string Path)
        {
            if (string.IsNullOrEmpty(Path))
            {
                return (false, $"Path can not be empty!");
            }
            return (true, "");
        }

    }
}

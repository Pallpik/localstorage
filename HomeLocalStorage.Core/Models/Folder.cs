namespace HomeLocalStorage.Core.Models
{
    public class Folder
    {
        public const int MAX_NAME_LENGTH = 32;
        public Guid Id { get; }
        public string Name { get; }
        public string Path { get; set; } = string.Empty;
        public string Author { get; }
        public AccessCodes Code { get; }
        public ICollection<Material> Materials { get; set; } = [];
        public Folder()
        {
            Id = Guid.Empty;
            Name = string.Empty;
            Author = string.Empty;
            Code = AccessCodes.REALREAD;
        }

        public Folder(Guid Id, string Name, string Author, AccessCodes Code)
        {
            this.Id = Id;
            this.Name = Name;
            this.Author = Author;
            this.Code = Code;
        }

        public static string ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                return $"Name can not be empty or longer than {MAX_NAME_LENGTH} symbols!";
            }
            return "";
        }

    }
}

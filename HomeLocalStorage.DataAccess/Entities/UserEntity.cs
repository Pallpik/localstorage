namespace HomeLocalStorage.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;


        public virtual ICollection<MaterialEntity> Materials { get; set; } = [];

        public virtual ICollection<FolderEntity> Folders { get; set; } = [];

    }
}

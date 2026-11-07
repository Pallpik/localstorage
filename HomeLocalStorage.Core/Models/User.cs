using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HomeLocalStorage.Core.Models
{
    public class User
    {
        public const int MAX_LOGIN_LENGTH = 25;
        public const int MAX_USERNAME_LENGTH = 25;
        public const int MIN_PASSWORD_LENGTH = 5;

        public Guid Id { get; }
        public string Name { get; }
        public string Login { get; }
        public string Password { get; set; }
        public string Role { get; }

        public User()
        {
            Id = Guid.NewGuid();
            Name = "";
            Login = "";
            Password = "";
            Role = "";
        }
        private User(Guid Id, string Name, string Login, string Password, string role)
        {
            this.Id = Id;
            this.Name = Name;
            this.Password = Password;
            this.Login = Login;
            Role = role;
        }

        public static (bool isValid, string error) ValidateName(string Name)
        {
            if (string.IsNullOrEmpty(Name) || Name.Length > MAX_USERNAME_LENGTH)
            {
                return (false, $"Name can not be empty or longer than {MAX_USERNAME_LENGTH} symbols");
            }
            else return (true, "");
        }

        public static (bool isValid, string error) ValidatePassword(string Password)
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < MIN_PASSWORD_LENGTH)
            {
                return (false, $"Password can not be empty or shorter than {MIN_PASSWORD_LENGTH} symbols");
            }
            else return (true, "");
        }

        public static (bool isValid, string error) ValidateLogin(string Login)
        {
            if (string.IsNullOrEmpty(Login) || Login.Length > MAX_LOGIN_LENGTH)
            {
                return (false, $"Login can not be empty or longer than {MAX_LOGIN_LENGTH} symbols");
            }
            else return (true, "");
        }


        public static (User user, string error) Create(Guid Id, string Login, string Password, string Name, string Role)
        {
            var user = new User(Id, Name, Login, Password, Role);
            var (isValid, error) = ValidateLogin(Login);

            if (isValid)
            {
                return (user, error);
            }

            (isValid, error) = ValidatePassword(Password);

            if (!isValid)
            {
                return (user, error);
            }

            (isValid, error) = ValidateName(Name);

            if (!isValid)
            {
                return (user, error);
            }

            return (user, error); 
        }


    }
}

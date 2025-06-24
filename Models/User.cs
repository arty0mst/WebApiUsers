using System.Text.RegularExpressions;

namespace Models
{
    public class User
    {
        private User(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime? modifiedOn, string modifiedBy, DateTime? revokedOn, string revokedBy)
        {
            Guid = guid;
            Login = login;
            Password = password;
            Name = name;
            Gender = gender;
            Birthday = birthday;
            Admin = admin;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            ModifiedOn = modifiedOn;
            ModifiedBy = modifiedBy;
            RevokedOn = revokedOn;
            RevokedBy = revokedBy;

        }

        public Guid Guid { get; } // Id 
        public string Login { get; } // Логин  
        public string Password { get; } // Пароль 
        public string Name { get; } // Имя 
        public int Gender { get; } // Пол 
        public DateTime? Birthday { get; } // Дата рождения
        public bool Admin { get; } // Является ли пользователь админом
        public DateTime CreatedOn { get; } // Дата создания
        public string CreatedBy { get; } //  Логин, от имени которого этот пользователь создан
        public DateTime? ModifiedOn { get; } // Дата изменения 
        public string ModifiedBy { get; }  // Логин, от имени которого этот пользователь изменен
        public DateTime? RevokedOn { get; } // Дата удаления
        public string RevokedBy { get; }  // Логин, от имени которого этот пользователь удален

        public static (User User, string Error) Create(Guid guid, string login, string password, string name, int gender, DateTime? birthday, bool admin,
            DateTime createdOn, string createdBy, DateTime? modifiedOn, string modifiedBy, DateTime? revokedOn, string revokedBy)
        {
            string error = String.Empty;

            if (!IsValidAuth(login))
            {
                error += "Login must consist of latin letters and numbers; ";
            }
            if (!IsValidAuth(password))
            {
                error += "Password must consist of latin letters and numbers; ";
            }
            if (!IsValidName(name))
            {
                error += "Name must consist of latin and russian letters; ";
            }

            User user = new User(guid, login, password, name, gender, birthday, admin, createdOn, createdBy, modifiedOn, modifiedBy, revokedOn, revokedBy);

            return (user, error);
        }

        private static bool IsValidAuth(string userInput)
        {
            return Regex.IsMatch(userInput, @"^[a-zA-Z0-9]+$"); // Только латинские буквы и цифры
        }

        private static bool IsValidName(string userInput)
        {
            return Regex.IsMatch(userInput, @"^[a-zA-ZА-Яа-я]+$"); // Только латинские и русские буквы
        }
    }
}
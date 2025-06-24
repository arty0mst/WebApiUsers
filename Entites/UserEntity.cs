using System.ComponentModel.DataAnnotations;

namespace Entites
{
    public class UserEntity
    {
        public Guid Guid { get; set; } // Id 
        public string Login { get; set; } = string.Empty; // Логин  
        public string Password { get; set; } = string.Empty;// Пароль 
        public string Name { get; set; } = string.Empty;// Имя 
        public int Gender { get; set; } // Пол 
        public DateTime? Birthday { get; set; } // Дата рождения
        public bool Admin { get; set; } // Является ли пользователь админом
        public DateTime CreatedOn { get; set; } // Дата создания
        public string CreatedBy { get; set; } = string.Empty;//  Логин, от имени которого этот пользователь создан
        public DateTime ModifiedOn { get; set; } // Дата изменения 
        public string ModifiedBy { get; set; } = string.Empty; // Логин, от имени которого этот пользователь изменен
        public DateTime RevokedOn { get; set; } // Дата удаления
        public string RevokedBy { get; set; }  = string.Empty; // Логин, от имени которого этот пользователь удален
    }
}
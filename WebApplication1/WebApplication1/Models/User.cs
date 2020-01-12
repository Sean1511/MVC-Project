using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class User
    {
        [Required]
        [RegularExpression("^[A-Z]*[a-z]*$", ErrorMessage = "First Name must countain only letters")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression("^[A-Z]*[a-z]*$", ErrorMessage = "Last Name must countain only letters")]
        public string LastName { get; set; }
        [Key]
        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "ID must countain 9 digits exactly")]
        public string ID { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", ErrorMessage = "Invalid email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "User name must contain only letters and digits")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$", ErrorMessage = "Password must countain 8 letters(1 capital, 1 lower-case, 1 digit at least)")]
        public string Password { get; set; }
        [Required]
        [RegularExpression("^[1-3]{1}$", ErrorMessage = "1 - Faculty administrator, 2 - Lecturer, 3 - Student")]
        public int Permission { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using WebApi.Entities;

namespace WebApi.Models.Users
{
    public class UpdateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [EnumDataType(typeof(Role))]
        public string Role { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}

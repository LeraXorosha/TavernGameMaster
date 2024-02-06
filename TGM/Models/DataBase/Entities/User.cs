using System.ComponentModel.DataAnnotations;

namespace TGM.Models.DataBase.Entities
{
    public class User
    {
		public int Id { get; set; }
		public string Login { get; set; }
		public string? Email { get; set; }
		public string Password { get; set; }

		// Role
		public int RoleId { get; set; }
		public Role Role { get; set; }

		// UserProfile
		public UserProfile Profile { get; set; }
	}
}

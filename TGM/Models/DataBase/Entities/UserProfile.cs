namespace TGM.Models.DataBase.Entities
{
	public class UserProfile
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public string NickName { get; set; }
		public string Phone { get; set; }
        public string PlaceOfResidence { get; set; }
		public string TimeZone { get; set; }

        // User
        public int UserId { get; set; }
		public User User { get; set; }
	}
}

namespace TGM.Models.DataBase.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // User
        public List<User> Users { get; set; }
    }
}

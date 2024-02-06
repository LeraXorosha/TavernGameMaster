using Azure;
using System.ComponentModel.DataAnnotations;

namespace TGM.Models.DataBase.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EditorialBoard { get; set; }
        public string Description { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public decimal Price { get; set; }


        //GM
        public int UserId { get; set; }
        public User User { get; set; }

        // Tag
        public List<Tag> Tags { get; set; }
    }
}

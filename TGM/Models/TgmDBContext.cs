using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TGM.Models.DataBase;
using TGM.Models.DataBase.Entities;
using Bogus;

namespace TGM.Models
{
    public class TgmDBContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
		public DbSet<UserProfile> UserProfiles { get; set; }
		public DbSet<Game> Games { get; set; }


        public TgmDBContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.AddInitialData();
        }
    }
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder AddInitialData(this ModelBuilder modelBuilder)
        {
            var faker = new Faker("ru");
            faker.IndexFaker = 6;
            var fakerUser = new Faker<User>()
                .RuleFor(u => u.Id, faker.IndexFaker++)
                .RuleFor(u => u.Login, faker.Person.UserName)
                .RuleFor(u => u.Email, faker.Person.Email)
                .RuleFor(u => u.Password, faker.Random.Word().ToHash())
                .RuleFor(u => u.RoleId, 2);

            List<Role> roles = new(){
                    new Role { Id = 1, Name = "master" },
                    new Role { Id = 2, Name = "player" },
            };

            List<User> users = new(){
                    new User {Id = 1, Email = "subba@mail.ru", Login="GMRocki", Password = "1".ToHash(), RoleId = 1},
                    new User {Id = 2, Email = "lerlera@mail.ru", Login="Leroka", Password = "1".ToHash(), RoleId = 2}
                    
            };

            List <UserProfile> userProfiles = new(){
                new UserProfile {Id = 1, Name = "Субботин Иван Максимович", NickName = "Rocki", Phone = "8(908)353-32-94", PlaceOfResidence = "Новосибирск", TimeZone = "MSK+4", UserId = 1},
                new UserProfile {Id = 2, Name = "Хорошунова Валерия Сергеевна", NickName = "Quinee", Phone = "8(908)353-32-94" ,PlaceOfResidence = "Улан-Удэ", TimeZone = "MSK+5",UserId = 2}
            };

            List<Game> games = new()
            {
                new Game { Id = 1, Name = "Возрождение короля Лича!", EditorialBoard = "D&D", Description = "Действие дополнения происходит на холодном северном материке Нордскол, во владениях Короля-лича, падшего принца Артаса Менетила.",Date = new DateTime(2024-05-20), Price = 2000, UserId = 1},
                new Game { Id = 2, Name = "Мир туманности!", EditorialBoard = "RussionCorparation", Description = "Действие происходит в далеких туманных горах Алтая. Необычные приколючение в пещере горного короля",Date = new DateTime(1024-01-12), Price = 1800, UserId = 1}
            };

            List<Tag> tags = new()
        {
            new Tag { Id = 1, Name = "#top-of-the-world"},
            new Tag { Id = 2, Name = "#mmo"},
            new Tag { Id = 3, Name = "#csharp"},
            new Tag { Id = 4, Name = "#imloosingit"},
            new Tag { Id = 5, Name = "#unluck"},
            new Tag { Id = 6, Name = "#undead"},
        };

            for (var i = 7; i < 21; i++)
            {
                tags.Add(new Tag { Id = i, Name = faker.Lorem.Word() });
            }


            modelBuilder.Entity<Role>().HasData(roles);
            modelBuilder.Entity<User>().HasData(users);
			modelBuilder.Entity<UserProfile>().HasData(userProfiles);
            modelBuilder.Entity<Game>().HasData(games);

            return modelBuilder;
        }
    }
}



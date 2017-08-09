using System.Collections.Generic;
using System.Globalization;
using Abp;
using Abp.Dependency;
using Microsoft.AspNet.Identity;
using YoYoCms.AbpProjectTemplate.UserManagement.Users;

namespace YoYoCms.AbpProjectTemplate.MultiTenancy.Demo
{
    public class RandomUserGenerator : ITransientDependency
    {
        public static string[] Names = new[]
        {
            "Agatha Christie",
            "Albert Einstein",
            "Aldous Huxley",
            "Amin Maalouf",
            "Andrew Andrewus",
            "Arda Turan",
            "Audrey Naulin",
            "Biff Tannen",
            "Bruce Wayne",
            "Butch Coolidge",
            "Carl Sagan",
            "Charles Quint",
            "Christophe Grange",
            "Christopher Nolan",
            "Christopher Lloyd",
            "Clara Clayton",
            "Clarice Starling",
            "Dan Brown",
            "Daniel Radcliffe",
            "Douglas Hall",
            "David Wells",
            "Emmett Brown",
            "Friedrich Hegel",
            "Forrest Gump",
            "Franz Kafka",
            "Gabriel Marquez",
            "Galileo Galilei",
            "Georghe Hagi",
            "Georghe Orwell",
            "Georghe Richards",
            "Gottfried Leibniz",
            "Hannibal Lecter",
            "Hercules Poirot",
            "Isaac Asimov",
            "Jane Fuller",
            "Jean Reno",
            "Jeniffer Parker",
            "Johan Elmander",
            "Jules Winnfield",
            "Kurt Vonnegut",
            "Laurence Fishburne",
            "Leo Tolstoy",
            "Lorraine Baines",
            "Marsellus Wallace",
            "Marty Mcfly",
            "Michael Corleone",
            "Oktay Anar",
            "Omer Hayyam",
            "Paulho Coelho",
            "Quentin Tarantino",
            "Rene Descartes",
            "Robert Lafore",
            "Stanislaw Lem",
            "Stefan Zweig",
            "Stephenie Mayer",
            "Stephen Hawking",
            "Thomas More",
            "Vincent Vega",
            "Vladimir Nabokov",
            "William Faulkner"
        };

        public static string[] EmailProviders = new[]
        {
            "yahoo.com",
            "gmail.com",
            "hotmail.com",
            "outlook.com",
            "live.com",
            "yandex.com",
            "aspnetzero.com",
            "aspnetboilerplate.com"
        };

        public List<User> GetRandomUsers(int userCount, int tenantId)
        {
            var users = new List<User>();

            var randomNames = RandomHelper.GenerateRandomizedList(Names);
            for (var i = 0; i < userCount && i < randomNames.Count; i++)
            {
                users.Add(CreateUser(tenantId, randomNames[i]));
            }

            return users;
        }

        private static User CreateUser(int? tenantId, string nameSurname)
        {
            return new User
            {
                TenantId = tenantId,
                UserName = GenerateUsername(nameSurname),
                EmailAddress = GenerateEmail(nameSurname),
                Password = new PasswordHasher().HashPassword("123456"),
                Name = nameSurname.Split(' ')[0],
                Surname = nameSurname.Split(' ')[1],
                ShouldChangePasswordOnNextLogin = false,
                IsActive = (RandomHelper.GetRandom(0, 100) < 80), //A user will be active by 80% probability
                IsEmailConfirmed = true
            };
        }

        private static string GenerateUsername(string nameSurname)
        {
            return nameSurname.Replace(" ", ".").ToLower(CultureInfo.InvariantCulture);
        }

        private static string GenerateEmail(string nameSurname)
        {
            return GenerateUsername(nameSurname) + "@" + RandomHelper.GetRandomOf(EmailProviders);
        }
    }
}
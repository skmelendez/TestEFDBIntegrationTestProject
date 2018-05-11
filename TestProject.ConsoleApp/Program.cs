using System;
using System.Collections.Generic;
using TestProject.DataModel;

namespace TestProject.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            using (var context = new Entities())
            {
                var user = new User
                {
                    EmailAddress = $"bobdole_{Guid.NewGuid().ToString("N").ToUpper()}@mailinator.com",
                    UserDetails = new List<UserDetail>()
                    {
                        new UserDetail
                        {
                            FirstName = "Bob",
                            LastName = "Dole"
                        }
                    }
                };
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
    }
}

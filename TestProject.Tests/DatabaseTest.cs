using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using TestProject.DataModel;

namespace TestProject.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        protected static string DatabaseName = $"UnitTestDB_{Guid.NewGuid().ToString("N").ToUpper()}";
        protected static string DatabaseConnectionString = $@"Data Source=(localdb)\ProjectsV13; Integrated Security=True; database={DatabaseName}";
        protected static Entities Entity;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            Entity = new Entities(DatabaseConnectionString);
            Entity.Database.Create();
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Database.Delete(DatabaseConnectionString);
        }
    }

    [TestClass]
    public class UserInfoTests : DatabaseTests
    {
        private User _user;
        private readonly string _userEmail = $"johndoe_{Guid.NewGuid().ToString("N").ToUpper()}@mailinator.com";
        private const string UserFirstName = "John";
        private const string UserLastName = "Doe";

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        [TestMethod]
        public async Task TestAddUser()
        {
            await AddUser();

            var user = await GetUser(_userEmail);
            Assert.IsNotNull(user);
            var userDetails = user.UserDetails.FirstOrDefault();
            Assert.IsNotNull(userDetails);

            Assert.IsTrue(user.EmailAddress == _userEmail);
            Assert.IsTrue(userDetails.FirstName == UserFirstName);
            Assert.IsTrue(userDetails.LastName == UserLastName);
        }

        [TestMethod]
        public async Task TestDeleteUser()
        {
            await AddUser();
            var user = await GetUser(_userEmail);
            await DeleteUser(user);
            user = await GetUser(_userEmail);

            Assert.IsNull(user);
        }

        [TestMethod]
        public async Task TestUpdateUser()
        {
            await AddUser();
            var user = await GetUser(_userEmail);
            user.UserDetails.FirstOrDefault().FirstName = "Jonathan";
            Entity.Users.AddOrUpdate(user);
            await Entity.SaveChangesAsync();

            user = await GetUser(_userEmail);
            Assert.IsTrue(user.UserDetails.FirstOrDefault().FirstName == "Jonathan");
        }


        private static async Task<User> GetUser(string emailAddress)
        {
            return await Entity.Users.Where(x => x.EmailAddress == emailAddress).Include(x => x.UserDetails).SingleOrDefaultAsync();
        }

        private static async Task DeleteUser(User user)
        {
            foreach (var detail in user.UserDetails.ToList())
                Entity.UserDetails.Remove(detail);

            Entity.Users.Remove(user);

            await Entity.SaveChangesAsync();
        }


        private async Task AddUser()
        {
            _user = new User
            {
                EmailAddress = _userEmail,
                UserDetails = new List<UserDetail>
                {
                    new UserDetail
                    {
                        FirstName = UserFirstName,
                        LastName = UserLastName
                    }
                }
            };

            Entity.Users.AddOrUpdate(_user);

            await Entity.SaveChangesAsync();
        }


    }
}

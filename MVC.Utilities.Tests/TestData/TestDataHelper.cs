using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Utilities.Tests.TestData
{
    /// <summary>
    /// Static helper class for generating new pieces of test data
    /// </summary>
    public static class TestDataHelper
    {
        public static UserAccount CreateNewUser()
        {
            //Create a brand new user
            var user = new UserAccount()
            {
                DateJoined = DateTime.UtcNow,
                EmailAddress = string.Format("{0}@gmail.com", Guid.NewGuid().ToString()),
                Password = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                FullName = Guid.NewGuid().ToString()
            };

            return user;
        }

    }

    public class UserAccount
    {
        public DateTime DateJoined { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}

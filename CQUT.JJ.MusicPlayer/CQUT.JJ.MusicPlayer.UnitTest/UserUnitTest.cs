using System;
using CQUT.JJ.MusicPlayer.WCFService;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQUT.JJ.MusicPlayer.UnitTest
{
    [TestClass]
    public class UserUnitTest
    {
        private readonly IUserService _userService;

        public UserUnitTest()
        {
            _userService = new UserService();
        }

        [TestMethod]
        public void LoginSuccess()
        {
            var user = _userService.Login("1060522057", "123456");
            Assert.AreEqual(user.NickName, "贾建军");
        }

        [TestMethod]
        public void LoginError()
        {
            try
            {
                var user = _userService.Login("1060522057", "1234356");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}

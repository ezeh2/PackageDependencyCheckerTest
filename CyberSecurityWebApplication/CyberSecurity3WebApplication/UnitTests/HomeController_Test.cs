using CyberSecurity3WebApplication.Controllers;
using NUnit.Framework;

namespace CyberSecurity3WebApplication.UnitTests
{
    [TestFixture]
    public class HomeController_test
    {
        [Test]
        public void Test1()
        {
            HomeController homeController = new HomeController(null);

            homeController.Example10();

        }

        [Test]
        public void Test2()
        {

        }
    }
}

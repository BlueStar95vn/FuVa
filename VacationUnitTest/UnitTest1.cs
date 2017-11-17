using FuturifyVacation.Data;
using FuturifyVacation.Services;
using FuturifyVacation.ServicesInterfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;

namespace VacationUnitTest
{
    [TestClass]
    public class ProfileService_IsProfileExist
    {
        IProfileService _profileService;
        Mock<ApplicationDbContext> _dbcontextMock;

        public ProfileService_IsProfileExist()
        {
            
            _dbcontextMock = new Mock<ApplicationDbContext>();
            _profileService = new ProfileService(_dbcontextMock.Object);
        }

        [TestMethod]
        public async Task ReturnTrue_Profile()
        {
            // Arrange
            var userId = "84b88b16-7bc4-4fbe-998e-a4259cecf09f";

            // Act
            var result = await _profileService.GetByIdAsync(userId);

            // Assert
            Assert.IsTrue(result!=null && result.FirstName=="Texas");
        }
    }
   
}

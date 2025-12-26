using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using SyllabusManager.App.Models;
using SyllabusManager.App.Services;
using SyllabusManager.App.Interfaces;

namespace SyllabusManager.Tests
{
    public class ServiceTests
    {
        [Fact]
        public void AuthorizationService_HeadOfDepartment_ShouldCanCreateAnyCourse()
        {
            // Arrange
            var auth = new AuthorizationService();
            var head = new HeadOfDepartmentUser("1", "Head", "CE");

            // Act
            var canCreateCE = auth.CanCreateOrEdit(head, "CE101");
            var canCreateSE = auth.CanCreateOrEdit(head, "SE101");

            // Assert
            Assert.True(canCreateCE);
            Assert.True(canCreateSE);
        }

        [Fact]
        public void AuthorizationService_Instructor_ShouldOnlyCreateOwnDept()
        {
            // Arrange
            var auth = new AuthorizationService();
            var instr = new InstructorUser("2", "Instr", "CE");

            // Act
            var canCreateCE = auth.CanCreateOrEdit(instr, "CE101");
            var canCreateSE = auth.CanCreateOrEdit(instr, "SE101");

            // Assert
            Assert.True(canCreateCE);
            Assert.False(canCreateSE);
        }

        [Fact]
        public void UserManagementService_CreateUser_Admin_ShouldCreate()
        {
            // Arrange
            var mockRepo = new Mock<IDataRepository>();
            var usersList = new List<User>();
            mockRepo.Setup(r => r.Users).Returns(usersList);
            
            var service = new UserManagementService(mockRepo.Object);
            var admin = new AdminUser("99", "Admin", "ALL");

            // Act
            service.CreateUser(admin, "TestUser", "pass", "SE", "instructor");

            // Assert
            Assert.Single(usersList);
            Assert.IsType<InstructorUser>(usersList[0]);
            Assert.Equal("TestUser", usersList[0].Name);
        }

        [Fact]
        public void UserManagementService_CreateUser_NonAdmin_ShouldFail()
        {
            // Arrange
            var mockRepo = new Mock<IDataRepository>();
            var usersList = new List<User>();
            mockRepo.Setup(r => r.Users).Returns(usersList);

            var service = new UserManagementService(mockRepo.Object);
            var head = new HeadOfDepartmentUser("1", "Head", "CE");

            // Act
            service.CreateUser(head, "TestUser", "pass", "SE", "instructor");

            // Assert
            Assert.Empty(usersList);
        }

        [Fact]
        public void NotificationService_Subscribe_ShouldAddSubscription()
        {
            // Arrange
            var mockRepo = new Mock<IDataRepository>();
            var subsList = new List<Subscription>();
            mockRepo.Setup(r => r.Subscriptions).Returns(subsList);
            mockRepo.Setup(r => r.SaveSubscriptions()).Verifiable();

            var service = new NotificationService(mockRepo.Object);

            // Act
            service.Subscribe("user1", "CE");

            // Assert
            Assert.Single(subsList);
            Assert.Equal("user1", subsList[0].ObserverId);
            Assert.Equal("CE", subsList[0].CoursePattern);
            mockRepo.Verify(r => r.SaveSubscriptions(), Times.Once);
        }
    }
}

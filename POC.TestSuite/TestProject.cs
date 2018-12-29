using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http.Results;
using Rhino.Mocks;
using POC.ViewModels;
using POC.Controllers;
using POC.Repository.Interface;
using System.Web.Http;
using System.Net;

namespace POC.TestSuite
{

    /// <summary>
    ///This is a sample Test class for Projects
    ///https://docs.microsoft.com/en-us/aspnet/web-api/overview/testing-and-debugging/unit-testing-with-aspnet-web-api
    ///https://wrightfully.com/using-rhino-mocks-quick-guide-to-generating-mocks-and-stubs
    /// </summary>
    [TestClass]
    public class TestProjectController
    {
        private IProjectRepository _IProjectRepository;
        private ILogger _logger;
        public TestProjectController()
        {
            _logger = MockRepository.GenerateMock<ILogger>();
            _IProjectRepository = MockRepository.GenerateMock<IProjectRepository>();
        }
        [TestMethod]
        public void GetAllProjects_ShouldReturnAllProjects()
        {
            //Arrange
            var _fakeProjects = GetMockProjects();
            _IProjectRepository.Expect(s => s.GetAll("ProjectName", "ASC", ""))
                               .IgnoreArguments()
                               .Return(_fakeProjects);
            ProjectController controller = new ProjectController(_IProjectRepository, _logger);
            //Act           
            var actionResult = controller.GetAll();
            var contentResult = actionResult as OkNegotiatedContentResult<List<ProjectMasterViewModel>>;
            var expected = (contentResult.Content) as List<ProjectMasterViewModel>;
            //Assert
            Assert.IsNotNull(expected);
            Assert.AreEqual(expected.ToList().Count, 4);
        }

        [TestMethod]
        public void GetProject_ShouldReturnCorrectProject()
        {
            //Arrange          
            var _project =new ProjectMasterViewModel(){ ProjectID = 1,ProjectCode = "A001",ProjectName = "ABC Bearings Ltd",IsActive = true,IsDeleted = false,Status = "Active"};
            _IProjectRepository.Expect(s => s.GetById(1))
                               .IgnoreArguments()
                               .Return(_project);
            ProjectController controller = new ProjectController(_IProjectRepository, _logger);
            //Act        
            var result = controller.GetById(2);
            var contentResult = result as OkNegotiatedContentResult<ProjectMasterViewModel>;
            var expected = (contentResult.Content) as ProjectMasterViewModel;
            //Assert
            Assert.IsNotNull(result);
            var projects = GetMockProjects();
            Assert.AreEqual(projects.ToList()[0].ProjectName, expected.ProjectName);
        }

        [TestMethod]
        public void GetProject_ShouldNotFindProject()
        {
            //Arrange           
            ProjectController controller = new ProjectController(_IProjectRepository, _logger);
            //Act    
            var result = controller.GetById(0);
           
            //Assert          
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
       
        private IQueryable<ProjectMasterViewModel> GetMockProjects()
        {
            var Projects = new List<ProjectMasterViewModel>()
            {
                new ProjectMasterViewModel(){ ProjectID=1,ProjectCode="A001",ProjectName="ABC Bearings Ltd",IsActive=true,IsDeleted=false,Status="Active"},
                new ProjectMasterViewModel(){ ProjectID=2,ProjectCode="A002",ProjectName="Alok Industries Ltd",IsActive=true,IsDeleted=false,Status="Active"},
                new ProjectMasterViewModel(){ ProjectID=3,ProjectCode="A003",ProjectName="Ambuja Cement Ltd",IsActive=true,IsDeleted=false,Status="Active"},
                new ProjectMasterViewModel(){ ProjectID=4,ProjectCode="A004",ProjectName="Anil Bioplus Ltd (Anil Group Ahmedabad)",IsActive=true,IsDeleted=false,Status="Active"}
            };
            return Projects.AsQueryable();
        }
    }
}

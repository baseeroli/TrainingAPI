using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Moq;
using System.Linq;
using TrainingAPI.Models;
using TrainingAPI.Controllers;
using System.Data.Entity;
using System.Web.Http;
using System.Data.Entity.Core.Objects;
using System.Web.Http.Results;

namespace TrainingAPI.Tests
{
    [TestClass]
    public class TrainingControllerTests
    {

        List<TrainingInfo> data;
        Mock<TrainingEntities> mockContext;

        [TestInitialize]
        public void MockSetup()
        {
            //var data = TrainingList().AsQueryable();
            data = new List<TrainingInfo>
            {

                    new TrainingInfo{ TId= 1, TrainingName="Training 1", StartDate= new DateTime(), EndDate=new DateTime().AddDays(2) },
                    new TrainingInfo{ TId= 2, TrainingName="Training 2", StartDate= new DateTime().AddDays(3), EndDate=new DateTime().AddDays(5) },
                    new TrainingInfo{ TId=3, TrainingName="Training 3", StartDate= new DateTime().AddDays(6), EndDate=new DateTime().AddDays(8) }

            };

            var mockData = data.AsQueryable();

            //Define the mock type as DbSet<Location>
            var mockSet = new Mock<DbSet<TrainingInfo>>();
            //mockSet.SetupData(data);
            //Define the mock Repository as databaseEf
            mockContext = new Mock<TrainingEntities>();

            //Setting up the mockSet to mockContext
            mockContext.Setup(c => c.TrainingInfoes).Returns(() => mockSet.Object);
            //Bind all data  attributes to your mockSe

            mockSet.As<IQueryable<TrainingInfo>>().Setup(m => m.Provider).Returns(mockData.Provider);
            mockSet.As<IQueryable<TrainingInfo>>().Setup(m => m.Expression).Returns(mockData.Expression);
            mockSet.As<IQueryable<TrainingInfo>>().Setup(m => m.ElementType).Returns(mockData.ElementType);
            mockSet.As<IQueryable<TrainingInfo>>().Setup(m => m.GetEnumerator()).Returns(mockData.GetEnumerator());
            //mockSet.Setup().mockData

            //mockContext.Setup(c => c.TrainingInfoes).Returns( () => mockSet.Object);
            

            //return mockContext;
        }

        [TestMethod]
        public void GetTrainingById()
        {
            //Init the WebAPI service
            var controller = new TrainingController(mockContext.Object);
            //Check the equality between the returned data and the expected data
           var result = controller.GetTrainingInfoById(data.First().TId.ToString()) as OkNegotiatedContentResult<TrainingInfo>;
         
            Assert.IsNotNull(result);
            
            Assert.AreEqual("Training 1", result.Content.TrainingName);

         }

        [TestMethod]
        public void GetAllTrainings()
        {            
            //Init the WebAPI service
            var controller = new TrainingController(mockContext.Object);
            //Check the equality between the returned data and the expected data
            var result = controller.GetTrainingInfo(); //as List<TrainingInfo>;

            Assert.IsNotNull(result);

            Assert.AreEqual(data.Count(), ((System.Linq.EnumerableQuery<TrainingAPI.Models.TrainingInfo>)result).Count());

        }

        [TestMethod]
        public void CreateNewTraining()
        {

            var maxTrainingIDBeforeAdd = data.Max(t => t.TId);
            //Init the WebAPI service
            var controller = new TrainingController(mockContext.Object);
            //Check the equality between the returned data and the expected data
            var result = controller.PostTrainingInfo(new TrainingInfo { TId = maxTrainingIDBeforeAdd + 1, TrainingName = "Training "+ (maxTrainingIDBeforeAdd + 1), StartDate = new DateTime().AddDays(9), EndDate = new DateTime().AddDays(12) }) as OkNegotiatedContentResult<TrainingInfo>;

            Assert.IsNotNull(result);

            Assert.AreEqual(maxTrainingIDBeforeAdd+1, result.Content.TId);

        }

        public List<TrainingInfo> TrainingList()
        {
            //Define the mock data
            return new List<TrainingInfo>
            {

                    new TrainingInfo{ TId= 1, TrainingName="Training 1", StartDate= new DateTime(), EndDate=new DateTime().AddDays(2) },
                    new TrainingInfo{ TId= 2, TrainingName="Training 2", StartDate= new DateTime().AddDays(3), EndDate=new DateTime().AddDays(5) },
                    new TrainingInfo{ TId=3, TrainingName="Training 3", StartDate= new DateTime().AddDays(6), EndDate=new DateTime().AddDays(8) }

            };
        }

        
    }
}

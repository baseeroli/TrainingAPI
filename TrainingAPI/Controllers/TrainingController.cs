using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TrainingAPI.Models;

namespace TrainingAPI.Controllers
{
    [RoutePrefix("Api/Training")]
    public class TrainingController : ApiController
    {

        // TrainingEntities objEntity = new TrainingEntities();

        public TrainingEntities objEntity { get; set; }

        ///
        /// Default constructor
        ///
        public TrainingController()
        {
            this.objEntity = new TrainingEntities();
        }

        ///
        /// Constructor used by Moq
        ///
        /// Repository 
        public TrainingController(TrainingEntities mockDB)
        {
            this.objEntity = mockDB;
        }

        [HttpGet]
        [Route("AllTrainings")]
        public IQueryable<TrainingInfo> GetTrainingInfo()
        {
            List<TrainingInfo> objTraining = new List<TrainingInfo>();
            try
            {
                objTraining = objEntity.TrainingInfoes.ToList();
                return objTraining.AsQueryable();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet]
        [Route("GetTrainingsInfoById/{TrainingId}")]
        public IHttpActionResult GetTrainingInfoById(string trainingId)
        {
            TrainingInfo objTraining = new TrainingInfo();
            int ID = Convert.ToInt32(trainingId);
            try
            {

                objTraining= objEntity.TrainingInfoes.Where(t => t.TId == ID).ToList().FirstOrDefault();
               // objTraining = objEntity.TrainingInfoes.Find(ID);
                if (objTraining == null)
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return Ok(objTraining);
        }

        [HttpPost]
        [Route("InsertTrainingInfo")]
        public IHttpActionResult PostTrainingInfo(TrainingInfo data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                objEntity.TrainingInfoes.Add(data);
                objEntity.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }



            return Ok(data);
        }

        [HttpPut]
        [Route("TrainingInfo")]
        public IHttpActionResult PutTrainingInfo(TrainingInfo training)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TrainingInfo objTraining = new TrainingInfo();
                objTraining = objEntity.TrainingInfoes.Find(training.TId);
                if (objTraining != null)
                {
                    objTraining.TrainingName = training.TrainingName;
                    objTraining.StartDate = training.StartDate;
                    objTraining.EndDate = training.EndDate;
                }
                int i = this.objEntity.SaveChanges();

            }
            catch (Exception)
            {
                throw;
            }
            return Ok(training);
        }
        [HttpDelete]
        [Route("DeleteTrainingInfo")]
        public IHttpActionResult DeleteTrainingInfo(int id)
        {
            //int empId = Convert.ToInt32(id);  
            TrainingInfo training = objEntity.TrainingInfoes.Find(id);
            if (training == null)
            {
                return NotFound();
            }

            objEntity.TrainingInfoes.Remove(training);
            objEntity.SaveChanges();

            return Ok(training);
        }
    }
}

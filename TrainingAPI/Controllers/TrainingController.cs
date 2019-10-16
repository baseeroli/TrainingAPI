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
        
        TrainingEntities objEntity = new TrainingEntities();
        
        [HttpGet]
        [Route("AllTrainings")]
        public IQueryable<TrainingInfo> GetTrainingInfo()
        {
            try
            {
                return objEntity.TrainingInfoes;
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
            TrainingInfo objEmp = new TrainingInfo();
            int ID = Convert.ToInt32(trainingId);
            try
            {
                objEmp = objEntity.TrainingInfoes.Find(ID);
                if (objEmp == null)
                {
                    return NotFound();
                }

            }
            catch (Exception)
            {
                throw;
            }

            return Ok(objEmp);
        }

        [HttpPost]
        [Route("InsertTrainingInfo")]
        public IHttpActionResult PostEmaployee(TrainingInfo data)
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
                TrainingInfo objEmp = new TrainingInfo();
                objEmp = objEntity.TrainingInfoes.Find(training.TId);
                if (objEmp != null)
                {
                    objEmp.TrainingName = training.TrainingName;
                    objEmp.StartDate = training.StartDate;
                    objEmp.EndDate = training.EndDate;
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

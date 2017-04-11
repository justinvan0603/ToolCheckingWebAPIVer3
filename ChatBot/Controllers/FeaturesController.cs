using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Text;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Features")]
    public class FeaturesController : Controller
    {
        [HttpPost]
        public async Task<int> Post([FromBody]Features feature)
        {
            DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            feature.CreateDt = DateTime.Now;
            feature.MakerId = "thieu1234";
            feature.RecordStatus = "1";
            feature.AuthStatus = "U";
            
            var command = $"dbo.Features_Ins @p_FEA_TYPE = '{feature.FeaType}' ,@p_CONTENTS = '{feature.Contents}',  @p_LEVEL ={feature.Level}, @p_RESOURCE = '{feature.Resource}', @p_RECORD_STATUS='{feature.RecordStatus}', @p_AUTH_STATUS = '{feature.AuthStatus}', @p_APPROVE_DT = '{feature.ApproveDt.Value.Date}', @p_EDIT_DT = '{feature.EditDt.Value.Date}', @p_CHECKER_ID = '{feature.CheckerId}', @p_EDITOR_ID = '{feature.EditorId}', @p_CREATE_DT='{feature.CreateDt.Value.Date}', @p_MAKER_ID = '{feature.MakerId}'";
            var result =  await context.Database.ExecuteSqlCommandAsync(command);

            return result;
        }
    }
}
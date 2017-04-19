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
using ChatBot.Infrastructure.Core;
using Microsoft.AspNetCore.Authorization;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Features")]
    public class FeaturesController : Controller
    {
        private readonly DEFACEWEBSITEContext _context;
        public FeaturesController(DEFACEWEBSITEContext context)
        {
            this._context = context;
        }
        [HttpPost]
        [Authorize(Roles = "AddFeature")]
        public async Task<ObjectResult> Post([FromBody]Features feature)
        {
            GenericResult rs = new GenericResult();
            //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
            try
            {
                
                feature.CreateDt = DateTime.Now;
                feature.MakerId = "thieu1234";
                feature.RecordStatus = "1";
                feature.AuthStatus = "U";

                var command = $"dbo.Features_Ins @p_FEA_TYPE = '{feature.FeaType}' ,@p_CONTENTS = '{feature.Contents}',  @p_LEVEL ={feature.Level}, @p_RESOURCE = '{feature.Resource}', @p_RECORD_STATUS='{feature.RecordStatus}', @p_AUTH_STATUS = '{feature.AuthStatus}', @p_APPROVE_DT = '{feature.ApproveDt.Value.Date}', @p_EDIT_DT = '{feature.EditDt.Value.Date}', @p_CHECKER_ID = '{feature.CheckerId}', @p_EDITOR_ID = '{feature.EditorId}', @p_CREATE_DT='{feature.CreateDt.Value.Date}', @p_MAKER_ID = '{feature.MakerId}'";
                var result = await _context.Database.ExecuteSqlCommandAsync(command);
                rs.Message = "Thêm feedback thành công";
                rs.Succeeded = true;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
            catch(Exception ex)
            {
                rs.Message = "Lỗi " + ex.Message;
                rs.Succeeded = false;
                ObjectResult objRes = new ObjectResult(rs);
                //context.Dispose();
                return objRes;
            }
        }
    }
}
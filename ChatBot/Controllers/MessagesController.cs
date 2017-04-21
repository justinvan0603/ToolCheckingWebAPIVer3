using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using DefaceWebsiteService;
using ChatBot.Models;
using Microsoft.EntityFrameworkCore;
using ChatBot.Infrastructure.Core;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ChatBot.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
  //  [Authorize]
    public class MessagesController : Controller, IDisposable
    {
        // int _page = 1;
        // int _pageSize = 10;
        // [Authorize(Roles = "Admin")]
        // public async Task<IEnumerable<Messages>> Get()
        // {
        //     Request.get
        //var x = User;
        //     bool maGiangVien = User.Identity.IsAuthenticated;
        //     var pagination = Request.Headers["Pagination"];

        //     if (!string.IsNullOrEmpty(pagination))
        //     {
        //         string[] vals = pagination.ToString().Split(',');
        //         int.TryParse(vals[0], out _page);
        //         int.TryParse(vals[1], out _pageSize);
        //     }

        //     DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
        //     var result = await context.Messages.FromSql("dbo.Messages_Search @USER = {0}, @DOMAIN = {1} , @STATUS = {2}", "thieu1234", null, null).ToArrayAsync();

        //     int currentPage = _page;
        //     int currentPageSize = _pageSize;

        //     var totalRecord = result.Count();
        //     var totalPages = (int)Math.Ceiling((double)totalRecord / _pageSize);

        //     var messages = result.Skip((currentPage - 1) * currentPageSize).Take(currentPageSize);
        //     Response.AddPagination(_page, _pageSize, totalRecord, totalPages);
        //     IEnumerable<Messages> listPagedMessage = Mapper.Map<IEnumerable<Messages>, IEnumerable<Messages>>(messages);
        //     return listPagedMessage;
        //     MessagesClient client = new MessagesClient();
        //     return await client.Messages_SearchAsync("thieu1234", "", "");

        // }
        //private readonly DEFACEWEBSITEContext _context;
        //public MessagesController(DEFACEWEBSITEContext context)
        //{
        //    _context = context
        //}
        //int _page = 1;
        //int _pageSize = 10;
        private readonly DEFACEWEBSITEContext _context;
        public MessagesController(DEFACEWEBSITEContext context)
        {
            this._context = context;
        }
        [HttpGet("{page:int=0}/{pageSize=12}/{username}/{searchstring=}")]
        [Authorize(Roles = "ViewMessage")]
        public async Task<IActionResult> Get(int? page, int? pageSize,string username, string searchstring = null)
        {

            //var file = Request.;
            //using (DEFACEWEBSITEContext context = new DEFACEWEBSITEContext())
            //{
                PaginationSet<Messages> pagedSet = new PaginationSet<Messages>();
                //   var pagination = Request.Headers;

                //if (!string.IsNullOrEmpty(pagination))
                //{
                //    string[] vals = pagination.ToString().Split(',');
                //    int.TryParse(vals[0], out _page);
                //    int.TryParse(vals[1], out _pageSize);
                //}
                //if (await _authorizationService.AuthorizeAsync(User, "AdminOnly"))
                //{
                //DEFACEWEBSITEContext context = new DEFACEWEBSITEContext();
                var result =
                     _context.Messages.FromSql("dbo.Messages_Search @USER = {0}, @DOMAIN = {1} , @STATUS = {2}",
                        username, null, null);

                int currentPage = page.Value;
                int currentPageSize = pageSize.Value;
                if (!String.IsNullOrEmpty(searchstring))
                {
                    result = result.Where(msg => msg.Domain.Contains(searchstring) ||
                                            msg.User.Contains(searchstring) ||
                                            msg.Title.ToLower().Contains(searchstring.ToLower()));
                }

                var totalRecord = result.Count();
                var totalPages = (int)Math.Ceiling((double)totalRecord / pageSize.Value);

                var messages = result.Skip((currentPage) * currentPageSize).Take(currentPageSize).ToList();
                Response.AddPagination(currentPage, currentPageSize, totalRecord, totalPages);
                //IEnumerable<Messages> listPagedMessage =
                    //Mapper.Map<IEnumerable<Messages>, IEnumerable<Messages>>(messages);
                //messages.ToList().Clear();
                //result.Clear();
                pagedSet = new PaginationSet<Messages>()
                {
                    Page = currentPage,
                    TotalCount = totalRecord,
                    TotalPages = totalPages,
                    Items = messages
                };
            //    for(int i = 0; i< messages.Count; i++)
            //{
            //    if(i% 6 == 0)
            //    {
            //        //create row
            //        //dislay array

            //    }
            //    else
            //    {
            //        for(int j = 0; j < messages.Count; j++)
            //        {
                        
            //        }
            //    }
                
            //}
                //context.Dispose();
                return new ObjectResult(pagedSet);
            }
            //   }
            //CodeResultStatus _codeResult = new CodeResultStatus(401);
            //return new ObjectResult(_codeResult);
       // }

    }
}
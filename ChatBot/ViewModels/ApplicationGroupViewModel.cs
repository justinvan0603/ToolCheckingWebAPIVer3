using System.Collections.Generic;
using ChatBot.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.ViewModels
{
    public class ApplicationGroupViewModel : BaseViewModel<ApplicationGroup>
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public IEnumerable<ApplicationRoleViewModel> Roles { set; get; }

        public bool Check{ set; get; }
    }
}
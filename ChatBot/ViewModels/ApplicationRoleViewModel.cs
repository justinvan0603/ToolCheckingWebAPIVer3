using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.ViewModels
{
    public class ApplicationRoleViewModel:BaseViewModel<IdentityRole>
    {
        public string Id { set; get; }
        public string Name { set; get; }
        //   public string Description { set; get; }
        public bool Check { set; get; }
    }
}
using System.Linq;
using AutoMapper;
using ChatBot.Model.Models;
using ChatBot.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ChatBot.Infrastructure.Mappings
{

    //public class VIPResolver : ValueResolver<int, bool>
    //{
    //    protected override bool ResolveCore(int source)
    //    {
    //        if (source == 1)
    //            return true;
    //        return false;
    //    }
    //}

    //public class VIPResolver2 : ValueResolver<bool, int>
    //{
    //    protected override int ResolveCore(bool source)
    //    {
    //        if (source)
    //            return 1;
    //        return 0;
    //    }
    //}

    public class DomainToViewModelMappingProfile : Profile
    {
        

        protected override void Configure()
        {
            //  Mapper.CreateMap<BOT_DOMAIN, DomainViewModel>();
            //  .ForMember(cv => cv.RECORD_STATUS, m => m.ResolveUsing<VIPResolver>().FromMember(x => x.RECORD_STATUS));


            //   Mapper.CreateMap<DomainViewModel, BOT_DOMAIN>();
            //   .ForMember(cv => cv.RECORD_STATUS, m => m.ResolveUsing<VIPResolver2>().FromMember(x => x.RECORD_STATUS));

            //    .ForMember(vm => vm.RECORD_STATUS, map => map.MapFrom(p => "/images/" + p.Uri));


            //Mapper.CreateMap<Photo, PhotoViewModel>()
            //   .ForMember(vm => vm.Uri, map => map.MapFrom(p => "/images/" + p.Uri));

            //Mapper.CreateMap<Album, AlbumViewModel>()
            //    .ForMember(vm => vm.TotalPhotos, map => map.MapFrom(a => a.Photos.Count))
            //    .ForMember(vm => vm.Thumbnail, map => 
            //        map.MapFrom(a => (a.Photos != null && a.Photos.Count > 0) ?
            //        "/images/" + a.Photos.First().Uri :
            //        "/images/thumbnail-default.png"));


            Mapper.CreateMap<ApplicationUser, ApplicationUserViewModel>();
            Mapper.CreateMap<ApplicationUserViewModel, ApplicationUser>();
            Mapper.CreateMap<ApplicationGroupViewModel, ApplicationGroup>();
            Mapper.CreateMap<ApplicationGroup, ApplicationGroupViewModel>();

            Mapper.CreateMap<ApplicationRoleViewModel, IdentityRole>();
            Mapper.CreateMap<IdentityRole, ApplicationRoleViewModel>();
        }
    }
}

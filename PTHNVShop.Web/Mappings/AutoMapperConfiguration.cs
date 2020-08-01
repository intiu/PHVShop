using AutoMapper;
using PTHNVShop.Model.Models;
using PTHNVShop.Web.Models;

namespace PTHNVShop.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Post, PostViewModel>();
                cfg.CreateMap<PostCategory, PostCategoryViewModel>();
                cfg.CreateMap<Tag, TagViewModel>();

                cfg.CreateMap<ProductCategory, ProductCategoryViewModel>();
                cfg.CreateMap<Product, ProductViewModel>();
                cfg.CreateMap<ProductTag, ProductTagViewModel>();
                cfg.CreateMap<Footer, FooterViewModel>();
                cfg.CreateMap<Slide, SlideViewModel>();
                cfg.CreateMap<ContactDetail, ContactDetailViewModel>();
            });          
        }
    }
}
﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PTHNVShop.Model.Models;
using PTHNVShop.Service;
using PTHNVShop.Web.Infrastructure.Core;
using PTHNVShop.Web.Models;
using PTHNVShop.Web.Infrastructure.Extensions;
using System.Web.Script.Serialization;

namespace PTHNVShop.Web.Api
{
    [RoutePrefix("api/productcategory")]
    //[Authorize]
    public class ProductCategoryController : ApiControllerBase
    {
        #region Initialize
        private IProductCategoryService _productCategoryService;

        public ProductCategoryController(IErrorService errorService, IProductCategoryService productCategoryService)
            : base(errorService)
        {
            this._productCategoryService = productCategoryService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetAll();

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
                });
                var mapper = new Mapper(config);
                var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(model);

                var responseData = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                return response;
            });
        }
        [Route("getbyid/{id:int}")]
        [HttpGet]
        public HttpResponseMessage GetById(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productCategoryService.GetById(id);

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
                });
                var mapper = new Mapper(config);
                var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(model);
                var responseData = mapper.Map<ProductCategory, ProductCategoryViewModel>(model);

                var response = request.CreateResponse(HttpStatusCode.OK, responseData);

                return response;
            });
        }

        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request, () =>
            {
                int totalRow = 0;
                var model = _productCategoryService.GetAll(keyword);

                totalRow = model.Count();
                var query = model.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

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
                });
                var mapper = new Mapper(config);
                var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(query);
                var responseData = mapper.Map<IEnumerable<ProductCategory>, IEnumerable<ProductCategoryViewModel>>(query);

                var paginationSet = new PaginationSet<ProductCategoryViewModel>()
                {
                    Items = responseData,
                    Page = page,
                    TotalCount = totalRow,
                    TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                };
                var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                return response;
            });
        }


        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public HttpResponseMessage Create(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newProductCategory = new ProductCategory();
                    newProductCategory.UpdateProductCategory(productCategoryVm);
                    newProductCategory.CreatedDate = DateTime.Now;
                    _productCategoryService.Add(newProductCategory);
                    _productCategoryService.Save();

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
                    });
                    var mapper = new Mapper(config);
                    var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(newProductCategory);
                    var responseData = mapper.Map<ProductCategory, ProductCategoryViewModel>(newProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductCategoryViewModel productCategoryVm)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbProductCategory = _productCategoryService.GetById(productCategoryVm.ID);

                    dbProductCategory.UpdateProductCategory(productCategoryVm);
                    dbProductCategory.UpdatedDate = DateTime.Now;

                    _productCategoryService.Update(dbProductCategory);
                    _productCategoryService.Save();

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
                    });
                    var mapper = new Mapper(config);
                    var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(dbProductCategory);
                    var responseData = mapper.Map<ProductCategory, ProductCategoryViewModel>(dbProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var oldProductCategory = _productCategoryService.Delete(id);
                    _productCategoryService.Save();

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
                    });
                    var mapper = new Mapper(config);
                    var listPostCategoryVm = mapper.Map<List<PostCategoryViewModel>>(oldProductCategory);
                    var responseData = mapper.Map<ProductCategory, ProductCategoryViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProductCategories)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProductCategories);
                    foreach (var item in listProductCategory)
                    {
                        _productCategoryService.Delete(item);
                    }

                    _productCategoryService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
    }
}
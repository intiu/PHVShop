﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using PTHNVShop.Model.Models;
using PTHNVShop.Service;
using PTHNVShop.Web.Infrastructure.Core;
using PTHNVShop.Web.Infrastructure.Extensions;
using PTHNVShop.Web.Models;

namespace PTHNVShop.Web.Api
{
    [RoutePrefix("api/product")]
    //[Authorize]
    public class ProductController : ApiControllerBase
    {
        #region Initialize
        private IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService)
            : base(errorService)
        {
            this._productService = productService;
        }

        #endregion

        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAll(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var model = _productService.GetAll();

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
                var responseData = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(model);

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
                var model = _productService.GetById(id);

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
                var responseData = mapper.Map<Product, ProductViewModel>(model);

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
                var model = _productService.GetAll(keyword);

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
                var responseData = mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                var paginationSet = new PaginationSet<ProductViewModel>()
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
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel productCategoryVm)
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
                    var newProduct = new Product();
                    newProduct.UpdateProduct(productCategoryVm);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newProduct);
                    _productService.Save();

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
                    var responseData = mapper.Map<Product, ProductViewModel>(newProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }

        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel productVm)
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
                    var dbProduct = _productService.GetById(productVm.ID);

                    dbProduct.UpdateProduct(productVm);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.UpdatedBy = User.Identity.Name;
                    _productService.Update(dbProduct);
                    _productService.Save();

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
                    var responseData = mapper.Map<Product, ProductViewModel>(dbProduct);
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
                    var oldProductCategory = _productService.Delete(id);
                    _productService.Save();

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
                    var responseData = mapper.Map<Product, ProductViewModel>(oldProductCategory);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }

                return response;
            });
        }
        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
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
                    var listProductCategory = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listProductCategory)
                    {
                        _productService.Delete(item);
                    }

                    _productService.Save();

                    response = request.CreateResponse(HttpStatusCode.OK, listProductCategory.Count);
                }

                return response;
            });
        }
    }
}




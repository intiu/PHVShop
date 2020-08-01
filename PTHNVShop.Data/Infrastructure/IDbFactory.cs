using System;

namespace PTHNVShop.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        PTHNVShopDbContext Init();
    }
}
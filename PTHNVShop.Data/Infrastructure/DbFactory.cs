namespace PTHNVShop.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private PTHNVShopDbContext dbContext;

        public PTHNVShopDbContext Init()
        {
            return dbContext ?? (dbContext = new PTHNVShopDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
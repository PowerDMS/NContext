namespace NContext.Extensions.EntityFramework
{
    using System;
    using System.Data.Entity;

    public interface IDbContextFactory
    {
        DbContext Create();

        DbContext Create(String key);

        TDbContext Create<TDbContext>() where TDbContext : DbContext;
    }
}
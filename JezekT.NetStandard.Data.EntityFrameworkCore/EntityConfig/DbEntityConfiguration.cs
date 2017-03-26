using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JezekT.NetStandard.Data.EntityFrameworkCore.EntityConfig
{
    public abstract class DbEntityConfiguration<TEntity> where TEntity : class
    {
        public abstract void Configure(EntityTypeBuilder<TEntity> entity);
    }
}

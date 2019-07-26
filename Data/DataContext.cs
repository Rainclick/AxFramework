using System.Threading;
using System.Threading.Tasks;
using Common.Utilities;
using Entities.Framework;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();
        }

        public override int SaveChanges()
        {
            //todo Amir: Log Changes to DB
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}

using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace MFAE.Jobs.EntityFrameworkCore
{
    public static class JobsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<JobsDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<JobsDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
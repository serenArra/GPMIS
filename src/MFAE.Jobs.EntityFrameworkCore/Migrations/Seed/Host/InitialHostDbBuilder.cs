using MFAE.Jobs.EntityFrameworkCore;

namespace MFAE.Jobs.Migrations.Seed.Host
{
    public class InitialHostDbBuilder
    {
        private readonly JobsDbContext _context;

        public InitialHostDbBuilder(JobsDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            new DefaultEditionCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();

            _context.SaveChanges();
        }
    }
}

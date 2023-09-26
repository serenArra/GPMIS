using CsvHelper;
using MFAE.Jobs.EntityFrameworkCore;
using MFAE.Jobs.Location;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MFAE.Jobs.Migrations.Seed.CsvSeed
{
    public class GovernoratesBuilder
    {
        private readonly JobsDbContext _context;
        public GovernoratesBuilder(JobsDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            SeedFromCsv();
        }

        private void SeedFromCsv()
        {
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Governorates ON");
            _context.SaveChanges();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MFAE.Jobs.Migrations.Seed.CsvSeed.CSVFiles.Governorates.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, new CultureInfo("en"));
                    var drivers = new List<Governorate>() { };
                    csvReader.Read();
                    csvReader.ReadHeader();
                    var palestineId = _context.Countries.Where(e => e.NameAr.Contains("فلسطين")).Select(e => e.Id).FirstOrDefault();
                    while (csvReader.Read())
                    {

                        if (_context.Governorates.FirstOrDefault(c => c.Id == int.Parse(csvReader.GetField("Id"))) == null)
                            _context.Governorates.Add(new Governorate
                            {
                                Id = int.Parse(csvReader.GetField("Id")),
                                NameAr = csvReader.GetField("NameAr"),
                                NameEn = csvReader.GetField("NameEn"),
                                UniversalCode = csvReader.GetField("Id"),
                                CountryId = palestineId
                            });
                    }
                    _context.SaveChanges();



                }
            }

            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Governorates OFF");
            _context.SaveChanges();
        }
    }
}

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
    public class LocalitiesBuilder
    {
        private readonly JobsDbContext _context;
        public LocalitiesBuilder(JobsDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            SeedFromCsv();
        }

        private void SeedFromCsv()
        {
            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Localities ON");
            _context.SaveChanges();
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MFAE.Jobs.Migrations.Seed.CsvSeed.CSVFiles.Localities.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, new CultureInfo("en"));
                    var drivers = new List<Locality>() { };
                    csvReader.Read();
                    csvReader.ReadHeader();

                    while (csvReader.Read())
                    {

                        if (_context.Localities.FirstOrDefault(c => c.Id == int.Parse(csvReader.GetField("Id"))) == null)

                            _context.Localities.Add(new Locality
                            {
                                Id = int.Parse(csvReader.GetField("Id")),
                                NameAr = csvReader.GetField("NameAr"),
                                NameEn = csvReader.GetField("NameEn"),
                                UniversalCode = csvReader.GetField("Id"),
                                GovernorateId = int.Parse(csvReader.GetField("GovernorateId"))
                            });
                    }
                    _context.SaveChanges();

                }
            }


            _context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.Localities OFF");
            _context.SaveChanges();
        }
    }
}

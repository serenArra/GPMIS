using CsvHelper;
using MFAE.Jobs.EntityFrameworkCore;
using MFAE.Jobs.Location;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MFAE.Jobs.Migrations.Seed.CsvSeed
{
    public class CountriesBuilder
    {
        private readonly JobsDbContext _context;
        public CountriesBuilder(JobsDbContext context)
        {
            _context = context;
        }
        public void Create()
        {
            SeedFromCsv();
        }

        private void SeedFromCsv()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MFAE.Jobs.Migrations.Seed.CsvSeed.CSVFiles.Countries.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader, new CultureInfo("en"));
                    var drivers = new List<Country>() { };
                    csvReader.Read();
                    csvReader.ReadHeader();
                    while (csvReader.Read())
                    {
                        if (_context.Countries.FirstOrDefault(c => c.IsoNumeric == csvReader.GetField("IsoNumeric")) == null)
                            _context.Countries.Add(new Country
                            {
                                NameAr = csvReader.GetField("NameAr"),
                                NameEn = csvReader.GetField("NameEn"),
                                UniversalCode = csvReader.GetField("UniversalCode"),
                                IsoNumeric = csvReader.GetField("IsoNumeric"),
                                IsoAlpha = csvReader.GetField("IsoAlpha")

                            });
                    }
                }
            }
        }
    }
}

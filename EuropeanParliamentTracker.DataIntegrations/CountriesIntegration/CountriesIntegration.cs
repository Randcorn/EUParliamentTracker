using EuropeanParliamentTracker.DataIntegrations.Common;
using EuropeanParliamentTracker.Domain;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EuropeanParliamentTracker.DataIntegrations.CountriesIntegration
{
    public class CountriesIntegration
    {
        private readonly DatabaseContext _context;

        public CountriesIntegration(DatabaseContext context)
        {
            _context = context;
        }

        public async Task IntegrateCountries()
        {
            var stream = await new Client().GetDataStream("https://www.countrycode.org/customer/countryCode/downloadCountryCodes");
            TextReader tr = new StreamReader(stream);
            var countryFile = tr.ReadToEnd();
            var countryLines = countryFile.Split("\n").ToList();
            countryLines.RemoveAt(0);
            foreach(var countryLine in countryLines)
            {
                if(string.IsNullOrWhiteSpace(countryLine))
                {
                    continue;
                }
                var countryInformation = countryLine.Split(",");
                var countryName = countryInformation[0].Replace("\"", "");
                var countryCode = countryInformation[1].Replace("\"", "");

                var currentCountry = _context.Countries.FirstOrDefault(x => x.Name == countryName);
                if(currentCountry == null)
                {
                    continue;
                }

                currentCountry.Code = countryCode;
                _context.Countries.Update(currentCountry);
            }
            _context.SaveChanges();
        }
    }
}

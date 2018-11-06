using EuropeanParliamentTracker.DataIntegrations.Common;
using EuropeanParliamentTracker.Domain;
using Newtonsoft.Json;
using System.IO;
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
            var stream = await new Client().GetDataStream("http://country.io/names.json");
            var tr = new StreamReader(stream);
            dynamic obj = JsonConvert.DeserializeObject(tr.ReadToEnd());
        }
    }
}

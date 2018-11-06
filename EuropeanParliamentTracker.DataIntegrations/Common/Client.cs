using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EuropeanParliamentTracker.DataIntegrations.Common
{
    public class Client
    {
        public async Task<Stream> GetDataStream(string url)
        {
            var client = new HttpClient();
            Stream responseData = null;

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseData = await response.Content.ReadAsStreamAsync();
            }
            catch (HttpRequestException e)
            {
                //TODO: Do Something
            }

            client.Dispose();
            return responseData;
        }
    }
}

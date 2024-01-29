using System.Net.Http.Headers;

namespace Inventory.Clients
{
    public class OrganizationClient
    {
        private readonly HttpClient httpClient;
        public OrganizationClient(string jwtToken)
        {
            Console.WriteLine($"{ClientStaticData.BackendUrl}");
            httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{ClientStaticData.BackendUrl}"),
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
        }

        public async Task AllowedOrganizationsAsync(string route)
        {
            HttpResponseMessage data = await httpClient.GetAsync(route);
            Console.WriteLine(data);
        }
    }
}

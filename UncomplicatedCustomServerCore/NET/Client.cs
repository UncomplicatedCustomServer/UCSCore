using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UncomplicatedCustomServerCore.Integrations;

namespace UncomplicatedCustomServerCore.NET
{
    internal class Client(string dashboardId)
    {
        private static readonly HttpClient httpClient = new()
        {
            Timeout = new(0, 0, 0, 30)
        };

        private readonly string dashboardUri = $"https://{dashboardId}.serverdash.ucserver.it";

        private readonly string centralServers = "https://api.ucserver.it";

        public async Task<string> GetAuthorizedIp()
        {
            return await httpClient.GetStringAsync($"{centralServers}/dashboardip");
        }

        public async Task<string> GetCustomRoles()
        {
            return await httpClient.GetStringAsync($"{dashboardUri}/api/ucr/roles?key={Plugin.Instance.Config.PrivateKey}");
        }

        public async Task SyncPutCustomRoles()
        {
            await httpClient.PutAsync($"{dashboardUri}/api/ucr/roles?key={Plugin.Instance.Config.PrivateKey}", new StringContent(UCR.GetRegisteredCustomRoles(), Encoding.UTF8, "application/json"));
        }
    }
}

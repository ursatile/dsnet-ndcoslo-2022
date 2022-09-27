using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Autobarn.Website.Hubs {
    public class AutobarnHub : Hub {
        public async Task AnythingWeLikeInHere(string user, string message) {
            await Clients.All.SendAsync("NotifyWebsiteUsersAboutNewVehicle",
                user,
                message);
        }
    }
}

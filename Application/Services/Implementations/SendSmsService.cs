using Application.Services.Interfaces;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class SendSmsService : ISendSmsService
    {
        private readonly string apiKey = "";
        public async Task SendActivateMobieCode()
        {
            //throw new NotImplementedException();
        }
    }
}

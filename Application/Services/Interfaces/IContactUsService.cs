using DataLayer.DTOs.Contacts;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IContactUsService : IAsyncDisposable
    {
        Task CreateContactUs(CreateContactUsDTO contactUs, string userIp, long? userId);
    }
}

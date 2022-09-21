using DataLayer.DTOs.Contacts;
using System;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IContactUsService : IAsyncDisposable
    {
        #region ContactUs
        Task CreateContactUs(CreateContactUsDTO contactUs, string userIp, long? userId);
        #endregion
        #region Ticket
        Task<AddTicketResult> AddTicket(AddTicketDTO ticketViewModel , long userId);
        Task<FilterTicketDTO> FilterTicket(FilterTicketDTO filterTicket);
        Task<TicketDetailDTO> GetTicketForShowAsync(long ticketId, long userId);
        Task<AnswerTicketResult> AnswereTicket(AnswerTicketDTO answere, long userId);
        #endregion
    }
}

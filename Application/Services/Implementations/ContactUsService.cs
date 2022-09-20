using Application.Services.Interfaces;
using DataLayer.DTOs.Contacts;
using DataLayer.Entities.Contacts;
using DataLayer.Repositories.GenericRepostitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class ContactUsService : IContactUsService
    {
        #region Constructore
        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;
        public ContactUsService(IGenericRepository<ContactUs> contactUsRepository , IGenericRepository<Ticket> ticketRepository
            , IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;   
            _ticketMessageRepository = ticketMessageRepository; 
        }

        #endregion

        #region Contact Us
        public async Task CreateContactUs(CreateContactUsDTO contactUsDTO, string userIp, long? userId)
        {
            ContactUs contactUs = new ContactUs
            {
                UserId = userId != null && userId.Value != 0 ? userId : (long?)null,
                Email = contactUsDTO.Email,
                FullName = contactUsDTO.FullName,
                Subject = contactUsDTO.Subject,
                Text = contactUsDTO.Text,
                UserIp = userIp,
            };

            await _contactUsRepository.AddEntityAsync(contactUs);
            await _contactUsRepository.SaveChangesAsync();

        }
        #endregion
        #region Ticket
        public async Task<AddTicketResult> AddTicket(AddTicketViewModel ticketViewModel, long userId)
        {
            if (ticketViewModel.Text == null) return AddTicketResult.Error;
            var tickket = new Ticket
            {
                IsReadByOwner = true ,
                TicketPriority = ticketViewModel.TicketPriority,
                Title = ticketViewModel.Title,
                OwnerId = userId, 
                TicketState = TicketState.InProgress 
            };

            await _ticketRepository.AddEntityAsync(tickket);
            await _ticketRepository.SaveChangesAsync();

            var ticketMessage = new TicketMessage
            {
                Text = ticketViewModel.Text,
                TicketId = tickket.Id,
                SenderId = userId
            };

            await _ticketMessageRepository.AddEntityAsync(ticketMessage);
            await _ticketMessageRepository.SaveChangesAsync();

            return AddTicketResult.Success;

        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }
        #endregion
    }
}

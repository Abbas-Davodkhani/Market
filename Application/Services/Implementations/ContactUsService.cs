using Application.Extensions;
using Application.Services.Interfaces;
using DataLayer.DTOs.Contacts;
using DataLayer.DTOs.Paging;
using DataLayer.Entities.Contacts;
using DataLayer.Repositories.GenericRepostitory;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.Implementations
{
    public class ContactUsService : IContactUsService
    {
        #region Constructore
        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;
        public ContactUsService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository
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
        public async Task<AddTicketResult> AddTicket(AddTicketDTO ticketViewModel, long userId)
        {
            if (ticketViewModel.Text == null) return AddTicketResult.Error;
            var tickket = new Ticket
            {
                IsReadByOwner = true,
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
        public async Task<FilterTicketDTO> FilterTicket(FilterTicketDTO filterTicket)
        {
            var ticket = _ticketRepository.GetQuery().AsQueryable();
            #region State
            switch (filterTicket.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    ticket = ticket.Where(t => t.IsDeleted);
                    break;
                case FilterTicketState.NotDeleted:
                    ticket = ticket.Where(t => !t.IsDeleted);
                    break;
            }

            switch (filterTicket.OrderBy)
            {
                case FilterTicketOrder.CreateDate_DES:
                    ticket = ticket.OrderByDescending(x => x.CreatedDate);
                    break;
                case FilterTicketOrder.CreateDate_ASC:
                    ticket = ticket.OrderBy(t => t.CreatedDate);
                    break;
            }
            #endregion

            #region Filter
            if (filterTicket.TicketSection != null)
                ticket = ticket.Where(t => t.TicketSection == filterTicket.TicketSection.Value);

            if (filterTicket.TicketPriority != null)
                ticket = ticket.Where(t => t.TicketPriority == filterTicket.TicketPriority.Value);

            if (!string.IsNullOrEmpty(filterTicket.Title))
                ticket = ticket.Where(t => EF.Functions.Like(filterTicket.Title, $"%{filterTicket.Title}%"));
            #endregion

            #region Paging
            var pager = Pager.Build(filterTicket.PageId, await ticket.CountAsync(), filterTicket.TakeEntity,
                    filterTicket.HomManyPageAfterAndBefore);

            var allEntities = await ticket.Paging(pager).ToListAsync();
            #endregion

            return filterTicket.SetPaging(pager).SetTickets(allEntities);
        }
        public async Task<TicketDetailDTO> GetTicketForShowAsync(long ticketId, long userId)
        {
            var ticket = await _ticketRepository.GetQuery().AsQueryable().
                Include(t => t.Owner)
                .SingleOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null || ticket.OwnerId != userId) return null;

            return new TicketDetailDTO
            {
                Ticket = ticket,
                TicketMessages = await _ticketMessageRepository.GetQuery().AsQueryable().
                    OrderByDescending(t => t.CreatedDate).Where(t => t.TicketId == ticket.Id && !t.IsDeleted)
                    .ToListAsync()
            };
        }
        public async Task<AnswerTicketResult> AnswereTicket(AnswerTicketDTO answere, long userId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(answere.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var ticketMessage = new TicketMessage
            {
                Text = answere.Text,
                SenderId = userId,
                TicketId = ticket.Id
            };

            await _ticketMessageRepository.AddEntityAsync(ticketMessage);
            await _ticketMessageRepository.SaveChangesAsync();

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            await _ticketRepository.SaveChangesAsync();

            return AnswerTicketResult.Success;
        }
        #endregion
        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
            await _ticketMessageRepository.DisposeAsync();
            await _ticketRepository.DisposeAsync();
        }
        #endregion
    }
}

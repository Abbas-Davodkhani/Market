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
        public ContactUsService(IGenericRepository<ContactUs> contactUsRepository)
        {
            _contactUsRepository = contactUsRepository;
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

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }
        #endregion
    }
}

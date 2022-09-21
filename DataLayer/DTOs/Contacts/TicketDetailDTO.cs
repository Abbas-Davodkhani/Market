using DataLayer.Entities.Contacts;
using System.Collections.Generic;

namespace DataLayer.DTOs.Contacts
{
    public class TicketDetailDTO
    {
        public Ticket Ticket { get; set; }

        public List<TicketMessage> TicketMessages { get; set; }
    }
}

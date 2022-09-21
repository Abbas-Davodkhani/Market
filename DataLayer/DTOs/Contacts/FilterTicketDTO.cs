using DataLayer.Entities.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DTOs.Contacts
{
    public class FilterTicketDTO : Paging.BasePaging
    {
        public string Title { get; set; }
        public long UserId { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }

        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public List<Ticket> Tickets { get; set; }

        #region Methods 
        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(Paging.BasePaging basePaging)
        {
            this.PageId = basePaging.PageId;
            this.AllEntitiesCount = basePaging.AllEntitiesCount;
            this.SkipEntity = basePaging.SkipEntity;
            this.TakeEntity = basePaging.TakeEntity;
            this.EndPage = basePaging.EndPage;
            this.StartPage = basePaging.StartPage;
            this.HomManyPageAfterAndBefore = basePaging.HomManyPageAfterAndBefore;
            this.PageCount = basePaging.PageCount;
            return this;
        }
        #endregion 
    }
    public enum FilterTicketState
    { 
        All , 
        Deleted , 
        NotDeleted
    }
    public enum FilterTicketOrder
    {
        CreateDate_DES , 
        CreateDate_ASC
    }
}

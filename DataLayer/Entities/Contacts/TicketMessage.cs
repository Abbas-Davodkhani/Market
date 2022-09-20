using DataLayer.Entities.Account;
using DataLayer.Entities.Common;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Contacts
{
    public class TicketMessage : BaseEntity
    {
        #region Properties
        public long SenderId { get; set; }
        public long TicketId { get; set; }
        [Display(Name = "متن پیام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
        #endregion
        #region Relationship
        public User Sender { get; set; }
        public Ticket Ticket { get; set; }
        #endregion
    }
}

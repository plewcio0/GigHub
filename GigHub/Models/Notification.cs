using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType NotificationType { get; private set; }
        public DateTime? OriginalDateTime { get; set; }
        public string OriginalVenue { get; set; }

        [Required]
        public Gig Gig  { get; private set; }

        public Notification()
        {
        }
        public Notification(Gig gig, NotificationType notificationType)
        {
            if (gig == null)
            {
                throw new ArgumentNullException("gig");
            }    
            Gig = gig;
            NotificationType = notificationType;
            DateTime = DateTime.Now;
        }
    }
}
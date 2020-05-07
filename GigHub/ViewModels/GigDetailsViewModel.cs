using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GigDetailsViewModel
    {
        public bool isAttending;
        public bool isFollowing;

        public Gig Gig { get; set; }
    }
}
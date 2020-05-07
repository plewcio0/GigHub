using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Index(string query = null)
        {
            var upcomingGigs = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Artist.Followers)
                .Include(g => g.Artist.Followees)
                .Include(g => g.Genre)
                .Where(g => g.DateTime > DateTime.Now && g.IsCanceled == false);

            if (!String.IsNullOrWhiteSpace(query))
            {
                upcomingGigs = upcomingGigs
                    .Where(g =>
                                g.Artist.Name.Contains(query) ||
                                g.Genre.Name.Contains(query) ||
                                g.Venue.Contains(query));
            }
            var userId = User.Identity.GetUserId();

            var attendances = _context.Attendances
                .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                .ToList()
                .ToLookup(a => a.GigId);

            var upcomingGigxs = upcomingGigs.ToList();
            var following = _context.Followings
                .Where(f => f.FollowerId == userId)
                .Select(f => f.Followee)
                .ToList()
                .ToLookup(f => f.Id);

            var viewModel = new GigsViewModel
            {
                UpcomingGigs = upcomingGigxs,
                ShowActions = true,
                Heading = "All upcoming Gigs",
                Attendances = attendances,
                SearchTerm = query,
                MyFollowees = following
            };

            return View("Gigs", viewModel);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
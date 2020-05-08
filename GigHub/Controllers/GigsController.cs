using GigHub.Models;
using GigHub.Repositories;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{
    public class GigsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GigRepository _gigRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _attendanceRepository = new AttendanceRepository(_context);
            _gigRepository = new GigRepository(_context);
        }
        [Authorize]
        public ActionResult Create()
        {
            var viewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Create a Gig."
            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gigToEdit = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId);
            var viewModel = new GigFormViewModel
            {
                Id = gigToEdit.Id,
                Genres = _context.Genres.ToList(),
                Date = gigToEdit.DateTime.ToString("dd.MM.yyyy"),
                Time = gigToEdit.DateTime.ToString("HH:mm"),
                Venue = gigToEdit.Venue,
                Genre = gigToEdit.GenreId,
                Heading = "Edit a Gig"

            };
            return View("GigForm", viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var gig = new Gig
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue,
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            var userId = User.Identity.GetUserId();
            if (!ModelState.IsValid)
            {
                viewModel.Genres = _context.Genres.ToList();
                return View("GigForm", viewModel);
            }
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .Single(g => g.Id == viewModel.Id && g.ArtistId == userId);

            gig.Update(viewModel.GetDateTime(), viewModel.Venue, viewModel.Genre);



            _context.SaveChanges();
            return RedirectToAction("Mine", "Gigs");
        }
        [Authorize]
        public ActionResult Attending()
        {
            var userId = User.Identity.GetUserId();

            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _gigRepository.GetGigUserAttending(userId),
                ShowActions = true,
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(userId).ToLookup(a => a.GigId),

            };

            return View("Gigs", viewModel);
        }





        [Authorize]
        public ActionResult Mine()
        {
            var userId = User.Identity.GetUserId();
            var mineGigs = _context.Gigs
                .Where(g => g.ArtistId == userId && g.DateTime > DateTime.Now && g.IsCanceled == false)
                .Include(g => g.Genre)
                .ToList();
            return View(mineGigs);
        }
        [HttpPost]
        public ActionResult Search(GigsViewModel viewModel)
        {
            return RedirectToAction("Index", "Home", new { query = viewModel.SearchTerm });
        }

        public ActionResult Details(int id)
        {
            var gig = _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .FirstOrDefault(g => g.Id == id);
            if (gig == null)
                return HttpNotFound();

            var viewModel = new GigDetailsViewModel { Gig = gig };

            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                viewModel.isAttending = _context.Attendances
                    .Any(a => a.GigId == gig.Id && a.AttendeeId == userId);
                viewModel.isFollowing = _context.Followings
                    .Any(f => f.FolloweeId == gig.ArtistId && f.FollowerId == userId);
            }

            return View(viewModel);
        }
    }
}
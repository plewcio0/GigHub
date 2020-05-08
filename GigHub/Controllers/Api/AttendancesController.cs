using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace GigHub.Controllers.Api
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ApplicationDbContext _context;
        public AttendancesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public HttpResponseMessage Attend(AttendanceDto gig)
        {
            var userId = User.Identity.GetUserId();
            var exists = _context.Attendances.Any(a => a.AttendeeId == userId && a.GigId == gig.GigId);
            if (exists)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Attendance already exist.");

            var attendance = new Attendance
            {
                GigId = gig.GigId,
                AttendeeId = userId
            };

            _context.Attendances.Add(attendance);
            _context.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        [HttpDelete]
        public HttpResponseMessage CancelAttend(int id)
        {
            var userId = User.Identity.GetUserId();

            var attendance = _context.Attendances.SingleOrDefault(a => a.AttendeeId == userId && a.GigId == id);

            if (attendance == null)
                return Request.CreateResponse(HttpStatusCode.NotFound, "Attendance does not exist.");

            _context.Attendances
                .Remove(attendance);
            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}

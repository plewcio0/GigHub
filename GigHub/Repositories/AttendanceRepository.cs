using GigHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GigHub.Repositories
{
    public class AttendanceRepository
    {
        public ApplicationDbContext _context { get; set; }
        public AttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Attendance> GetFutureAttendances(string userId)
        {
            return _context.Attendances
                            .Where(a => a.AttendeeId == userId && a.Gig.DateTime > DateTime.Now)
                            .ToList();
        }
        public Attendance GetAttendance(string userId, int gigId)
        {
            return _context.Attendances
                    .Single(a => a.GigId == gigId && a.AttendeeId == userId);
        }

        internal Following GetFollowing(string userId, string artistId)
        {
            return _context.Followings
                    .Any(f => f.FolloweeId == artistId && f.FollowerId == userId);
        }
    }
}
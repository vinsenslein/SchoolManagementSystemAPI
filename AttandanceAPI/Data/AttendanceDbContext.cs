using AttendanceAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAPI.Data
{
    public class AttendanceDbContext : DbContext //inherid Dbcontext
    {

        public AttendanceDbContext(DbContextOptions options) : base(options)
        {



        }
        public DbSet<AttendanceEntity> Attendances { get; set; } //Representasi tabel
    }
}

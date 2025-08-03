using Microsoft.EntityFrameworkCore;
using StudentAPI.Models.Entities;

namespace StudentAPI.Data
{
    public class StudentDbContext : DbContext //inherid Dbcontext
    {

        public StudentDbContext(DbContextOptions options) : base(options)
        {



        }
        public DbSet<StudentEntity> Students { get; set; } //Representasi tabel
    }
}

using Microsoft.EntityFrameworkCore;
using SchoolManagementSystemAPI.Models.Entities;

namespace SchoolManagementSystemAPI.Data
{
    public class StudentDbContext : DbContext //inherid Dbcontext
    {
    
        public StudentDbContext(DbContextOptions options) : base(options) 
        {



        }
        public DbSet<StudentEntity> Students { get; set; } //Representasi tabel
    }
}

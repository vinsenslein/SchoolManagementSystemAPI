using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

//inherit DbContext: Menandakan bahwa StudentDbContext mewarisi semua kemampuan dari DbContext.

//public StudentDbContext(DbContextOptions options) : base(options)
////Itu berarti constructor dari class StudentDbContext sedang memanggil constructor dari class induknya,
//DbContextOptions options: Ini adalah objek yang dibutuhkan oleh DbContext (parent class) untuk konfigurasi — seperti connection string, provider database (SQL Server, SQLite, dll), dan lainnya.

//: base(options): Ini memanggil constructor milik class induk(base class), yaitu DbContext, dan mengoper objek options ke sana.
//DbSet<StudentEntity>: Merepresentasikan sebuah tabel bernama Students di database.

//Dengan properti ini, kamu bisa melakukan query seperti Students.ToListAsync(), AddAsync(), Remove(), dll.

//StudentEntity: Adalah class yang mewakili setiap baris (record) di tabel Students.

////DbContext	Kelas utama dari EF Core untuk mengakses dan mengatur koneksi DB.
//DbContextOptions Konfigurasi yang digunakan untuk mengatur perilaku DbContext.
//DbSet<T>	Koleksi entitas yang dipetakan ke tabel tertentu di database.
//Entity	Class C# yang mewakili struktur tabel di database.
//ORM	Object Relational Mapping – menghubungkan objek C# dengan tabel SQL.
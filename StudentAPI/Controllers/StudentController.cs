using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAPI.Models.Entities;
using StudentAPI.Data;
using StudentAPI.Models;

namespace StudentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _StudentDbContext;
        private readonly ILogger<StudentController> _logger;

        public StudentController(StudentDbContext StudentDbContext, ILogger<StudentController> logger)
        {
            _StudentDbContext = StudentDbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                _logger.LogInformation("GET /api/students called");
                var students = await _StudentDbContext.Students.ToListAsync();

                if (students == null || students.Count == 0)
                {
                    _logger.LogWarning("No students found in the database.");
                    return NotFound(new { message = "No students found." });
                }

                _logger.LogInformation("{Count} students found.", students.Count);
                return Ok(students);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all students");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            try
            {
                _logger.LogInformation("GET /api/student/{Id} called", id);
                var student = await _StudentDbContext.Students.FindAsync(id);
                if (student == null)
                {
                    _logger.LogWarning("❗ Student with ID {Id} not found", id);
                    return NotFound(new { message = $"Student with ID {id} not found" });
                }

                _logger.LogInformation("Student with ID {Id} found: {@Student}", id, student);
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{nis}")]
        public async Task<IActionResult> GetStudentByNis(string nis)
        {
            try
            {
                _logger.LogInformation("GET /api/student/{NIS} called", nis);

                var student = await _StudentDbContext.Students.FirstOrDefaultAsync(s => s.CSTUDENT_NIS == nis);

                if (student == null)
                {
                    _logger.LogWarning("Student with NIS {NIS} not found", nis);
                    return NotFound(new { message = $"Student with NIS {nis} not found" });
                }

                _logger.LogInformation("Student with NIS {NIS} found: {@Student}", nis, student);
                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting student by NIS");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentDTO addStudentDTO)
        {
            try
            {
                _logger.LogInformation("POST /api/student called");

                var studentEntity = new StudentEntity
                {
                    CSTUDENT_NIS = addStudentDTO.CSTUDENT_NIS,
                    CSTUDENT_NAME = addStudentDTO.CSTUDENT_NAME,
                    CSTUDENT_EMAIL = addStudentDTO.CSTUDENT_EMAIL,
                    CSTUDENT_PHONE = addStudentDTO.CSTUDENT_PHONE,
                    CSTUDENT_GENDER = addStudentDTO.CSTUDENT_GENDER,
                    LSTUDENT_ISACTIVE = addStudentDTO.LSTUDENT_ISACTIVE,
                    DCREATE_DATE = addStudentDTO.DCREATE_DATE,
                    DUPDATE_DATE = addStudentDTO.DUPDATE_DATE,
                };

                await _StudentDbContext.Students.AddAsync(studentEntity);
                await _StudentDbContext.SaveChangesAsync();

                _logger.LogInformation("Student {Name} added with ID {Id}", studentEntity.CSTUDENT_NAME, studentEntity.Id);

                return CreatedAtAction(nameof(GetStudentById), new { id = studentEntity.Id }, studentEntity); //mengembalikan respon HTTP 201 (Created) saat kamu berhasil menambahkan data ke server 
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding student");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding student");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateStudent(Guid id, UpdateStudentDTO updateStudentDTO)
        {
            try
            {
                _logger.LogInformation("PUT /api/students/{Id} called", id);
                var student = await _StudentDbContext.Students.FindAsync(id);

                if (student == null)
                {
                    _logger.LogWarning("❗ Student with ID {Id} not found", id);
                    return NotFound(new { message = $"Student with ID {id} not found" });
                }

                student.CSTUDENT_NIS = updateStudentDTO.CSTUDENT_NIS;
                student.CSTUDENT_NAME = updateStudentDTO.CSTUDENT_NAME;
                student.CSTUDENT_EMAIL = updateStudentDTO.CSTUDENT_EMAIL;
                student.CSTUDENT_PHONE = updateStudentDTO.CSTUDENT_PHONE;
                student.CSTUDENT_GENDER = updateStudentDTO.CSTUDENT_GENDER;
                student.LSTUDENT_ISACTIVE = updateStudentDTO.LSTUDENT_ISACTIVE;
                student.DUPDATE_DATE = updateStudentDTO.DUPDATE_DATE;

                await _StudentDbContext.SaveChangesAsync();
                _logger.LogInformation("Student with ID {Id} updated", id);

                return Ok(student);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating student");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating student");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            try
            {
                _logger.LogInformation("DELETE /api/students/{Id} called", id);

                var student = await _StudentDbContext.Students.FindAsync(id);

                if (student == null)
                {
                    _logger.LogWarning("❗ Student with ID {Id} not found", id);
                    return NotFound(new { message = $"Student with ID {id} not found" });
                }

                _StudentDbContext.Students.Remove(student);
                await _StudentDbContext.SaveChangesAsync();

                _logger.LogInformation("Student with ID {Id} deleted", id);
                return Ok(student);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting student");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting student");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

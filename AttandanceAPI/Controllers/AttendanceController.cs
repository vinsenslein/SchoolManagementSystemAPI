using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AttendanceAPI.Models.Entities;
using Microsoft.Extensions.Logging;
using AttendanceAPI.Models;
using AttendanceAPI.Data;
using System.Net.Http;

namespace AttendanceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly AttendanceDbContext _attendanceDbContext;
        private readonly ILogger<AttendanceController> _logger;
        private readonly HttpClient _httpClient; // Client untuk melakukan request ke Student API

        //AttendanceDbContext → untuk akses database absensi.
        //ILogger<AttendanceController> → untuk logging.
        //HttpClient → untuk komunikasi dengan API lain (dalam hal ini, Student API). (ASP.NET Core Dependency Injection)

        public AttendanceController(AttendanceDbContext attendanceDbContext, ILogger<AttendanceController> logger, HttpClient httpClient) //=>PARAM HTTP CLIENT UNTUK JOIN KE STUDENT
        {
            _attendanceDbContext = attendanceDbContext;
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAttendances()
        {
            try
            {
                _logger.LogInformation("GET /api/attendances called");
                var attendances = await _attendanceDbContext.Attendances.ToListAsync();

                if (attendances == null || attendances.Count == 0)
                {
                    _logger.LogWarning("No attendances found in the database.");
                    return NotFound(new { message = "No attendances found." });
                }

                _logger.LogInformation("{Count} attendances found.", attendances.Count);
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all attendances");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAttendanceById(Guid id)
        {
            try
            {
                _logger.LogInformation("GET /api/attendance/{Id} called", id);
                var attendance = await _attendanceDbContext.Attendances.FindAsync(id);
                if (attendance == null)
                {
                    _logger.LogWarning("❗ Attendance with ID {Id} not found", id);
                    return NotFound(new { message = $"Attendance with ID {id} not found" });
                }

                _logger.LogInformation("Attendance with ID {Id} found: {@Attendance}", id, attendance);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting attendance by ID");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendance(AddAttendanceDTO addAttendanceDTO)
        {
            try
            {
                _logger.LogInformation("POST /api/attendance called");

                // Validasi apakah NIS valid (ada di Student API)
                var studentApiUrl = $"https://localhost:7188/api/Student/{addAttendanceDTO.CSTUDENT_NIS}";
                var response = await _httpClient.GetAsync(studentApiUrl);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Student NIS {NIS} not found in Student API", addAttendanceDTO.CSTUDENT_NIS);
                    return BadRequest("Invalid NIS: student not found.");
                }

                var attendanceEntity = new AttendanceEntity
                {
                    CSTUDENT_NIS = addAttendanceDTO.CSTUDENT_NIS,
                    DATTENDANCE_DATE = addAttendanceDTO.DATTENDANCE_DATE,
                    CATTENDANCE_STATUS = addAttendanceDTO.CATTENDANCE_STATUS,
                    DCREATE_DATE = addAttendanceDTO.DCREATE_DATE,
                    DUPDATE_DATE = addAttendanceDTO.DUPDATE_DATE,
                };

                await _attendanceDbContext.Attendances.AddAsync(attendanceEntity);
                await _attendanceDbContext.SaveChangesAsync();

                _logger.LogInformation("Attendance {NIS} added with ID {Id}", attendanceEntity.CSTUDENT_NIS, attendanceEntity.Id);

                return CreatedAtAction(nameof(GetAttendanceById), new { id = attendanceEntity.Id }, attendanceEntity);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while adding attendance");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while adding attendance");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAttendance(Guid id, UpdateAttendanceDTO updateAttendanceDTO)
        {
            try
            {
                _logger.LogInformation("PUT /api/attendances/{Id} called", id);
                var attendance = await _attendanceDbContext.Attendances.FindAsync(id);

                if (attendance == null)
                {
                    _logger.LogWarning("❗ Attendance with ID {Id} not found", id);
                    return NotFound(new { message = $"Attendance with ID {id} not found" });
                }

                attendance.CSTUDENT_NIS = updateAttendanceDTO.CSTUDENT_NIS;
                attendance.DATTENDANCE_DATE = updateAttendanceDTO.DATTENDANCE_DATE;
                attendance.CATTENDANCE_STATUS = updateAttendanceDTO.CATTENDANCE_STATUS;
                attendance.DUPDATE_DATE = updateAttendanceDTO.DUPDATE_DATE;

                await _attendanceDbContext.SaveChangesAsync();
                _logger.LogInformation("Attendance with ID {Id} updated", id);

                return Ok(attendance);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while updating attendance");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating attendance");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAttendance(Guid id)
        {
            try
            {
                _logger.LogInformation("DELETE /api/attendances/{Id} called", id);

                var attendance = await _attendanceDbContext.Attendances.FindAsync(id);

                if (attendance == null)
                {
                    _logger.LogWarning("❗ Attendance with ID {Id} not found", id);
                    return NotFound(new { message = $"Attendance with ID {id} not found" });
                }

                _attendanceDbContext.Attendances.Remove(attendance);
                await _attendanceDbContext.SaveChangesAsync();

                _logger.LogInformation("Attendance with ID {Id} deleted", id);
                return Ok(attendance);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update error while deleting attendance");
                return StatusCode(500, "Database error");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while deleting attendance");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

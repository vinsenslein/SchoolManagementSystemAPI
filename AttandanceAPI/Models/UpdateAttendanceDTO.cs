namespace AttendanceAPI.Models
{
    public class UpdateAttendanceDTO
    {
        public required string CSTUDENT_NIS { get; set; }
        public DateTime DATTENDANCE_DATE { get; set; }
        public required string CATTENDANCE_STATUS { get; set; } //"Hadir", "Izin", "Sakit", dll
        public DateTime DUPDATE_DATE { get; set; } = DateTime.Now;
    }
}

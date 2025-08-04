namespace AttendanceAPI.Models.Entities
{
    public class AttendanceEntity
    {
       
        public Guid Id { get; set; }
        public required string CSTUDENT_NIS { get; set; }
        public DateTime DATTENDANCE_DATE { get; set; }
        public required string CATTENDANCE_STATUS { get; set; } //"Hadir", "Izin", "Sakit", dll
        public DateTime DCREATE_DATE { get; set; } = DateTime.Now;
        public DateTime DUPDATE_DATE { get; set; } = DateTime.Now;
        public required string CSTUDENT_NIS3 { get; set; }
    }
}

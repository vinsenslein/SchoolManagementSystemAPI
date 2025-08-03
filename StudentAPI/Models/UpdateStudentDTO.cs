namespace SchoolManagementSystemAPI.Models
{
    public class UpdateStudentDTO
    {


        public required string CSTUDENT_NIS { get; set; }
        public required string CSTUDENT_NAME { get; set; }
        public required string CSTUDENT_EMAIL { get; set; }
        public required string CSTUDENT_PHONE { get; set; }
        public required string CSTUDENT_GENDER { get; set; }
        public bool LSTUDENT_ISACTIVE { get; set; }
        public DateTime DUPDATE_DATE { get; set; } = DateTime.Now;

    }
}

namespace StudentAPI.Models.Entities
{
    public class StudentEntity
    {
        public Guid Id { get; set; }
        public required string CSTUDENT_NIS { get; set; }
        public required string CSTUDENT_NAME { get; set; }
        public required string CSTUDENT_EMAIL { get; set; }
        public required string CSTUDENT_PHONE { get; set; }
        public required string CSTUDENT_GENDER { get; set; }
        public bool LSTUDENT_ISACTIVE { get; set; }
        public DateTime DCREATE_DATE { get; set; } = DateTime.Now;
        public DateTime DUPDATE_DATE { get; set; } = DateTime.Now;


    }
}

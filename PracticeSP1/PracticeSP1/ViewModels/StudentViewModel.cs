using PracticeSP1.Models;
using System.ComponentModel.DataAnnotations;

namespace PracticeSP1.ViewModels
{
    public class StudentViewModel
    {
        public int StudentId { get; set; }
        [Required]
        [Display(Name ="Student Name")]
        public string StudentName { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Of Birth")]
        public DateTime Dob { get; set; }=DateTime.Now;
        [Display(Name = "Mobile No")]
        public string MobileNo { get; set; }
        [Display(Name = "Enrollment")]
        public bool IsEnrolled { get; set; }
        [Display(Name = "Registration Fee")]
        public decimal RegistrationFee { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        public int CourseId { get; set; }
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        public int CourseModuleId { get; set; }
        public string ModuleName { get; set; }
        public int Duration { get; set; }

        public IFormFile ProfileFile { get; set; }


        public virtual IList<Student> Students { get; set; }
        public virtual IList<Course> Courses { get; set; }
        public virtual IList<CourseModule> Modules { get; set; }=new List<CourseModule>();  



    }
}

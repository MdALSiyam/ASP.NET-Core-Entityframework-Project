using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PracticeSP1.Models;
using PracticeSP1.ViewModels;
using System.Data;
using System.Reflection;

namespace PracticeSP1.Controllers
{
    public class StudentsController : Controller
    {
        //Students

        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public StudentsController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {
            var student = _db.Students.Include(c=>c.Course).Include(m=>m.Modules).ToList();
            return View(student);
        }

        public IActionResult Delete(int id) 
        {
            var student = _db.Students.Find(id);
            if (!string.IsNullOrEmpty(student.ImageUrl)) 
            {
                var imageFile = Path.Combine(_env.WebRootPath, "Img", student.ImageUrl);
                if (System.IO.File.Exists(imageFile)) 
                {
                    System.IO.File.Delete(imageFile);
                }               
            }
            _db.Students.Remove(student);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create() 
        {
            StudentViewModel student = new StudentViewModel();
            student.Courses = _db.Courses.ToList();
            return View(student);
        }
        [HttpPost]
        public IActionResult Create(StudentViewModel student) 
        {
            string imageFile = null!;
            if (student.ProfileFile != null) 
            {
                imageFile = GetImage(student.ProfileFile);
            }
            var imageUrl = imageFile;
            var sName = student.StudentName;
            var dob = student.Dob;
            var mNo = student.MobileNo;
            var isIn = student.IsEnrolled;
            var regeFee = student.RegistrationFee;
            var courseId = student.CourseId;
            var moduleTable = new DataTable();

            moduleTable.Columns.Add("ModuleName", typeof(string));
            moduleTable.Columns.Add("Duration", typeof(int));
            if (student.Modules != null && student.Modules.Any()) 
            {
                foreach (var m in student.Modules)
                {
                    moduleTable.Rows.Add(m.ModuleName, m.Duration);
                }
            }

            var paraMeters = new[]
            {
                new SqlParameter("@StudentName",sName),
                new SqlParameter("@Dob",dob),
                new SqlParameter("@MobileNo",mNo),
                new SqlParameter("@IsEnrolled",isIn),
                new SqlParameter("@RegistrationFee",regeFee),
                new SqlParameter("@CourseId",courseId),
                new SqlParameter("@ImageUrl",imageUrl),
                new SqlParameter
                {
                    ParameterName = "@Modules",
                    SqlDbType=SqlDbType.Structured,
                    TypeName="dbo.PModuleType",
                    Value= moduleTable
                }

            };
            _db.Database.ExecuteSqlRaw("EXEC spInsertStudent @StudentName,@Dob,@MobileNo,@IsEnrolled,@RegistrationFee,@CourseId,@ImageUrl,@Modules", paraMeters);
            return RedirectToAction("Index");

        }

        private string? GetImage(IFormFile profileFile)
        {
            string fName = null!;
            if (profileFile != null) 
            {
                fName = Guid.NewGuid().ToString()+Path.GetExtension(profileFile.FileName);
                var fPath = Path.Combine(_env.WebRootPath, "Img");
                var fFile = Path.Combine(fPath, fName);
                using (var fileS = new FileStream(fFile, FileMode.Create)) 
                {
                    profileFile.CopyTo(fileS);
                }
            }
            return fName;
        }
        [HttpGet]
        public IActionResult Edit(int id) 
        {
            var s = _db.Students.Include(c=>c.Course).Include(m=>m.Modules)
                .FirstOrDefault(s=>s.StudentId == id);
            if (s != null)
            {
                var vModel = new StudentViewModel
                {
                    StudentId = s.StudentId,
                    StudentName = s.StudentName,
                    Dob = s.Dob,
                    MobileNo = s.MobileNo,
                    IsEnrolled = s.IsEnrolled,
                    CourseId = s.CourseId,
                    CourseName = s.Course.CourseName,
                    RegistrationFee = s.RegistrationFee,
                    Courses = _db.Courses.ToList(),
                    ImageUrl = s.ImageUrl,
                    Modules = _db.Modules.ToList()
                };

                return View(vModel);
            }
            else 
            {
                return NotFound("Not Found");
            }


        }
        [HttpPost]
        public IActionResult Edit(StudentViewModel m,string oldImg) 
        {
            var s = _db.Students.Include(c => c.Course).Include(m => m.Modules)
               .FirstOrDefault(s => s.StudentId == m.StudentId);
            if (s != null)
            {
                var img = GetImage(m.ProfileFile);
                s.ImageUrl = img!;
                if (img != null)
                {
                    s.ImageUrl = img!;
                }
                else 
                {
                    s.ImageUrl = oldImg;
                }
                s.StudentName=m.StudentName;
                s.Dob = m.Dob;
                s.MobileNo = m.MobileNo;
                s.IsEnrolled = m.IsEnrolled;
                s.RegistrationFee = m.RegistrationFee;
                s.CourseId = m.CourseId;

                var exModule = s.Modules.ToList();
                foreach (var ex in exModule) 
                {
                    _db.Modules.Remove(ex);
                }
                foreach (var n in m.Modules) 
                {
                    _db.Modules.Add(new CourseModule 
                    {
                        StudentId = s.StudentId,
                        ModuleName = n.ModuleName,
                        Duration = n.Duration,
                    });
                }
                
              
            }
            else { NotFound(); }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }







        public IActionResult Aggregate()
        {
            var totalRegistrationFee = _db.Students.Sum(s => s.RegistrationFee);
            var averageRegistrationFee = _db.Students.Average(s => s.RegistrationFee);
            var maxRegistrationFee = _db.Students.Max(s => s.RegistrationFee);
            var minRegistrationFee = _db.Students.Min(s => s.RegistrationFee);
            var totalEnrolledStudents = _db.Students.Count(s => s.IsEnrolled);
            var aggregateModel = new
            {
                TotalRegistrationFee = totalRegistrationFee,
                AverageRegistrationFee = averageRegistrationFee,
                MaxRegistrationFee = maxRegistrationFee,
                MinRegistrationFee = minRegistrationFee,
                TotalEnrolledStudents = totalEnrolledStudents

            };
            return View(aggregateModel);
        }


        public IActionResult AggregagateWithGroup()
        {
            var data = _db.Students;
            var max = data.Max(e => e.RegistrationFee);
            var min = data.Min(e => e.RegistrationFee);
            var avg = data.Average(e => e.RegistrationFee);
            var sum = data.Sum(e => e.RegistrationFee);
            var groupresult = data.GroupBy(s => new { s.Course.CourseId, s.Course.CourseName })
            .Select(s => new AggregateWithGroupModel
            {
                CourseId = s.Key.CourseId,
                CourseName = s.Key.CourseName,
                MinValue = s.Min(e => e.RegistrationFee),
                MaxValue = s.Max(e => e.RegistrationFee),
                AvgValue = s.Average(e => e.RegistrationFee),
                SumValue = s.Sum(e => e.RegistrationFee),
                CountValue = s.Count(),
            }).ToList();
            var model = new AggregateWithSummaryModel
            {
                MinSummary = min,
                MaxSummary = max,
                AvgSummary = avg,
                SumSummary = sum,
                CountSummary = data.Count(),
                AggregateResult = groupresult
            };
            return View(model);
        }



    }
}

using Microsoft.AspNetCore.Mvc;
using AureusVitalis.Models;

namespace AureusVitalis.Controllers
{
    public class EducationController : Controller
    {
        public IActionResult Main()
        {
            var model = new EducationModel
            {
                Title = "Education",
                Blocks = new List<EducationBlock>
                {
                    new EducationBlock { Name = "Nutrition", Type = "Learning", Url = "/Education/Nutrition" },
                    new EducationBlock { Name = "Nutrition Practice", Type = "Practice", Url = "/Education/NutritionPractice" },
                    new EducationBlock { Name = "Nutrition", Type = "Test", Url = "/Education/NutritionTest" },
                    new EducationBlock { Name = "Sport", Type = "Learning", Url = "/Education/Sport" },
                    new EducationBlock { Name = "Sport Practice", Type = "Practice", Url = "/Education/SportPractice" },
                    new EducationBlock { Name = "Sport", Type = "Test", Url = "/Education/SportTest" },
                    new EducationBlock { Name = "Sleep", Type = "Learning", Url = "/Education/Sleep" },
                    new EducationBlock { Name = "Sleep Practice", Type = "Practice", Url = "/Education/SleepPractice" },
                    new EducationBlock { Name = "Exam", Type = "Exam", Url = "/Education/FinalExam" }
                }
            };
            return View(model);
        }
    }
}
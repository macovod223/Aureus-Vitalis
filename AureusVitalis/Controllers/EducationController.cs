using Microsoft.AspNetCore.Mvc;
using AureusVitalis.Models;
using System.Collections.Generic;

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
                    new EducationBlock { Name = "Nutrition Test", Type = "Test", Url = "/Education/NutritionTest" },
                    new EducationBlock { Name = "Sport", Type = "Learning", Url = "/Education/Sport" },
                    new EducationBlock { Name = "Sport Practice", Type = "Practice", Url = "/Education/SportPractice" },
                    new EducationBlock { Name = "Sport Test", Type = "Test", Url = "/Education/SportTest" },
                    new EducationBlock { Name = "Sleep", Type = "Learning", Url = "/Education/Sleep" },
                    new EducationBlock { Name = "Sleep Practice", Type = "Practice", Url = "/Education/SleepPractice" },
                    new EducationBlock { Name = "Final Exam", Type = "Exam", Url = "/Education/FinalExam" }
                }
            };

            ViewBag.Progress = 60; // Пример: 60% прогресса
            ViewBag.ExamAvailable = ViewBag.Progress >= 100;

            return View(model);
        }

        public IActionResult Nutrition()
        {
            var model = new EducationContentModel
            {
                Title = "Основы правильного питания",
                Content = "<p>Здесь будет теория по питанию. Добавь сюда свой текст или HTML.</p>",
                Images = new List<string> { "/images/nutrition1.png" },
                Progress = 10
            };
            return View(model);
        }

        public IActionResult NutritionPractice()
        {
            var model = new PracticeModel
            {
                Title = "Практика: Питание",
                Progress = 20,
                Tasks = new List<PracticeTask>
                {
                    new PracticeTask
                    {
                        Question = "Что из перечисленного является источником белка?",
                        Options = new List<string> { "Курица", "Хлеб", "Молоко", "Масло" },
                        CorrectAnswer = "Курица",
                        Explanation = "Курица — отличный источник белка."
                    }
                }
            };
            return View(model);
        }

        public IActionResult NutritionTest()
        {
            var model = new TestModel
            {
                Title = "Тест по питанию",
                TimeLimit = 10,
                IsCompleted = false,
                Questions = new List<TestQuestion>
                {
                    new TestQuestion
                    {
                        Question = "Сколько порций овощей и фруктов рекомендуется есть в день?",
                        Options = new List<string> { "2-3", "5-7", "10-12", "1-2" },
                        CorrectAnswer = "5-7"
                    }
                }
            };
            return View(model);
        }

        public IActionResult Sport()
        {
            var model = new EducationContentModel
            {
                Title = "Физическая активность и здоровье",
                Content = "<p>Здесь будет теория по спорту.</p>",
                Images = new List<string> { "/images/sport1.png" },
                Progress = 30
            };
            return View(model);
        }

        public IActionResult SportPractice()
        {
            var model = new PracticeModel
            {
                Title = "Практика: Физическая активность",
                Progress = 40,
                Tasks = new List<PracticeTask>
                {
                    new PracticeTask
                    {
                        Question = "Выберите правильную последовательность тренировки.",
                        Options = new List<string> { "Разминка", "Основная часть", "Заминка", "Растяжка" },
                        CorrectAnswer = "Разминка",
                        Explanation = "Тренировка всегда начинается с разминки."
                    }
                }
            };
            return View(model);
        }

        public IActionResult SportTest()
        {
            var model = new TestModel
            {
                Title = "Тест по физической активности",
                TimeLimit = 10,
                IsCompleted = false,
                Questions = new List<TestQuestion>
                {
                    new TestQuestion
                    {
                        Question = "Сколько минут умеренной активности рекомендуется в неделю?",
                        Options = new List<string> { "90", "150", "200", "120" },
                        CorrectAnswer = "150"
                    }
                }
            };
            return View(model);
        }

        public IActionResult Sleep()
        {
            var model = new EducationContentModel
            {
                Title = "Здоровый сон",
                Content = "<p>Здесь будет теория по сну.</p>",
                Images = new List<string> { "/images/sleep1.png" },
                Progress = 50
            };
            return View(model);
        }

        public IActionResult SleepPractice()
        {
            var model = new PracticeModel
            {
                Title = "Практика: Сон",
                Progress = 60,
                Tasks = new List<PracticeTask>
                {
                    new PracticeTask
                    {
                        Question = "Что помогает улучшить качество сна?",
                        Options = new List<string> { "Регулярный режим", "Кофеин вечером", "Смартфон перед сном" },
                        CorrectAnswer = "Регулярный режим",
                        Explanation = "Регулярный режим сна способствует его качеству."
                    }
                }
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult FinalExam()
        {
            var model = new ExamModel
            {
                Title = "Финальный экзамен",
                TimeLimit = 30,
                IsAvailable = true,
                IsCompleted = false,
                MinimumPassingScore = 80,
                Questions = new List<TestQuestion>
                {
                    new TestQuestion
                    {
                        Question = "Что важно для здорового образа жизни?",
                        Options = new List<string> { "Питание", "Сон", "Физическая активность", "Все перечисленное" },
                        CorrectAnswer = "Все перечисленное"
                    }
                }
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateProgress(string blockType, string blockName, bool isCompleted)
        {
            // TODO: Сохранять прогресс пользователя в БД
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult CheckExamAvailability()
        {
            var isAvailable = (ViewBag.Progress ?? 0) >= 100;
            return Json(new { isAvailable, message = isAvailable ? "" : "Необходимо завершить все предыдущие блоки обучения" });
        }
    }
}
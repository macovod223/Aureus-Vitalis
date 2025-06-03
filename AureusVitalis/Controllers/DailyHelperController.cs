using Microsoft.AspNetCore.Mvc;
using AureusVitalis.Models;

namespace AureusVitalis.Controllers
{
    public class DailyHelperController : Controller
    {
        public IActionResult Selection()
        {
            return View(new DailyHelperModel());
        }

        public IActionResult Calculator()
        {
            return View(new CalculatorModel());
        }

        [HttpPost]
        public IActionResult Calculator(CalculatorModel model)
        {
            if (ModelState.IsValid)
            {
                // Здесь будет логика расчета диеты и режима сна
                return View(model);
            }

            return View(model);
        }

        public IActionResult Exercises()
        {
            return View(new ExerciseModel());
        }

        [HttpPost]
        public IActionResult Exercises(ExerciseModel model)
        {
            if (ModelState.IsValid)
            {
                // Пример генерации упражнений
                model.GeneratedExercises = GenerateExercises(model);
                return View(model);
            }

            return View(model);
        }

        private List<Exercise> GenerateExercises(ExerciseModel model)
        {
            var exercises = new List<Exercise>();

            // Примеры упражнений в зависимости от выбранной группы мышц
            switch (model.MuscleGroup.ToLower())
            {
                case "fullbody":
                    exercises.Add(new Exercise
                    {
                        Name = "Берпи (Burpee)",
                        Repetitions = 10,
                        Recommendations = "Выполняйте в среднем темпе, следите за дыханием"
                    });
                    exercises.Add(new Exercise
                    {
                        Name = "Приседания с выпрыгиванием",
                        Repetitions = 12,
                        Recommendations = "Приземляйтесь мягко, сгибая колени"
                    });
                    exercises.Add(new Exercise
                    {
                        Name = "Планка с переходом в отжимание",
                        Repetitions = 8,
                        Recommendations = "Держите корпус прямым на протяжении всего упражнения"
                    });
                    break;

                case "upperbody":
                    exercises.Add(new Exercise
                    {
                        Name = "Отжимания",
                        Repetitions = CalculateReps(model.Gender, "pushups"),
                        Recommendations = "Локти держите близко к телу"
                    });
                    exercises.Add(new Exercise
                    {
                        Name = "Обратные отжимания от стула",
                        Repetitions = 12,
                        Recommendations = "Опускайтесь медленно, контролируя движение"
                    });
                    break;

                case "core":
                    exercises.Add(new Exercise
                    {
                        Name = "Планка",
                        Repetitions = 30, // секунд
                        Recommendations = "Держите тело в прямой линии, втяните живот"
                    });
                    exercises.Add(new Exercise
                    {
                        Name = "Скручивания",
                        Repetitions = 15,
                        Recommendations = "Поднимайте плечи от пола, не дергайте шею"
                    });
                    break;

                // Добавьте другие группы мышц по аналогии
            }

            // Добавляем рекомендации по весу и возрасту
            if (model.Age > 50)
            {
                foreach (var exercise in exercises)
                {
                    exercise.Recommendations += " Выполняйте упражнения в комфортном темпе.";
                    exercise.Repetitions = (int)(exercise.Repetitions * 0.8); // Уменьшаем количество повторений
                }
            }

            return exercises;
        }

        private int CalculateReps(string gender, string exercise)
        {
            // Простая логика расчета повторений в зависимости от пола
            switch (exercise)
            {
                case "pushups":
                    return gender == "M" ? 15 : 10;
                case "squats":
                    return gender == "M" ? 20 : 18;
                default:
                    return 12;
            }
        }
    }
}
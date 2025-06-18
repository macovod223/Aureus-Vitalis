using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AureusVitalis.Data;
using AureusVitalis.Models;

namespace AureusVitalis.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly AppDbContext _db;
        public AchievementsController(AppDbContext db) => _db = db;

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public IActionResult Index()
        {
            var userId = CurrentUserId;

            // 1) Жёстко прописанный список всех ачивок
            var all = new List<AchievementModel>
            {
                new AchievementModel {
                    Title       = "Это только начало...",
                    Description = "Пройти блок теории по питанию",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Nutrition" // вместо просто "Theorie", 
                                              // берём ключ, который вы используете в UserProgress.ModuleKey
                },
                new AchievementModel {
                    Title       = "Поварёнок",
                    Description = "Пройти блок практики по питанию",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Nutrition.Practice"
                },
                new AchievementModel {
                    Title       = "Знаток питания",
                    Description = "Пройти тест по питанию",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Nutrition.Test"
                },
                new AchievementModel {
                    Title       = "В движении сила",
                    Description = "Пройти блок теории по спорту",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Sport"
                },
                new AchievementModel {
                    Title       = "Тренировочный день",
                    Description = "Пройти блок практики по спорту",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Sport.Practice"
                },
                new AchievementModel {
                    Title       = "Мастер спорта",
                    Description = "Пройти тест по спорту",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Sport.Test"
                },
                new AchievementModel {
                    Title       = "Сон — залог успеха",
                    Description = "Пройти блок теории для сна",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Sleep"
                },
                new AchievementModel {
                    Title       = "Гуру сна",
                    Description = "Пройти блок практики для сна",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Sleep.Practice"
                },
                new AchievementModel {
                    Title       = "Победитель марафона",
                    Description = "Пройти финальный экзамен",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "FinalExam"
                },
                // опционально: «мастер марафона» — когда пройдены все предыдущие
                new AchievementModel {
                    Title       = "Финишная прямая",
                    Description = "Завершить весь курс",
                    Icon        = "/images/trophy.png",
                    ModuleKey   = "Course.Completed"
                }
            };

            // 2) Получаем из БД все уже завершённые пользователем модули
            var doneKeys = _db.UserProgress
                .Where(up => up.UserId == userId && up.IsCompleted)
                .Select(up => up.ModuleKey)
                .ToHashSet();

            // 3) Расставляем IsUnlocked
            foreach (var a in all)
            {
                if (a.ModuleKey == "Course.Completed")
                {
                    // флаг «весь курс пройден»: все остальные кроме себя
                    var required = all
                        .Where(x => x.ModuleKey != "Course.Completed")
                        .Select(x => x.ModuleKey);

                    a.IsUnlocked = required.All(rk => doneKeys.Contains(rk));
                }
                else
                {
                    a.IsUnlocked = doneKeys.Contains(a.ModuleKey);
                }
            }

            return View(all);
        }
    }
}
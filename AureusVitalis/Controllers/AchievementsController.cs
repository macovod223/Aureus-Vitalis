using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AureusVitalis.Models;
using AureusVitalis.Services;

namespace AureusVitalis.Controllers
{

    public class AchievementsController : Controller
    {
       public IActionResult Index()
{
    var achievements = new List<AchievementModel>
    {
        new AchievementModel
        {
            Title = "Это только начало...",
            Description = "Пройти блок теории по питанию",
            IsUnlocked = true, // или false, если не получено
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Поварёнок",
            Description = "Пройти блок практики по питанию",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Знаток питания",
            Description = "Пройти тест по питанию",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "В движении сила",
            Description = "Пройти блок теории по спорту",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Тренировочный день",
            Description = "Пройти блок практики по спорту",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Мастер спорта",
            Description = "Пройти тест по спорту",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Сон — залог успеха",
            Description = "Пройти блок теории для сна",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Гуру сна",
            Description = "Пройти блок практики для сна",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        },
        new AchievementModel
        {
            Title = "Победитель марафона",
            Description = "Пройти финальный экзамен",
            IsUnlocked = false,
            Icon = "/images/trophy.png"
        }
    };

    return View(achievements);
        }
    }
}
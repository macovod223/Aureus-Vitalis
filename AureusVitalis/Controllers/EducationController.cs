using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using AureusVitalis.Data;
using AureusVitalis.Data.Entities;
using AureusVitalis.Models;
using Microsoft.AspNetCore.Authorization;

namespace AureusVitalis.Controllers
{
    [Authorize]
    public class EducationController : Controller
    {
        private readonly AppDbContext _db;
        public EducationController(AppDbContext db) => _db = db;

        private int CurrentUserId =>
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        private int ComputeProgress() => GetUserProgress(CurrentUserId);

        private int GetUserProgress(int userId)
        {
            string[] modules =
            {
                "Nutrition", "Nutrition.Practice", "Nutrition.Test",
                "Sport",     "Sport.Practice",     "Sport.Test",
                "Sleep",     "Sleep.Practice",
                "FinalExam"
            };

            int done = _db.UserProgress
                .Where(p => p.UserId == userId && p.IsCompleted)
                .Select(p => p.ModuleKey)
                .Distinct()
                .Count(m => modules.Contains(m));

            int total = modules.Length;

            return total == 0 ? 0 : done * 100 / total;
        }
        
        private void EnsureTheoryCompleted(string moduleKey)
        {
            var userId = CurrentUserId;
            var prog = _db.UserProgress
                .FirstOrDefault(p => p.UserId == userId && p.ModuleKey == moduleKey);

            if (prog == null)
            {
                _db.UserProgress.Add(new UserProgress
                {
                    UserId      = userId,
                    ModuleKey   = moduleKey,
                    IsCompleted = true,
                    UpdatedAt   = DateTime.UtcNow
                });
                _db.SaveChanges();
            }
        }

        private static readonly List<PracticeTask> NutritionPracticeTasks = new()
        {
            new PracticeTask
            {
                Question = "Что из перечисленного является источником белка?",
                Options = new List<string> { "Курица", "Хлеб", "Молоко", "Масло" },
                CorrectAnswer = "Курица",
                Explanation = "Курица — отличный источник белка."
            }
        };

        private static readonly List<TestQuestion> NutritionTestQuestions = new()
        {
            new TestQuestion
            {
                Question = "Сколько порций овощей и фруктов рекомендуется есть в день?",
                Options  = new List<string> { "2-3", "5-7", "10-12", "1-2" },
                CorrectAnswer = "5-7"
            },
            new TestQuestion
            {
                Question = "Василий Зайцев ничего не делал по проекту",
                Options  = new List<string> { "Не верю", "Нет", "Почему", "Очевидно" },
                CorrectAnswer = "Очевидно"
            },
        };
        
        private static readonly List<TestQuestion> SportTestQuestions = new()
{
    new TestQuestion {
        Question      = "Какова минимальная рекомендуемая продолжительность умеренной физической активности в неделю для взрослого человека?",
        Options       = new List<string> { "90 минут", "150 минут", "200 минут", "120 минут" },
        CorrectAnswer = "150 минут"
    },
    new TestQuestion {
        Question      = "Какие упражнения относятся к кардио тренировкам?",
        Options       = new List<string> { "Бег", "Отжимания", "Плавание", "Велосипед" },
        // несколько правильных — разделяем точкой с запятой
        CorrectAnswer = "Бег;Плавание;Велосипед"
    },
    new TestQuestion {
        Question      = "Как определить умеренную интенсивность тренировки?",
        Options       = new List<string> {
            "Можете легко петь во время тренировки",
            "Можете разговаривать, но не петь",
            "Не можете говорить во время тренировки",
            "Чувствуете сильную усталость"
        },
        CorrectAnswer = "Можете разговаривать, но не петь"
    },
    new TestQuestion {
        Question      = "Какие преимущества дают силовые тренировки?",
        Options       = new List<string> {
            "Укрепление мышц",
            "Улучшение метаболизма",
            "Укрепление костей",
            "Повышение гибкости"
        },
        CorrectAnswer = "Укрепление мышц;Улучшение метаболизма;Укрепление костей"
    },
    new TestQuestion {
        Question      = "Что важно сделать перед началом тренировки?",
        Options       = new List<string> {
            "Плотно поесть",
            "Выполнить разминку",
            "Выпить много воды",
            "Принять холодный душ"
        },
        CorrectAnswer = "Выполнить разминку"
    },
};
        
        private static readonly List<TestQuestion> FinalExamQuestions = new()
    {
        // Раздел 1: Питание
        new TestQuestion {
            Question      = "Какие из следующих продуктов являются хорошими источниками белка?",
            Options       = new List<string> { "Куриная грудка", "Чечевица", "Яйца", "Белый хлеб" },
            CorrectAnswer = "Куриная грудка;Чечевица;Яйца"
        },
        new TestQuestion {
            Question      = "Сколько порций овощей и фруктов рекомендуется употреблять ежедневно?",
            Options       = new List<string> { "2-3 порции", "5-7 порций", "8-10 порций", "3-4 порции" },
            CorrectAnswer = "5-7 порций"
        },
        // Раздел 2: Спорт
        new TestQuestion {
            Question      = "Выберите термины относящиеся к тренировкам:",
            Options       = new List<string> { "Разминка", "Перекус", "Заминка", "Растяжка" },
            CorrectAnswer = "Разминка;Заминка;Растяжка"
        },
        new TestQuestion {
            Question      = "Какие утверждения о кардио тренировках верны?",
            Options       = new List<string> {
                "Улучшают работу сердечно-сосудистой системы",
                "Помогают сжигать калории",
                "Увеличивают мышечную массу",
                "Повышают выносливость"
            },
            CorrectAnswer = "Улучшают работу сердечно-сосудистой системы;Помогают сжигать калории;Повышают выносливость"
        },
        // Раздел 3: Сон
        new TestQuestion {
            Question      = "Какие факторы негативно влияют на качество сна?",
            Options       = new List<string> {
                "Использование смартфона перед сном",
                "Кофеин во второй половине дня",
                "Регулярный режим сна",
                "Тяжелая пища перед сном"
            },
            CorrectAnswer = "Использование смартфона перед сном;Кофеин во второй половине дня;Тяжелая пища перед сном"
        },
        new TestQuestion {
            Question      = "Какова оптимальная продолжительность сна для взрослого человека?",
            Options       = new List<string> { "5-6 часов", "7-9 часов", "10-12 часов", "4-5 часов" },
            CorrectAnswer = "7-9 часов"
        }
    };

        [HttpPost]
        public JsonResult SubmitFinalExam([FromBody] List<AnswerDto> answers)
        {
            var userId = CurrentUserId;
            var attempt = new UserExamAttempt { UserId = userId, StartedAt = DateTime.UtcNow };
            _db.UserExamAttempts.Add(attempt);
            _db.SaveChanges();

            int correctCount = 0;
            foreach (var a in answers)
            {
                if (!a.QuestionKey.StartsWith("question_")) continue;
                if (!int.TryParse(a.QuestionKey.Substring("question_".Length), out var idx)) continue;
                if (idx < 1 || idx > FinalExamQuestions.Count) continue;

                var q        = FinalExamQuestions[idx - 1];
                var corrects = q.CorrectAnswer.Split(';').OrderBy(x => x).ToArray();
                var userVals = a.Selected.Split(';').OrderBy(x => x).ToArray();
                var isCorrect = corrects.SequenceEqual(userVals);
                if (isCorrect) correctCount++;

                _db.UserTestResults.Add(new UserTestResult {
                    AttemptId   = attempt.Id,
                    QuestionKey = q.Question,
                    IsCorrect   = isCorrect,
                    AnsweredAt  = DateTime.UtcNow
                });
            }

            attempt.CompletedAt = DateTime.UtcNow;
            var total = answers.Count;
            attempt.Score = total > 0 ? 100 * correctCount / total : 0;

            var prog = _db.UserProgress.FirstOrDefault(up =>
                up.UserId == userId && up.ModuleKey == "FinalExam");
            if (prog == null)
            {
                _db.UserProgress.Add(new UserProgress {
                    UserId      = userId,
                    ModuleKey   = "FinalExam",
                    IsCompleted = attempt.Score >= 80,
                    UpdatedAt   = DateTime.UtcNow
                });
            }
            else
            {
                prog.IsCompleted = attempt.Score >= 80;
                prog.UpdatedAt   = DateTime.UtcNow;
            }

            _db.SaveChanges();

            return Json(new {
                correctAnswers = correctCount,
                totalQuestions = total,
                percentage     = attempt.Score,
                passed         = attempt.Score >= 80
            });
        }


// 2) метод приёма ответов из SportTest.cshtml
[HttpPost]
public JsonResult SubmitSportTest([FromBody] List<AnswerDto> answers)
{
    var userId = CurrentUserId;

    // 2.1) создаём попытку
    var attempt = new UserExamAttempt {
        UserId    = userId,
        StartedAt = DateTime.UtcNow
    };
    _db.UserExamAttempts.Add(attempt);
    _db.SaveChanges();

    // 2.2) проверяем ответы
    int correctCount = 0;
    foreach (var a in answers)
    {
        if (!a.QuestionKey.StartsWith("question_")) continue;
        if (!int.TryParse(a.QuestionKey.Substring("question_".Length), out var idx)) continue;
        if (idx < 1 || idx > SportTestQuestions.Count) continue;

        var q      = SportTestQuestions[idx - 1];
        var correctSet = q.CorrectAnswer.Split(';').OrderBy(x => x).ToArray();
        var userSet    = a.Selected.Split(';').OrderBy(x => x).ToArray();
        var isCorrect  = correctSet.SequenceEqual(userSet);
        if (isCorrect) correctCount++;

        _db.UserTestResults.Add(new UserTestResult {
            AttemptId   = attempt.Id,
            QuestionKey = q.Question,
            IsCorrect   = isCorrect,
            AnsweredAt  = DateTime.UtcNow
        });
    }

    // 2.3) завершаем попытку
    attempt.CompletedAt = DateTime.UtcNow;
    var total = answers.Count;
    attempt.Score = total > 0 ? 100 * correctCount / total : 0;

    // 2.4) сохраняем прогресс
    var prog = _db.UserProgress
        .FirstOrDefault(up => up.UserId == userId && up.ModuleKey == "Sport.Test");
    if (prog == null)
    {
        _db.UserProgress.Add(new UserProgress {
            UserId      = userId,
            ModuleKey   = "Sport.Test",
            IsCompleted = attempt.Score >= 70, // допустим 70% проходной
            UpdatedAt   = DateTime.UtcNow
        });
    }
    else
    {
        prog.IsCompleted = attempt.Score >= 70;
        prog.UpdatedAt   = DateTime.UtcNow;
    }

    _db.SaveChanges();

    // 2.5) возвращаем JSON
    return Json(new {
        correctAnswers = correctCount,
        totalQuestions = total,
        percentage     = attempt.Score,
        passed         = attempt.Score >= 70
    });
}


public IActionResult Main()
{
    var uid  = CurrentUserId;

    // какие модули пользователь уже закрыл
    var done = _db.UserProgress
        .Where(p => p.UserId == uid && p.IsCompleted)
        .Select(p => p.ModuleKey)
        .ToHashSet();

    // откроем ли финальный экзамен
    bool examAvailable = done.IsSupersetOf(new[]
    {
        "Nutrition.Practice","Nutrition.Test",
        "Sport.Practice","Sport.Test",
        "Sleep.Practice"
    });
    
    var blocks = new List<EducationBlock>
    {
        new() { Name="Nutrition",          Type="Learning", Url="/Education/Nutrition"          },
        new() { Name="Nutrition Practice", Type="Practice", Url="/Education/NutritionPractice" },
        new() { Name="Nutrition Test",     Type="Test",     Url="/Education/NutritionTest"     },
        new() { Name="Sport",              Type="Learning", Url="/Education/Sport"             },
        new() { Name="Sport Practice",     Type="Practice", Url="/Education/SportPractice"     },
        new() { Name="Sport Test",         Type="Test",     Url="/Education/SportTest"         },
        new() { Name="Sleep",              Type="Learning", Url="/Education/Sleep"             },
        new() { Name="Sleep Practice",     Type="Practice", Url="/Education/SleepPractice"     },
        new() { Name="Final Exam",         Type="Exam",     Url="/Education/FinalExam"         }
    };

    /* проставляем IsCompleted для каждой карточки */
    foreach (var b in blocks)
    {
        string key = b.Name switch
        {
            "Nutrition"           => "Nutrition",
            "Nutrition Practice"  => "Nutrition.Practice",
            "Nutrition Test"      => "Nutrition.Test",
            "Sport"               => "Sport",
            "Sport Practice"      => "Sport.Practice",
            "Sport Test"          => "Sport.Test",
            "Sleep"               => "Sleep",
            "Sleep Practice"      => "Sleep.Practice",
            "Final Exam"          => "FinalExam",
            _                     => ""
        };
        b.IsCompleted = done.Contains(key);
    }

    ViewBag.Progress         = GetUserProgress(uid);
    ViewBag.ExamAvailable    = examAvailable;
    ViewBag.CompletedModules = done;

    var model = new EducationModel { Title = "Education", Blocks = blocks };
    return View(model);
}

        public IActionResult Nutrition()
        {
            EnsureTheoryCompleted("Nutrition");
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new EducationContentModel
            {
                Title = "Основы правильного питания",
                Content = "<p>Здесь будет теория по питанию. Добавь сюда свой текст или HTML.</p>",
                Images = new List<string> { "/images/nutrition1.png" },
                Progress = progress
            };
            return View(model);
        }

        public IActionResult NutritionPractice()
        {
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new PracticeModel
            {
                Title = "Практика: Питание",
                Progress = progress,
                Tasks = NutritionPracticeTasks
            };
            return View(model);
        }

        public IActionResult NutritionTest()
        {
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new TestModel
            {
                Title = "Тест по питанию",
                TimeLimit = 10,
                Questions = NutritionTestQuestions,
                IsCompleted = false
            };
            return View(model);
        }

        public IActionResult Sport()
        {
            EnsureTheoryCompleted("Sport"); 
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new EducationContentModel
            {
                Title = "Физическая активность и здоровье",
                Content = "<p>Здесь будет теория по спорту.</p>",
                Images = new List<string> { "/images/sport1.png" },
                Progress = progress
            };
            return View(model);
        }

        public IActionResult SportPractice()
        {
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new PracticeModel
            {
                Title = "Практика: Физическая активность",
                Progress = progress,
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
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new TestModel
            {
                Title = "Тест по физической активности",
                TimeLimit = 10,
                Questions = SportTestQuestions,
                IsCompleted = false
                
            };
            return View(model);
        }

        public IActionResult Sleep()
        {
            EnsureTheoryCompleted("Sleep");
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new EducationContentModel
            {
                Title = "Здоровый сон",
                Content = "<p>Здесь будет теория по сну.</p>",
                Images = new List<string> { "/images/sleep1.png" },
                Progress = progress
            };
            return View(model);
        }

        public IActionResult SleepPractice()
        {
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            var model = new PracticeModel
            {
                Title = "Практика: Сон",
                Progress = progress,
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
        [HttpGet]
        public IActionResult FinalExam()
        {
            var progress = ComputeProgress();
            ViewBag.Progress = progress;
            ViewBag.ExamAvailable = progress >= 100;

            // Вот единственное изменение — передаём сразу весь список FinalExamQuestions
            var model = new ExamModel
            {
                Title               = "Финальный экзамен",
                TimeLimit           = 30,
                Questions           = FinalExamQuestions,
                IsAvailable         = ViewBag.ExamAvailable,
                IsCompleted         = false,
                MinimumPassingScore = 80
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult CheckPracticeAnswers([FromBody] Dictionary<string, string> answers)
        {
            var userId = CurrentUserId;
            var now = DateTime.UtcNow;
            var results = new Dictionary<string, bool>();

            foreach (var kv in answers)
            {
                var question = kv.Key;
                var selected = kv.Value;
                var task = NutritionPracticeTasks.FirstOrDefault(t => t.Question == question);
                var isCorrect = task != null && task.CorrectAnswer == selected;
                results[question] = isCorrect;

                var existing = _db.UserPracticeResults
                    .FirstOrDefault(r => r.UserId == userId && r.TaskKey == question);

                if (existing != null)
                {
                    existing.IsCorrect = isCorrect;
                    existing.AnsweredAt = now;
                }
                else
                {
                    _db.UserPracticeResults.Add(new UserPracticeResult
                    {
                        UserId = userId,
                        TaskKey = question,
                        IsCorrect = isCorrect,
                        AnsweredAt = now
                    });
                }
            }

            // прогресс ставим только когда все верно
            if (results.Values.All(v => v))
            {
                var prog = _db.UserProgress
                    .FirstOrDefault(up => up.UserId == userId
                                          && up.ModuleKey == "Nutrition.Practice");
                if (prog == null)
                {
                    _db.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        ModuleKey = "Nutrition.Practice",
                        IsCompleted = true,
                        UpdatedAt = now
                    });
                }
                else
                {
                    prog.IsCompleted = true;
                    prog.UpdatedAt = now;
                }
            }

            _db.SaveChanges();
            return Json(results);
        }

        public class SportPracticeDto
        {
            public List<DayPlanDto> WorkoutPlan { get; set; } = null!;
            public List<string> Sequence { get; set; } = null!;
        }

        public class DayPlanDto
        {
            public string Day { get; set; } = null!;
            public string Type { get; set; } = null!;
            public string Intensity { get; set; } = null!;
            public int Duration { get; set; }
        }

        [HttpPost]
        public JsonResult CheckSportPracticeAnswers([FromBody] SportPracticeDto dto)
        {
            var userId = CurrentUserId;
            var now = DateTime.UtcNow;

            // 1) проверяем план: минимум 3 тренировочных дня
            bool planValid = dto.WorkoutPlan.Count(p =>
                p.Type != "rest" &&
                !string.IsNullOrEmpty(p.Type) &&
                !string.IsNullOrEmpty(p.Intensity) &&
                p.Duration > 0
            ) >= 3;

            // 2) проверяем последовательность упражнений
            var correctSeq = new[]
            {
                "Разминка и разогрев мышц",
                "Основная часть тренировки",
                "Заминка и растяжка",
                "Восстановление и отдых"
            };
            bool seqValid = dto.Sequence.SequenceEqual(correctSeq);

            // 3) сохраняем оба результата в БД
            var results = new Dictionary<string, bool>
            {
                ["1"] = planValid,
                ["2"] = seqValid
            };
            foreach (var kv in results)
            {
                var key = $"Sport.Practice.{kv.Key}";
                var existing = _db.UserPracticeResults
                    .FirstOrDefault(r => r.UserId == userId && r.TaskKey == key);
                if (existing != null)
                {
                    existing.IsCorrect = kv.Value;
                    existing.AnsweredAt = now;
                }
                else
                {
                    _db.UserPracticeResults.Add(new UserPracticeResult
                    {
                        UserId = userId,
                        TaskKey = key,
                        IsCorrect = kv.Value,
                        AnsweredAt = now
                    });
                }
            }

            // 4) если обе части верны — отмечаем модуль как пройденный
            if (planValid && seqValid)
            {
                var prog = _db.UserProgress
                    .FirstOrDefault(up => up.UserId == userId && up.ModuleKey == "Sport.Practice");
                if (prog == null)
                {
                    _db.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        ModuleKey = "Sport.Practice",
                        IsCompleted = true,
                        UpdatedAt = now
                    });
                }
                else
                {
                    prog.IsCompleted = true;
                    prog.UpdatedAt = now;
                }
            }

            _db.SaveChanges();
            return Json(results);
        }

        public class SleepPracticeDto
        {
            public List<SleepDayDto> SleepEntries { get; set; } = null!;
            public List<string> SelectedFactors { get; set; } = null!;
        }

        public class SleepDayDto
        {
            public string Day { get; set; } = null!;
            public string SleepTime { get; set; } = null!;
            public string WakeTime { get; set; } = null!;
            public int Quality { get; set; }
        }

        [HttpPost]
        public JsonResult CheckSleepPracticeAnswers([FromBody] SleepPracticeDto dto)
        {
            var userId = CurrentUserId;
            var now = DateTime.UtcNow;

            // 1) Проверяем дневник: каждый день есть sleep, wake, quality и длительность 7–9 ч
            bool diaryValid = dto.SleepEntries.All(e =>
            {
                var sleep = DateTime.ParseExact(e.SleepTime, "HH:mm", null);
                var wake = DateTime.ParseExact(e.WakeTime, "HH:mm", null);
                var hours = (wake - sleep).TotalHours;
                return !double.IsNaN(hours) && hours >= 7 && hours <= 9 && e.Quality >= 1 && e.Quality <= 4;
            });

            // 2) Проверяем, что выбран хотя бы один фактор
            bool factorsValid = dto.SelectedFactors.Any();

            // 3) Сохраняем результаты в UserPracticeResults
            var results = new Dictionary<string, bool>
            {
                ["1"] = diaryValid,
                ["2"] = factorsValid
            };
            foreach (var kv in results)
            {
                var key = $"Sleep.Practice.{kv.Key}";
                var existing = _db.UserPracticeResults
                    .FirstOrDefault(r => r.UserId == userId && r.TaskKey == key);
                if (existing != null)
                {
                    existing.IsCorrect = kv.Value;
                    existing.AnsweredAt = now;
                }
                else
                {
                    _db.UserPracticeResults.Add(new UserPracticeResult
                    {
                        UserId = userId,
                        TaskKey = key,
                        IsCorrect = kv.Value,
                        AnsweredAt = now
                    });
                }
            }

            // 4) Если обе части прошли — отмечаем прогресс
            if (diaryValid && factorsValid)
            {
                var prog = _db.UserProgress
                    .FirstOrDefault(up => up.UserId == userId && up.ModuleKey == "Sleep.Practice");
                if (prog == null)
                {
                    _db.UserProgress.Add(new UserProgress
                    {
                        UserId = userId,
                        ModuleKey = "Sleep.Practice",
                        IsCompleted = true,
                        UpdatedAt = now
                    });
                }
                else
                {
                    prog.IsCompleted = true;
                    prog.UpdatedAt = now;
                }
            }

            _db.SaveChanges();
            return Json(results);
        }

        public class AnswerDto
        {
            public string QuestionKey { get; set; } = null!;
            public string Selected { get; set; } = null!;
        }

        [HttpPost]
        public JsonResult SubmitTest([FromBody] List<AnswerDto> answers)
        {
            var userId = CurrentUserId;

            // 1) Начинаем попытку
            var attempt = new UserExamAttempt
            {
                UserId = userId,
                StartedAt = DateTime.UtcNow
            };
            _db.UserExamAttempts.Add(attempt);
            _db.SaveChanges();

            // 2) Проверяем ответы
            int correctCount = 0;
            for (int i = 0; i < answers.Count; i++)
            {
                var dto = answers[i];
                // имя приходит в виде "question_0", "question_1" и т. д.
                if (!dto.QuestionKey.StartsWith("question_"))
                    continue;

                if (!int.TryParse(dto.QuestionKey.Substring("question_".Length), out var idx))
                    continue;

                if (idx < 0 || idx >= NutritionTestQuestions.Count)
                    continue;

                var question = NutritionTestQuestions[idx];
                var isCorrect = question.CorrectAnswer == dto.Selected;
                if (isCorrect) correctCount++;

                _db.UserTestResults.Add(new UserTestResult
                {
                    AttemptId = attempt.Id,
                    QuestionKey = question.Question, // сохраняем текст вопроса как ключ
                    IsCorrect = isCorrect,
                    AnsweredAt = DateTime.UtcNow
                });
            }

            // 3) Завершаем попытку
            attempt.CompletedAt = DateTime.UtcNow;
            var total = answers.Count;
            attempt.Score = total > 0
                ? 100 * correctCount / total
                : 0;

            // 4) Добавляем или обновляем прогресс модуля Nutrition.Test
            var prog = _db.UserProgress
                .FirstOrDefault(up => up.UserId == userId
                                      && up.ModuleKey == "Nutrition.Test");
            if (prog == null)
            {
                _db.UserProgress.Add(new UserProgress
                {
                    UserId = userId,
                    ModuleKey = "Nutrition.Test",
                    IsCompleted = attempt.Score >= 80,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                prog.IsCompleted = attempt.Score >= 80;
                prog.UpdatedAt = DateTime.UtcNow;
            }

            _db.SaveChanges();

            // 5) Возвращаем JSON, который ждёт фронтенд
            var percentage = total > 0
                ? 100 * correctCount / total
                : 0;
            return Json(new
            {
                correctAnswers = correctCount,
                totalQuestions = total,
                percentage = percentage,
                passed = (percentage >= 80)
            });
        }
    }
}
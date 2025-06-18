using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using AureusVitalis.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AureusVitalis.Controllers
{
    public class DailyHelperController : Controller
    {
        public IActionResult Selection()
        {
            return View(new DailyHelperModel());
        }

        [HttpGet]
        public IActionResult Calculator()
        {
            return View(new CalculatorModel());
        }
        
        [HttpPost]
        public IActionResult Calculator(CalculatorModel model)
        {
            ModelState.Remove(nameof(model.Allergens));
            ModelState.Remove(nameof(model.DietPlan));
            ModelState.Remove(nameof(model.SleepSchedule));

            if (!ModelState.IsValid)
                return View(model);

            var perMeal = model.DailyCalories / 3;
            var avoid = (model.Allergens?.Any() ?? false)
                ? $"Избегайте: {string.Join(", ", model.Allergens)}."
                : "Без ограничений по аллергенам.";

            model.DietPlan =
                $"Ваш план питания ({model.DailyCalories} kcal):\n" +
                $"- Завтрак: ~{perMeal} kcal\n" +
                $"- Обед: ~{perMeal} kcal\n" +
                $"- Ужин: ~{perMeal} kcal\n" +
                $"{avoid}";

            var duration = model.SleepType?.ToLower() == "short"
                ? TimeSpan.FromMinutes(270)
                : TimeSpan.FromHours(8);

            var wake = model.SleepTime + duration;
            if (wake.TotalHours >= 24)
                wake = wake - TimeSpan.FromHours(24);

            model.SleepSchedule =
                $"Спать: {model.SleepTime:hh\\:mm}, " +
                $"проснуться: {wake:hh\\:mm} ({duration.Hours}h {duration.Minutes}m).";

            return View(model);
        }

        public IActionResult Exercises()
        {
            return View(new ExerciseModel());
        }

        [HttpPost]
        public IActionResult Exercises(ExerciseModel model)
        {
            ModelState.Remove(nameof(model.GeneratedExercises));
            if (ModelState.IsValid)
            {
                // Пример генерации упражнений
                model.GeneratedExercises = GenerateExercises(model);
            }

            return View(model);
        }

        private List<Exercise> GenerateExercises(ExerciseModel model)
{
    var exercises = new List<Exercise>();
    var gender = model.Gender;
    var ageFactor = model.Age > 50 ? 0.8 : 1.0;

    switch (model.MuscleGroup.ToLower())
    {
        case "fullbody":
            exercises.Add(new Exercise { Name = "Берпи (Burpee)", Repetitions = Scale(10), Recommendations = "Делайте медленно, фокусируясь на технике" });
            exercises.Add(new Exercise { Name = "Приседания с выпрыгиванием", Repetitions = Scale(12), Recommendations = "Приземляйтесь мягко, спина прямая" });
            exercises.Add(new Exercise { Name = "Медвежья ходьба", Repetitions = Scale(20), Recommendations = "Двигайтесь уверенно, корпус стабилен" });
            exercises.Add(new Exercise { Name = "Альпинист (Mountain Climbers)", Repetitions = Scale(30), Recommendations = "Следите, чтобы бедра не поднимались выше спины" });
            exercises.Add(new Exercise { Name = "Планка с подтягиванием коленей", Repetitions = Scale(15), Recommendations = "Контролируйте движение, держите корпус ровным" });
            break;

        case "upperbody":
            exercises.Add(new Exercise { Name = "Отжимания классические", Repetitions = Scale(gender == "M" ? 15 : 10), Recommendations = "Локти близко к корпусу" });
            exercises.Add(new Exercise { Name = "Отжимания широким хватом", Repetitions = Scale(gender == "M" ? 12 : 8), Recommendations = "Работают грудные мышцы" });
            exercises.Add(new Exercise { Name = "Обратные отжимания от стула", Repetitions = Scale(12), Recommendations = "Опускайтесь медленно" });
            exercises.Add(new Exercise { Name = "Отжимания на брусьях (или между стульями)", Repetitions = Scale(8), Recommendations = "Максимально раздвиньте локти" });
            exercises.Add(new Exercise { Name = "Подтягивания (с резинкой или ассистом)", Repetitions = Scale(6), Recommendations = "Свобода движений, старайтесь тянуть локти вниз" });
            break;

        case "lowerbody":
        case "legs":
            exercises.Add(new Exercise { Name = "Приседания со штангой (или с весом тела)", Repetitions = Scale(15), Recommendations = "Глубоко садитесь, колени не выходят за носки" });
            exercises.Add(new Exercise { Name = "Выпады вперёд", Repetitions = Scale(12), Recommendations = "Колено не касается пола" });
            exercises.Add(new Exercise { Name = "Румынская тяга на одной ноге", Repetitions = Scale(10), Recommendations = "Постарайтесь держать спину ровной" });
            exercises.Add(new Exercise { Name = "Ягодичный мостик", Repetitions = Scale(20), Recommendations = "Внизу полностью опускайте таз" });
            exercises.Add(new Exercise { Name = "Болгарские выпады", Repetitions = Scale(10), Recommendations = "Задняя нога на скамье, корпус прямой" });
            break;

        case "core":
            exercises.Add(new Exercise { Name = "Планка", Repetitions = (int)(30 * ageFactor), Recommendations = "Держите корпус параллельно полу" });
            exercises.Add(new Exercise { Name = "Боковая планка (на каждую сторону)", Repetitions = (int)(20 * ageFactor), Recommendations = "Не прогибайтесь в пояснице" });
            exercises.Add(new Exercise { Name = "Скручивания классические", Repetitions = Scale(15), Recommendations = "Поднимайте плечи, не тяните шею" });
            exercises.Add(new Exercise { Name = "Велосипед (скручивания с разведением локтей)", Repetitions = Scale(20), Recommendations = "Контролируйте движение" });
            exercises.Add(new Exercise { Name = "Подъёмы ног в висе или лёжа", Repetitions = Scale(12), Recommendations = "Не прогибайте поясницу" });
            break;

        case "arms":
            exercises.Add(new Exercise { Name = "Сгибания рук со штангой/гантелями", Repetitions = Scale(12), Recommendations = "Локти зафиксированы" });
            exercises.Add(new Exercise { Name = "Французский жим лёжа", Repetitions = Scale(10), Recommendations = "Контролируйте траекторию" });
            exercises.Add(new Exercise { Name = "Молотковые сгибания", Repetitions = Scale(12), Recommendations = "Запястья прямые" });
            exercises.Add(new Exercise { Name = "Разгибания рук на блоке", Repetitions = Scale(15), Recommendations = "Полный амплитудный ход" });
            exercises.Add(new Exercise { Name = "Удержание веса в положении 90°", Repetitions = Scale(30), Recommendations = "Статическое напряжение" });
            break;

        case "back":
            exercises.Add(new Exercise { Name = "Тяга штанги в наклоне", Repetitions = Scale(12), Recommendations = "Спина ровная, локти тянутся назад" });
            exercises.Add(new Exercise { Name = "Тяга верхнего блока к груди", Repetitions = Scale(12), Recommendations = "Лопатки сводите вместе" });
            exercises.Add(new Exercise { Name = "Тяга гантели в наклоне", Repetitions = Scale(10), Recommendations = "Работайте медленно" });
            exercises.Add(new Exercise { Name = "Гиперэкстензии", Repetitions = Scale(15), Recommendations = "Не задирайте излишне корпус" });
            exercises.Add(new Exercise { Name = "Подтягивания обратным хватом", Repetitions = Scale(8), Recommendations = "Локти вдоль тела" });
            break;

        case "chest":
            exercises.Add(new Exercise { Name = "Жим штанги лёжа", Repetitions = Scale(10), Recommendations = "Контролируйте опускание" });
            exercises.Add(new Exercise { Name = "Жим гантелей на наклонной скамье", Repetitions = Scale(12), Recommendations = "Скамья 30°-45°" });
            exercises.Add(new Exercise { Name = "Разводка гантелей лёжа", Repetitions = Scale(15), Recommendations = "Руку ведите чуть ниже уровня плеч" });
            exercises.Add(new Exercise { Name = "Отжимания на скамье", Repetitions = Scale(20), Recommendations = "Ноги на скамье, корпус ровный" });
            exercises.Add(new Exercise { Name = "Пуловер с гантелью", Repetitions = Scale(12), Recommendations = "Рука слегка согнута" });
            break;

        default:
            // на случай неизвестной группы — просто базовый комплекс
            exercises.Add(new Exercise { Name = "Планка", Repetitions = Scale(30), Recommendations = "Держите корпус в одной линии" });
            exercises.Add(new Exercise { Name = "Берпи", Repetitions = Scale(8), Recommendations = "Выполняйте осторожно" });
            break;
    }

    // Применяем возрастной коэффициент к повторениям
    foreach (var ex in exercises)
    {
        ex.Repetitions = (int)(ex.Repetitions * ageFactor);
    }

    return exercises;

    // локальная функция для удобства
    int Scale(int baseCount) => baseCount;
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
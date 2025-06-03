using System.ComponentModel.DataAnnotations;

namespace AureusVitalis.Models
{
    public class CalculatorModel
    {
        [Required(ErrorMessage = "Please select daily calories")]
        public int DailyCalories { get; set; }

        [Required(ErrorMessage = "Please select allergens")]
        public List<string> Allergens { get; set; }

        [Required(ErrorMessage = "Please select sleep type")]
        public string SleepType { get; set; }

        [Required(ErrorMessage = "Please select sleep time")]
        public TimeSpan SleepTime { get; set; }

        public string DietPlan { get; set; }
        public string SleepSchedule { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace AureusVitalis.Models
{
    public class ExerciseModel
    {
        [Required(ErrorMessage = "Please select your gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(14, 100, ErrorMessage = "Age must be between 14 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Weight is required")]
        [Range(30, 200, ErrorMessage = "Weight must be between 30 and 200 kg")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Please select a muscle group")]
        public string MuscleGroup { get; set; }

        public List<Exercise> GeneratedExercises { get; set; }
    }

    public class Exercise
    {
        public string Name { get; set; }
        public int Repetitions { get; set; }
        public string Recommendations { get; set; }
    }
}
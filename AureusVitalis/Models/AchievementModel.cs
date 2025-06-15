namespace AureusVitalis.Models
{
    public class AchievementModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsUnlocked { get; set; }
        public string Icon { get; set; } // путь к иконке, если нужно
    }
}
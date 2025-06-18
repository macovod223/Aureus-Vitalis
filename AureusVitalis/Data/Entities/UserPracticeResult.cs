namespace AureusVitalis.Data.Entities {
    public class UserPracticeResult {
        public int     UserId     { get; set; }
        public string  TaskKey    { get; set; } = null!;
        public bool    IsCorrect  { get; set; }
        public DateTime AnsweredAt { get; set; }

        // навигация
        public AppUser User { get; set; } = null!;
    }
}
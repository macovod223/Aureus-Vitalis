namespace AureusVitalis.Data.Entities {
    public class UserExamAttempt {
        public int      Id          { get; set; }
        public int      UserId      { get; set; }
        public DateTime StartedAt   { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int?     Score       { get; set; }      // процент

        // навигация
        public AppUser               User        { get; set; } = null!;
        public ICollection<UserTestResult> TestResults { get; set; } = new List<UserTestResult>();
    }
}
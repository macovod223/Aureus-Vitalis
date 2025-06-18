namespace AureusVitalis.Data.Entities {
    public class UserTestResult {
        public int     AttemptId    { get; set; }
        public string  QuestionKey  { get; set; } = null!;
        public bool    IsCorrect    { get; set; }
        public DateTime AnsweredAt  { get; set; }

        // навигация
        public UserExamAttempt Attempt { get; set; } = null!;
    }
}
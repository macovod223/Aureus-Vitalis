namespace AureusVitalis.Data.Entities {
    public class AppUser {
        public int    Id           { get; set; }
        public string Email        { get; set; } = null!;
        public string Username     { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Gender       { get; set; } = null!;
        public DateTime CreatedAt  { get; set; }  // теперь у вас хранится время регистрации

        // Навигация
        public ICollection<UserProgress>       Progresses      { get; set; } = new List<UserProgress>();
        public ICollection<UserPracticeResult> PracticeResults { get; set; } = new List<UserPracticeResult>();
        public ICollection<UserExamAttempt>    ExamAttempts    { get; set; } = new List<UserExamAttempt>();
        public ICollection<Certificate>        Certificates    { get; set; } = new List<Certificate>();
    }
}
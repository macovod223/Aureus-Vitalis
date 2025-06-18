namespace AureusVitalis.Data.Entities {
    public class UserProgress {
        public int     UserId      { get; set; }
        public string  ModuleKey   { get; set; } = null!;
        public bool    IsCompleted { get; set; }
        public DateTime UpdatedAt  { get; set; }

        public AppUser User { get; set; } = null!;
    }
}
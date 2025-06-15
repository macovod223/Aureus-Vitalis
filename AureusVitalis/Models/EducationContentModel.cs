// Models/EducationContentModel.cs
namespace AureusVitalis.Models
{
    public class EducationContentModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public List<string> Images { get; set; }
        public int Progress { get; set; }
    }

    public class PracticeModel
    {
        public string Title { get; set; }
        public List<PracticeTask> Tasks { get; set; }
        public int Progress { get; set; }
    }

    public class PracticeTask
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
    }

    public class TestModel
    {
        public string Title { get; set; }
        public List<TestQuestion> Questions { get; set; }
        public int TimeLimit { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class TestQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
    }

    public class ExamModel
    {
        public string Title { get; set; }
        public List<TestQuestion> Questions { get; set; }
        public int TimeLimit { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsCompleted { get; set; }
        public int MinimumPassingScore { get; set; }
    }
}
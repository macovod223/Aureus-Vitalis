using System.ComponentModel.DataAnnotations;

namespace AureusVitalis.Models
{
    public class EducationModel
    {
        public string Title { get; set; }
        public List<EducationBlock> Blocks { get; set; }
    }

    
    public class EducationBlock
    {
        public string Name  { get; set; } = null!;
        public string Type  { get; set; } = null!;
        public string Url   { get; set; } = null!;
        public bool   IsCompleted { get; set; }
    }
}
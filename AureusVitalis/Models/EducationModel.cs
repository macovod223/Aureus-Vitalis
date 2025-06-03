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
        public string Name { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
    }
}
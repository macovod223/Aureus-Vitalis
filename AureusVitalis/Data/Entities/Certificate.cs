namespace AureusVitalis.Data.Entities;

public class Certificate
{
    public int      Id        { get; set; }
    public int      UserId    { get; set; }
    public string   CourseKey { get; set; } = null!;   // «AV_Base_v1» и т.п.
    public DateTime IssuedAt  { get; set; }
    public string   FilePath  { get; set; } = null!;

    public AppUser  User      { get; set; } = null!;
}
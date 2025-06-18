namespace AureusVitalis.Infrastructure;
public sealed class EmailOptions
{
    public string Server    { get; set; } = null!;
    public int    Port      { get; set; }
    public string User      { get; set; } = null!;
    public string Pass      { get; set; } = null!;
    public string From      { get; set; } = null!;
    public string FromName  { get; set; } = "Aureus & Vitalis";
}
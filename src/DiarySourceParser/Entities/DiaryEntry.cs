namespace DiarySourceParser.Entities;

public class DiaryEntry
{
    public DateTime Date { get; set; }
    public List<string> Lines { get; set; } = new List<string>();
}
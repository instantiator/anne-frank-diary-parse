namespace DiarySourceParser.Tasks;

public class SpellingFixer
{
    public static Dictionary<string,string> Fixes = new Dictionary<string, string>()
    {
        { "2g", "29" },
        { "APRIF", "APRIL" }
    };

    public static string Fix(string line)
    {
        var fixedLine = line;
        foreach (var word in Fixes.Keys.Where(k => line.Contains(k, StringComparison.OrdinalIgnoreCase)))
        {
            fixedLine = fixedLine.Replace(word, Fixes[word], StringComparison.OrdinalIgnoreCase);
        }
        return fixedLine;
    }

}
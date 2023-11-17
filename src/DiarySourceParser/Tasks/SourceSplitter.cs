using System.Globalization;
using DiarySourceParser.Entities;

namespace DiarySourceParser.Tasks;

public class SourceSplitter
{
    public static IEnumerable<DiaryEntry> Split(IEnumerable<string> allLines)
    {
        var entries = new List<DiaryEntry>();
        DiaryEntry? currentEntry = null;

        foreach (var line in allLines.Select(l => l.Trim()))
        {
            var date = ParseDateLine(line);
            if (date != null)
            {
                if (currentEntry != null)
                {
                    entries.Add(currentEntry);
                }
                
                currentEntry = new DiaryEntry()
                {
                    Date = ParseDateLine(line)!.Value
                };
            }
            else
            {
                currentEntry?.Lines.Add(line);
            }
        }
        entries.Add(currentEntry!); // last entry
        return entries;
    }

    // Diary entry dates in the source text are found alone on a line.
    // They are roughly in the format DAYOFWEEK, MONTH DAY, YEAR
    // The only years are 1942, 1943, 1944

    // Examples from the source:
    // SUNDAY, JUNE 14, 1942
    // June 12, 1942
    // SATURDAY, JUNE 20,1942
    // SUNDAY, JUNE 21, 1942
    // MONDAY, NOVEMBER 9,1942

    private static DateTime? ParseDateLine(string line)
    {
        var trimmed = line.Trim();
        if (trimmed.EndsWith("1942") || trimmed.EndsWith("1943") || trimmed.EndsWith("1944"))
        {
            var parts = trimmed.Split(',');
            var year = int.Parse(parts[^1].Trim());
            var monthDayPart = parts[^2].Trim();
            var monthPart = monthDayPart.Split(' ')[0];
            var dayPart = monthDayPart.Split(' ')[1];
            var month = DateTime.ParseExact(monthPart, "MMMM", CultureInfo.InvariantCulture).Month;
            var day = int.Parse(dayPart);
            return new DateTime(year, month, day);
        }
        else
        {
            return null;
        }
    }

}
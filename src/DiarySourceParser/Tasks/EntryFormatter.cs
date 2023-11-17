using DiarySourceParser.Entities;

namespace DiarySourceParser.Tasks;

public class EntryFormatter
{
    public static List<string> FormatParagraphs(DiaryEntry entry)
    {
        var paragraphs = new List<string>();

        var currentParagraph = string.Empty;
        int blankLineCounter = 0;
        foreach (var line in entry.Lines.Select(l => l.Trim()))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                blankLineCounter++;
            }
            else
            {
                if (blankLineCounter == 0)
                {
                    // not preceded by a blank line, just add it to the current paragraph
                    if (string.IsNullOrWhiteSpace(currentParagraph))
                    {
                        currentParagraph = line;
                    }
                    else
                    {
                        currentParagraph += $" {line}";
                    }
                }
                else
                {
                    // 1 blank line or more
                    if (currentParagraph.EndsWith('.') ||
                        currentParagraph.EndsWith('?') ||
                        currentParagraph.EndsWith('!') ||
                        currentParagraph.EndsWith("Kitty,"))
                    {
                        // start a new paragraph IFF the current paragraph ends in a period
                        paragraphs.Add(currentParagraph);
                        currentParagraph = line;
                    }
                    else
                    {
                        // just add the line to the current paragraph - the extra blank lines were an OCR error
                        if (string.IsNullOrWhiteSpace(currentParagraph))
                        {
                            currentParagraph = line;
                        }
                        else
                        {
                            currentParagraph += $" {line}";
                        }
                    }
                }
                blankLineCounter = 0;
            }
        }
        paragraphs.Add(currentParagraph); // last paragraph
        return paragraphs;
    }


}

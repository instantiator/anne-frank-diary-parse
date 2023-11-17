using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using CommandLine;
using DiarySourceParser.Entities;
using DiarySourceParser.Tasks;

namespace DiarySourceParser;

public class Program
{
    private static readonly JsonSerializerOptions jsonOptions = new JsonSerializerOptions()
    {
        WriteIndented = true
    };

    public class SharedOptions
    {
        [Option('s', "source", Required = false, HelpText = "Source file, containing diary entries, text format")]
        public string? SourcePath { get; set; } = null;
    }

    [Verb("spelling", HelpText = "Correct known spelling issues")]
    public class SpellingOptions : SharedOptions
    {
    }

    [Verb("split", HelpText = "Split the source material into dated entries")]
    public class SplitOptions : SharedOptions
    {
    }

    [Verb("reformat", HelpText = "Reformat diary entries")]
    public class ReformatOptions : SharedOptions
    {
    }

    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<SpellingOptions, SplitOptions, ReformatOptions>(args)
            .WithParsed<SpellingOptions>(MakeSpellingCorrections)
            .WithParsed<SplitOptions>(SplitSource)
            .WithParsed<ReformatOptions>(ReformatParagraphs);
    }

    private static void MakeSpellingCorrections(SpellingOptions options)
    {
        var lines = GetInputLines(options);
        var fixedLines = lines.Select(SpellingFixer.Fix);
        Console.WriteLine(string.Join(Environment.NewLine, fixedLines));
    }

    private static void SplitSource(SplitOptions options)
    {
        var lines = GetInputLines(options);
        var entries = SourceSplitter.Split(lines);
        Console.WriteLine(JsonSerializer.Serialize(entries, jsonOptions));
    }

    private static void ReformatParagraphs(ReformatOptions options)
    {
        var lines = GetInputLines(options);
        var json = string.Join('\n', lines);
        var entries = JsonSerializer.Deserialize<List<DiaryEntry>>(json)!;
        var reformatted = entries.Select(entry => new DiaryEntry() { Date = entry.Date, Lines = EntryFormatter.FormatParagraphs(entry) });
        Console.WriteLine(JsonSerializer.Serialize(reformatted, jsonOptions));
    }

    private static IEnumerable<string> GetInputLines(SharedOptions options)
    {
        if (options.SourcePath == null)
        {
            var data = Console.In.ReadToEnd();
            return data.Split(Environment.NewLine);
        }
        else
        {
            return File.ReadAllLines(options.SourcePath);
        }
    }
}

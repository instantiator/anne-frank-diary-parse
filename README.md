# Diary of Anne Frank

A set of scripts to clean up the Internet Archive OCR of Anne Frank's "Diary of a Young Girl", and generate a structured dataset of formatted entries by date.

## Source material

The source material for these scripts comes from the Internet Archive:

* [Anne Frank The Diary Of A Young Girl](https://archive.org/details/AnneFrankTheDiaryOfAYoungGirl_201606)

If you're intending to use or publish anything derived from this work, please keep an eye on the latest information regarding [copyright](https://en.wikipedia.org/wiki/The_Diary_of_a_Young_Girl#Copyright_and_ownership_of_the_originals) of the book.

The scripts in this repository do not contain material from the book, and are available under a very permissive MIT license.

## Prerequisites

These scripts assumes access to a `bash` shell, and that you have installed:

* [DotNet runtime 8.0 or above](https://dotnet.microsoft.com/en-us/download)



## Usage

Download the source material from the Internet Archive, and then parse it:

```bash
./download.sh
./parse.sh
```

* The source material will be downloaded to: `input/diary.txt`
* Structured output will be written to: `output/diary-entries.json`

## Parsing

The diary is an OCR text file, uploaded in 2016. The OCR is good, but not perfect. Parsing helps to tidy up the text, and generate structured entries from it.

### OCR issues

The OCR formatting is imperfect:

* There are spelling mistakes (eg. the number `29` was OCR'd in at least 1 place as `2g`)
* Lines are split (at around `92` characters long)
* There are multiple blank lines between paragraphs
* Sometimes there are multiple blank lines between lines that shouldn't be split

I believe some of the spurious line breaks are caused by OCR handling lines that run over page breaks.

### Spelling

In at least a couple of cases, the spelling errors resulted in faulty dates that could not be parsed. These are corrected with a short list of special case corrections in the `SpellingFixer` class.

### Dates

Entries need to be split by date. This is done in the `SourceSplitter` class, which reads a text file, and generates `IEnumerable<DiaryEntry>`.

Entry dates are recognised as a line that ends with `1942`, `1943`, or `1944`. The day, month and year is then parsed from these lines.

### Reformatting

The line breaks are cleaned up with a set of rules that tries to identify lines that shouldn't have been split. If there are 1 or more blank lines between a pair of lines, it will attempt to rejoin them as if a broken line, if the previous line did not end a sentence. (ie. with `.`, `!`, `?`, or `Kitty,`.) Otherwise it recognises the blanks as an indicator of a new paragraph.

_I'm sure these rules are imperfect, and no doubt I'll return to them at some point._

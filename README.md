# Diary of Anne Frank

A set of scripts to clean up the Internet Archive OCR of Anne Frank's "Diary of a Young Girl", and generate a structured dataset of formatted entries, by date.

## Source material

The original source material comes from the [Internet Archive](https://archive.org/):

* [Anne Frank The Diary Of A Young Girl](https://archive.org/details/AnneFrankTheDiaryOfAYoungGirl_201606)

## Usage

Download the source material from the Internet Archive, and then parse it:

```bash
./download.sh
./parse.sh
```

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

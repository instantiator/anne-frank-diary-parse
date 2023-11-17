#!/bin/bash

SOURCE_TEXT=input/diary.txt
mkdir -p output


./build.sh

dotnet run --project src/DiarySourceParser/DiarySourceParser.csproj spelling -s $SOURCE_TEXT | \
dotnet run --project src/DiarySourceParser/DiarySourceParser.csproj split | \
dotnet run --project src/DiarySourceParser/DiarySourceParser.csproj reformat > output/diary-entries.json

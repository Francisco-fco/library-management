# Library Management System

Ett enkelt bibliotekssystem byggt i C# .NET 9 som hanterar boklån och förseningsavgifter.

## Hur man kör lokalt

### Förutsättningar
- .NET 9 SDK installerat
- Visual Studio 2022

### Steg för att köra projektet

1. **Klona repositoriet**
2. **Bygga projektet**
3. **Köra applikationen**
4. **Alternativt i Visual Studio**
- Öppna solution-filen (.sln)
- Tryck F5 eller klicka "Start" för att köra

### Affärsregler
- **Lånegränser**: Standard medlemmar (3 böcker), Premium medlemmar (5 böcker)
- **Låneperiod**: 14 dagar för alla medlemmar
- **Förseningsavgift**: 5 kr per dag och bok
- **Avgiftsgräns**: Medlemmar med över 100 kr utestående avgifter kan inte låna fler böcker

### Arkitektur
- **Clean Architecture**: Projektet följer layered architecture mellan Business, Data Access och Common
- **Repository Pattern**: Använder repository pattern för dataåtkomst med interfaces för testbarhet
- **In-Memory Storage**: All data skapas och lagras i minnet (Lists och Dictionaries) istället för databas

## Användning

1. **Välj medlem** vid start
2. **Huvudmeny** erbjuder följande alternativ:
- Visa tillgängliga böcker
- Låna bok
- Visa mina lånade böcker
- Returnera bok
- Avsluta

### Tidsåtgång

**Totalt**: Tog cirka fyra timmar att implementera
- Arkitektur: Cirka 1,5 h
- Bygga klasser & interfaces i respektive lager: 2 h
- Genomgång & refaktorering: 30 min

**Vad jag skulle göra med mer tid**
- Uppfylla fler funktionalitet som exempelvis betalning
- Skapa inlogg
- Skapa projekt för varje lager istället för att mappa dem
- Skapa gränssnitt och i detta fall prova på Blazor utifrån instruktionerna (c#)
- Bryta upp till fler service klasser, exempelvis BookManager, MemberManager etc
- Unit tester
- Kommentera kod

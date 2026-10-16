# Kafe Terningkast

blazor web app for brettspill kafeen

# bygg og kjør

```bash
cd KafeTerningkast
dotnet restore
dotnet run

## Oppgave 1

### Ansvar

- Games - vise listen, søk, filter, antall treff og "kaste terningen"
- GameCard - viser en spill, får boardgame inn som parameter, filtrerer ikke og eier ikke listen
- GameCatalog - eier spill samlingen, data ligger ikke i komponenten. Brukt ai kode forslag for å finne spill + beskrivelse



## Oppgave 2

### DiceRating
- Value (int) - data fra forelder
- ValueChanged (eventcallback<int>) - hendelse opp
- Readonly - kun lesetillatelse (gamecard)
- brukes med @bind-value på spill siden
- verdier utenfor 1 - 6 blir clampet til 1 eller 6 nå det vises

### GameCard
- Parameter: Game
- EventCallback Onaddtogamenight - kortet eier ikke spillekveld logikk
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

## oppgave 3

### lokal vs applikasjon tilstand
- lokal - søketekst og spill filter på games
- applikasjons tilstand - gamenightstate, beholder data når man navigerer siden, relevant data vil bli delt med hverandre på diverse sider

### designvalg
1. parametere og eventcallback gjennom mange lag - tungvint når alle sidene trenger samme data
2. cascading values - er bra nok for lesing, blir fort mer knotete for oppdateringer fra flere steder
3. tilstandstjeneste - en tydelig hjem, bruker inject der man trenger det, "changed" event for ui oppdatering (valgte dette)

### levetid - scoped
- samme side - beholder tilstand ved navigering
- flere sider - hver fane / side får egen scoped instans, listene er ikke delt (ikke testet)
- flere brukere - alle brukere får eget handling, dataen deles ikke mellom brukere

### abonnement
komponenter abonnerer på "changed" og melder seg av i "dispose" / "idisposable"
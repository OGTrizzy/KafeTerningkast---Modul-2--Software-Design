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

## Oppgave 4

### oppsett / hvordan det henger sammen
spilldata ligger i gamecatalog (singleton, in memory)
rest api eksponerer crud på /api/games
ui snakker ikke direkte med gamecatalog, for alt går via GameApiClient (DI + httpclient)
sider (games, gamedetails, admin) -> GameApiClient -> GET/POST/PUT/DELETE /api/games -> gameCatalog

### api endepunkt
GET | /api/games | 200 - alle spill
GET | /api/games/{id} | 200 / 404
POST | /api/games | 201 | 400
PUT | /api/games/{id} | 204 / 400 / 404
DELETE | /api/games/{id} | 204 / 404

validering skjer i gamevalidation
Navn påkrevd, minplayers minimum 1, max >= min players, spilletid > 0, rating mellom 1 - 6

### gameApiClient
samler alle http calls. komponenter oppretter ikke httpclient selv
metoder: getallasync, getbyidasync, createasync, updateasync, deleteasync - alle har async / await

### livssyklus
- spillhylla (games): data hentes i OnInitializedAsync, opprettes en gang, listen laster ved start
- spillsiden (gamedetails): data hentes i OnParametersSetAsync, når id i url endres blir de samme komponentene brukt igjen - OnInitialized kjører ikke på nytt

### tilstander på sider som henter data
- laster - vises mens api callet henter data
- feil - dette kommer når api/port feiler/er nede. for å unngå kræsj
- tom liste - ingen treff

### admin /admin/games
- opprett, endre og slett via editform og dataannotations
- bekrefte før sletting ja/nei
- listen oppdateres etter lagring/slett uten full side reload

### terning kast
endring på spillsiden lagres med updateasync mot api

### slettet spill som ligger i min spillekveld
ved slett fra admin fjernes spillet fra kataloget / api
gamenightstate er separat (scoped) - allerede valgt spill ligger fortsatt i spillekvelden til den blir fjernet av brukeren, spillekveld er lokal session så det trenger ikke å spørre api live

### gjengivelsesmodus (interactive server)
valgt fordi
ui kode og httpclient call kjører på server over signalr
enkelt å dele gamenightstate pr krets (scoped)
ingen ekstra cors oppsett for kall til eget api fra nettleser

med webassembly ville api call gått fra nettleseren, baseadress/cors måtte tilpasses, og tilstand ville levd i klienten istedenfor server kretsen

### testresultater (manuelt)
test | resultat
----------------
spillhylla laster fra api | ok
api utilgjengelig / feil | feilmelding, kræsjer ikke
/games/1 og /games/99999 | detaljer / ikke funnet
endre terningkast | lagres via put
admin: tomt navn | valideringsfeil
admin: max < min | avvist
opprett / endre / slett | listen oppdateres
bekreftelse før slett | ok
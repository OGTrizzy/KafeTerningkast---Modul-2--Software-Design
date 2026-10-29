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

## oppg 5

### side oversikt
rute | komponent | formål
--------------------------
/   |   home     | forside
/games | games   | spillhylla - søk, filter, terning
/games/{id} |   gamedetails | detaljer, terningkast, legg till
/game-night |   gamenight | valgte spill og samlet spilletid
/admin/games |  admingames | crud for mods
/not-found  |   notfound | ukjent adresse

### komponenttre
bilde ligger i mappen img

### dataflyt
bilde ligger i mappen img

### ansvarliste
del | ansvar
---------------
games   | søk, filter, liste, kaste terning, henter data via api
gamecard    | viser en spill, gir varsel ved legg til
dicerating  | viser/velger 1-6, data ned hendelse opp
gamedetails | ett spill fra api, lagre rating, legg til spillekveld
gamenight   | viser valgt liste og viser total tid, fjerne spill
admingames  | crud mot api med validering
navmenu     | navigasjon + antall i spillekveld
gamecatalog | in memory lagring av spill
gameapiclient | all http mot /api/games
gamenightstate | applikasjonstilstand for spillkveld
gamevalidation | server side validering av boardgame

### designvalg

#### 1. deling av tilstand (spillekveld)
utfordring: flere sider og menyen må dele samme liste med valgte spill

alternativer: 
1. parametere + eventcallback gjennom mange lag
2. cascading values
3. tilstandstjeneste - valgte denne

valg: tjeneste med changed event, ett tydelig hjem, lett å injecte i games, details, gamenight og navmenu

kostnad: må huske idisposable og avmelde event, krever interactive render mode på menyen

#### 2. gjengivelsemodus
utfordring: appen trenger click, søk, api call og delt tilstand

alternativer:
1. static server
2. interactive webassembly
3. interactive server - valgte denne

valg: kode og httpclient kjører på server over signalr, enkelt for scoped gamenightstate og eget api

kostnad: krever åpen server tilkobling, tåler offline dårligere enn wasm

#### 3. in memory lagring (gamecatalog)
utfordring: trenger crud uten databasekrav

alternativer:
1. hardkodet liste uten skriving
2. database (ef core)
3. in memory singleton - valgte denne

valg: oppfyller oppgaven, enkel api og lettere å lage 

kostnad: data nullstilles ved restart, ikke egnet for bedrifter med flere servere

### hva er lett / vanskelig å gjøre
- lett - nytt felt på boardgame, modell, form, api og ui
- lett - ekstra filter på hylla, logikk i getfilteredgames
- vanskelig - bytte til database, ny persistens under samme api kontrakt, gamecatalog erstattes, ui trenger ikke å bli endra
- vanskelig - dele spillekveld på tvers av faner

### kjente begrensinger
- in memory - data forsvinner ved restart
- spillekveld deler ikke mellom faner (scoped)
- slettet spill kan fortsatt ligge i spillekveld til bruker fjerner det
- ingen login for admin

### manuell testlogg
    testtilfelle    |   steg    |   forventet   |   resultat
1. søk case insensitive | skriv catan | Catan vises | ok
2. tomt søk         |           |                   | ok
3. filter grense    |   2 på 2-4    | vises         | ok
4. /games/9999      |   åpne url    | fant ikke     | ok
5. api feil         | server off    |   feilmelding | ok
6. admin validering |   tomt navn   |   feil i skjema   | ok
7. slett bekreftelse|   nei         | ikke slettet  | ok
8. responsiv response| web devtool  | mobil interface| ok
9. to faner åpen    | legg til i fane 1 | ikke delt med fane 2  | ok

### kjøre test koder
dotnet test KafeTerningkast.Tests/KafeTerningkast.Tests.csproj

## ai bruk
jeg har brukt ai til å støtte meg med prosjektet, ikke ferdig løsning

### hvordan jeg brukte ai
- ideer til struktur
- kode forslag f.eks gamecatalog spill, crud mønster og tester
- forklaring når noe feilet (f.eks scoped mot singleton, livssyklus og build feil)
- hjelp til å lage ryddig oppsett i readme og designvalg

### hva ai har bidratt med
eksempler på blazor mønstre (parametere, eventcallback, httpclient) og feilsøking når kompileringsfeil dukket opp, readme oppsett forslag

### hva jeg selv har gjort
jeg har valgt arkitektur (gamecatalog, api, gameapiclient, gamenightstate scoped, interactive server), skrevet og tilpasset koden til caset, rettet feil (f.eks jeg skrev court når det egt skulle stå count, markup i @code, testprosjekt vs web prosjekt), skrevet dokumentasjonen med egne ord

### begrensinger med ai
forslager kan være vrient til tider, ai insisterte at jeg skulle fjerne gamecatalog og endre det til gamestore med helt nytt kode som gjorde noe helt annet(den trodde jeg skulle lage et spill butikk av noe slag), jeg måtte forstå og endre de aktuelle forslagene for at det skulle funke. 
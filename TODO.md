# BACKLOG

DONE Opzetten omgeving  
DONE Stap 1: Overzetten van oude Zaaikalender naar nieuwe Zk!  
DONE Database connectie leggen.  
DONE Deployment omgeving regelen.  
DONE Betere server dan XSP4 (e.g., fastcgi-mod-mono)  

### Autorisatie:
DONE Forms authenticatie  
DONE Return url verwijderen (moet weg met externe login)  
DONE Inlogscherm  
DONE Inloggen gebruikersnaam of e-mailadres  
DONE Wachtwoord reset  
DONE Implementeer succes pagina na versturen (werd link).  
CANCELLED Facebook (?) authenticatie (Neen. Wij zijn tegen.)  
DONE Gebruiker kan inloggen en agenda hoort bij hem.  
DONE Profiel maken gebruiker.  
TODO Meer informatie gebruiker.  
TODO Mogelijkheid tot maken vrienden.  
TODO Kalender linken op profiel  

### Datamodel (Model)  
DONE Datamodel relaties inbouwen (foreign keys)  
DONE Classes en relaties  
IN PROGRESS Bepalen groeitijd  
IN PROGRESS Bepalen groeitijd keukenkruiden, groenbemesters en kiemgroenten  
TODO Plaatjes erbij zoeken  
TODO Weghalen spaties achter data?  
TODO Uitzoeken kilogrammen en grondtypes (FutDev)  
TODO Splitsen repositories  

### Business logic (Controllers)
DONE van zaai- en oogstmaand naar view en vice versa.  
DONE 'echte' businesslogic laag toegevoegd met managers.  
DONE Pseudo code voor berekeningen.  
DONE Pseudo code naar echte code.  
DONE Mogelijkheid farming months zelfs toe te voegen.  
DONE Gerelateerde farming actions updaten.  
DONE Aangeven hoeveel stuks.  
TODO Vrienden kunnen aanmaken.  
TODO Error handling.  

### Security
DONE Check of geupdate acties wel bij de ingelogde gebruiker horen.  
TODO Op basis van debug of release build bepalen welke connectiestring.  

### Notifications (Controllers/View)
TODO Wanneer implementeren?  
TODO Implementatie herinneringen  
TODO Implementatie grappen  

### Initiële invulproces
DONE Bouwen selecteren gewas typeahead implementeren  
DONE Over nadenken  
DONE Drag-and-drop  
TODO Plaatjes ophalen vanaf server  

### Extra's
TODO Integratie weer API geavanceerde algoritmes  
TODO Integratie webshop  

### User Interface (View)
DONE Minder grote bovenkant.  
DONE Kleurtjes bij foute invoer.  
DONE Maanden-code van de view verplaatsen naar models en controllers.  
DONE Menubalk netjes met Bootstrap.  
DONE De jaarkalender laten bestaan uit maand kalender view modellen.  
DONE Initial User interface maand.  
DONE Weergeven als er die maand geen zaai- of oogstmomenten zijn.  
DONE Touch werkend op Mobile.  
TODO Afkappen decimalen.  
TODO Standaard oogsten aangevinkt in het sleepvak.  
TODO Niet groter dan 999 in veld.  
TODO Inlogscherm kleiner.  
TODO Zorgen dat het selecteer scherm (netjes) onder de maand overview komt op Mobile.  
TODO Weergave Mobile.  
TODO Fixed blokje boven menu? (web-tiki).  
TODO Indien register form groter is pagina langer maken plus wat pixels in zk.resizeLoginArea(). (??)  
TODO Direct meeveranderen van zaaien/oogsten op basis van input (JavaScript/jQuery).  
TODO Web-tiki vragen tot doen van verbeteringen middels pull-request. 
TODO Jaarkalender naar PDF.  

# Improvements
DONE Refactor FarmingActionHelper methode door opsplitsing en out-parameters weg te halen.  
DONE Fix bug met entity framework contexts die niet samenwerken bij objecten samengesteld uit meerdere elementen.  
DONE Zorg dat je weer plant of planten ziet.  
DONE Gewas aantal groter vak.  
DONE Tekst moet leesbaarder bij maandoverzicht.  
DONE Streep linksonder weg bij maandoverzicht.  
DONE E-mailadres klikbaar.  
DONE Zorgen dat toevoegen van een gewas het aantal ophoogt indien er al eenzelfde gewas is die maand.  
DONE Optillen wordt het een zaadje (bij zaaien) of een zeis (bij oogsten).(gekozen voor plant bij oogsten, misschien anders) 
TODO Indien sessie verloopt uitloggen! 
TODO Gebruiker hoeft niet te registreren: in memory kan je al oogstplanner maken, registreren kan later.  
TODO Vindbaarheid verbeteren.  
TODO Automatisch selecteren van oogsten. Lukt niet.  
TODO Schots en scheefheid rechtzetten. (?)  

### Documentatie:
README beetje netje maken.  

### BUGS:
FIXED Gewassen zonder oogstmaanden vertonen bug bij slepen.  
FIXED No scrolling while dragging on Mobile
HACKY-FIX Op bevestigen kliken bij lege maand levert nullreferencexception op.  
2x klikken op maand levert dubbele data op.  
Als user niet langer is ingelogd krijg je exception bij toevoegen gewas
0 invoeren in numeric text box is niet meer aan te passen met de pijltjes.  
Anti-forgery token fixen.  
Oude invoer blijft hangen op sleep scherm.  

### WENSEN:
Een gebruiker moet de zaaikalender een naam kunnen geven.  
Een gebruiker moet de zaaikalender overzichtelijk kunnen zien, en kunnen onderverdelen in zaaikalender of oogstkalender.  
Een waarschuwing als je iets wilt oogsten wanneer je te laat ben.  
Er dient rekening te houden geworden met de maand.  
In hoeverre wijk ik af met wat ik als eerste had aangegeven.  
"Initiele invulproces." => Vasthouden.  

### Suggesties:
Tuintje tijdbalk (slepen door de tijd en je ziet opkleuringen van planten, zaaien et cetera. 
Welke planten groeien goed bij elkaar (let op: er zijn veel meer factoren, boeken vol,je kan het niet zeker weten).  
Recept (op basis van beschikbaar geoogst materiaal een recept samenstellen).  
Allebei de kanten op: ik wil zoveel lasagna eten, dit wordt het zaaischema, en vice versa, ik wil dan en dan zaaien en dit kan je eten.  
Allebei de kanten op: ik wil dan en dan planten of ik wil dat en dat eten.  
Sorteren op of maand, type gewas.  
Weergave met een kalender. Je kan hem uitprinten. Agenda items in Google Calendar.  
Maximale oppervlakte voor een gewas.  
Onderscheid tussen planten en zaaien.  

Plant informatie:  
gildes  
planten encyclopedie (standplaats, gildes, gemiddelde opbrengst, kenmerken, plaatje)  

Eenheid van product aangeven per product -->  
exporteren naar excel/pdf --> printbaar  

default oogstkalender standaard groenten met maximum grondoppervlak  
Initiele invulflow: default => standaard gezin, hipster, et cetera ?  

Verbinden aan kcal --> voedingswijzer koppelen  
Wellicht ook vitamines e.d. --> verdeeld ook voedingswijzer  

Groeitijd.  

Interactief maanden opvoeren.  
De lijst is leuk, maar ik wil iets nieuws. Maar ik wil nieuwe groente.  

Intern in code nummers / datum objecten, en bij weergave internationalisatie.  

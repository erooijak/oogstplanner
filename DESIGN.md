## PLAN VAN AANPAK ANONIEM KUNNEN INLOGGEN

=>  Injecteren andere implementatie IUserManager op basis van logged in of niet.

    Hulpbron: 
    http://stackoverflow.com/questions/29724069/is-it-possible-to-inject-different-service
    -into-controller-based-on-if-user-is-l

    - Manager dient ander UserId te retourneren.
    - Het UserId dient af te hangen van de sessie.
    - Zorgen dat Add(user) methode en GetCurrentUserID() bovenstaande ondersteunen.
        * Add(user) slaat een tijdelijke gebruiker op.
    - Er is sprake van een andere database context (de temp context) die naar andere db 
      schrijft.
        * Deze database moet periodiek worden geschoont van oude data? (Tijdvlag?)
    - Het UserId is gekoppeld aan een cookie.

=>  Zodra een user registreert dient de data te worden gekopieerd van de temp context
    naar de persistent context.

    Hulpbron:
    http://stackoverflow.com/questions/2185155/cloning-data-on-entity-framework

=>  De UI dient te worden aangepast. Als een user niet is ingelogd krijgt deze dit te zien
    en een uitleg over de voordelen van registreren (opslaan en vriendennetwerk). 



## PLAN VAN AANPAK VRIENDENNETWERK

=>  Nieuwe manager: FriendsManager
    
    - Er komt een many-to-many relatie tussen Users genaamd Friends.
    - De FriendsManager helpt met deze relaties leggen.

=>  Het profiel wordt verbeterd. 

    - De kalender is zichtbaar van een gebruiker, en gebruikers 
      kunnen elkaars kalender bekijken.
    - Profiel bekijken kan alleen voor geregistreerde gebruikers.
    - Gebruikers kunnen elkaar zoeken op basis van naam of e-mail.
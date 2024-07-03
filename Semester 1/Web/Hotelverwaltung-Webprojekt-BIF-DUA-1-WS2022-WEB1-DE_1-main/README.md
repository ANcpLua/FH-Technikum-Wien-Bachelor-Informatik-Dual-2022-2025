# Hotelverwaltung-Webprojekt-BIF-DUA-1-WS2022-WEB1-DE_1-JavaScript war nicht erlaubt!

1) XAMPP muss installiert werden und der gepackte Ordner in die htdocs Datei in einen Ordner namens "hotelsite" entpackt werden. 

2) XAMPP muss gestartet werden, 
"Apache" und "MySQL" müssen gestartet werden. 

3) Bilderverkleinerung: 
Im XAMPP->Control Panel
Bei Apache->Config->php.ini
das `;` vor `extension=gd` muss entfernt werden, damit das Thumbnail bei Newsupload erstellt werden kann. 

**Datenbank:**
Im XAMPP->Control Panel
MySQL->Admin
dort auf Neu und dann eine Datenbank namens "hotelpanda" anlegen. Nun diese auf der linken Seite anklicken und
dann auf importieren und die inkludierte "hotelpanda.sql" importieren. 

Diese Datenbank benutzt kein Passwort! 

Es gibt folgende Benutzer:
```
guest1 mit PW: guest1
guest2 mit PW: guest2
guest3 mit PW: guest3 (kann sich nicht einloggen)
admin mit PW: admin
```

![Registrierung](https://raw.githubusercontent.com/panda-lambda/Hotelsite-Panda/main/register.png)
![News](https://raw.githubusercontent.com/panda-lambda/Hotelsite-Panda/main/news.png)
![Admin -User management](https://raw.githubusercontent.com/panda-lambda/Hotelsite-Panda/main/user_management.png)

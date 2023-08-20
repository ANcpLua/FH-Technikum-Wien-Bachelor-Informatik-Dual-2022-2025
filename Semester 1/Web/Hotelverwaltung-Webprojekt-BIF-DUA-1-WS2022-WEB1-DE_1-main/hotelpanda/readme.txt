1) XAMPP muss installiert werden und der gepackte Ordner in die htdocs Datei in einen Ordner namens "hotelsite" entpackt werden. 

2) XAMPP muss gestartet werden, 
"Apache" und "MySQL" müssen gestartet werden. 

3)Bilderverkleinerung: 
Im XAMPP->Control Panel
Bei Apache->Config->php.ini
das ";" vor "extension=gd" muss entfernt werden, damit das Thumbnail bei Newsupload erstellt werden kann. 

Datenbank: 
Im XAMPP->Control Panel
MySQL->Admin
dort auf Neu und dann eine Datenbank namens "hotelpanda" anlegen. Nun diese auf der linken Seite anklicken und
dann auf importieren und die inkludierte "hotelpanda.sql" importieren. 

Diese Datenbank benutzt kein Passwort! Dies sollte auf jeden Fall schnellstmöglich geändert werden. 
Siehe zB. hier:  https://kinsta.com/knowledgebase/xampp-mysql-password/

Es gibt folgende Benutzer:
guest1 mit PW: guest1
guest2 mit PW: guest2
guest3 mit PW: guest3 (kann sich nicht einloggen)
admin mit PW: admin

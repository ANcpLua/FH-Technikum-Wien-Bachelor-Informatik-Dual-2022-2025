<?php
header("Content-Type: application/json");
http_response_code(200);

$array = array();
array_push($array, "Christian");
array_push($array, "Huber");
array_push($array, "Ist");
array_push($array, "der");
array_push($array, "coolste");
array_push($array, "Lektor");
array_push($array, "Die Angabe von UE4 war so kryptisch.");
array_push($array, "Als Erstes habe ich Live Server und PHP Debug eingerichtet in VSC -- keine Ahnung wieso.");
array_push($array, "Dann meinten die anderen, ich soll einfach ein Array ausgeben, das haben wir im Unterricht besprochen.");
array_push($array, "Ich dachte mir, ok, schön, mal nicht einen Tag mit Web verbringen - haha, da habe ich mich getäuscht.");
array_push($array, "XAMPP geöffnet, mich erinnert, dass das Zeug nicht mit meinem Mac funktioniert, meinen uralten Stand-PC hochgefahren.");
array_push($array, "Nach einer Stunde Windows-Updates und Fragen, ob ich Windows 11 installieren will, obwohl beim Drücken steht, mein PC ist zu schlecht, endlich mein UE3-Beispiel heruntergeladen, eine Zeile AJAX-JS hinzugefügt und 200 deutsche Wörter.");
array_push($array, "Leider ging nichts, dachte mir, ich bin zu ungeschickt, um eine Array-Funktion auszugeben und habe jeglichen Fehler geprüft(dev console,network etc) und nichts. Absolut nichts.");
array_push($array, "Nach 3 Stunden und unabsichtlichem Anschlagen meines Kopfes gegen die Wohnzimmertür, so laut, dass meine Freundin dachte, ich sterbe,dachte ich mir, hmm, vielleicht lösche ich mal den Cache -- Ultima Ratio.");
array_push($array, "Siehe da, es ging.");
array_push($array, "Wenn Sie bis hierher gelesen haben, danke für Ihre Aufmerksamkeit, aber guten AJAX-Stuff gibt es hier nicht. Mein Kollege Ben hat eine nette Wetteranzeige.");

echo(json_encode($array));
?>
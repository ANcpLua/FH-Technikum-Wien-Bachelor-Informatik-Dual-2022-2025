<?php
/*  +++++++++Übersicht++++++++++++
function checkInput($data) 
function emptyInputSignup($firstname, $lastname, $email, $username, $pwd, $pwdRepeat)
function checkForAlph($input)  
function pwdMatch($pwd, $pwdRepeat)
function usernameTaken($conn, $username) 
function createUser($conn, $anrede, $firstname, $lastname, $email, $username, $pwd, $typ, $status, $aktivinaktiv)
function emptyInputLogin($username, $pwd)  
function loginUser($conn, $username, $pwd, $status)
function updateNames($conn, $userId,  $lastname, $firstname) 
function updateEmail($conn, $userId,  $email)    
function OnlyEmailExists($conn, $email)
function invalidUid($username) 
function invalidEmail($email)
function updateUsername($conn, $userId, $username)
function OnlyUidExists($conn, $username)
function getExtraCost($breakfast, $parking, $pets, $room_type)
function checkActiveReservation($conn, $userId)*/

function checkInput($data)    /* Die Funktion "checkInput" bereinigt und schützt eine übergebene Eingabe. 
Sie nutzt dazu die PHP-Funktionen "trim", "stripslashes" und "htmlspecialchars".
"trim" entfernt Leerzeichen, Tabulatoren und Zeilenumbrüche am Anfang und Ende der Eingabe.
"stripslashes" entfernt Backslashes, die möglicherweise von der Eingabe hinzugefügt wurden.
"htmlspecialchars" wandelt bestimmte Zeichen in ihre HTML-Entitäten um, um sicherzustellen, dass kein Schadcode ausgeführt werden kann. */
{
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);
    return $data;
}

function emptyInputSignup($firstname, $lastname, $email, $username, $pwd, $pwdRepeat)    // Die Funktion "emptyInputSignup" prüft, ob eines der Felder 
// bei der Anmeldung leer ist. Wenn eines der Felder leer ist, wird "true" zurückgegeben, andernfalls "false". Die Funktion könnte genutzt werden, um sicherzustellen, dass der Benutzer alle erforderlichen Felder ausgefüllt hat, bevor er versucht, sich anzumelden.
{
    
    if (empty($firstname) || empty($lastname) || empty($email) || empty($username) || empty($pwd) || empty($pwdRepeat)) {
        $result = true;
    } else {
        $result = false;
    }
    return $result;
}

function checkForAlph($input)           /* Die Funktion "checkForAlph" prüft, ob ein übergebener Eingabewert ausschließlich aus Buchstaben besteht. 
Sie nutzt dazu die PHP-Funktion "preg_match" mit einem regulären Ausdruck, der nur Buchstaben von A bis Z und a bis z erlaubt. 
Wenn der Eingabewert ausschließlich aus Buchstaben besteht, wird "true" zurückgegeben und eine leere Zeichenkette zurückgegeben. 
Andernfalls wird "false" zurückgegeben und die Zeichenkette "Nur Buchstaben!" zurückgegeben. 
Die Funktion könnte genutzt werden, um sicherzustellen, dass der Benutzer nur Buchstaben für bestimmte Eingabefelder eingibt, zum Beispiel für den Vor- und Nachnamen*/
{
    return preg_match("/^[a-zA-Z]*$/", $input) ? "" : "Nur Buchstaben!";
}
function checkForAlphNum($input)
{
    return preg_match("/^[a-zA-Z0-9]*$/", $input) ? "" : "Keine Sonderzeichen!";
}
function checkEmail($input)
{
    return filter_var($input, FILTER_VALIDATE_EMAIL) ? "" : "Adresse ungültig!";
}


// Die Funktion "pwdMatch" prüft, ob zwei übergebene Passwörter übereinstimmen. Wenn die Passwörter nicht übereinstimmen, wird "true" zurückgegeben, andernfalls "false".
function pwdMatch($pwd, $pwdRepeat)   // same pW
{
    
    if ($pwd !== $pwdRepeat) {
        $result = true;
    } else {
        $result = false;
    }

    return $result;
}


/*Die Funktion "usernameTaken" prüft, ob ein bestimmter Benutzername bereits in der Datenbank vorhanden ist. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die alle Benutzer mit dem übergebenen Benutzernamen abruft.
 Wenn das Abrufen des Ergebnisses erfolgreich ist, wird das Ergebnis in einem Array gespeichert und zurückgegeben. Andernfalls wird "false" zurückgegeben. 
 Schließlich wird die Verbindung zum Datenbank-Statement geschlossen. */

function usernameTaken($conn, $username)            // taken 
{
    $sql = "SELECT * FROM users WHERE usersUid = ?;";   // ? = ein platzhalter sql injection schutz
    $stmt = mysqli_stmt_init($conn);     // schützen vor huans
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        header("location: ../index.php?page=register&error=stmtfailed");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "s",  $username);   // parameter an das statment gebunden statt ? der username, bindfunction stellt sicher keine injection
    mysqli_stmt_execute($stmt);

    $resultData = mysqli_stmt_get_result($stmt);    // holt result raus

    if ($row = mysqli_fetch_assoc($resultData))    // speichert zeile in row die gecheckt wurde davor,  stellt sicher das nur erstellt wird wenns keinen user gibt
    {
        return $row;
    } else {
        $result = false;
        return $result;
    }
    mysqli_stmt_close($stmt);
}


/*Die Funktion "createUser" erstellt einen neuen Benutzer in der Datenbank. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die einen neuen Benutzer mit den übergebenen Informationen hinzufügt. 
Das Passwort wird zunächst gehasht, um sicherzustellen, dass es in der Datenbank sicher gespeichert wird. 
Wenn das Hinzufügen des Benutzers erfolgreich ist, wird der Benutzer zu einer bestimmten Webseite weitergeleitet. 
Andernfalls wird der Benutzer zu einer anderen Webseite weitergeleitet. Schließlich wird die Verbindung zum Datenbank-Statement geschlossen.*/

function createUser($conn, $anrede, $firstname, $lastname, $email, $username, $pwd, $typ, $status, $aktivinaktiv)             // sicherheit
{
    $sql = "INSERT INTO users (usersAnrede, usersVorname, usersNachname, usersEmail, usersUid, usersPassword, usersTyp, usersStatus, oneAktiv_zeroInaktiv) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);";
    $stmt = mysqli_stmt_init($conn);     
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        header("location: ../index.php?page=register&error=usernametaken");
        exit();
    }

    $hashedPwd = password_hash($pwd, PASSWORD_DEFAULT);

    mysqli_stmt_bind_param($stmt, "ssssssssi", $anrede, $firstname, $lastname, $email, $username, $hashedPwd, $typ, $status, $aktivinaktiv);
    mysqli_stmt_execute($stmt);
    mysqli_stmt_close($stmt);
    header("location: ../index.php?page=login&error=none");
    exit();
}


/*Die Funktion "emptyInputLogin" prüft, ob die Felder für den Benutzernamen und das Passwort beim Anmelden leer sind.
 Wenn eines der beiden Felder leer ist, wird "true" zurückgegeben, andernfalls "false". */

function emptyInputLogin($username, $pwd)
{
    
    if (empty($username) || empty($pwd)) {
        $result = true;
    } else {
        $result = false;
    }
    return $result;
}


function loginUser($conn, $username, $pwd, $status)
{
    $usernameTaken = usernameTaken($conn, $username);     // check if the username exists

    if ($usernameTaken === false) {
        // redirect the user to the login page with an error message if the username is not taken
        header("location: ../index.php?page=login&error=wronglogin");
        exit();
    }

    if ($status !== "active") {
        // redirect the user to the login page with an error message if their status is "inaktiv"
        header("location: ../index.php?page=login&error=inactive");
        exit();
    } else {
        // start a session and redirect the user to the index page if the username is taken and their status is not "inaktiv"
        session_start();                                                // start session logged in, important so that "logged in" stands when logged in
        $_SESSION["userid"] = $usernameTaken["usersId"];
        $_SESSION["useruid"] = $usernameTaken["usersUid"];
        $_SESSION["userrole"] = $usernameTaken["usersTyp"];
        header("location: ../index.php");
        exit();
    }
}

/*Die Funktion "updateNames" aktualisiert den Vor- und Nachnamen eines Benutzers in der Datenbank. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die den Vor- und Nachnamen des Benutzers mit einer bestimmten Benutzer-ID in der Datenbank aktualisiert.
 Wenn das Ausführen des Statements erfolgreich ist, wird der Benutzer zu einer bestimmten Webseite weitergeleitet. 
 Schließlich wird die Verbindung zum Datenbank- Statement geschlossen.*/

function updateNames($conn, $userId,  $lastname, $firstname)       // ändert  user 
{

    $sql = "UPDATE users SET usersNachname = ?, usersVorname = ? WHERE usersId = ? ;";
    $stmt = mysqli_stmt_init($conn);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        header("location: ../index.php?page=profile&error=noChanges");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "sss",  $lastname, $firstname, $userId);

    if (mysqli_stmt_execute($stmt)) {
        mysqli_stmt_close($stmt);
        header("location: ../index.php?page=profile&success=namechanged");
    }
    mysqli_stmt_close($stmt);
}
/*Die Funktion "updateEmail" aktualisiert die E-Mail-Adresse eines Benutzers in der Datenbank. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die die E-Mail-Adresse des Benutzers mit einer bestimmten Benutzer-ID in der Datenbank aktualisiert. 
Wenn das Ausführen des Statements fehlschlägt, wird der Benutzer zu einer bestimmten Webseite weitergeleitet. 
Andernfalls wird der Benutzer zu einer anderen Webseite weitergeleitet. Schließlich wird die Verbindung zum Datenbank-Statement geschlossen.*/

function updateEmail($conn, $userId,  $email)
{

    $sql = "UPDATE users SET usersEmail = ? WHERE usersId = ? ;";
    $stmt = mysqli_stmt_init($conn);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        header("location: ../index.php?page=profile&error=noChanges");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "ss",  $email, $userId);

    if (mysqli_stmt_execute($stmt)) {

        header("location: ../index.php?page=profile&success=emailchanged");
    } else {
        header("location: ../index.php?page=profile&error=noChanges");
        exit();
    }

    mysqli_stmt_close($stmt);
}
/*Die Funktion "OnlyEmailExists" prüft, ob eine bestimmte E-Mail-Adresse bereits in der Datenbank vorhanden ist. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die alle Benutzer mit der übergebenen E-Mail-Adresse abruft. 
Wenn das Abrufen des Ergebnisses fehlschlägt, wird der Benutzer zu einer bestimmten Webseite weitergeleitet. 
Ansonsten wird überprüft, ob das Ergebnis der Abfrage leer ist oder nicht. 
Wenn es leer ist, wird "true" zurückgegeben, andernfalls "false". Schließlich wird die Verbindung zum Datenbank- Statement geschlossen*/
function OnlyEmailExists($conn, $email)
{
    $sql = "SELECT * FROM users WHERE usersEmail = ?;";
    $stmt = mysqli_stmt_init($conn);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        // header("location: ../signup.php?error=onlyemailexits");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "s", $email);
    mysqli_stmt_execute($stmt);

    $resultData = mysqli_stmt_get_result($stmt);

    if (mysqli_fetch_assoc($resultData)) {
        return false;
    } else {
        return true;
    }

    mysqli_stmt_close($stmt);
}
function invalidUid($username)     // richtige buchstaben/zahlen, wie checkforalphnum eig redundant 
{
    
    if (!preg_match("/^[a-zA-Z0-9]*$/", $username)) {
        $result = true;
    } else {
        $result = false;
    }
    return $result;
}

function invalidEmail($email)   // checkt falsche emails, wie checkEmail eig redundant 
{
   
    if (!filter_var($email, FILTER_VALIDATE_EMAIL)) {
        $result = true;
    } else {
        $result = false;
    }

    return $result;
}
/*Die Funktion "updateUsername" aktualisiert den Benutzernamen (UID) eines Benutzers in der Datenbank. 
Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die den Benutzernamen des Benutzers mit einer bestimmten Benutzer-ID in der 
Datenbank aktualisiert. Wenn das Ausführen des Statements fehlschlägt, wird der Benutzer zu einer bestimmten Webseite weitergeleitet. Andernfalls 
wird der Benutzer zu einer anderen Webseite weitergeleitet. Schließlich wird die Verbindung zum Datenbank-Statement geschlossen.*/
function updateUsername($conn, $userId, $username)
{

    $sql = "UPDATE users SET usersUid = ? WHERE usersId = ? ;";
    $stmt = mysqli_stmt_init($conn);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
        header("location: ../index.php?page=profile&error=noChanges");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "ss",  $username, $userId);

    if (mysqli_stmt_execute($stmt)) {
        header("location: ../index.php?page=profile&success=usernamechanged");
    } else {
        header("location: ../index.php?page=profile&error=noChanges");
        exit();
    }

    mysqli_stmt_close($stmt);
}

/* Die Funktion "OnlyUidExists" prüft, ob ein bestimmter Benutzername (UID) bereits in
der Datenbank vorhanden ist. Dazu wird eine SQL-Abfrage an die Datenbank gesendet, die
alle Benutzer mit dem übergebenen Benutzernamen abruft. Wenn das Abrufen des Ergebnisses fehlschlägt, wird 
der Benutzer zu einer bestimmten Webseite weitergeleitet. Ansonsten wird überprüft, ob das Ergebnis der Abfrage leer
ist oder nicht. Wenn es leer ist, wird "true" zurückgegeben, andernfalls "false". Schließlich wird die Verbindung zum Datenbank-Statement geschlossen.
*/
function OnlyUidExists($conn, $username)
{
    $sql = "SELECT * FROM users WHERE usersUid = ?;";
    $stmt = mysqli_stmt_init($conn);
    if (!mysqli_stmt_prepare($stmt, $sql)) {
       // header("location: ../signup.php?error=stmtFailedOnlyUidExists");
        exit();
    }

    mysqli_stmt_bind_param($stmt, "s", $username);
    mysqli_stmt_execute($stmt);

    $resultData = mysqli_stmt_get_result($stmt);

    if (mysqli_fetch_assoc($resultData))                 // checked row/zeile datenbank
    {
        return false;
    } else {
        return true;
    }

    mysqli_stmt_close($stmt);
}
// Function to get the cost of extra amenities
function getExtraCost($breakfast, $parking, $pets, $room_type)
{
    $cost = 0;
    // Connect to the database
    $db_host = 'localhost';
    $db_user = 'root';
    $db_pass = '';
    $db_name = 'hotelpanda';
    $connn = mysqli_connect($db_host, $db_user, $db_pass, $db_name);

    // Check the connection
    if (!$connn) {
        die("Connection failed: " . mysqli_connect_error());
    }

    // Query the database for the cost of the room type
    $query = "SELECT room_type FROM reservations WHERE room_type = '$room_type'";
    $result = mysqli_query($connn, $query);

    if ($room_type === 'Suite') {
        $cost += 210;
    } else if ($room_type === 'Deluxe') {
        $cost += 150;
    } else if ($room_type === 'Standard') {
        $cost += 100;
    }
    // Add the cost of the room type to the total cost
    if ($breakfast === 'Ja') {
        $cost += 16;
    }

    if ($parking > 0) {
        $cost += 10 * $parking;
    }

    if ($pets > 0) {
        $cost += 15 * $pets;
    }

    return $cost;
}
/*function checkActiveReservation($conn, $userId,$room_type)
{
    // Query the database for active reservations
    $query = "SELECT * FROM reservations WHERE usersId='$userId'AND status='neu' AND room_type='.$room_type.'";
    $result = mysqli_query($conn, $query);

    // If there are any active reservations, return false
    if (mysqli_num_rows($result) > 0) {
        return false;
    }

    // Otherwise, return true
    return true;
}*/

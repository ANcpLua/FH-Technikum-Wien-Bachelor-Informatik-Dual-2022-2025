<?php
// check if the submit button was clicked
if (isset($_POST["submit"]))
{
    // get the form data
    $anrede = $_POST["anrede"];
    $firstname = $_POST["firstname"];
    $lastname = $_POST["lastname"];
    $email = $_POST["email"];
    $username = $_POST["username"];
    $pwd = $_POST["pwd"];
    $pwdRepeat = $_POST["pwdRepeat"];
    $typ = "guest";
    $status = "active";    // status: aktiv,inaktiv
    $aktivinaktiv = "1";
    

    // include the database and functions files
    require_once 'dbaccess.php';
    require_once 'funktionsinc.php';

    // check if any of the fields are empty
    if (emptyInputSignup($firstname, $lastname, $email, $username, $pwd, $pwdRepeat) !== false){
        // redirect to the register page with an error message
        header("location: ../index.php?page=register&error=emptyinput");
        exit();
    }

    // check if the username is invalid
    if (invalidUid($username) !== false){
        // redirect to the register page with an error message
        header("location: ../index.php?page=register&error=invalidusername");
        exit();
    }

    // check if the email is invalid
    if (invalidEmail($email) !== false){
        // redirect to the register page with an error message
        header("location: ../index.php?page=register&error=invalidemail");
        exit();
    }
    // check if the passwords match
    if (pwdMatch($pwd, $pwdRepeat) !== false){
        // redirect to the register page with an error message
        header("location: ../index.php?page=register&error=passwordsdontmatch");
        exit();
    }

    // check if the username or email is already taken
    if (usernameTaken($conn, $username) !== false){
        // redirect to the register page with an error message
        header("location: ../index.php?page=register&error=usernametaken");
        exit();
    }

    // create the user in the database
    createUser($conn, $anrede, $firstname, $lastname, $email, $username, $pwd, $typ, $status, $aktivinaktiv);
}
else{
    // redirect to the register page
    header("location: ../index.php?page=register");
    exit();        
}
?>
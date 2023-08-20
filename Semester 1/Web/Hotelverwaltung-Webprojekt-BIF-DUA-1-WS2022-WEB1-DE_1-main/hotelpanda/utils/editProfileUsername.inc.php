<?php
include_once "navigation.php";
// check if the user is a guest
if($guest){

    // Users can change their username here

    // get the user's ID
    $usersId = $_SESSION["userid"];

    // include the database and functions files
    include_once "dbaccess.php";
    include_once "funktionsinc.php";

    // check if the username field is not empty
    if(!empty($_POST["ChangeUsername"])){
        // get the new username
        $newUsername = $_POST["ChangeUsername"];

        // check if the input for the new username consists only of letters and numbers
        if(checkForAlphNum($newUsername) == ""){
            // check if the username already exists in the database
            if(!OnlyUidExists($conn, $newUsername)){
                // redirect to the profile page with an error message
                header("location: ../index.php?page=profile&error=usernameAlreadyTaken");
                exit(); 
            }
            else{
                // update the username in the database
                updateUsername($conn, $usersId, $newUsername);
            }
        }
        else{
            // redirect to the profile page with an error message
            header("location: ../index.php?page=profile&error=notAValidUsername");
        }
    }
    else{
        // redirect to the profile page with an error message
        header("location: ../index.php?page=profile&error=noChanges");
    }

}
else{
    header("location: ../index.php?page=profile&error=accessdenied");
}
?>
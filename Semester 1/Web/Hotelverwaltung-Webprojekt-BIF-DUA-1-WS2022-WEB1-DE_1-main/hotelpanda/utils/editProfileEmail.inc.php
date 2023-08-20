<?php

include_once "navigation.php";
// if the user is a guest
if($guest){

    // get the user's ID from the session
    $usersId = $_SESSION["userid"];

    // include the dbaccess.php and funktionsinc.php files
    include_once "dbaccess.php";
    include_once "funktionsinc.php";

    // if the ChangeEmail form has been submitted
    if(!empty($_POST["ChangeEmail"])){
        // get the new email from the form
        $newEmail = $_POST["ChangeEmail"];

        // check if the new email is valid
        if(checkEmail($newEmail) == ""){
            // if the email is not already taken
            if(!OnlyEmailExists($conn, $newEmail)){
                // redirect the user back to the index page with an error message
                header("location: ../index.php?page=profile&error=emailAlreadyTaken");
                exit(); 
            }
            else{
                // if the email is not taken, update the user's email
                updateEmail($conn, $usersId, $newEmail);
                

                
            }
        }
        else{
            // if the email is not valid, redirect the user back to the index page with an error message
            header("location: ../index.php?page=profile&error=notAValidemail");
        }
    }
    else{
        // if the ChangeEmail form has not been submitted, redirect the user back to the index page with a message
        header("location: ../index.php?page=profile&error=noChanges");
    }
    
}
else{
    // if the user is not a guest, redirect the user back to the index page with an access denied message
    header("location: ../index.php?page=profile&error=accessdenied");
    
}
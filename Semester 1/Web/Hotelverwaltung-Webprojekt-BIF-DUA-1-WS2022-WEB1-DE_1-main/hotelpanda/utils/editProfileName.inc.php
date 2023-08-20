<?php
// include the navigation file
include_once "navigation.php";

// check if the user is a guest
if($guest){
    // Users can change their first and last name here

    // get the user's ID
    $usersId = $_SESSION["userid"];

    // include the database and functions files
    include_once "dbaccess.php";
    include_once "funktionsinc.php";

    // check if the request method is POST
    if(($_SERVER["REQUEST_METHOD"]) == "POST"){

        // check if the first and last name fields are not empty
        if(!empty($_POST["ChangeFirstname"]) && !empty($_POST["ChangeLastname"])){
            
            // sanitize the input for the first and last names
            $newFirstname = checkInput($_POST["ChangeFirstname"]);
            $newLastname = checkInput($_POST["ChangeLastname"]);

            // check if the input for the first and last names consists only of letters
            if(checkForAlph($newFirstname) == "" && checkForAlph($newLastname) == ""){
                // update the first and last names in the database
                updateNames($conn, $usersId, $newLastname, $newFirstname);
                
            }
            else{
                // redirect to the index page with an error message
                header("location: ../index.php?page=profile&error=LettersOnly");
            }
        }
        else{
            // redirect to the index page with an error message
            header("location: ../index.php?page=profile&error=noChanges");
        }
    }
}
else{
    // redirect to the index page with an error message
    header("location: ../index.php?page=profile&error=accessdenied");
}
?>
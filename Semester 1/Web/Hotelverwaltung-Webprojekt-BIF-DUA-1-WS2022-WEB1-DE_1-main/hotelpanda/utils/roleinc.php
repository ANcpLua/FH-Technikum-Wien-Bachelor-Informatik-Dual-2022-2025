<?php
// set the variables to false initially
$anon =  false;
$guest = false;
$admin = false;

// check if the useruid session variable is set
if(isset($_SESSION["userid"])){
    // get the user's role
    $userRole = $_SESSION["userrole"];

    // check the user's role
    switch($userRole){
        case "guest":
            $guest = true;
            break;
        case "admin":
            $admin = true;
            break;
    }
}
else{
    // set the anonymous variable to true if the useruid session variable is not set
    $anon = true;
}
?>
<?php
include_once "navigation.php";
if($guest)
{
    // Users can change their first and last name, as well as their password here

    $usersId = $_SESSION["userid"];

    include_once "dbaccess.php";
    include_once "funktionsinc.php";

    if(($_SERVER["REQUEST_METHOD"]) == "POST"){
        // Check if the form has been submitted
        if(isset($_POST['ChangePasswordSubmit'])) {
            // Get the old password and new password from the form
            $oldPassword = $_POST['OldPassword'];
            $newPassword = $_POST['ChangePassword'];

            // Connect to the database
            $db = mysqli_connect('localhost', 'root', '', 'hotelpanda');

            // Retrieve the user's hashed password from the database
            $sql = "SELECT usersPassword FROM users WHERE usersId=?";
            $stmt = mysqli_prepare($db, $sql);
            mysqli_stmt_bind_param($stmt, 'i', $usersId);
            mysqli_stmt_execute($stmt);
            mysqli_stmt_bind_result($stmt, $userPassword);
            mysqli_stmt_fetch($stmt);
            mysqli_stmt_close($stmt);
// Compare the old password with the user's password
if (password_verify($oldPassword, $userPassword) && !empty($oldPassword) ) {
    // Hash the new password
    $hashedNewPassword = password_hash($newPassword, PASSWORD_DEFAULT);

    // Update the user's password in the database
    $sql = "UPDATE users SET usersPassword=? WHERE usersId=?";
    $stmt = mysqli_prepare($db, $sql);
    mysqli_stmt_bind_param($stmt, 'si', $hashedNewPassword, $usersId);
    mysqli_stmt_execute($stmt);
    mysqli_stmt_close($stmt);

    // Redirect the user back to the profile page
    header('Location: ../index.php?page=profile&success=passwordchanged');
exit();
} else {
    // Old password is incorrect
    header('Location: ../index.php?page=profile&error=wrongpassword');
    exit();
}
} else 
{
    // Form has not been submitted
    if(!empty($_POST["ChangeFirstname"]) && !empty($_POST["ChangeLastname"]))
    {
        
        $newFirstname = checkInput($_POST["ChangeFirstname"]);
        $newLastname = checkInput($_POST["ChangeLastname"]);
        if(checkForAlph($newFirstname) == "" && checkForAlph($newLastname) == "")
        {
            updateNames($conn, $usersId, $newLastname, $newFirstname);
        }
        else
        {
            header("location: ../index.php?page=profile&error=LettersOnly");
        }
    }
    else
    {
        header("location: ../index.php?page=profile&error=noChanges");
    }
}
}
else
{
    header("location: ../index.php?page=profile&error=accessdenied");
}
}
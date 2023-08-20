<?php
require_once 'dbaccess.php';
require_once 'funktionsinc.php';
if (isset($_POST["submit"])) {
  $username = checkInput( $_POST["username"]);
  $pwd = checkInput($_POST["pwd"]);

  

  if (emptyInputLogin($username, $pwd) !== false) {
      // redirect the user to the login page with an error message if the inputs are empty
      header("location: ../index.php?page=login&error=emptyinput");
      exit();
  }

  // query the database to get the user's status
  $query = "SELECT usersStatus FROM users WHERE usersUid=?;";
  $stmt = mysqli_stmt_init($conn);
  if (!mysqli_stmt_prepare($stmt, $query)) {
      // redirect the user to the login page with an error message if the query fails
      header("location: ../index.php?page=login&error=sqlerror");
      exit();
  }
  mysqli_stmt_bind_param($stmt, "s", $username);
  mysqli_stmt_execute($stmt);
  $result = mysqli_stmt_get_result($stmt);
  if ($row = mysqli_fetch_assoc($result)) {
      // set the $status variable to the user's status
      $status = $row["usersStatus"];
  } else {
      // redirect the user to the login page with an error message if the username is not found
      header("location: ../index.php?page=login&error=wronglogin");
      exit();
  }

  loginUser($conn, $username, $pwd, $status);

} else {
  // redirect the user to the login page if the submit parameter is not present
  header("location: ../index.php?page=login");
  exit();
}


<?php








if (!isset($guest) || !$guest) {
  echo '<p class="fs-1 text-white lead">Bitte melden Sie mit Ihrem Benutzernamen an,<br> bevor Sie Ihre Stammdaten bearbeiten!</p> ';
  header('Refresh: 2; URL = index.php?page=login');
} else {
  if ($guest) {
    $usersId = $_SESSION["userid"];
  }

  include_once "./utils/dbaccess.php";
  if (!$conn) {
    die("Connection failed: " . mysqli_connect_error());
  }
  $sql = "SELECT usersVorname, usersNachname, usersEmail, usersUid, usersPassword FROM users WHERE usersId='$usersId'";
  $result = mysqli_query($conn, $sql);

  // Check if the query was successful
  if (mysqli_num_rows($result) > 0) {
    // Output the data
    while ($row = mysqli_fetch_assoc($result)) {

       echo '
    
   <main class="container d-flex  justify-content-center h-100 align-items-center text-dark text-start">
  <div class=" col-lg-8  m-auto p-5 rounded bg-light lead fw-normal text-dark " style="--bs-bg-opacity: .9;">
    <p class="display-6">Persönliche Daten</p>

    <section class="d-flex w-100 justify-content-between align-items-center">
      <div>
        <span style="color: #873D48; text-decoration:underline;">Name:</span> ' . $row['usersVorname'] . ' ' . $row['usersNachname'] . '
        <div class="collapse" id="ChangeName">
          <div class="card card-body">
            <div>
              <form action="./utils/editProfileName.inc.php" method="post">

                <div class="container row pb-2">
                  <label class="col-md-8 " for="ChangeFirstname"><span style="color: #873D48;">Neuer Vorname:</span> </label>
                  <input class="form-control col-md-6" type="text" name="ChangeFirstname">
                </div>
                <div class="container row pb-2">
                  <label class="col-md-12 " for="ChangeLastname"><span style="color: #873D48;">Neuer Nachname:</span> </label>
                  <input class="form-control col-md-6" type="text" name="ChangeLastname">
                </div>
                <button class="btn btn-primary btn" name="ChangeNameSubmit" type="submit">Ändern</button>
                <button class="btn btn-secondary btn" name="ChangeNameClose" type="reset" data-bs-toggle="collapse" data-bs-target="#ChangeName" aria-expanded="false" aria-controls="collapseExample">Schließen</button>
              </form>
            </div>
          </div>
        </div>
      </div>
      <button class="btn btn-dark mb-1" data-bs-toggle="collapse" href="#ChangeName" role="button">Bearbeiten</a></button>
    </section>


    <section class=" d-flex w-100 justify-content-between align-items-center">
      <div>
      <span style="color: #873D48; text-decoration:underline;">E-Mail:</span> ' . $row["usersEmail"] . '</span>
        
        <div class="collapse" id="ChangeEmail">
          <div class="card card-body">
            <div>
              <form action="./utils/editProfileEmail.inc.php" method="post">
                <div class="container row pb-2">
                  <label class="col-md-12 " for="ChangeEmail">
                    <p style="color: #873D48;">Neue E-Mail:</p>
                  </label>
                  <input class="form-control col-md-6" type="text" name="ChangeEmail">
                </div>
                <button class="btn btn-primary btn" name="ChangeEmailSubmit" type="submit">Ändern</button>
                <button class="btn btn-secondary btn" name="ChangeEmailClose" type="reset" data-bs-toggle="collapse" data-bs-target="#ChangeEmail" aria-expanded="false" aria-controls="collapseExample">Schließen</button>

              </form>
            </div>
          </div>
        </div>
      </div>
      <button class="btn btn-dark mb-1 " data-bs-toggle="collapse" href="#ChangeEmail" role="button">Bearbeiten</a></button>
    </section>


    <section class=" d-flex w-100 justify-content-between align-items-center">
      <div>
      <span style="color: #873D48; text-decoration:underline;">Username:</span> ' . $row['usersUid'] . '
      <div class="collapse" id="ChangeUsername">
        <div class="card card-body">
          <div>
            <form action="./utils/editProfileUsername.inc.php" method="post">
              <div class="container row pb-2">
                <label class="col-md-12 " for="ChangeUsername"><span style="color: #873D48;">Neuer Benutzername:</span> </label>
                <input class="form-control col-md-6" type="text" name="ChangeUsername">
              </div>
              <button class="btn btn-primary btn" name="ChangeUsernameSubmit" type="submit">Ändern</button>
              <button class="btn btn-secondary btn" name="ChangeUsernameClose" type="reset" data-bs-toggle="collapse" data-bs-target="#ChangeUsername" aria-expanded="false" aria-controls="collapseExample">Schließen</button>
            </form>
          </div>
        </div>
</div>
      </div>
      <button class="btn btn-dark mb-1" data-bs-toggle="collapse" href="#ChangeUsername" role="button">Bearbeiten</a></button>
    </section>

    <section class=" d-flex w-100 justify-content-between align-items-center">
      <div>
      <span style="color: #873D48; text-decoration:underline;">Passwort</span> 
      
      <div class="collapse" id="ChangePassword">
        <div class="card card-body">
          <div>
            <form action="utils/editProfilePwd.inc.php" method="post">
              <div class="container row pb-2">
                <label style="color:#873D48" class="col-md-12" for="OldPassword">Altes Passwort:</label><br>
                <input type="password" id="OldPassword" name="OldPassword"><br>
                <label class="col-md-12"  for="ChangePasswordNew"><span style="color: #873D48;">Neues Passwort:</span> </label>
                <input class="form-control col-md-12" type="password" name="ChangePasswordNew">
              </div>
              <div class="container row pb-2">
                <label class="col-md-12 " for="ChangePasswordNewRepeat"><span style="color: #873D48;">Neues Passwort wiederholen:</span> </label>
                <input class="form-control col-md-6" type="password" name="ChangePasswordNewRepeat">
              </div>

              <button class="btn btn-primary btn-sm" name="ChangePasswordSubmit" type="submit">Ändern</button>
              <button class="btn btn-secondary btn-sm" name="ChangePasswordClose" type="reset" data-bs-toggle="collapse" data-bs-target="#ChangePassword" aria-expanded="false" aria-controls="collapseExample">Schließen</button>

            </form>
          </div>
        </div>
      </div>
  </div>
  <button class="btn btn-dark float-end mb-4 " data-bs-toggle="collapse" href="#ChangePassword" role="button">Bearbeiten</a></button>
  </section>
  </div>


</main>
  ';
    }
  }
}
if (isset($_GET["error"])) {
  echo '<div class="text-center text-white mx-auto fs-2 text-dark bg-danger bg-opacity-75 w-50 rounded">';
  echo "Das hat leider nicht funktioniert!<br>";
  switch ($_GET["error"]) {

    case "LettersOnly":
      echo "Bitte nur Buchstaben eingeben!";
      break;

    case "emailAlreadyTaken":
      echo "Diese Email wird leider bereits verwendet!";
      break;

    case "notAValidemail":
      echo "Sie haben keine gültige Emailadresse eingegeben!";
      break;


    case "accessdenied":
      echo 'Unberechtigter Zugriff. Bitte melden Sie sich an!';
      break;

    case "usernameAlreadyTaken":
      echo 'Dieser Benutzername ist leider bereits vergeben!';
      break;


    case "wrongpassword":
      echo 'Das eingegebene Passwort war leider nicht korrekt!';
      break;


    case "notAValidUsername":
      echo 'Dies ist leider kein gültiger Benutzername!';
      break;
  }
  echo "</div>;";
}
if (isset($_GET["success"])) {
  echo '<div class="text-center mx-auto fs-2 text-dark p-3 bg-light bg-opacity-75 w-50 rounded">';

  switch ($_GET["success"]) {

    case "passwordchanged":
      echo "Ihr Password wurde erfolgreich geändert!";
      break;

    case "emailchanged":
      echo "Ihre Email wurde erfolgreich geändert!";
      break;

    case "namechanged":
      echo "Ihr Name wurde erfolgreich geändert!";
      break;

    case "usernamechanged":
      echo "Ihr Benutzername wurde erfolgreich geändert!";
      break;
  }
  echo " </div>";
}

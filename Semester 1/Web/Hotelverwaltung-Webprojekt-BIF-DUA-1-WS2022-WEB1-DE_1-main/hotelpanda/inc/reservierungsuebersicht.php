<?php
if (!$guest && !$admin) { //check if user is guest or admin
  echo '<p class="fs-1 text-white">Bitte melden Sie an, <br> um die Reservierungsübersicht einzusehen!</p> ';

  header('Refresh: 2; URL = index.php?page=login');
} else { ?>
  <div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur text-center">
            
    <?php include './utils/funktionsinc.php';
    include "./utils/dbaccess.php";

    // Check if the user is logged in
    if (isset($_SESSION['userid'])) {
      // Get the user's ID from the session variable
      $userId = $_SESSION['userid'];

      // Check if the user has submitted a room reservation request
      if (isset($_POST['submit'])) {

        // Get the reservation details from the form3
        $start_date = $_POST['start_date'];
        $end_date = $_POST['end_date'];
        $room_type = $_POST['room_type'];
        @$breakfast = $_POST['breakfast'];  // falls dich das @ stört --[kein value wenn nicht selected]$breakfast = isset($_POST['breakfast']) ? $_POST['breakfast'] : 0; insert $breakfast into database
        $parking = $_POST['parking'];
        $pets = $_POST['pets'];

        // Validate the form data
        $errors = array();
        if (empty($start_date)) {
          $errors[] = "Start date is required.";
        }
        if (empty($end_date)) {
          $errors[] = "End date is required.";
        }
        if (empty($room_type)) {
          $errors[] = "Room type is required.";
        }

       /* if (!checkActiveReservation($conn, $userId, $room_type)) {
          $errors[] = '<div class="text-center mx-auto fs-4 text-dark p-3 bg-light bg-opacity-75 w-50 rounded">Fehler: Bitte warten Sie auf die Bearbeitung Ihre Reservierung bevor Sie eine neue Reservierung vornehmen"</div>';;
          header("REFRESH:5, URL= index.php?page=reservation");
        }*/
        // Convert the dates to timestamps
        $start_timestamp = strtotime($start_date);
        $end_timestamp = strtotime($end_date);

        // Check if the end date is before the start date
        if ($end_timestamp < $start_timestamp) {
          // End date is before start date, so display an error message
          $errors[] = '<div class="text-center mx-auto fs-4 text-dark p-3 bg-light bg-opacity-75 w-50 rounded">Fehler: Die Abreise liegt vor der Anreise! Sie können Ihre Daten in fünf Sekunden erneut eingeben."</div>';
          header("REFRESH:5, URL= index.php?page=reservation");
        } else {
          // Dates are valid, so check if the dates are available in the database
          $sql = "SELECT * FROM reservations WHERE start_date <= '$end_date' AND end_date >= '$start_date' AND status <>'storniert'";
          $result = mysqli_query($conn, $sql);
          if (mysqli_num_rows($result) > 0) {
            // Dates are not available, so display an error message
            $errors[] = '<div class="text-center mx-auto fs-4 text-dark p-3 bg-light bg-opacity-75 w-50 rounded">Ausgebucht! Bitte wählen Sie ein anderes Datum. (5 Sekunden bis Weiterleitung)</div>';
            header("REFRESH:5, URL= index.php?page=reservation");
          }
        }
        // If there are no errors, insert the reservation data into the database
        if (empty($errors)) {
          // Calculate the total cost of the reservation

          $extra_cost = getExtraCost($breakfast, $parking, $pets, $room_type);
          // Insert the reservation data into the database
          
          
          $diff=strtotime($end_date)-strtotime($start_date);
          $zeit= abs(round($diff/86400));
          $extra_cost=$extra_cost*$zeit;

          $query = "INSERT INTO reservations (start_date, end_date, room_type, breakfast, parking, pets, total_cost, status, usersId) VALUES ('$start_date', '$end_date', '$room_type', '$breakfast', '$parking', '$pets', '$extra_cost', 'neu', '$userId')";
          if (mysqli_query($conn, $query)) {
            // Reservation was successfully inserted
            $success_message = 'Ihre Buchung war erfolgreich! Ein Administrator wird Ihren Reservierungsstatus in Kürze bestätigen.';
          } else {
            // There was an error inserting the reservation
            $errors[] = "Error adding reservation: " . mysqli_error($conn);
            header("REFRESH:5, URL= index.php?page=reservation");
          }
        }
      }
    }

    // Display any errors or success messages
    if (!empty($errors)) {
      // Display errors
      echo "<div class='error'>" . implode("<br>", $errors) . "</div>";  // https://www.php.net/manual/en/function.implode.php Implode
    } else if (isset($success_message)) {
      // Display success message
      echo '<div class="text-center mx-auto fs-4 text-dark p-3 bg-success bg-opacity-75 w-50 rounded">' . $success_message . "</div>";
    }

// Get the user's reservations from the database
$query = "SELECT * FROM reservations WHERE usersId='$userId'";
$result = mysqli_query($conn, $query);

// Display a list of the user's reservations
echo '
<div class="table-responsive"> 
<table class="table table-bordered table-dark mt-3">
      <thead>
        <tr>
          <th scope="col">Zeitraum Start</th>
          <th scope="col">Zeitraum Ende</th>
          <th scope="col">Zimmertyp</th>
          <th scope="col">Frühstück?</th>
          <th scope="col">Parkplätze?</th>
          <th scope="col">Haustiere?</th>
          <th scope="col">Gesamtpreis in €</th>
          <th scope="col">Reservierungsstatus</th>
          <th scope="col">Datum</th>
          
        </tr>
      </thead>';
      
while ($row = mysqli_fetch_assoc($result)) {
 
 echo "<tr>";
 echo "<td><font color='white'>" . $row['start_date'] . "</td>";
 echo "<td><font color='white'>" . $row['end_date'] . "</td>";
 echo "<td><font color='white'>" . $row['room_type'] . "</td>";
 echo "<td><font color='white'>" . $row['breakfast'] . "</td>";
 echo "<td><font color='white'>" . $row['parking'] . "</td>";
 echo "<td><font color='white'>" . $row['pets'] . "</td>";
 echo "<td><font color='white'>" . $row['total_cost'] . "</td>";
 echo "<td><font color='white'>" . $row['status'] . "</td>";
 echo "<td><font color='white'>" . $row['reservation_date'] . "</td>";
 echo "</tr>";
}
echo "</table></div>";

// Close the database connection
mysqli_close($conn);}?>

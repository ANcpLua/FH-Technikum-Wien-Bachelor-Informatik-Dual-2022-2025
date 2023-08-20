<div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur text-center">

  <?php if (!$admin) {
    echo '<p class="fs-1 text-white">Bitte melden Sie als Admin an, um Daten zu ändern oder News zu erstellen!</p> ';

    header('Refresh: 2; URL = index.php?page=login');
  } else {
    include './utils/dbaccess.php'; ?>

    <div class="container-fluid mt-5 text-dark bg-light bg-opacity-75 rounded adminres">
      <h1>Reservation Management for Admins</h1>
      <div class="mt-5">
        <h2>List of All Reservations <small class="text-muted fs-6">(only edit one user at a time)</small></h2>
        <div class="table-responsive" >
          <table class="table table-bordered table-dark mt-3 " >
            <thead>
              <tr>
                <th class="th-lg-9">Status</th>
                <th>User ID</th>
                <th>Arrival</th>
                <th>End</th>
                <th>Room Type</th>
                <th>Breakfast</th>
                <th>Parking</th>
                <th>Pets</th>
                <th>Total Cost</th>
                <th>Date</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
        </div>
        <?php
        require_once './utils/dbaccess.php';

        if (empty(mysqli_connect_error())) { //no db error
          try {
            $sql1 = "SELECT status, usersId, start_date, end_date, room_type, breakfast, parking, pets, total_cost,reservation_date FROM reservations";
            $result3 = $conn->query($sql1);

            if ($result3->num_rows <= 0) {
              echo "Bisher keine Reservierungen.";
            } else {
              while ($row = $result3->fetch_assoc()) {

        ?>

                <tr>
                  <form method="POST" action="">
                    <td>
                      <input type="text" name="status" id="status" value="<?php echo $row['status']; ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="usersId" id="usersId" value="<?php echo $row['usersId'] ?>" class="form-control" readonly>
                    </td>
                    <td>
                      <input type="text" name="start_date" id="start_date" value="<?php echo $row['start_date']; ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="end_date" id="end_date" value="<?php echo $row['end_date'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="room_type" id="room_type" value="<?php echo $row['room_type'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="breakfast" id="breakfast" value="<?php echo $row['breakfast'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="parking" id="parking" value="<?php echo $row['parking'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="pets" id="pets" value="<?php echo $row['pets'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="total_cost" id="total_cost" value="<?php echo $row['total_cost'] ?>" class="form-control">
                    </td>
                    <td>
                      <input type="text" name="date" id="reservation_date" disabled value="<?php echo $row['reservation_date'] ?>" class="form-control"></input>
                    </td>
                    <td>
                      <button type="submit" name="submit" class="btn btn-primary">Save</button>
                    </td>
                  </form>
                </tr>
                <?php
                if (isset($_POST['submit'])) {
                  // form has been submitted, process the data

                  // retrieve form data
                  $status = $_POST['status'];
                  $start_date = $_POST['start_date'];
                  $end_date = $_POST['end_date'];
                  $room_type = $_POST['room_type'];
                  $breakfast = $_POST['breakfast'];
                  $parking = $_POST['parking'];
                  $pets = $_POST['pets'];
                  $total_cost = $_POST['total_cost'];
                  $usersId = $_POST['usersId'];


                  // create prepared statement
                  $query = "UPDATE reservations SET status = ?, start_date = ?, end_date = ?, room_type = ?, breakfast = ?, parking = ?, pets = ?, total_cost = ? WHERE usersId = ?";
                  $stmt = mysqli_prepare($conn, $query);

                  // bind parameters
                  mysqli_stmt_bind_param($stmt, "ssssssssi", $status, $start_date, $end_date, $room_type, $breakfast, $parking, $pets, $total_cost, $usersId);

                  // execute prepared statement
                  mysqli_stmt_execute($stmt);
                  $affected_rows = mysqli_stmt_affected_rows($stmt);
                  if ($affected_rows == 1) {
                    $page = $_SERVER['PHP_SELF'];
                    echo '<meta http-equiv="Refresh" content="2;./index.php?page=adminres">';
                    echo '<p class="bg-success lead px-5">Der Benutzer wurde erfolgreich bearbeitet!</p>';
                    mysqli_stmt_close($stmt);
                  } else {
                    $page = $_SERVER['PHP_SELF'];
                    echo '<meta http-equiv="Refresh" content="2;./index.php?page=adminres">';
                    echo '<p class="bg-danger">Benutzer nicht bearbeitet!</p>';
                    mysqli_stmt_close($stmt);
                  }
                }


                ?>
        <?php
              }
              echo "</div></div>";
            }
          } catch (Exception $e) {
            echo "Error: " . $e->getMessage();
          }
        }
        ?>
        </tbody>
        </table>
      </div>
    </div>
</div>
<?php } ?>
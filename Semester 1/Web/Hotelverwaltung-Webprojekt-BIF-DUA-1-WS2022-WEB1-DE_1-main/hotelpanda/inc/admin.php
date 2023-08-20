<?php if (!$admin) { //check if user is admin
echo'<p class="fs-1 text-white">Bitte melden Sie als Admin an, um Daten zu ändern oder News zu erstellen!</p> ';

  header('Refresh: 2; URL = index.php?page=login');
} else {
  include_once "./utils/dbaccess.php" ?>


  <div class=" container mt-5 text-dark bg-light bg-opacity-75 rounded ">
    <div class="container mt-5">
      <h1>User Management for Admins</h1>

      <h2>List of Registered Users <small class="text-muted fs-6">(only edit one user at a time)</small></h2>
      
      <table class="table table-bordered table-dark mt-3">
        <thead>
          <tr>
            <th scope="col">ID</th>
            <th scope="col">First Name</th>
            <th scope="col">Last Name</th>
            <th scope="col">Login Name</th>
            <th scope="col">Password</th>
            <th scope="col">Email</th>
            <th scope="col">Type</th>
            <th scope="col">Status</th>
            <th scope="col"></th>
          </tr>
        </thead>
        <tbody> <?php require_once './utils/dbaccess.php';
                if (empty(mysqli_connect_error())) { //no db error
                  {
                    $sql = "SELECT usersId, usersVorname ,usersNachname, usersPassword, usersUid, usersEmail, usersTyp, usersStatus FROM users where usersTyp='guest'";
                    $result2 = $conn->query($sql);

                    if ($result2->num_rows > 0) {
                      while ($line = $result2->fetch_assoc()) {
                ?>
                  <tr>
                    <form style="border-radius:30%" method="POST" action="">
                      <td>
                        <input type="text" name="usersId" id="usersId" value="<?php echo $line['usersId']; ?>" class="form-control" readonly>
                      </td>
                      <td>
                        <input type="text" name="usersVorname" id="usersVorname" value="<?php echo $line['usersVorname']; ?>" class="form-control">
                      </td>
                      <td>
                        <input type="text" name="usersNachname" id="usersNachname" value="<?php echo $line['usersNachname']; ?>" class="form-control">
                      </td>
                      <td>
                        <input type="text" name="usersUid" id="usersUid" value="<?php echo $line['usersUid']; ?>" class="form-control">
                      </td>
                      <td>
                        <input type="password" name="usersPassword" id="usersPassword" value="<?php echo $line['usersPassword']; ?>" class="form-control">
                      </td>
                      <td>
                        <input type="text" name="usersEmail" id="usersEmail" value="<?php echo $line['usersEmail']; ?>" class="form-control">
                      </td>
                      <td>
                        <input type="text" name="usersTyp" id="usersTyp" value="<?php echo $line['usersTyp']; ?>" class="form-control">
                      </td>
                      <td>

                        <!-- <input type="text" name="usersStatus" id="usersStatus" value="
                        <?php
                        /* echo $line['usersStatus']; */ ?>" class="form-control">-->
                        <select name="usersStatus" id="usersStatus" class="form-select" aria-label="Default select example">
                          <option selected><?php echo $line['usersStatus']; ?></option>

                          <?php
                          if ($line['usersStatus'] == "active") {
                            echo '<option value="inactive">inactive</option>';
                          } else if ($line['usersStatus'] == "inactive") echo ' <option value="active">active</option>';
                          else {
                            echo ' <option value="active">active</option>';
                            echo '<option value="inactive">inactive</option>';
                          }
                          ?>
                        </select>


                      </td>
                      <td>
                        <button type="submit" name="submit" class="btn btn-primary">Save</button>
                      </td>
                    </form>
                  </tr>
          <?php
                      }
                    } else {
                      echo "No users found";
                    }
                  }
                } else {
                  echo "Error connecting to database: " . mysqli_connect_error();
                }
          ?>
        </tbody>
      </table>
    </div>
  <?php
  if (isset($_POST['submit'])) {
    // form has been submitted, process the data

    // retrieve form data
    $usersVorname = $_POST['usersVorname'];
    $usersNachname = $_POST['usersNachname'];
    $usersUid = $_POST['usersUid'];
    $usersEmail = $_POST['usersEmail'];
    $usersPassword = $_POST['usersPassword'];
    $usersTyp = $_POST['usersTyp'];
    $usersStatus = $_POST['usersStatus'];
    $usersId = $_POST['usersId'];

    // hash the password using the password_hash function
    $hashedPassword = password_hash($usersPassword, PASSWORD_DEFAULT);


    // create prepared statement
    $query = "UPDATE users SET usersVorname = ?, usersNachname = ?, usersUid = ?, usersPassword = ?, usersEmail = ?, usersTyp = ?, usersStatus = ? WHERE usersId = ?";
    $stmt = mysqli_prepare($conn, $query);

    // bind parameters
    mysqli_stmt_bind_param($stmt, "sssssssi", $usersVorname, $usersNachname, $usersUid, $hashedPassword, $usersEmail, $usersTyp, $usersStatus, $usersId);

    // execute prepared statement
    mysqli_stmt_execute($stmt);
    $affected_rows = mysqli_stmt_affected_rows($stmt);
   
    if ($affected_rows == 1) {
      
      echo '<meta http-equiv="Refresh" content="2;./index.php?page=admin">';
      echo '<p class="bg-success lead px-5">Der Benutzer wurde erfolgreich bearbeitet!</p>';
      mysqli_stmt_close($stmt);
    } else {
      $page = $_SERVER['PHP_SELF'];
      echo '<meta http-equiv="Refresh" content="2;./index.php?page=delnews">';
      echo '<p class="bg-danger">Benutzer nicht bearbeitet!</p>';
      mysqli_stmt_close($stmt);
    }
    if (empty($usersVorname) || empty($usersNachname) || empty($usersUid) || empty($hashedPassword) || empty($usersEmail) || empty($usersTyp) || empty($usersStatus)) {
      echo '<br><p class="bg-danger" style="display:inline-block;"> 1 or more columns are empty  </p></br>';
    }

    if ($usersStatus == "inaktiv") {
      $conn->query("UPDATE users set oneAktiv_zeroInaktiv = 0 where usersStatus = 'inaktiv'");
    }
    if ($usersStatus == "aktiv") {
      $conn->query("UPDATE users set oneAktiv_zeroInaktiv = 1 where usersStatus = 'aktiv'");
    }
    
  }
 
} ;
 ?>
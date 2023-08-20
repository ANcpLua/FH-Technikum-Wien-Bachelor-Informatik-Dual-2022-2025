

<div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur">
  <div class="container d-flex  justify-content-center h-75 align-items-center">
    <div class="row col-12 col-sm-10 col-md-6 col-lg-4 col-xl-4">
      <div class="card card-body p-4 bg-dark text-white " style="border-radius: 1rem;">
        <h2 class=" mb-2 text-uppercase text-center">Login</h2>
        <div class="form-outline form-white mb-4 ">
          <form action="./utils/logininc.php" method="post">
            <label class="form-label ps-1 ">Username</label>
            <input type="text" name="username" class="form-control form-control-lg" />
        </div>
        <div class="form-outline form-white mb-4  ">
          <label class="form-label  d-flex justify-content-between align-items-center px-1  "><span>Passwort</span> <a class=" text-right text-white-50 small "></a></label>
          <input type="password" name="pwd" class="form-control form-control-lg" />
        </div>
        <!-- include -->
        <div class="d-flex justify-content-center pb-4"><button class="btn btn-lg  col-6" style="height: 3.1rem;  border:solid 2px white;" type="submit" name="submit">Login</button>
        </div>
        <div class=" text-center align-items-center  small">Besitzen Sie noch kein Konto? <a href="index.php?page=register" class="text-white-50 fw-bold">Hier registrieren</a></div>

        <!-- errors -->
        <div class="d-flex justify-content-center">
          </form>
          <?php

          if (isset($_GET["error"]))     // get = wenn url was sagt sonst post
          {
            if ($_GET["error"] == "emptyinput") {
              echo "<h3><p> Bitte füllen Sie alle Felder aus.</p></h3>";
            } else if ($_GET["error"] == "wronglogin") {
              echo "<h3><p> Leider waren die Login-Daten nicht korrekt. </p></h3>";
            } else if ($_GET["error"] == "none") {
              echo "<h3>Sie haben sich erfolreich registriert! Bitte melden Sie sich mit Ihrem Benutzernamen an. </h3>";
            } else if ($_GET["error"] == "inactive") {
              echo  '<br><p class="bg-danger rounded" style="display:inline-block;">Ihr Benutzerkonto wurde deaktiviert, bitte wenden Sie sich an einen Admin.</p></br>';
            }
          }

          ?>
        </div>

      </div>
    </div>
  </div>

</div>
<?php

if (!$guest) { //check if user is guest
  echo '<p class="fs-1 text-white">Bitte melden Sie mit Ihrem Benutzernamen an, <br> bevor Sie ein Zimmer reservieren!</p> ';

  header('Refresh: 2; URL = index.php?page=login');
} else { ?>

  <div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur">
    <div class="container d-flex justify-content-center h-75 align-items-center ">
      <div class="row col-12 col-sm-10 col-md-6 col-lg-4 col-xl-6">
        <div class="card card-body p-4 bg-dark text-white " style="border-radius: 1rem;">
          <form action="index.php?page=reservierungsuebersicht" method="post">
            <div class="form-group p-2 px-5">
              <label for="start_date" class="text-white lead p-1">Check-in</label>
              <input type="date" class="form-control" name="start_date" id="start_date" required>
            </div>
            <div class="form-group p-2 px-5">
              <label for="end_date" class="text-white lead p-1">Check-out</label>
              <input type="date" class="form-control" name="end_date" id="end_date" required>
            </div>
            <div class="form-group p-2 px-5">
              <label for="room_type" class="text-white lead p-1">Zimmertyp </label>
              <select class="form-control" name="room_type" id="room_type" required>
                <option value="Standard">Standard</option>
                <option value="Deluxe">Deluxe</option>
                <option value="Suite">Suite</option>
              </select>
              <p class="lead">Standard 100€ | Deluxe 150€ | Suite 210€</p>
            </div>
            <div class="form-group form-check text-center p-2" style="float:none !important;">
              <input type="checkbox" class="form-check-input lead" style="float:none !important;" name="breakfast" value="Ja">
              <label class="form-check-label text-white lead">Frühstück | Buffet inkl. Sekt | 16€ pro Tag & Person </label>
            </div>
            <div class="form-group p-2 px-5">
              <label class="text-white lead p-1">Benötigen Sie Parkplätze? | 10€ | Tag</label>
              <input type="number" class="form-control" name="parking" min="0" max="5" value="0">
            </div>
            <div class="form-group p-2 px-5">
              <label class="text-white lead p-1">Haustiere? | 15€/Tag  | inkl. Betreuung</label>
              <input type="number" class="form-control" name="pets" min="0" max="2" value="0">
            </div>
            <div class="col-md-3 mx-auto mt-2">
              <button type="submit" name="submit" class="btn btn-dark border">Submit</button>
          </form>
        </div>
      </div>
    </div>
  </div>
<?php } ?>
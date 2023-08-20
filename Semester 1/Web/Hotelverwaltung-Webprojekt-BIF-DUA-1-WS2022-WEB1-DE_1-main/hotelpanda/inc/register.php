<h3>
    <div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur" >
        <div class="container d-flex justify-content-center h-75 align-items-center h5">
            <div class="row col-12 col-sm-10 col-md-6 col-lg-4 col-xl-4">
                <div class="card card-body p-4 bg-dark text-white " style="border-radius: 1rem;">
                    <form action="./utils/registerinc.php" method="post"> <!-- include -->
                        <div class="container row pb-2 ">
                            <label class="form-label ps-1 " for="Anrede">Anrede: </label>
                            <select class="form-label ps-1 " name="anrede" id="anrede">
                                <option selected name="anrede" value="">Keine Angabe</option>
                                <option name="anrede" value="Herr">Herr</option>
                                <option name="anrede" value="Frau">Frau</option>
                                <option name="anrede" value="Diverse">Diverse</option>
                            </select>
                        </div>
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="firstname">Vorname*:</label>
                            <input class="form-control form-control-lg" type="text" name="firstname">
                        </div class="container row pb-2">
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="lastname">Nachname*:</label>
                            <input class="form-control form-control-lg" type="text" name="lastname">
                        </div>
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="email">E-Mail*:</label>
                            <input class="form-control form-control-lg" type="text" name="email">
                        </div>
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="username">Benutzername*:</label>
                            <input class="form-control form-control-lg" type="text" name="username">
                        </div>
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="pwd">Passwort wählen*:</label>
                            <input class="form-control form-control-lg" type="password" name="pwd">
                        </div>
                        <div class="container row pb-2">
                            <label class="form-label ps-1 " for="pwdRepeat">Passwort wiederholen*:</label>
                            <input class="form-control form-control-lg" type="password" name="pwdRepeat">
                        </div>
                        <div class="d-flex justify-content-center">
                            <button type="submit" name="submit" class="btn btn-lg  col-5" style="height: 3.1rem;  border:solid 3px white;">Senden</button>
                        </div>
                        <div class="d-flex justify-content-center">
                            <!-- errors -->
                            <?php
                            if (isset($_GET["error"]))     // get = wenn url was sagt sonst post
                            {
                                if ($_GET["error"] == "emptyinput") {
                                    echo "<p>Alle Felder mit * sind Pflichtfelder!</p>";
                                } else if ($_GET["error"] == "invaliduid") {
                                    echo "<p>Bitte geben Sie Ihren Vornamen ein! </p>";
                                } else if ($_GET["error"] == "invalinachname") {
                                    echo "<p>Bitte geben Sie Ihren Nachnamen ein! </p>";
                                } else if ($_GET["error"] == "invalidemail") {
                                    echo "<p>Bitte geben Sie eine gültige Emailadresse ein! </p>";
                                } else if ($_GET["error"] == "passwordsdontmatch") {
                                    echo "<p>Die Passwörter stimmen nicht überein! </p>";
                                } else if ($_GET["error"] == "stmtfailed") {
                                    echo "<p>Something went wrong, try again! </p>";
                                } else if ($_GET["error"] == "usernametaken") {
                                    echo "<p>Der Benutzername ist bereits vergeben! </p>";
                                } else if ($_GET["error"] == "none") {
                                    echo "<p>Sie wurden erfolgreich registriert bitte loggen Sie sich mit Ihrem Benutzernamen ein. </p>";
                                }
                            }
                            ?>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</h3>
<div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur text-center">

    <?php if (!$admin) {
        echo '<p class="fs-1 text-white">Bitte melden Sie als Admin an, um Daten zu ändern oder News zu erstellen!</p> ';

        header('Refresh: 2; URL = index.php?page=login');
    } else {
        include './utils/dbaccess.php'; ?>

        <div class="container mt-5 text-dark bg-light bg-opacity-75 rounded">
            <h1>News Management for Admins</h1>
            <div class="mt-5">

                <div class="table-responsive">
                    <table class="table table-bordered table-dark mt-3">
                        <thead>
                            <tr>
                                <th>ID</th>
                                <th>Img</th>
                                <th>Titel</th>
                                <th>Text</th>
                                <th>Datum</th>
                                <th></th>
                                <th></th>

                            </tr>
                        </thead>
                        <tbody>
                </div>
            <?php
            require_once './utils/dbaccess.php';

            if (empty(mysqli_connect_error())) { //no db error
                try {
                    $sql4 = "SELECT newsid, newsfile_path, newstitle, newsarticle, newsdate, newsfile_path FROM news";
                    $result4 = $conn->query($sql4);

                    if ($result4->num_rows <= 0) {
                        echo "Bisher keine News eingetragen.";
                    } else {
                        while ($row = $result4->fetch_assoc()) {

                            echo '
                            <tr>
                                <form method="POST" action="">
                                    <td>
                                        <input type="text" name="newsid" id="newsid" value="' . $row['newsid'] . '" class="form-control"  readonly>
                                    </td>
                                    <td>
                                        <input type="text" name="newsfile_path" id="newsfile_path" value="' . $row['newsfile_path'] . '" class="form-control" readonly>
                                    </td>
                                    <td>
                                        <input type="text" name="newstitle" id="newstitle" value="' . $row['newstitle'] . '" class="form-control"  readonly>
                                    </td>
                                    <td>
                                        <input type="text" name="newsarticle" id="newsarticle" value="' . $row['newsarticle'] . '" class="form-control"  readonly>
                                    </td>
                                    <td>
                                        <input type="text" name="newsdate" id="newsdate" value="' . $row['newsdate'] . '" class="form-control"  readonly>
                                    </td>
                                    
                                    <td><input class="form-check-input" type="checkbox" name ="test" value="' . $row['newsid'] . '" id="flexCheckDefault">
                                    </td>
                                    <td>
                                        <button type="submit" name="submit" class="btn btn-primary">Löschen</button>
                                    </td>
                                </form>
                            </tr>';
                        }
                        if (isset($_POST['submit'])) {
                            // form has been submitted, process the data

                            // retrieve form data

                            if (isset($_POST['test'])) {

                                /* var_dump($_POST);*/

                                $id_to_del = $_POST['test'];

                                if (isset($_POST['newsfile_path'])) {
                                    $filepath = $_POST['newsfile_path'];
                                }




                                // create prepared statement
                                /*https://stackoverflow.com/questions/57790968/delete-multiple-rows-with-checkbox-and-pdo-prepared-statements*/


                                $query = "DELETE FROM news where newsid=?";
                                $stmt = mysqli_prepare($conn, $query);

                                // bind parameters
                                mysqli_stmt_bind_param($stmt, "i", $id_to_del);

                                // execute prepared statement
                                mysqli_stmt_execute($stmt);
                                $affected_rows = mysqli_stmt_affected_rows($stmt);

                                if ($affected_rows == 1) {
                                    /***delete img in thumbnail and news */
                                    if (!empty($filepath)) {
                                        echo'<div class="container w-50">';
                                        $pattern = "/res\/uploads\/thumbnails\//";
                                        $arrayRegex = preg_split($pattern, $_POST['newsfile_path']);
                                        $pathToImg = end($arrayRegex);
                                        $base_directory = 'res/uploads/thumbnails/';
                                        if (@unlink($base_directory . $pathToImg))
                                            echo '<p class="  bg-success lead px-5 ">Das Bild ' . $pathToImg . ' wurde aus dem Thumbnails-Ordner gelöscht!</p>';
                                        else {
                                            echo '<p class="danger">Das Bild ' . $pathToImg . ' wurde nicht aus dem Thumbnails-Ordner gelöscht!</p>';
                                        }
                                        $base_directory = './res/uploads/news/';
                                        if (@unlink($base_directory . $pathToImg))
                                            echo '<p class="bg-success lead px-5">Das Bild ' . $pathToImg . ' wurde aus dem News-Ordner gelöscht!</p>';
                                        else {
                                            echo '<p class="bg-danger">Das Bild ' . $pathToImg . ' wurde nicht aus dem News-Ordner gelöscht!</p><br>';
                                        }
                                       echo "</div>";
                                    }
                                    
                                    echo '<meta http-equiv="Refresh" content="3;./index.php?page=delnews">';
                                    echo '<p class="bg-success lead px-5">';
                                    echo 'Der News-Beitrag wurde aus der DB gelöscht!<br></p>';



                                    mysqli_stmt_close($stmt);
                                } else {
                                    $page = $_SERVER['PHP_SELF'];
                                    echo '<meta http-equiv="Refresh" content="3;./index.php?page=delnews">';
                                    echo '<p class="bg-danger">Der News-Beitrag wurde nicht gelöscht!</p>';


                                    mysqli_stmt_close($stmt);
                                }
                            }
                        }
                    }
                    echo "</div></div>";
                } catch (Exception $e) {
                    echo "Error: " . $e->getMessage();
                }
            }

            echo '
            </tbody>
            </table>
        </div>
    </div>
</div>';
        } ?>
<div class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image-blur overflow-visible">



    <?php
    require_once './utils/dbaccess.php';
    if (empty(mysqli_connect_error())) { //no db error
        try {
            $sql = "SELECT newsid, newsfile_path, newstitle, newsarticle, newsdate FROM news ORDER BY newsdate DESC";
            $result = $conn->query($sql);

            if ($result->num_rows > 0) {
                echo '
                <div class="col-lg-8 container  mx-auto">';
                // output data of each row
                while ($row = $result->fetch_assoc()) {
                    echo '                    
                    <div class="col-lg-12 mt-4  ">
                    <div class="bg-dark pt-3 rounded">';
                    if (!empty($row["newsfile_path"])) {
                        echo '<img class="img-fluid   " src="' . $row["newsfile_path"] . '"> <br>';
                    }
                    echo '<br>
                            <div class="lead fs-3 mt-3 p-2" style="background-color:#f6b47aec";><strong>' .    $row["newstitle"] . "</strong></div>";
                    echo '<div class="text-start px-5 py-3 news-text lead fw-normal  " style="background-color:#f6b47aec; border-bottom-right-radius:0.5rem; border-bottom-left-radius:0.5rem; ">';
                    echo  $row["newsarticle"];
                    echo "<hr>";

                    echo '<p class="fs-6">veröffentlicht am ' . substr($row['newsdate'], 0, -8) . '</p>';
                    echo "</div>";
                    echo "";
                    echo '</div>';
                }
                echo "</div>
                </div>";
            } else {
                echo ' <p class=" style="color:white;" >Bisher wurden noch keine News eingetragen! Wenden Sie sich an einen Admin. ';
            }
        } catch (Exception $err) {
            $uploadError .= "dbErr|";
            $conn->close();
        }
    }
    $conn->close();
    ?>



</div>
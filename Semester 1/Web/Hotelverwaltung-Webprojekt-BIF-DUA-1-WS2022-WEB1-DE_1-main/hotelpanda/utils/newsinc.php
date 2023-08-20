<?php
//CAUTION: MUST ACTIVATE GD IN PHP.INI (VIA CONTRON PANEL) FOR IMG RESIZING TO WORK, JUST REMOVE ";" BEFORE IT
function unsetAll()
{
    unset($_FILES);
    unset($POST);
    unset($filePath);
    unset($targetDir);
}


function cleanInput($data)
{
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);
    return $data;
}
//var_dump(@$_GET['error']);



$uploadError = "";
//var_dump($_FILES);


//process upload only if all fields are filled -img not required
if (isset($_POST["newsTitle"]) && isset($_POST["newsText"])) {
    // if there is an image, process it first
    // var_dump($_FILES["imgUpload"]);
    if (!empty($_FILES["imgUpload"]["name"])) {
        /* &&  !empty($_POST["imgUpload"]) */
        
        //change it to ___FILE__ or ___DIR__?
        $targetDir =  "../res/uploads/news/";
        $thumbDir =  "../res/uploads/thumbnails/";
        $fileType = "img/*";
        $maxSize = 15728640; // 52428805mb 15728640 15 mb

        //check if $targetdir exists and creates one if nonexistent
        if (!is_dir($targetDir)) {
            mkdir($targetDir, 0777, true);
            if (is_dir($targetDir)) {
                echo "target dir created";
            }
        }

        $fileType = $_FILES["imgUpload"]["type"];
        $filePath = $_FILES["imgUpload"]["tmp_name"];
        $fileSize = $_FILES["imgUpload"]["size"];
        $fileName = cleanInput($_FILES["imgUpload"]["name"]);
        
        //maybe do it with mimetype -> not really safer!
        if (!($fileType == "image/jpeg")) {
            $uploadError = "wrongfiletype";

            //header('Refresh: 6; URL = index.php?page=createnews&error=wrongfiletype');

            // header("location: ../index.php?page=profile&error=noChanges");
        }

        if ($fileSize > $maxSize) {
            $uploadError = "oversized";

            
        }

        if (!empty($_FILES['userfile']['error'])) {
            //missing array!!!
            $uploadError = "unknown";
            //$uploadError = "php" . $_FILES['userfile']['error'] . "|";
            // header('location: ../index.php?page=createnews&error=unknown');
        }

        if (is_file($targetDir . $fileName)) {
            $uploadError = "imgexists";
            //header('Refresh: 4; URL = index.php?page=createnews&error=imgexists');
        }
        //upload img

        // we dont need to check if file was uploaded with post because move_uploaded_file already does that ->
        //https://www.php.net/manual/en/function.move-uploaded-file.php

        if (move_uploaded_file($filePath, $targetDir . $fileName)) {
        } else {

            $uploadError = "filetransfer";
            // header('location: ../index.php?page=createnews&error=filetransfer');
            //header('Refresh: 4; URL = index.php?page=createnews');
        }
        //check if there is no error from uploading! else remove the image from news folder
        if (!empty($uploadError)) {

            if ($uploadError != "imgexists") {
                @unlink($targetDir . $fileName);
            }
            unsetAll();
            header('location: ../index.php?page=createnews&error=' . $uploadError);
        }

        //img resizing

        $fileTmp = $targetDir . $fileName;
        $thumbnailFile = $thumbDir . $fileName;

        $sourceTmp = imagecreatefromjpeg($fileTmp);

        list($width, $height) = getimagesize($fileTmp);

        $offsetWidth = 0;
        $offsetHeight = 0;
        if ($width > 720 || $height > 480) {
            $factor = min($width / 720, $height / 480);

            //check if one side is already smaller
            if ($factor < 1) {
                $factor = max($width / 720, $height / 480);
            }
            $offsetWidth = ($width - 720 * $factor) / 2;
            $offsetHeight = ($height - 480 * $factor) / 2;


            $width = $width - 2 * $offsetWidth;
            $height = $height - 2 * $offsetHeight;
        }

        $thumb = imagecreatetruecolor(720, 480); //draws empty image

        if (imagecopyresized($thumb, $sourceTmp, 0, 0, $offsetWidth, $offsetHeight, 720, 480, $width, $height)) {
            echo "resize success";
        } else {
            echo "resize fail";
            $uploadError .= "resizeFail|";
            unsetAll();
            header('location: ../index.php?page=createnews&error=resizefail');
        }

        if (imagejpeg($thumb, $thumbnailFile)) { //creates copy, if it fails db has no filepath
            $imgPath = substr($thumbnailFile, 3); //remove ../ from path

        }
    }


    //insert into database

    require_once 'dbaccess.php';
    if (empty(mysqli_connect_error())); //no errors when connecting to db
    if (!isset($imgPath)) {
        $imgPath = "";
    }
    $sql = "INSERT INTO news (newsfile_path, newstitle, newsarticle) VALUES (?, ?, ?);";

    $newsTitle=cleanInput($_POST["newsTitle"]);
    $newsText=cleanInput($_POST["newsText"]);
    try {
        if ($stmt = $conn->prepare($sql)) {
            mysqli_stmt_bind_param($stmt, "sss", $imgPath, $newsTitle, $newsText);
            mysqli_stmt_execute($stmt);
            mysqli_stmt_close($stmt);

            $conn->close();

            //reset on succesful upload
            unsetAll();
            unset($imgPath);

            header('location: ../index.php?page=news');
        }
    } catch (Exception $err) {
        $uploadError .= "dbErr|";

        $conn->close();

        header('location: ../index.php?page=createnews&error=dbfail');
        // header('Refresh: 2; URL = index.php?page=login');
    }
} else  if (!empty($_POST)) {
    $uploadError .= "fieldsMiss|";
    header('location: ../index.php?page=createnews&error=fieldsmissing');

}

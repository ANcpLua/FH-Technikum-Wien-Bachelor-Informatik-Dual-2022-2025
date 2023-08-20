<!doctype html>
<html lang="en">

<?php include_once './utils/headinc.php';

?>

<body class="overflow-auto body-home d-flex flex-column min-vh-100 bg-image text-center">
    <?php include_once './utils/navigation.php'; ?>
    <?php include './utils/roleinc.php'; ?>

    <?php
  

    $page = (isset($_GET['page'])) ? $_GET['page'] : "landing";
    $pages = [
        "landing" => "./inc/landing.php",
        "news" => "inc/news.php",
        "faq" => 'inc/faq.php',
        "login" => "inc/login.php",
        "profile" => "inc/profile.php",
        "register" => "inc/register.php",
        "reservation" => "inc/reservation.php",
        "reservierungsuebersicht" => "inc/reservierungsuebersicht.php",
        "admin" => "inc/admin.php",
        "adminres" => "inc/adminres.php",
        "impressum" => "inc/impressum.php",
        "createnews" => "inc/news-create.php",
        "delnews" => "inc/news-del.php"

    ];

    //check if file exists then include it
    if (isset($pages[$page])) {
        if (file_exists($pages[$page])) {
            include $pages[$page];
        }
    } else {
        echo  '<p class="   text-center mx-auto fs-2 text-dark bg-light bg-opacity-75 w-50 rounded">Die Seite '.$page . ' wurde leider nicht gefunden.<br> Wenden Sie sich an useren Admin (admin@imperial-panda.at) für weitere Informationen!</p>';
    }
    include_once './utils/footer.php'; ?>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-OERcA2EqjJCMA+/3y+gxIOqMEjwtxJY7qPCqsdltbNJuaOe923+mo//f6V8Qbsw3" crossorigin="anonymous"></script>
</body>


</html>
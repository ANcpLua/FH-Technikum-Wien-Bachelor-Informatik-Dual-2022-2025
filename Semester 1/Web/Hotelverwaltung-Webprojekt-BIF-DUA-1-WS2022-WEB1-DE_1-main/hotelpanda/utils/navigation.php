<?php
@session_unset();
@session_start();
include_once 'roleinc.php';
?>
<!--Navbar    https://getbootstrap.com/docs/5.2/components/navbar/#how-it-works-->
<nav class="navbar navbar-expand-lg ">
    <div class="container-fluid">
        <a class="navbar-brand" href="index.php"><img src="res/img/logo5.png" width="55" alt="" class="d-inline-block align-middle mr-2"></a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse " id="navbarSupportedContent">
            <ul class="navbar-nav mx-auto mb-2 mb-lg-0 ">
                <li class="nav-item lead fw-normal ">
                    <a class="nav-link" aria-current="page" href="index.php">Home</a>
                </li>
                <li class="nav-item lead  fw-normal">
                    <a class="nav-link " href="index.php?page=faq">Help/FAQs</a>
                </li>
                <li class="nav-item lead  fw-normal">
                    <a class="nav-link" href="index.php?page=news">News</a>
                </li>


                <?php
                if (!$anon && $guest) {
                    echo '
           
            
            
                <li class="nav-item lead fw-normal">
                    <a class="nav-link  " href="index.php?page=reservation">Zimmerreservierung</a>
                </li>    
                <li class=" lead fw-normal nav-item">
                    <a class="nav-link lead " href="index.php?page=reservierungsuebersicht">Reservierungsübersicht</a>
                </li>
            ';
                }

                if ($admin) {
                    echo '
                   
                <li class="nav-item lead fw-normal "><a class="nav-link" href="index.php?page=createnews">News erstellen</a></li>
                <li class="nav-item lead fw-normal "><a class="nav-link" href="index.php?page=delnews">News löschen</a></li>
                <li class="nav-item lead fw-normal">
                <a class="nav-link " href="index.php?page=adminres">Reservierungsverwaltung </a>
            </li>
                  <li class="nav-item lead fw-normal "><a class="nav-link" href="index.php?page=admin">Userverwaltung</a></li>
                
                   
         <div class="nav-item dropdown last-item lead" >
            <a class="nav-link dropdown-toggle btn btn-dark dropdown-toggle lead" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Hallo, ' . $_SESSION["useruid"] . '  </a>
            <ul class="dropdown-menu dropdown-menu-end  ">
                  <li><a class="dropdown-item" href="utils/logoutinc.php">Logout</a></li>      
            </ul>
        </div >';
                }


                if ($guest) {

                    echo '
                    <div class="nav-item dropdown last-item lead " >
                    <a class="nav-link dropdown-toggle btn btn-dark dropdown-toggle lead" href="#" role="button" data-bs-toggle="dropdown" aria-expanded="false">Hallo, ' . $_SESSION["useruid"] . '  </a>
                    <ul class="dropdown-menu dropdown-menu-end  ">
                        <li><a class="dropdown-item" href="index.php?page=profile">Profilverwaltung</a></li>
                          <li><a class="dropdown-item" href="utils/logoutinc.php">Logout</a></li>      
                    </ul>
                </div >


             
                ';
                }
                if (!isset($_SESSION["useruid"])) {

                    echo "</ul></Ul><a href='index.php?page=register' class=' fs-5 btn btn-hover  fw-normale lead me-1'> Register </a>";
                    echo "<a href='index.php?page=login' class='btn btn-hover fs-5  lead me-1'>Log in </a>";
                }
                ?>

            </ul>

        </div>
    </div>
</nav>
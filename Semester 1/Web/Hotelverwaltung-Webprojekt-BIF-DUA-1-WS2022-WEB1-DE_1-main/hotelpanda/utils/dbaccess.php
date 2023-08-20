<?php 
//include_once "../utils/dbaccess.php";
$serverName = "localhost";           // guide für DB https://youtu.be/gCo6JqGMi30?t=1801
$DBusername = "root"; //muss auf jeden Fall geändert werden für Release -> PoLP
$DBpassword = "";    //Passwort vergeben für Release
$DBname = "hotelpanda";
$conn = mysqli_connect($serverName, $DBusername, $DBpassword, $DBname);        // das i nach sql wichtig
if (!$conn)     // wenn connection failed gibts error
{
    die("Connection failed: " . mysqli_connect_error());
}
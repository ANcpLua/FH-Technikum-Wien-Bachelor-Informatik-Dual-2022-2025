<?php
if (@$admin) {
    echo  "<h1>  Willkommen Admin!</h1></font>";
} else if (@$guest) {
    echo  '<main class="container d-flex  justify-content-center h-100 align-items-center text-dark text-start">
    <section class=" mt-4 col-lg-8 lead m-auto p-5 rounded bg-light  "style="--bs-bg-opacity: .9; ">Vielen Dank, dass Sie sich für unser Hotel entschieden haben. Um eine Zimmerreservierung vorzunehmen, besuchen Sie bitte die <a style="color:black;" href="index.php?page=reservation">Buchungsseite</a>   auf unserer Website.
    <br> Folgende Zimmerkategorien stehen für Sie zur Verfügung: 
    <br><br>
    <p class="text-center"><strong >Standard</strong></p> 
    
    <img class="img-thumbnail rounded img-fluid p-2 mt-1 mx-auto " src="./res/img/standard.jpg" > 
    Unser Standardzimmer ist stilvoll eingerichtet und sorgt für einen angenehmen Aufenthalt mit komfortablen Bett, einen großen Schreibtisch sowie ein eigenes Badezimmer mit Badewanne.
    
    <table class="table mt-4 table-dark table-striped fs-5 mb-5">
    <thead >
      <tr>
        <th scope="col">Preis pro Nacht und Person</th>
        <th scope="col">max. Personenzahl</th>
        <th scope="col">Aufpreis Frühstück</th>
        <th scope="col">Aufpreis Parkplatz</th>
        <th scope="col">Größe</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th scope="row">100€</th>
        <td>2</td>
        <td>16€ (Büffet mit Sekt) </td>
        <td>10€ </td>
        <td>  35m²</td>
      </tr>
    </tbody>
  </table>
   
    <p class="text-center"><strong >Deluxe</strong></p> 
    <img class="img-thumbnail rounded img-fluid p-2 mt-1 mx-auto " src="./res/img/deluxe.jpg" > 
    Unsere Deluxezimmer verbindet modernes Interieur mit Gemütlichkeit: Entspannen Sie nach einem langen Tag im Kingsize-Bett, gönnen Sei sich einen Drink oder einen Snack aus der Minibar oder genießen Sie den großartigen Blick  von unserem bequemen Sitzbereich aus. 

    <table class="table mt-4 table-dark table-striped fs-5 mb-5">
    <thead >
      <tr>
        <th scope="col">Preis pro Nacht und Person</th>
        <th scope="col">max. Personenzahl</th>
        <th scope="col">Aufpreis Frühstück</th>
        <th scope="col">Aufpreis Parkplatz</th>
        <th scope="col">Größe</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th scope="row">150€</th>
        <td>2</td>
        <td>16€ (Büffet mit Sekt) </td>
        <td>10€</td>
        <td>  45m²</td>
      </tr>
    </tbody>
  </table>

  
  
    <p class="text-center"><strong >Suite</strong></p> 
    <img class="  img-thumbnail rounded img-fluid p-2 mt-1 mx-auto " src="./res/img/suite.jpg" > 
    Unsere Suite lässt keine Wünsche offen und bietet luxuriöse Annehmlichkeiten, die ihren Aufenthalt unvergesslich machen: Machen Sie es sich gemütlich im separaten Wohnbereich mit bequemen Sofa und Minibar, genießen Sie die Nacht im Kingsize-Bett oder gönnen Sie sich eine Auszeit im Badezimmer mit Whirlpool. Für laue Sommerabende empfiehlt sich außerdem ein Drink auf der eigenen Dachterrasse, um den Blick über die Dächer schweifen zu lassen.  
   
    <table class="table mt-4 table-dark table-striped fs-5 mb-5">
    <thead >
      <tr>
        <th scope="col">Preis pro Nacht und Person</th>
        <th scope="col">max. Personenzahl</th>
        <th scope="col">Aufpreis Frühstück</th>
        <th scope="col">Aufpreis Parkplatz</th>
        <th scope="col">Größe</th>
      </tr>
    </thead>
    <tbody>
      <tr>
        <th scope="row">210€</th>
        <td>3</td>
        <td>16€ (Büffet mit Sekt) </td>
        <td>10€ </td>
        <td> 105m²</td>
      </tr>
    </tbody>
  </table>
    
    Sie können die Zimmer direkt bei der <a style="color:black;" href="index.php?page=reservation">Buchung</a>  auswählen. Sollten Sie Fragen haben, kontaktieren Sie uns bitte direkt unter office@hotelimperialpanda.at. Wir helfen Ihnen gerne bei der Planung Ihres Aufenthalts.
    Wir freuen uns auf Ihren Besuch im Hotel Imperial Panda.
    </div>
    ';
} else if (@$anon) {
    echo  '<main class="container d-flex  justify-content-center h-100 align-items-center text-dark text-start">
    <section class=" mt-4 col-lg-8 lead m-auto p-5 rounded bg-light  "style="--bs-bg-opacity: .9;"><strong>Willkommen im Hotel Imperial Panda!</strong> Wir freuen uns, Sie bei uns begrüßen zu dürfen. Unser Hotel ist die perfekte Wahl für Geschäftsreisende und ein großartiger Ausgangspunkt für Kulturbegeisterte. 
    <img class=" img-thumbnail img-fluid p-2 mt-2 mx-auto " src="./res/img/suite.jpg" > 

    Wir bieten unseren Gästen moderne und komfortable Zimmer, die alle mit eigenem Bad, kostenlosem WLAN und TV ausgestattet sind. Für unsere Business-Gäste bieten wir einen modernen Seminarraum, der mit der neuesten Technologie ausgestattet ist und Platz für bis zu 40 Personen bietet. 
    Unser Team steht Ihnen jederzeit zur Verfügung, um Ihren Aufenthalt so angenehm und erfolgreich wie möglich zu gestalten. Wir freuen uns darauf, Sie im Hotel Imperial Panda begrüßen zu dürfen und Ihnen einen unvergesslichen Aufenthalt zu ermöglichen.<br><br>
    
    <div>
    </div>';
}
?>

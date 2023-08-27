// Modul 1
window.onload = function() {
  let isSubmittedMulti1 = false;
  let isSubmittedSingle1 = false;

  document.getElementById("submit-multiple-choice-1").addEventListener("click", function() {
    if (!isSubmittedMulti1) {
      document.querySelector(".multiple-choice-answer-container").classList.add('show');
      var option1Multi = document.getElementById("option1Multi");
      var option2Multi = document.getElementById("option2Multi");
      var option3Multi = document.getElementById("option3Multi");
      var option4Multi = document.getElementById("option4Multi");

      var answerTextMulti = document.querySelector(".multiple-choice-answer-text");
      var explanationMulti = document.querySelector(".multiple-choice-explanation");

      if (option1Multi.checked && option3Multi.checked && !option2Multi.checked && option4Multi.checked) { // 1 und 3 richtig
        answerTextMulti.innerText = "Richtig!";
        explanationMulti.innerText = "Kaum ein Bereich des alltäglichen Lebens kommt ganz ohne Algorithmen aus.";
    } else {
        answerTextMulti.innerText = "Falsch!";
        explanationMulti.innerText = "Beim TikTok-Feed, der Steuerung von Ampeln und beim Abheben am Bankomat kommen Algorithmen zum Einsatz. Das Wetter wird zwar mithilfe von Algorithmen vorhergesagt, aber nicht gesteuert.";
    }
      isSubmittedMulti1 = true;
      this.disabled = true;
    }
  });

  document.getElementById("submit-single-choice-1").addEventListener("click", function() {
      if (!isSubmittedSingle1) {
        document.querySelector(".single-choice-answer-container").classList.add('show');
        var option1Single = document.getElementById("option1Single");
        var answerTextSingle = document.querySelector(".single-choice-answer-text");
        var explanationSingle = document.querySelector(".single-choice-explanation");

        if (option1Single.checked) {
          answerTextSingle.innerText = "Richtig!"; // wenn nicht alle 3 richtig sind obv. hier "Richtig!" schreiben.
          explanationSingle.innerText = "Hier sind alle Antworten richtig. KI ist der Versuch, bestimmte (menschliche) kognitive Fähigkeiten über Algorithmen nachzubilden. Maschinelles Lernen ist eine Methode, um KI zu entwickeln. Und Algorithmen sind die Schritt-für-Schritt-Anleitungen, die Ada Lovelace als Computerprogramm schon lange vor den heutigen Computern beschrieben hat.";
        } else {
            answerTextSingle.innerText = "Richtig!";
            explanationSingle.innerText = "Hier sind alle Antworten richtig. KI ist der Versuch, bestimmte (menschliche) kognitive Fähigkeiten über Algorithmen nachzubilden. Maschinelles Lernen ist eine Methode, um KI zu entwickeln. Und Algorithmen sind die Schritt-für-Schritt-Anleitungen, die Ada Lovelace  als Computerprogramm schon lange vor den heutigen Computern beschrieben hat.";
        }
        isSubmittedSingle1 = true;
        this.disabled = true;
      }
  });


// Modul 2 

  let isSubmittedMulti2 = false;
  let isSubmittedSingle2 = false;

  // ID für modul 2 angepasst
  document.getElementById("submit-multiple-choice-2").addEventListener("click", function() {
    if (!isSubmittedMulti2) {
      // ID 2
      document.querySelector(".multiple-choice-answer-container-2").classList.add('show');
      
      // ID 2
      var option1Multi = document.getElementById("option1Multi2");
      var option2Multi = document.getElementById("option2Multi2");
      var option3Multi = document.getElementById("option3Multi2");
      var option4Multi = document.getElementById("option4Multi2");

      // ID 2
      var answerTextMulti = document.querySelector(".multiple-choice-answer-text-2");
      var explanationMulti = document.querySelector(".multiple-choice-explanation-2");

      if (option2Multi.checked && option4Multi.checked && !option1Multi.checked && !option3Multi.checked) { // 2 und 4 richtig
          answerTextMulti.innerText = "Richtig!";
          explanationMulti.innerText = "Grace Hopper hat tatsächlich maßgeblich an der Entwicklung des ersten Compilers mitgewirkt. Dieser revolutionäre Fortschritt ermöglichte es, menschenverständlichen Code in Maschinensprache zu übersetzen und trug dazu bei, die Art und Weise, wie wir Software programmieren, grundlegend zu verändern. Darüber hinaus ist es auch wahr, dass der Begriff 'Debugging' durch Grace Hopper entstand, als sie und ihr Team einen echten Käfer aus einem ihrer Computer entfernten, was einen Fehler verursachte. Diese Ereignisse sind wichtige Beiträge von Grace Hopper zur Informatik.";
      } else {
          answerTextMulti.innerText = "Falsch!";
          explanationMulti.innerText = "Die ausgewählte(n) Aussage(n) sind nicht ganz korrekt. Obwohl Grace Hopper maßgeblich an der Prägung des Begriffs 'Debugging' beteiligt war, war es nicht der erste Computer, den sie so nannte. Der Begriff entstand aus einem Ereignis, bei dem ein echter Käfer (Moth) einen Fehler in einem Computer verursachte. Darüber hinaus ist die Aussage, dass vor Grace Hopper's Erfindung des Compilers Programmierer*innen in einer menschenverständlichen Sprache schreiben mussten, irreführend. Tatsächlich war es vor dem Compiler genau das Gegenteil - Code musste in der komplizierten Maschinensprache geschrieben werden. Mit dem Compiler konnte Code in einer höheren, menschenverständlicheren Sprache geschrieben werden, der dann in Maschinensprache übersetzt wurde.";
      }
      isSubmittedMulti2 = true;
      this.disabled = true;
    }
  });

  // ID 2
  document.getElementById("submit-single-choice-2").addEventListener("click", function() {
      if (!isSubmittedSingle2) {
        // ID 2
        document.querySelector(".single-choice-answer-container-2").classList.add('show');
        var option1Single = document.getElementById("option1Single2");
        // ID 2
        var answerTextSingle = document.querySelector(".single-choice-answer-text-2");
        var explanationSingle = document.querySelector(".single-choice-explanation-2");

        if (option1Single.checked) {  // nur Frage 1 ist richtig
            answerTextSingle.innerText = "Richtig!";
            explanationSingle.innerText = "Grace Hopper war maßgeblich an der Entwicklung der Programmiersprache COBOL (Abkürzung für: Common Business-Oriented Language) beteiligt. COBOL, eine Sprache zur Automatisierung von Geschäftsprozessen, ist auch nach Jahrzehnten noch in vielen Systemen weltweit im Einsatz. So nutzen Banken COBOL, um täglich Millionen von Finanztransaktionen zu verarbeiten, etwa beim Geldabheben am Bankautomaten.";
        } else {
            answerTextSingle.innerText = "Falsch!";
            explanationSingle.innerText = "Die Technologien hinter Digitalkameras und der Spracherkennung in Mobiltelefonen sind zwar beeindruckend, aber sie wurden nicht durch die Arbeit von Grace Hopper und ihre Entwicklung von COBOL (Abkürzung für Common Business-Oriented Language) ermöglicht. Im Gegensatz dazu basieren viele Bankautomaten auf COBOL, einer von Grace Hopper entwickelten Programmiersprache, um ihre Transaktionen zu verarbeiten. Daher ist die richtige Antwort: Bankautomaten.";
        }
        isSubmittedSingle2 = true;
        this.disabled = true;
      }
  });


// Modul 3

let isSubmittedMulti3 = false;
let isSubmittedSingle3 = false;

document.getElementById("submit-single-choice-3").addEventListener("click", function() {
  if (!isSubmittedSingle3) {
    document.querySelector(".single-choice-answer-container-3").classList.add('show');
    var option1Single = document.getElementById("option1Single3");
    var answerTextSingle = document.querySelector(".single-choice-answer-text-3");
    var explanationSingle = document.querySelector(".single-choice-explanation-3");

    if (option1Single.checked) {
      answerTextSingle.innerText = "Richtig!"; // nur Frage 1 ist richtig
  explanationSingle.innerText = " Genau! Viele gute Programmerinnen und Programmierer haben keine formale Ausbildung in Informatik. So zum Beispiel auch Grace Hopper, Karen Spärck Jones oder Radia Perlman. Sie haben sich das Programmieren selbst beigebracht. Und das kannst du auch!";
    } else {
        answerTextSingle.innerText = "Falsch!";
        explanationSingle.innerText = "Viele gute Programmerinnen und Programmierer haben keine formale Ausbildung in Informatik. So zum Beispiel auch Grace Hopper, Karen Spärck Jones oder Radia Perlman. Sie haben sich das Programmieren selbst beigebracht. Und das kannst du auch!";
    }
    isSubmittedSingle1 = true;
    this.disabled = true;
  }
});

document.getElementById("submit-multiple-choice-3").addEventListener("click", function() {
  if (!isSubmittedMulti3) {
    
    document.querySelector(".multiple-choice-answer-container-3").classList.add('show');
    
    
    var option1Multi = document.getElementById("option1Multi3");
    var option2Multi = document.getElementById("option2Multi3");
    var option3Multi = document.getElementById("option3Multi3");
    var option4Multi = document.getElementById("option4Multi3");

    
    var answerTextMulti = document.querySelector(".multiple-choice-answer-text-3");
    var explanationMulti = document.querySelector(".multiple-choice-explanation-3");

    if (option1Multi.checked && !option4Multi.checked && !option2Multi.checked && !option3Multi.checked) { // 1 richtig
        answerTextMulti.innerText = "Richtig!";
    explanationMulti.innerText = "Du musst weder super gut in Mathe sein, noch viel von Computern verstehen, um Coden zu lernen. Und natürlich können alle, egal welchen Geschlechts coden lernen. Coding ist wie eine Fremdsprache, die du lernen kannst und die dir hilft, mit Computern zu sprechen. ";
    } else {
        answerTextMulti.innerText = "Falsch!";
        explanationMulti.innerText = "Du musst weder super gut in Mathe sein, noch viel von Computern verstehen, um Coden zu lernen. Und natürlich können alle, egal welchen Geschlechts coden lernen. Coding ist wie eine Fremdsprache, die du lernen kannst und die dir hilft, mit Computern zu sprechen. ";
    }
    isSubmittedMulti3 = true;
    this.disabled = true;
  }
});




// Modul 4
let isSubmittedMulti4 = false;
let isSubmittedSingle4 = false;


document.getElementById("submit-multiple-choice-4").addEventListener("click", function() {
  if (!isSubmittedMulti4) {
    
    document.querySelector(".multiple-choice-answer-container-4").classList.add('show');
    
    
    var option1Multi = document.getElementById("option1Multi4");
    var option2Multi = document.getElementById("option2Multi4");
    var option3Multi = document.getElementById("option3Multi4");
    var option4Multi = document.getElementById("option4Multi4");

    
    var answerTextMulti = document.querySelector(".multiple-choice-answer-text-4");
    var explanationMulti = document.querySelector(".multiple-choice-explanation-4");

    if (option1Multi.checked && option2Multi.checked && !option3Multi.checked && !option4Multi.checked) { // 1 und 2 richtig
        answerTextMulti.innerText = "Richtig!";
        explanationMulti.innerText = "Das STP-Protokoll ist wie ein Verkehrsleiter für Informationen in einem Netzwerk. Es sorgt dafür, dass alle Daten dort ankommen, wo sie sollen, und nicht durcheinander geraten. Außerdem sorgt es dafür, dass es immer einen Plan B gibt, falls der übliche Weg blockiert ist. Es hilft also, Staus im Netzwerk zu vermeiden und dafür zu sorgen, dass keine Daten verloren gehen.";
    } else {
        answerTextMulti.innerText = "Falsch!";
        explanationMulti.innerText = "Radia Perlman hat das Spanning-Tree-Protokoll entwickelt. Das STP-Protokoll ist wie ein Verkehrsleiter für Informationen in einem Netzwerk. Es sorgt dafür, dass alle Daten dort ankommen, wo sie sollen, und nicht durcheinander geraten. Außerdem sorgt es dafür, dass es immer einen Plan B gibt, falls der übliche Weg blockiert ist. Es hilft also, Staus im Netzwerk zu vermeiden und dafür zu sorgen, dass keine Daten verloren gehen. ";
    }
    isSubmittedMulti4 = true;
    this.disabled = true;
  }
});


document.getElementById("submit-single-choice-4").addEventListener("click", function() {
    if (!isSubmittedSingle4) {
      
      document.querySelector(".single-choice-answer-container-4").classList.add('show');
      var option2Single = document.getElementById("option2Single4");
      
      var answerTextSingle = document.querySelector(".single-choice-answer-text-4");
      var explanationSingle = document.querySelector(".single-choice-explanation-4");

      if (option2Single.checked) {  // nur Frage 2 ist richtig
          answerTextSingle.innerText = "Richtig!";
          explanationSingle.innerText = "Netzwerkprotokolle sind wichtige Regeln, die dafür sorgen, dass Computer miteinander kommunizieren können. Sie machen es möglich, dass Computer Daten senden und empfangen. Ohne Netzwerkprotokolle könnten Computer nicht verstehen, was andere Computer ihnen senden. Daher helfen sie Computern, miteinander zu sprechen und Informationen auszutauschen.";
      } else {
          answerTextSingle.innerText = "Falsch!";
          explanationSingle.innerText = "Netzwerkprotokolle, wie das von Radia Perlman entwickelte Spanning-Tree-Protokoll, spielen eine entscheidende Rolle bei der Kommunikation zwischen Computern. Sie definieren Regeln und Verfahren, die Computer befolgen, um Daten zwischen Netzwerken auszutauschen. Daher ist die Aussage 2 richtig. Die Aussagen 1 und 3 sind nicht korrekt. Netzwerkprotokolle haben nichts mit Grafiken oder der aktuellsten Software von Computern zu tun.";
      }
      isSubmittedSingle4 = true;
      this.disabled = true;
    }
});

// Modul 4 ende  ------------------------------------------------------------------------------------------------------------------------------------------------

// Modul 5 
 
let isSubmittedMulti5 = false;
let isSubmittedSingle5 = false;


document.getElementById("submit-multiple-choice-5").addEventListener("click", function() {
  if (!isSubmittedMulti4) {
   
    document.querySelector(".multiple-choice-answer-container-5").classList.add('show');
    
    
    var option1Multi = document.getElementById("option1Multi5");
    var option2Multi = document.getElementById("option2Multi5");
    var option3Multi = document.getElementById("option3Multi5");
    var option4Multi = document.getElementById("option4Multi5");

    
    var answerTextMulti = document.querySelector(".multiple-choice-answer-text-5");
    var explanationMulti = document.querySelector(".multiple-choice-explanation-5");

    if (!option1Multi.checked && !option2Multi.checked && option3Multi.checked && !option4Multi.checked) {  // Frage 3 und 4 ist richtig
      answerTextMulti.innerText = "Richtig!";
      explanationMulti.innerText = "Genau! Katherine Johnson berechnete die Flugbahnen der Raumkapseln und spielte damit eine wichtige Rolle bei der Apollo-Mondlandung. Ihre präzisen Berechnungen waren entscheidend für den Erfolg der Mission.";
    } else {
      answerTextMulti.innerText = "Falsch!";
      explanationMulti.innerText = "Schade! Die richtige Antwort ist: Sie berechnete die Flugbahnen der Raumkapseln. Katherine Johnson spielte eine entscheidende Rolle bei der Apollo-Mondlandung, indem sie präzise Berechnungen durchführte, die für die sichere Landung auf dem Mond erforderlich waren.";
    }
    isSubmittedMulti5 = true;
    this.disabled = true;
  }
});


document.getElementById("submit-single-choice-5").addEventListener("click", function() {
    if (!isSubmittedSingle5) {
      
      document.querySelector(".single-choice-answer-container-5").classList.add('show');
      var option3Single = document.getElementById("option3Single5");
      
      var answerTextSingle = document.querySelector(".single-choice-answer-text-5");
      var explanationSingle = document.querySelector(".single-choice-explanation-5");

      if (option3Single.checked) {
        answerTextSingle.innerText = "Richtig!"; // nur Frage 3 ist richtig
        explanationSingle.innerText = "Ihr wichtigster Beitrag war wahrscheinlich ihre Arbeit an der Apollo-11-Mission, die zur ersten Mondlandung führte. Johnson wurde ausgewählt, um die Flugbahn des Raumschiffs zu berechnen. Sie stellte sicher, dass die Mondlandefähre von der Mondoberfläche abheben und wieder mit dem Befehlsmodul, das den Mond umkreiste, koppeln konnte. Es war ein unglaublich komplexer Prozess, der eine beispiellose Genauigkeit erforderte. Ein Fehler in ihrer Berechnung hätte die Astronauten in den Weiten des Weltraums verloren gehen lassen. Es ist auch bemerkenswert, dass, obwohl die NASA begonnen hatte, Computer für solche Berechnungen zu verwenden, Astronaut John Glenn persönlich darum bat, dass Johnson die Zahlen überprüft, bevor er bereit war, seine historische Mission zu starten. Er vertraute Johnsons Berechnungen mehr als denen des Computers.";
      } else {
        answerTextSingle.innerText = "Falsch!";
        explanationSingle.innerText = "Katherine Johnson hat als menschlicher 'Computer' bei der NASA gearbeitet. Ihr wichtigster Beitrag war wahrscheinlich ihre Arbeit an der Apollo-11-Mission, die zur ersten Mondlandung führte. Johnson wurde ausgewählt, um die Flugbahn des Raumschiffs zu berechnen. Sie stellte sicher, dass die Mondlandefähre von der Mondoberfläche abheben und wieder mit dem Befehlsmodul, das den Mond umkreiste, koppeln konnte. Es war ein unglaublich komplexer Prozess, der eine beispiellose Genauigkeit erforderte. Ein Fehler in ihrer Berechnung hätte die Astronauten in den Weiten des Weltraums verloren gehen lassen. Es ist auch bemerkenswert, dass, obwohl die NASA begonnen hatte, Computer für solche Berechnungen zu verwenden, Astronaut John Glenn persönlich darum bat, dass Johnson die Zahlen überprüft, bevor er bereit war, seine historische Mission zu starten. Er vertraute Johnsons Berechnungen mehr als denen des Computers.";

      }
      isSubmittedSingle5 = true;
      this.disabled = true;
    }
});


}

// Modul 5 ende -----------------------------------------------------------------------------------------------------------------------------------------------------


//code beispiel für modul 1
function calculateSum() {
  let input = document.getElementById("numberInput");
  let number = parseInt(input.value);
  
  // Limit the entered number to a maximum of 100
  if (number > 100) {
    number = 100;
    input.value = number;
  }
  
  let sum = 0;

  for (let i = 1; i <= number; i++) {
    sum += i;
  }

  let resultElement = document.getElementById("result");
  resultElement.textContent = "Die Summe von 1 bis " + number + " ist " + sum + ".";
}


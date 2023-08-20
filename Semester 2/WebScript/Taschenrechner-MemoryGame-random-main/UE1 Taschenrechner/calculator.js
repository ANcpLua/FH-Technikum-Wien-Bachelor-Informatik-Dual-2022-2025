function berechne(operator) 
{
  var in1 = parseFloat(document.getElementById("in1").value.replace(',', '.'));  	// parsefloat string input into float, ref element in1 into var and , into .
  var in2 = parseFloat(document.getElementById("in2").value.replace(',', '.'));		// in 2 same 

  if (isNaN(in1) || isNaN(in2)) // nur zahlen, wenn NaN(error) dann message
  {
   
    alert("bitte in beide felder(Erste Zahl und Zweite Zahl) eine zahl schreiben, da steht ZAHL!!! die schauen so aus 0123456789");
    return; // Exit the function early
  }

    var ergebnis = eval(in1 + operator + in2).toFixed(10); // runded wegen float 10 stellen : )
  document.getElementById("output").value = ergebnis;
  document.getElementById("history").innerHTML += "<li>" + in1 + " " + operator + " " + in2 + " = " + ergebnis + "</li>"; // blackbox wie stringcopy aus C
}




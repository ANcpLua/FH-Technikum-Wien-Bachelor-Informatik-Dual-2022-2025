  document.addEventListener("DOMContentLoaded", () => 
  {
    //card options
    const cardArray = [
      { name: "card1", img: "pics/card1.png" },
      { name: "card1", img: "pics/card1.png" },
      { name: "card2", img: "pics/card2.png" },
      { name: "card2", img: "pics/card2.png" },
      { name: "card3", img: "pics/card3.png" },
      { name: "card3", img: "pics/card3.png" },
      { name: "card4", img: "pics/card4.png" },
      { name: "card4", img: "pics/card4.png" },
      { name: "card5", img: "pics/card5.png" },
      { name: "card5", img: "pics/card5.png" },
      { name: "card6", img: "pics/card6.png" },
      { name: "card6", img: "pics/card6.png" },
      { name: "card7", img: "pics/card7.png" },
      { name: "card7", img: "pics/card7.png" },
      { name: "card8", img: "pics/card8.png" },
      { name: "card8", img: "pics/card8.png" },
    ];
 // Get user name and display it
 const userName = prompt("Please enter your name:");

// Validate input to prevent script injection
const nameRegex = /^[a-zA-Z\s]*$/;
if (nameRegex.test(userName)) 
{
  document.getElementById("userName").textContent = userName;
} 
else 
{
  alert("Invalid input");
}
 // Timer logic
 let timer = setInterval(() =>
   {
     document.getElementById("timer").innerHTML = Math.round((Date.now() - start) / 1000);
   }, 1000);
 let start = Date.now();
 // Shuffle the card array and create the game board
 cardArray.sort(() => 0.5 - Math.random());
 const grid = document.querySelector("#spielbereich");
 const resultDisplay = document.querySelector("#try");
 let cardsChosen = [];
 let cardsChosenId = [];
 let cardsWon = [];
 // Create card elements and add event listeners
 cardArray.forEach((card, index) =>
 {
   const cardElement = document.createElement("img");
   cardElement.src = "pics/memoryBg.png";
   cardElement.dataset.id = index;
   cardElement.addEventListener("click", flipCard);
   grid.appendChild(cardElement);
 });
 // Initialize the try counter with a value of 0
 resultDisplay.innerHTML = 0;
 // Add 1 to the try counter
 function addTry()
 {
   resultDisplay.innerHTML = parseInt(resultDisplay.innerHTML) + 1;
 }
 // Check if the two chosen cards match
 function checkForMatch()
 {
   addTry();
   const [optionOneId,optionTwoId] = cardsChosenId;
   const [cardOne,cardTwo] = cardsChosen;
   const cards = document.querySelectorAll("img");
   const isMatch = cardOne === cardTwo;
   cards[optionOneId].setAttribute("src", `pics/memoryBg${isMatch ? "l" : ""}.png`);
   cards[optionTwoId].setAttribute("src", `pics/memoryBg${isMatch ? "l" : ""}.png`);
   if (isMatch)
   {
     cards[optionOneId].removeEventListener("click", flipCard);
     cards[optionTwoId].removeEventListener("click", flipCard);
     cardsWon.push(cardOne);
     if (cardsWon.length === cardArray.length / 2)
     {
       clearInterval(timer);
       resultDisplay.textContent = "Congratulations! You found them all!";
     }
   }
   cardsChosen = [];
   cardsChosenId = [];
 }
 // Flip the clicked card over and check for match
 function flipCard()
 {
   const cardId = this.dataset.id;
   if (cardsWon.includes(cardArray[cardId].name) || cardsChosenId.includes(cardId))
   {
     return;
   }
   cardsChosen.push(cardArray[cardId].name);
   cardsChosenId.push(cardId);
   this.src = cardArray[cardId].img;
   if (cardsChosen.length === 2)
   {
     setTimeout(checkForMatch, 100);
   }
 }
});
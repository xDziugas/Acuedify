const termElement = document.getElementById("term");
const definitionElement = document.getElementById("definition");

let isFlipped = false;

termElement.addEventListener("click", () => {
    if (isFlipped) {
        termElement.style.transform = "rotateY(0deg)";
        definitionElement.style.transform = "rotateY(180deg)";
    } else {
        termElement.style.transform = "rotateY(180deg)";
        definitionElement.style.transform = "rotateY(0deg)";
    }

    isFlipped = !isFlipped;
});

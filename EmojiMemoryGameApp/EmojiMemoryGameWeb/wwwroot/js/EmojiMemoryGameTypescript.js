//vars
const emojis = ["😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉", "😊", "😂", "😍", "😒", "😘", "😁", "😢", "😜", "😎", "😉"];
const buttonsBase = [...document.querySelectorAll(".emojiBtn")];
let buttons = [];
let removedButtons = []; //take out?
const playerone = "Player One";
const playertwo = "Player Two";
let currentTurn = playerone;
let cardone;
let cardtwo;
let numtimesclicked = 0;
let nonvisible = 0;
let gameover = false;
let winner = "";
let span;
let scoreone = parseInt(document.querySelector("#scoreone").innerHTML);
let scoretwo = parseInt(document.querySelector("#scoretwo").innerHTML);
//events
buttonsBase.forEach(b => b.addEventListener("click", chooseCard));
document.querySelector("#startBtn").addEventListener("click", startGame);
//code
function startGame() {
    if (document.querySelector("#startBtn").textContent == "Start Game") {
        resetGameBoard();
        initializeGameCards();
        document.querySelector("#startBtn").textContent = "End Game";
        document.querySelector("#gamestat").innerHTML = currentTurn;
    }
    else {
        setEndGame();
    }
    cardone = null;
    cardtwo = null;
}
function resetGameBoard() {
    buttons = [...buttonsBase];
    buttons.forEach((b) => {
        const button = b;
        button.style.display = 'inline-block';
        button.classList.add("emojiBtn");
        // button.style = 'emojiBtn';
        button.removeEventListener("click", chooseCard);
        button.addEventListener("click", chooseCard);
        button.disabled = false;
    });
    span = [...document.querySelectorAll(".emoji-text")];
    span.forEach((s) => {
        const span = s;
        span.style.visibility = "hidden";
    });
}
function setEndGame() {
    document.querySelector("#startBtn").textContent = "Start Game";
    resetGameBoard();
    buttonsBase.forEach((b) => {
        const btn = b;
        btn.disabled = true;
    });
    buttons.forEach((b) => {
        const btn = b;
        btn.disabled = true;
    });
}
function initializeGameCards() {
    shuffleArray(emojis);
    buttons.forEach((button, index) => {
        if (emojis[index]) {
            span = document.createElement('span');
            span.classList.add('emoji-text');
            span.textContent = emojis[index];
            button.innerHTML = "";
            button.appendChild(span);
            span.style.visibility = 'hidden';
        }
    });
}
function shuffleArray(arr) {
    for (let i = arr.length - 1; i > 0; i--) {
        const j = Math.floor(Math.random() * (i + 1));
        [arr[i], arr[j]] = [arr[j], arr[i]];
    }
}
function chooseCard(event) {
    numtimesclicked++;
    const emoji = event.target;
    span = emoji.querySelector('.emoji-text');
    span.style.visibility = 'visible';
    // span.style.visibility = 'visible';
    switch (numtimesclicked) {
        case 1:
            cardone = emoji;
            emoji.disabled = true;
            break;
        case 2:
            cardtwo = emoji;
            emoji.disabled = true;
            break;
    }
    if (cardtwo != null) {
        checkForMatch(cardone, cardtwo);
        setTimeout(function () {
            switchTurn();
        }, 1000);
        checkForWinner();
    }
}
function checkForMatch(emojicard1, emojicard2) {
    const emj1 = emojicard1;
    const emj2 = emojicard2;
    if (emojicard1.textContent == emojicard2.textContent) {
        nonvisible = nonvisible + 2;
        if (currentTurn == playerone) {
            document.querySelector("#scoreone").innerHTML = (parseInt(document.querySelector("#scoreone").innerHTML) + 1).toString();
            //  scoreone  = (+scoreone + 1).toString();
        }
        else if (currentTurn == playertwo) {
            document.querySelector("#scoretwo").innerHTML = (parseInt(document.querySelector("#scoretwo").innerHTML) + 1).toString();
            //  scoretwo = (+scoretwo + 1).toString();
        }
        setTimeout(function () {
            emj1.disabled = false;
            emj2.disabled = false;
        }, 500);
        setTimeout(function () {
            emj1.style.display = 'none';
            emj2.style.display = 'none';
        }, 1000);
        removedButtons.push(emojicard1);
        removedButtons.push(emojicard2);
        requestAnimationFrame(function () {
            displayMessage(" You got a match!");
        });
    }
    else {
        emj1.disabled = false;
        emj2.disabled = false;
        setTimeout(function () {
            span = [...document.querySelectorAll(".emoji-text")];
            span.forEach((s) => {
                const span = s;
                span.style.visibility = "hidden";
            });
        }, 1000);
        requestAnimationFrame(function () {
            displayMessage(" No Match!");
        });
    }
    cardone = null;
    cardtwo = null;
    numtimesclicked = 0;
}
function displayMessage(message) {
    let messagebox = document.querySelector("#gamestat");
    messagebox.innerHTML += message;
    setTimeout(function () {
        let messagedisplay = messagebox; // added let and datatype element??
    }, 1000);
}
function checkForWinner() {
    if (nonvisible == 20) {
        gameover = true;
    }
    else if (scoreone == 8 || scoretwo == 8) {
        gameover = true;
    }
    if (gameover) {
        if (scoreone > scoretwo) {
            winner = playerone;
        }
        else if (scoretwo > scoreone) {
            winner = playertwo;
        }
        else if (scoreone == scoretwo) {
            winner = "Tie";
        }
        document.querySelector("#gamestat").innerHTML = "";
        displayMessage("Winner is " + winner + "!!");
        setEndGame();
    }
}
function switchTurn() {
    currentTurn = currentTurn == playerone ? playertwo : playerone;
    document.querySelector("#gamestat").innerHTML = "";
    document.querySelector("#gamestat").innerHTML = currentTurn;
}
//# sourceMappingURL=EmojiMemoryGameTypescript.js.map
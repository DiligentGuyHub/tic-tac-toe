"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();
var current = 'X';

// Establish connection
connection.start().then(function () {
/*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    return console.error(err.toString());
});

if (document.getElementById("publish") != null)
{
    document.getElementById("publish").addEventListener("click", function (event)
    {
        var cryptoStor = new Uint16Array(1);
        window.crypto.getRandomValues(cryptoStor);
        document.getElementById("SessionID").innerHTML = `Gaming session ID: <b>${cryptoStor[0]}</b>`;
        let user = document.getElementById('ActiveUser').innerHTML;
        connection.invoke("CreateSession", user).catch(function (err)
        {
            return console.error(err.toString());
        });
        event.preventDefault();
        document.getElementById("publish").disabled = true;
    });
}

//Disable send button until connection is established
/*document.getElementById("sendButton").disabled = true;*/

if (document.getElementById("sendTurn") != null)
{
    document.getElementById("sendTurn").addEventListener("click", function (event)
    {
        var user = null;
        var turn = event.target.id;
        connection.invoke("SendMessage", user, turn).catch(function (err)
        {
            return console.error(err.toString());
        });
        event.preventDefault();
    });
}


connection.on("CreateSession", function (user) {
    let html =
        `<p class='card-body'>Play with <b>${user}</b>
            <a class='btn btn-outline-primary' href='/Home/Game'>Join</a>
        </p>`
    document.getElementById("activeGames").innerHTML += html;
});

connection.on("ReceiveMessage", function (user, turn) {
    if (current == 'X') current = 'O'
    else current = 'X';
    document.getElementById(turn).innerHTML = `<p class="text-center display-4">${current}</p>`;
});


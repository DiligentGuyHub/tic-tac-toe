"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/GameHub").build();

connection.start().then(function () {
    /*document.getElementById("sendButton").disabled = false;*/
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("publish").addEventListener("click", function (event) {
    alert(1);
    let user = document.getElementById('ActiveUser').innerHTML
    connection.invoke("CreateSession", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

connection.on("CreateSession", function (user) {
    let html =
        `<p class='card-body'>Play with <b>${user}</b>
            <button class='btn btn-outline-primary' onclick=''>Play</button>
        </p>`
    document.getElementById("activeGames").innerHTML += html;
});
"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/twsHub").build();

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var div = document.createElement("div");
    div.textContent = msg;
    var parentNode = document.getElementById(user);
    var firstChild = parentNode.querySelector(":first-child");
    parentNode.insertBefore(div, firstChild.nextSibling);
});

connection.on("RtvTicked", function (rtv) {
    var fields =
        "<td>" + rtv.price + "</td>" +
        "<td>" + (rtv.price - rtv.prevPrice) + "</td>" +
        "<td>" + rtv.size + "</td>" +
        "<td>" + (rtv.unixTime - rtv.prevUnixTime) + "</td>" +
        "<td>" + rtv.vwap.toFixed(2) + "</td>" +
        "<td>" + rtv.isSingleMarketMaker + "</td>";

    $('#tickString > tbody')
        .prepend('<tr>' + fields + '</tr>');

});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});
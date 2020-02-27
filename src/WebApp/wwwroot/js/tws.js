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
    if (rtv.price === rtv.prevPrice) {
        var $tr = $("#tickString > tbody > tr:first-child");
        $tr.find(":nth-child(3)").text(rtv.size);
        $tr.find(":nth-child(3)").attr("class", "size" + rtv.sizeCode);
        $tr.find(":nth-child(4)").text(rtv.isSingleMarketMaker ? "true" : "FALSE");
        $tr.find(":nth-child(5)").text(rtv.unixTime - rtv.prevUnixTime);
        $tr.find(":nth-child(6)").text(rtv.vwap.toFixed(2));
    }
    else {
        var fields =
            "<td>" + rtv.price + "</td>" +
            "<td class=\"dir" + rtv.direction + "\">" + (rtv.price - rtv.prevPrice) + "</td>" +
            "<td class=\"size" + rtv.sizeCode + "\">" + rtv.size + "</td>" +
            "<td>" + (rtv.isSingleMarketMaker ? "true" : "FALSE") + "</td>" +
            "<td>" + (rtv.unixTime - rtv.prevUnixTime) + "</td>" +
            "<td>" + rtv.vwap.toFixed(2) + "</td>";

        $('#tickString > tbody')
            .prepend('<tr>' + fields + '</tr>');
    }
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});
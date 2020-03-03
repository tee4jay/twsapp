"use strict";

var $prevTr = $();
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
    var $tr = null;
    var price = 0.00;

    $prevTr.removeClass();

    $("#tickString > tbody > tr > td.price").each(function (index) {
        price = parseFloat($(this).text());
        if (rtv.price >= price) {
            $tr = $(this).parent();
            return false;
        }
    });

    if (rtv.price === price) {
        $tr.find(":nth-child(2)").text(rtv.tickSize);
        $tr.find(":nth-child(3)").text(rtv.sizePercent.toFixed(5));
        $tr.find(":nth-child(3)").attr("class", "size" + rtv.sizeCode);
        $prevTr = $tr;
    }
    else {
        var newTr =
            "<tr>" +
            "<td class=\"price\">" + rtv.price + "</td>" +
            "<td>" + rtv.tickSize + "</td>" +
            "<td class=\"size" + rtv.sizeCode + "\">" + rtv.sizePercent.toFixed(5) + "</td>" +
            "</tr>";

        if (rtv.price < price || price === 0) {
            $prevTr = $('#tickString > tbody')
                .append(newTr);
        }
        else {
            $tr.before(newTr);
            $prevTr = $tr;
        }
    }

    $prevTr.addClass("dir" + rtv.direction);
});

connection.start().then(function () {
    
}).catch(function (err) {
    return console.error(err.toString());
});
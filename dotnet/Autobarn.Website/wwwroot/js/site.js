// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function displayNotification(user, message) {
    var data = JSON.parse(message);
    console.log(user);
    console.log(data);
    const $div = $(`
<div>${data.ManufacturerName} ${data.ModelName} (${data.Year}, ${data.Color})<br />
PRICE; ${data.Price} ${data.CurrencyCode}<br />
<a href="/vehicles/details/${data.Registration}">click for more info...</a>
</div>`);
    var $container = $("#signalr-notifications");
    $div.css("background-color", data.Color);
    $container.prepend($div);
    window.setTimeout(function () {
        $div.fadeOut(2000,
            function () {
                $div.remove();
            });
    }, 5000);
}

$(document).ready(function () {
    const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
    conn.on("NotifyWebsiteUsersAboutNewVehicle", displayNotification);
    conn.start().then(function () {
        console.log("Connected to SignalR!");
    }).catch(function (err) {
        console.log("Failed!");
        console.log(err);
    });
});
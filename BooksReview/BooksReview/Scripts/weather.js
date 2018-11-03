//weather.js

var weatherCallback = function (data) {
    var wind = data.query.results.channel.wind;
    var item = data.query.results.channel.item;
    var text = item.condition.temp + " °C";
    $("#temperatureDiv p").html(text);
};

function updateTemperatureDiv(text) {
    $("#temperatureDiv").append("<p>" + text + "</p>");
}
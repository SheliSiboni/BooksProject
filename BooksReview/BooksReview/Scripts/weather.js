var apiCalls = 'http://api.openweathermap.org/data/2.5/weather?q=Tel' + " " + 'Aviv&appid=bdb3f76b0cb6e16136aec2410502dd16'
$.getJSON(apiCalls, weatherCallBack)

function weatherCallBack(WeatherData) {
    var temp = WeatherData.main.temp;
    temp -= 273.15;
    var city = WeatherData.name;

    document.getElementById('temperatureDiv').append("The Temperature in " + city + " is " + temp + "C degrees.");
};

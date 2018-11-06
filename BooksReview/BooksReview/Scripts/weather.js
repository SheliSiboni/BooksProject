var apiCalls = 'http://api.openweathermap.org/data/2.5/weather?id=293397&units=metric' + '&appid=bdb3f76b0cb6e16136aec2410502dd16'
$.getJSON(apiCalls, weatherCallBack)

function weatherCallBack(WeatherData) {
    var temp = WeatherData.main.temp;
    var city = WeatherData.name;

    document.getElementById('tempP').append("  " + temp + " °C");
};

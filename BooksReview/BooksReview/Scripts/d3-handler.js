// graph.js

function createPopularUsersGraph(data, id) {

    var width = 400,
        height = 400,
        radius = Math.min(width, height) / 2;

    var color = d3.scale.ordinal()
        .range(["#247BA0", "#70C1B3", "#B2DBBF", "#F3FFBD", "#FF1654", "#740A27", "#000000"]);

    var arc = d3.svg.arc()
        .outerRadius(radius - 10)
        .innerRadius(0);

    var pie = d3.layout.pie()
        .sort(null)
        .value(function (d) { return d.Count; });

    var svg = d3.select("#" + id).append("svg")
        .attr("width", width)
        .attr("height", height)
        .append("g")
        .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");

    data.forEach(function (d) {
        d.Count = +d.Count;
    });

    var g = svg.selectAll(".arc")
        .data(pie(data))
        .enter().append("g")
        .attr("class", "arc");

    g.append("path")
        .attr("d", arc)
        .style("fill", function (d) { return color(d.data.Name); });

    g.append("text")
        .attr("transform", function (d) { return "translate(" + arc.centroid(d) + ")"; })
        .attr("dy", ".35em")
        .style("text-anchor", "middle")
        .text(function (d) { return d.data.Name; });
}

function createPopularReviewsGraph(data) {

    var margin = { top: 20, right: 20, bottom: 70, left: 40 },
        width = 600,
        height = 300;

    // set the ranges
    var x = d3.scale.ordinal().rangeRoundBands([0, width], .05);
    var y = d3.scale.linear().range([height, 0]);

    var xAxis = d3.svg.axis()
        .scale(x)
        .orient("bottom");

    var yAxis = d3.svg.axis()
        .scale(y)
        .orient("left")
        .ticks(10);

    // add the SVG element
    var svg = d3.select("#popular-reviews-graph").append("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
        .append("g")
        .attr("transform",
        "translate(" + margin.left + "," + margin.top + ")");

    // load the data
    data.forEach(function (d) {
        d.Title = d.Title;
        d.NumberOfReviews = +d.NumberOfReviews;
    });

    // scale the range of the data
    x.domain(data.map(function (d) { return d.Title; }));
    y.domain([0, d3.max(data, function (d) { return d.NumberOfReviews; })]);

    // add axis
    svg.append("g")
        .attr("class", "x axis")
        .attr("transform", "translate(0," + height + ")")
        .call(xAxis)
        .selectAll("text")
        .style("text-anchor", "end")
        .attr("dx", "-.8em")
        .attr("dy", "-.55em")
        .attr("transform", "rotate(-90)");

    svg.append("g")
        .attr("class", "y axis")
        .call(yAxis)
        .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 0)
        .attr("dy", "1em")
        .style("text-anchor", "end")
        .text("NumberOfReviews");

    // Add bar chart
    svg.selectAll("bar")
        .data(data)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.Title)+10; })
        .attr("width", x.rangeBand()/2)
        .attr("y", function (d) { return y(d.NumberOfReviews); })
        .attr("height", function (d) { return height - y(d.NumberOfReviews); });
}
var drawCircle = function (x, y) {
    context.beginPath();
    context.arc(x, y, parliamentarianRadius, 0, 2 * Math.PI, false);
    context.fillStyle = parliamentarianFillStyle;
    context.fill();
}

var canvas = document.getElementById("ParliamentCanvas");
var context = canvas.getContext("2d");

var centerX = canvas.width / 2;
var centerY = canvas.height * 0.9;

var parliamentarianRadius = 8;
var parliamentarianFillStyle = 'green';

var totalParliamentarians = 750;
var parliamentariansInFirstRow = 34;
var arcRadius = 180;

var parliamentariansInCurrentRow = parliamentariansInFirstRow;
var rowsToDraw = 14;

for (var i = 0; i < rowsToDraw; i++) {
    for (var j = 0; j < parliamentariansInCurrentRow; j++) {
        var lengthAroundArc = Math.PI + Math.PI * (j / (parliamentariansInCurrentRow - 1));
        x = centerX + arcRadius * Math.cos(lengthAroundArc);
        y = centerY + arcRadius * Math.sin(lengthAroundArc);
        drawCircle(x, y);
    }
    parliamentariansInCurrentRow += 3;
    arcRadius += 20;
    if (i == rowsToDraw - 2) {
        parliamentariansInCurrentRow++;
    }
}
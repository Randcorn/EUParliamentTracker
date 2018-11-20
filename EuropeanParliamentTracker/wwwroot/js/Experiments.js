var partyFillStyle = ['#39f', '#f0001c', '#ffd700', '#ffd700', '#9c3', '#990000', '#24b9b9', '#2b3856', '#c0c0c0'];
var partyCount = [219, 189, 68, 73, 52, 51, 43, 35, 21];

//Due to the Parliament president not voting
partyCount[0]--;

var canvas = document.getElementById("ParliamentCanvas");
var context = canvas.getContext("2d");

var centerX = canvas.width / 2;
var centerY = canvas.height * 0.9;

var totalParliamentarians = 751;
var numberOfRows = 14;
var parliamentariansInFirstRow = 34;
var parliamentarianRadius = 8;
var arcRadius = 180;

var rows = [];

var drawCircle = function (x, y, partyNumber) {
    context.beginPath();
    context.arc(x, y, parliamentarianRadius, 0, 2 * Math.PI, false);
    context.fillStyle = partyFillStyle[partyNumber];
    context.fill();
}

var drawCircleOnSeat = function (row, seat, partyNumber) {
    var seatsOnRow = parliamentariansInFirstRow + (row * 3);
    if (row == numberOfRows - 1) {
        seatsOnRow++;
    }
    var rowRadius = arcRadius + (row * 20);
        
    var lengthAroundArc = Math.PI + Math.PI * (seat / (seatsOnRow - 1));
    x = centerX + rowRadius * Math.cos(lengthAroundArc);
    y = centerY + rowRadius * Math.sin(lengthAroundArc);
    drawCircle(x, y, partyNumber);
}

var getLeastFilledRow = function () {
    var leastFilledRow = 0;
    var leastFilled = 1;
    for (var i = 0; i < numberOfRows; i++) {
        if (rows[i].amountFilled() < leastFilled) {
            leastFilledRow = i;
            leastFilled = rows[i].amountFilled();
        }
    }
    return leastFilledRow;
}

var Row = function () {
    this.color = "green";
    this.parliamentariansPlaced = 0;
    this.row = 0;
    this.seatOnRow = 0;
    this.amountFilled = function () {
        return this.parliamentariansPlaced / this.seatOnRow;
    }
};

var placeParliamentarianOnAllRows = function (partyNumber) {
    for (var i = 0; i < numberOfRows; i++) {
        drawCircleOnSeat(i, rows[i].parliamentariansPlaced, partyNumber);
        rows[i].parliamentariansPlaced++;
    }
}

var placeParliamentarianOnLeastFilledRow = function (partyNumber) {
    var leastFilledRow = getLeastFilledRow();
    drawCircleOnSeat(leastFilledRow, rows[leastFilledRow].parliamentariansPlaced, partyNumber);
    rows[leastFilledRow].parliamentariansPlaced++;
}

var parliamentariansInCurrentRow = parliamentariansInFirstRow;

for (var i = 0; i < numberOfRows; i++) {
    var row = new Row();
    row.row = i;
    row.color = partyFillStyle[i];
    row.seatOnRow = parliamentariansInCurrentRow;
    rows.push(row);

    parliamentariansInCurrentRow += 3;
    if (i == numberOfRows - 2) {
        parliamentariansInCurrentRow++;
    }
}

for (var i = 0; i < partyCount.length; i++) {
    var parliamentariansToPlace = partyCount[i];
    while (parliamentariansToPlace > 0) {
        placeParliamentarianOnLeastFilledRow(i);
        parliamentariansToPlace--;
        /*if (parliamentariansToPlace >= numberOfRows) {
            placeParliamentarianOnAllRows(i);
            parliamentariansToPlace -= numberOfRows;
        }
        else {
            placeParliamentarianOnLeastFilledRow(i);
            parliamentariansToPlace--;
        }*/
    }
}

/*
parliamentariansInCurrentRow = parliamentariansInFirstRow;
for (var i = 0; i < numberOfRows; i++) {
    for (var j = 0; j < parliamentariansInCurrentRow; j++) {
        var lengthAroundArc = Math.PI + Math.PI * (j / (parliamentariansInCurrentRow - 1));
        x = centerX + arcRadius * Math.cos(lengthAroundArc);
        y = centerY + arcRadius * Math.sin(lengthAroundArc);
        drawCircle(x, y);
    }
    parliamentariansInCurrentRow += 3;
    arcRadius += 20;
    if (i == numberOfRows - 2) {
        parliamentariansInCurrentRow++;
    }
}
*/

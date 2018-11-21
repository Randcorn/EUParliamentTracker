class Row {
    constructor(row, seatsOnRow) {
        this.row = row;
        this.seatsOnRow = seatsOnRow;
        this.parliamentariansPlaced = 0;
    }

    addParliamentarian() {
        this.parliamentariansPlaced++;
    }

    getAmountFilled() {
        return this.parliamentariansPlaced / this.seatsOnRow;
    }
};

class ParliamentSection {
    constructor(size, color) {
        this.size = size;
        this.color = color;
    }
}

class Parliament {
    constructor(canvas) {
        this.canvas = canvas;
        this.context = canvas.getContext("2d");
        this.numberOfRows = 14;
        this.parliamentariansInFirstRow = 34;
        this.parliamentarianRadius = 8;
        this.arcRadius = 180;
        this.rows = [];
        this.sections = [];
        this.createAllRows();
    }

    createAllRows() {
        var parliamentariansInCurrentRow = this.parliamentariansInFirstRow;
        for (var i = 0; i < this.numberOfRows; i++) {
            var row = new Row(i, parliamentariansInCurrentRow);
            this.addRow(row);

            parliamentariansInCurrentRow += 3;
            if (i == this.numberOfRows - 2) {
                parliamentariansInCurrentRow++;
            }
        }
    }

    getCenterX() {
        return this.canvas.width / 2;
    }

    getCenterY() {
        return this.canvas.height * 0.9;
    }

    addRow(row) {
        this.rows.push(row);
    }

    addSection(section) {
        this.sections.push(section);
    }

    drawParliamentarian(x, y, color) {
        this.context.beginPath();
        this.context.arc(x, y, this.parliamentarianRadius, 0, 2 * Math.PI, false);
        this.context.fillStyle = color;
        this.context.fill();
    }

    drawParliamentarianOnSeat(row, seat, color) {
        var seatsOnRow = this.parliamentariansInFirstRow + (row * 3);
        if (row == this.numberOfRows - 1) {
            seatsOnRow++;
        }
        var rowRadius = this.arcRadius + (row * 20);

        var lengthAroundArc = Math.PI + Math.PI * (seat / (seatsOnRow - 1));
        var x = this.getCenterX() + rowRadius * Math.cos(lengthAroundArc);
        var y = this.getCenterY() + rowRadius * Math.sin(lengthAroundArc);
        this.drawParliamentarian(x, y, color);
    }

    getLeastFilledRow() {
        var leastFilledRow = 0;
        var leastFilled = 1;
        for (var i = 0; i < this.numberOfRows; i++) {
            if (this.rows[i].getAmountFilled() < leastFilled) {
                leastFilledRow = i;
                leastFilled = this.rows[i].getAmountFilled();
            }
        }
        return leastFilledRow;
    }

    placeParliamentarianOnAllRows(partyNumber) {
        for (var i = 0; i < this.numberOfRows; i++) {
            this.drawCircleOnSeat(i, this.rows[i].parliamentariansPlaced, partyNumber);
            this.rows[i].addParliamentarian();
        }
    }

    placeParliamentarianOnLeastFilledRow(partyNumber) {
        var leastFilledRow = this.getLeastFilledRow();
        this.drawParliamentarianOnSeat(leastFilledRow, this.rows[leastFilledRow].parliamentariansPlaced, this.sections[partyNumber].color);
        this.rows[leastFilledRow].addParliamentarian();
    }

    drawParliament() {
        for (var i = 0; i < this.sections.length; i++) {
            var parliamentariansToPlace = this.sections[i].size;
            while (parliamentariansToPlace > 0) {
                this.placeParliamentarianOnLeastFilledRow(i);
                parliamentariansToPlace--;
            }
        }
    }
}
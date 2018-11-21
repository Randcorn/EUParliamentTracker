var partyFillStyle = ['#3399ff', '#f0001c', '#ffd700', '#0054a5', '#99cc33', '#990000', '#24b9b9', '#2b3856', '#c0c0c0'];
var partyCount = [219, 189, 68, 73, 52, 51, 43, 35, 21];

//Due to the Parliament president not voting
partyCount[0]--;

var canvas = document.getElementById("ParliamentCanvas");
var parliament = new Parliament(canvas);

for (var i = 0; i < partyCount.length; i++) {
    var section = new ParliamentSection(partyCount[i], partyFillStyle[i]);
    parliament.addSection(section);
}

parliament.drawParliament();
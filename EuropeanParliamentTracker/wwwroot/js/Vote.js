var canvas = document.getElementById("ParliamentCanvas");
var parliament = new Parliament(canvas);

var sectionApprove = new ParliamentSection(document.getElementById("NumberOfApproveVotes").value, 'green');
parliament.addSection(sectionApprove);

var sectionAbstain = new ParliamentSection(document.getElementById("NumberOfAbstainVotes").value, '#cccccc');
parliament.addSection(sectionAbstain);

var sectionAbsent = new ParliamentSection(document.getElementById("NumberOfAbsentVotes").value, '#eeeeee');
parliament.addSection(sectionAbsent);

var sectionReject = new ParliamentSection(document.getElementById("NumberOfRejectVotes").value, 'red');
parliament.addSection(sectionReject);

parliament.drawParliament();
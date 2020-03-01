//bar
function prisutniRadniciPoDatumu(Data,BrojRadnika) {

    $("#chartovi").html("");

    $("#chartovi").append(`<canvas id="barChart"></canvas>`);

    var ctxB = document.getElementById("barChart").getContext('2d');

    var myBarChart = new Chart(ctxB, {
        type: 'bar',
        data: {
            labels: ["Na poslu", "Privatno odsutan", "Višednevno odsutan", "Nepoznato", "Službeno odustan"],
            series:[1,2,3,4,5],
            datasets: [{
                label: '',
                data: Data,
                backgroundColor: [
                    'rgba(163, 203, 56, 0.7)',
                    'rgba(241, 196, 15, 0.7)',
                    'rgba(0, 83, 122, 0.7)',
                    'rgba(231, 76, 60, 0.7)',
                    'rgba(255, 255, 26, 0.7)'
                ],
                borderColor: [
                    'rgba(163, 203, 56, 1)',
                    'rgba(241, 196, 15, 1)',
                    'rgba(0, 83, 122, 1)',
                    'rgba(231, 76, 60, 1)',
                    'rgba(255, 255, 26, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            maintainAspectRatio: false,
            legend: {
                display: false
            },
            scales: {
                yAxes: [{
                    ticks: {
                        stepSize: 2,
                        beginAtZero: true,
                        max: BrojRadnika
                    }
                }]
            }
        }
    });

    $('#barChart').on('click', function (evt) {
        var activePoints = myBarChart.getElementsAtEventForMode(evt, 'point', myBarChart.options);
        var firstPoint = activePoints[0];
        var vrijednost = 0;
        if (firstPoint !== undefined) {
            vrijednost = myBarChart.data.series[firstPoint._index];
        }
            var data = {
                GrupaId: $("#dropDownGrupe").data('grupaid'),
                Datum: $('#date').val(),
                StatusId: vrijednost
        };

        $("#dropDownStatusi").data('statusid', vrijednost);
        $("#dropDownStatusi").val(vratiTekstStatusa(vrijednost) + ' ▽');
        Radnici();
    });

}
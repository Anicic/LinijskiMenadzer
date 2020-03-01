function PieZaRadnika(radnikID) {
    var datumi = $("#daterangeaco").val();
    var listadatuma = datumi.split("do");

    var podaci = {
        RadnikID: radnikID,
        DatumOd: listadatuma[0],
        DatumDo: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
    };

    var listaPrisutnosti = [];

    $.get('/Pocetna/GetPrisutnostModal', podaci, function (result, status) {
        listaPrisutnosti.push(result.NaPoslu);
        listaPrisutnosti.push(result.PrivatnoOdsutan);
        listaPrisutnosti.push(result.VisednevnoOdsutan);
        listaPrisutnosti.push(result.Nepoznato);
        listaPrisutnosti.push(result.PreostaloDana);
        listaPrisutnosti.push(result.SluzbenoOdsutan);

        $("#krofnaChartModal").html('')
        $("#krofnaChartModal").html('<canvas id="prisutnostModal" class="chartjs-render-monitor"></canvas>')

        var ctxD = document.getElementById("prisutnostModal").getContext('2d');
        var myLineChart = new Chart(ctxD, {
            type: 'doughnut',
            data: {
                labels: ["Na poslu", "Privatno odsutan", "Višednevno odsutan", "Nepoznato", "Preostalo dana", "Službeno odsutan"],
                datasets: [{
                    data: listaPrisutnosti,
                    backgroundColor: ['rgba(163, 203, 56)', 'rgb(255, 140, 26)', 'rgba(0, 83, 122)', 'rgba(231, 76, 60)', 'rgb(242, 242, 242)', 'rgba(255, 255, 26)'],
                    hoverBackgroundColor: ['rgba(163, 203, 56)', 'rgb(255, 140, 26)', 'rgba(0, 83, 122)', 'rgba(231, 76, 60)', 'rgb(242, 242, 242)', 'rgba(255, 255, 26)']
                }]
            },
            options: {
                legend: {
                    display: true,
                    position: 'bottom',
                    labels: {
                        fontColor: "black"
                    }
                },
                responsive: true,
                maintainAspectRatio: true,
            }
        });
    });

    $.get('/Pocetna/OdradjenoVrijemeRadnikaUIntervalu', podaci, function (result, status) {
        
        console.log(result.potrebnoRadnoVrijemeZaPeriodString);
        console.log(result.brojOdradjenihRadnihSatiString);
        console.log(result.brojSatiKojejeProveoPrekovremenoString);
        console.log(result.brojSatiKojeJeProveoManjeString);
    });
};

//function VratiStatistikuSluzbeniIzlazak(radnik) {
//    var datumi = $("#daterangeaco").val();
//    var listadatuma = datumi.split("do");
//    var podaci = {
//        Datum1: listadatuma[0],
//        Datum2: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
//        RadnikID: radnik.RadnikID
//    };

//    console.log(podaci);

//    $.get('/Pocetna/VrijemePoDanuNaSluzbenomPutuZaPeriod', podaci, function (result, status) {
//        var Labele = result.ListaLabela;
//        var Data = result.ListaMinutaNaSluzbenomPutu;
//        StatistikaSluzbeniIzlazak(Labele, Data);
//    })
//}
//function StatistikaSluzbeniIzlazak(Labele, Data) {
//    var ctx = document.getElementById('modalStatistikaSluzbeniIzlazak').getContext("2d");

//    var myChart = new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: Labele,
//            datasets: [{
//                label: "Data",
//                borderColor: "#80b6f4",
//                pointBorderColor: "#80b6f4",
//                pointBackgroundColor: "#80b6f4",
//                pointHoverBackgroundColor: "#80b6f4",
//                pointHoverBorderColor: "#80b6f4",
//                pointBorderWidth: 10,
//                pointHoverRadius: 10,
//                pointHoverBorderWidth: 1,
//                pointRadius: 3,
//                fill: false,
//                borderWidth: 4,
//                data: Data
//            }]
//        },
//        options: {
//            legend: {
//                position: "bottom"
//            },
//            scales: {
//                yAxes: [{
//                    ticks: {
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold",
//                        beginAtZero: true,
//                        maxTicksLimit: 5,
//                        padding: 20
//                    },
//                    gridLines: {
//                        drawTicks: false,
//                        display: false
//                    }

//                }],
//                xAxes: [{
//                    gridLines: {
//                        zeroLineColor: "transparent"
//                    },
//                    ticks: {
//                        padding: 20,
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold"
//                    }
//                }]
//            }
//        }
//    });
//}

//function VratiStatistikuPrivatniIzlazak(radnik) {
//    var datumi = $("#daterangeaco").val();
//    var listadatuma = datumi.split("do");
//    var podaci = {
//        Datum1: listadatuma[0],
//        Datum2: listadatuma[1],
//        RadnikID: radnik.RadnikID
//    }

//    $.get('/Pocetna/VrijemePoDanuNaPrivatnomOdsustvuZaPeriod', podaci, function (result, status) {
//        var Labele = result.ListaLabela;
//        var Data = result.ListaMinutaNaPrivatnomOdsustvu;
//        StatistikaPrivatniIzlazak(Labele, Data);
//    })
//}

//function StatistikaPrivatniIzlazak(Labele, Data) {
//    var ctx = document.getElementById('modalStatistikaPrivatniIzlazak').getContext("2d");

//    var myChart = new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: Labele,
//            datasets: [{
//                label: "Data",
//                borderColor: "#80b6f4",
//                pointBorderColor: "#80b6f4",
//                pointBackgroundColor: "#80b6f4",
//                pointHoverBackgroundColor: "#80b6f4",
//                pointHoverBorderColor: "#80b6f4",
//                pointBorderWidth: 10,
//                pointHoverRadius: 10,
//                pointHoverBorderWidth: 1,
//                pointRadius: 3,
//                fill: false,
//                borderWidth: 4,
//                data: Data
//            }]
//        },
//        options: {
//            legend: {
//                position: "bottom"
//            },
//            scales: {
//                yAxes: [{
//                    ticks: {
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold",
//                        beginAtZero: true,
//                        maxTicksLimit: 5,
//                        padding: 20
//                    },
//                    gridLines: {
//                        drawTicks: false,
//                        display: false
//                    }

//                }],
//                xAxes: [{
//                    gridLines: {
//                        zeroLineColor: "transparent"
//                    },
//                    ticks: {
//                        padding: 20,
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold"
//                    }
//                }]
//            }
//        }
//    });
//}

//function VratiStatistikuPauza(radnik) {
//    var datumi = $("#daterangeaco").val();
//    var listadatuma = datumi.split("do");
//    var podaci = {
//        Datum1: listadatuma[0],
//        Datum2: listadatuma[1],
//        RadnikID: radnik.RadnikID
//    }

//    $.get('/Pocetna/VrijemePoDanuNaPauziZaPeriod', podaci, function (result, status) {
//        var Labele = result.ListaLabela;
//        var Data = result.ListaMinutaNaPauzi;
//        StatistikaPauza(Labele, Data);
//    })
//}


//function StatistikaPauza(Labele, Data) {
//    var ctx = document.getElementById('modalStatistikaPauza').getContext("2d");

//    var myChart = new Chart(ctx, {
//        type: 'line',
//        data: {
//            labels: Labele,
//            datasets: [{
//                label: "Data",
//                borderColor: "#80b6f4",
//                pointBorderColor: "#80b6f4",
//                pointBackgroundColor: "#80b6f4",
//                pointHoverBackgroundColor: "#80b6f4",
//                pointHoverBorderColor: "#80b6f4",
//                pointBorderWidth: 10,
//                pointHoverRadius: 10,
//                pointHoverBorderWidth: 1,
//                pointRadius: 3,
//                fill: false,
//                borderWidth: 4,
//                data: Data
//            }]
//        },
//        options: {
//            legend: {
//                position: "bottom"
//            },
//            scales: {
//                yAxes: [{
//                    ticks: {
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold",
//                        beginAtZero: true,
//                        maxTicksLimit: 5,
//                        padding: 20
//                    },
//                    gridLines: {
//                        drawTicks: false,
//                        display: false
//                    }

//                }],
//                xAxes: [{
//                    gridLines: {
//                        zeroLineColor: "transparent"
//                    },
//                    ticks: {
//                        padding: 20,
//                        fontColor: "rgba(0,0,0,0.5)",
//                        fontStyle: "bold"
//                    }
//                }]
//            }
//        }
//    });
//}


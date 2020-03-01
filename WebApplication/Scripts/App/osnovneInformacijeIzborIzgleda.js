$(document).ready(function () {

    $('#Osnovne-informacije-active').css("color", "#00bdab");
    $('.Statusi').on('click', function (e) {
        e.stopPropagation();
        $('.dropStatus').fadeIn();
        $('.dropGrupe').fadeOut();
    });
    $("#stavitiDatum").append(`<input type="text" class="form-control" id="dateOsnovneInformacije" placeholder="Izaberite datum">`);

    $('#dateOsnovneInformacije').flatpickr({
        minDate: "1990-01-01",
        maxDate: "today",
        dateFormat: "Y-m-d",
        defaultDate: 'today'
    });
    ucitajJednodnevnuStatistiku();
    $(document).on('click', function () {
        $('.dropStatus').fadeOut();
    });

    $(".dropStatus li").on('click', function () {
        var selText = ($(this).text() + ' ▽');
        $("#dropDownStatusi").data('statusid', this.id);
        $("#dropDownStatusi").val(selText);

        if (Number($("#dropDownStatusi").data('statusid')) === 1) {//jednodnevno 

            $("#stavitiDatum").html('');
            $("#stavitiDatum").append(`<input type="text" class="form-control" id="dateOsnovneInformacije" placeholder="Izaberite datum">`);

            $('#dateOsnovneInformacije').flatpickr({
                minDate: "1990-01-01",
                maxDate: "today",
                dateFormat: "Y-m-d",
                defaultDate: 'today'
            });
            ucitajJednodnevnuStatistiku();
        } else if (Number($("#dropDownStatusi").data('statusid')) === 2) {//visednevno 
            $("#stavitiDatum").html('');
            $("#stavitiDatum").append(`<input type="text" class="form-control" id="dateOsnovneInformacije" placeholder="Izaberite datum">`);

            $('#dateOsnovneInformacije').flatpickr({
                mode: "range",
                minDate: "1990-01-01",
                maxDate: "today",
                dateFormat: "Y-m-d"
            })
            ucitajVisednevnuStatistiku();
        }
    });

    function ucitajJednodnevnuStatistiku() {

        $.get("/OsnovneInformacije/_VratiJednodnevniIzgled", function (result, status) {

            $("#prikazOsnovnihInformacija").html(result);

            function VratiAktivnosti() {
                var podaci = {
                    RadnikID: 0,
                    Datum: $('#dateOsnovneInformacije').val()
                }

                $.get('/Pocetna/VratiAktivnosti', podaci, function (result, status) {
                    $('#Aktivnosti').html(result);
                });
            }

            function VratiRadnoVrijeme() {

                var data = {
                    Datum: $('#dateOsnovneInformacije').val()
                };

                $.get("/OsnovneInformacije/VratiRadnoVrijeme", data, function (result, status) {
                    $("#RadnoVrijemeRadnik").html("");
                    $("#RadnoVrijemeRadnik").append(`<span class="chart RadnoVrijemeChart" data-percent="${result}">
                <span class="percent"></span>
            </span>`);

                    $('.chart').easyPieChart({
                        easing: 'easeOutBounce',
                        barColor: '#A3CB38',
                        lineWidth: 5,
                        onStep: function (from, to, percent) {
                            $(this.el).find('.percent').text(Math.round(percent));
                        }

                    });

                });

            }

            function VratiLabeluPrivatnoOdsustvo() {
                var podaci = {
                    RadnikID: 0,
                    Datum: $('#dateOsnovneInformacije').val()
                }
                $.get('/Pocetna/VrijemeNaPrivatnomOdsustvu', podaci, function (result, status) {
                    var Labela = result.ListaLabela;
                    var PrivatnoOdsustvo = result.ListaMinutaNaSluzbenomPutu;
                    VratiPrivatnoOdsustvo(Labela, PrivatnoOdsustvo);
                })
            }
            function VratiPrivatnoOdsustvo(Labele, PrivatnoOdsustvo) {

                var ctx = document.getElementById("myChartPrivatno").getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: Labele,
                        datasets: [{
                            label: 'Minuta',
                            data: PrivatnoOdsustvo,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }],
                            xAxes: [{
                                barPercentage: 0.3
                            }]
                        }
                    }
                });
            }

            function VratiLabele() {
                var podaci = {
                    RadnikID: 0,
                    Datum: $('#dateOsnovneInformacije').val()
                }
                $.get('/Pocetna/VrijemeNaSluzbenomPutu', podaci, function (result, status) {
                    var Labele = result.ListaLabela;
                    var SluzbeniPut = result.ListaMinutaNaSluzbenomPutu;
                    VratiSluzbeniPut(Labele, SluzbeniPut);
                })

            }
            function VratiSluzbeniPut(Labele, SluzbeniPut) {
                $("#Sluzbeni").html("");
                $("#Sluzbeni").append('<canvas id="myChart"></canvas>');


                var ctx = document.getElementById("myChart").getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: Labele,
                        datasets: [{
                            label: 'Minuta',
                            data: SluzbeniPut,
                            backgroundColor: [
                                'rgba(255, 99, 132, 0.2)',
                                'rgba(54, 162, 235, 0.2)',
                                'rgba(255, 206, 86, 0.2)',
                                'rgba(75, 192, 192, 0.2)',
                                'rgba(153, 102, 255, 0.2)',
                                'rgba(255, 159, 64, 0.2)'
                            ],
                            borderColor: [
                                'rgba(255,99,132,1)',
                                'rgba(54, 162, 235, 1)',
                                'rgba(255, 206, 86, 1)',
                                'rgba(75, 192, 192, 1)',
                                'rgba(153, 102, 255, 1)',
                                'rgba(255, 159, 64, 1)'
                            ],
                            borderWidth: 1
                        }],
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    beginAtZero: true
                                }
                            }],
                            xAxes: [{
                                barPercentage: 0.3
                            }]
                        }
                    }
                });
            }

            function DajPodatke() {
                var podaci = {
                    RadnikID: 0,
                    Datum: $('#dateOsnovneInformacije').val()
                }
                $.get('/Pocetna/VrijemeProvedenoNaPauzi', podaci, function (result, status) {
                    var Dozvoljeno = result.PauzeDo;
                    var Prekoraceno = result.PauzePreko;
                    VratiPauzu(Dozvoljeno, Prekoraceno)
                })
            }
            function VratiPauzu(Dozvoljeno, Prekoraceno) {

                $("#MojID").html("");
                $("#MojID").append('<canvas id="myChartPauza"></canvas>');

                var ctx = document.getElementById("myChartPauza").getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ["Pauza"],
                        datasets: [
                            {
                                label: "Dozvoljeno vrijeme na pauzi",
                                data: [Dozvoljeno],
                                backgroundColor: [
                                    '#A3CB38',
                                    '#A3CB38'
                                ]


                            },
                            {
                                label: "Prekoračeno vrijeme",
                                data: [Prekoraceno],
                                backgroundColor: [
                                    'rgba(231, 76, 60, 0.7)',
                                    'rgba(231, 76, 60, 0.7)'
                                ]
                            }
                        ]
                    },
                    options: {
                        scales: {
                            yAxes: [{
                                ticks: {
                                    max: 60,
                                    beginAtZero: true
                                },
                                stacked: true

                            }],

                            xAxes: [{
                                stacked: true,
                                barPercentage: 0.25
                            }],
                        }
                    }
                });
            }

            $('#dateOsnovneInformacije').on('change', function () {
                VratiAktivnosti();
                VratiRadnoVrijeme();
                VratiLabele();
                DajPodatke();
                VratiLabeluPrivatnoOdsustvo();
            });

            VratiAktivnosti();
            VratiRadnoVrijeme();
            DajPodatke();
            VratiLabele();
            VratiLabeluPrivatnoOdsustvo();

        });

    };

    function ucitajVisednevnuStatistiku() {
        $.get("/OsnovneInformacije/_VratiVisednevniIzgled", function (result, status) {

            $("#prikazOsnovnihInformacija").html(result);
            var radnik = {
                RadnikID: 0
            }

            function PieStatistika(radnik) {
                var datumi = $("#dateOsnovneInformacije").val();
                var listadatuma = datumi.split("do");

                var podaci = {
                    RadnikID: radnik.RadnikID,
                    DatumOd: listadatuma[0],
                    DatumDo: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1])
                };

                var listaPrisutnosti = [];

                $.get('/Pocetna/GetPrisutnostModal', podaci, function (result, status) {
                    listaPrisutnosti.push(result.NaPoslu);
                    listaPrisutnosti.push(result.PrivatnoOdsutan);
                    listaPrisutnosti.push(result.VisednevnoOdsutan);
                    listaPrisutnosti.push(result.Nepoznato);
                    listaPrisutnosti.push(result.PreostaloDana);
                    listaPrisutnosti.push(result.SluzbenoOdsutan);

                    $("#krofnaChartStatistika").html('')
                    $("#krofnaChartStatistika").html('<canvas id="prisutnostOsnovneInformacije" class="chartjs-render-monitor"></canvas>')

                    var ctxD = document.getElementById("prisutnostOsnovneInformacije").getContext('2d');
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
            }

            function VratiStatistikuSluzbeniIzlazak(radnik) {
                var datumi = $("#dateOsnovneInformacije").val();
                var listadatuma = datumi.split("do");
                var podaci = {
                    Datum1: listadatuma[0],
                    Datum2: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
                    RadnikID: radnik.RadnikID
                };

                $.get('/Pocetna/VrijemePoDanuNaSluzbenomPutuZaPeriod', podaci, function (result, status) {
                    var Labele = result.ListaLabela;
                    var Data = result.ListaMinutaNaSluzbenomPutu;
                    StatistikaSluzbeniIzlazak(Labele, Data);
                })
            }
            function StatistikaSluzbeniIzlazak(Labele, Data) {
                var ctx = document.getElementById('StatistikaSluzbeniIzlazak').getContext("2d");

                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: Labele,
                        datasets: [{
                            label: "Data",
                            borderColor: "#80b6f4",
                            pointBorderColor: "#80b6f4",
                            pointBackgroundColor: "#80b6f4",
                            pointHoverBackgroundColor: "#80b6f4",
                            pointHoverBorderColor: "#80b6f4",
                            pointBorderWidth: 10,
                            pointHoverRadius: 10,
                            pointHoverBorderWidth: 1,
                            pointRadius: 3,
                            fill: false,
                            borderWidth: 4,
                            data: Data
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold",
                                    beginAtZero: true,
                                    maxTicksLimit: 5,
                                    padding: 20
                                },
                                gridLines: {
                                    drawTicks: false,
                                    display: false
                                }

                            }],
                            xAxes: [{
                                gridLines: {
                                    zeroLineColor: "transparent"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold"
                                }
                            }]
                        }
                    }
                });
            }

            function VratiStatistikuPrivatniIzlazak(radnik) {
                var datumi = $("#dateOsnovneInformacije").val();
                var listadatuma = datumi.split("do");
                var podaci = {
                    Datum1: listadatuma[0],
                    Datum2: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
                    RadnikID: radnik.RadnikID
                }

                $.get('/Pocetna/VrijemePoDanuNaPrivatnomOdsustvuZaPeriod', podaci, function (result, status) {
                    var Labele = result.ListaLabela;
                    var Data = result.ListaMinutaNaPrivatnomOdsustvu;
                    StatistikaPrivatniIzlazak(Labele, Data);
                })
            }

            function StatistikaPrivatniIzlazak(Labele, Data) {
                var ctx = document.getElementById('StatistikaPrivatniIzlazak').getContext("2d");

                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: Labele,
                        datasets: [{
                            label: "Data",
                            borderColor: "#80b6f4",
                            pointBorderColor: "#80b6f4",
                            pointBackgroundColor: "#80b6f4",
                            pointHoverBackgroundColor: "#80b6f4",
                            pointHoverBorderColor: "#80b6f4",
                            pointBorderWidth: 10,
                            pointHoverRadius: 10,
                            pointHoverBorderWidth: 1,
                            pointRadius: 3,
                            fill: false,
                            borderWidth: 4,
                            data: Data
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold",
                                    beginAtZero: true,
                                    maxTicksLimit: 5,
                                    padding: 20
                                },
                                gridLines: {
                                    drawTicks: false,
                                    display: false
                                }

                            }],
                            xAxes: [{
                                gridLines: {
                                    zeroLineColor: "transparent"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold"
                                }
                            }]
                        }
                    }
                });
            }

            function VratiStatistikuPauza(radnik) {
                var datumi = $("#dateOsnovneInformacije").val();
                var listadatuma = datumi.split("do");
                var podaci = {
                    Datum1: listadatuma[0],
                    Datum2: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
                    RadnikID: radnik.RadnikID
                }

                $.get('/Pocetna/VrijemePoDanuNaPauziZaPeriod', podaci, function (result, status) {
                    var Labele = result.ListaLabela;
                    var Data = result.ListaMinutaNaPauzi;
                    StatistikaPauza(Labele, Data);
                })
            }


            function StatistikaPauza(Labele, Data) {
                var ctx = document.getElementById('StatistikaPauza').getContext("2d");

                var myChart = new Chart(ctx, {
                    type: 'line',
                    data: {
                        labels: Labele,
                        datasets: [{
                            label: "Data",
                            borderColor: "#80b6f4",
                            pointBorderColor: "#80b6f4",
                            pointBackgroundColor: "#80b6f4",
                            pointHoverBackgroundColor: "#80b6f4",
                            pointHoverBorderColor: "#80b6f4",
                            pointBorderWidth: 10,
                            pointHoverRadius: 10,
                            pointHoverBorderWidth: 1,
                            pointRadius: 3,
                            fill: false,
                            borderWidth: 4,
                            data: Data
                        }]
                    },
                    options: {
                        legend: {
                            display: false
                        },
                        scales: {
                            yAxes: [{
                                ticks: {
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold",
                                    beginAtZero: true,
                                    maxTicksLimit: 5,
                                    padding: 20
                                },
                                gridLines: {
                                    drawTicks: false,
                                    display: false
                                }

                            }],
                            xAxes: [{
                                gridLines: {
                                    zeroLineColor: "transparent"
                                },
                                ticks: {
                                    padding: 20,
                                    fontColor: "rgba(0,0,0,0.5)",
                                    fontStyle: "bold"
                                }
                            }]
                        }
                    }
                });
            }
            $("#dateOsnovneInformacije").on('change', function () {
                VratiStatistikuSluzbeniIzlazak(radnik);
                VratiStatistikuPrivatniIzlazak(radnik);
                VratiStatistikuPauza(radnik);
                PieStatistika(radnik);
            })
        });

    };

});
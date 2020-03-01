$(document).ready(function () {


    //$('.Statusi').on('click', function (e) {
    //    e.stopPropagation();
    //    $('.dropStatus').fadeIn();
    //    $('.dropGrupe').fadeOut();
    //});

    //$(document).on('click', function () {
    //    $('.dropStatus').fadeOut();
    //});

    //$(".dropStatus li").on('click', function () {
    //    var selText = ($(this).text() + ' ▽');
    //    $("#dropDownStatusi").data('statusid', this.id);
    //    $("#dropDownStatusi").val(selText);
    //});

    //function VratiAktivnosti() {
    //    var podaci = {
    //        RadnikID: 0,
    //        Datum: $('#dateOsnovneInformacije').val()
    //    }

    //    $.get('/Pocetna/VratiAktivnosti', podaci, function (result, status) {
    //        $('#Aktivnosti').html(result);
    //    });
    //}

    //function VratiRadnoVrijeme() {

    //    var data = {
    //        Datum : $('#dateOsnovneInformacije').val()
    //    };

    //    $.get("/OsnovneInformacije/VratiRadnoVrijeme", data, function (result, status) {
    //        $("#RadnoVrijemeRadnik").html("");
    //        $("#RadnoVrijemeRadnik").append(`<span class="chart RadnoVrijemeChart" data-percent="${result}">
    //            <span class="percent"></span>
    //        </span>`);

    //        $('.chart').easyPieChart({
    //            easing: 'easeOutBounce',
    //            barColor: '#A3CB38',
    //            lineWidth: 5,
    //            onStep: function (from, to, percent) {
    //                $(this.el).find('.percent').text(Math.round(percent));
    //            }

    //        });

    //    });

    //}

    ////$('#dateOsnovneInformacije').on('change', function () {
    ////    VratiAktivnosti();
    ////    VratiRadnoVrijeme();
    ////    VratiSluzbeniPut();
    ////    DajPodatke();
    ////    VratiLabele();
    ////    VratiLabeluPrivatnoOdsustvo();
    ////});
    //function VratiLabeluPrivatnoOdsustvo() {
    //    var podaci = {
    //        RadnikID: 0,
    //        Datum: $('#dateOsnovneInformacije').val()
    //    }
    //    $.get('/Pocetna/VrijemeNaPrivatnomOdsustvu', podaci, function (result, status) {
    //        var Labela = result.ListaLabela;
    //        var PrivatnoOdsustvo = result.ListaMinutaNaSluzbenomPutu;
    //        VratiPrivatnoOdsustvo(Labela, PrivatnoOdsustvo);
    //    })
    //}
    //function VratiPrivatnoOdsustvo(Labele, PrivatnoOdsustvo) {

    //    var ctx = document.getElementById("myChartPrivatno").getContext('2d');
    //    var myChart = new Chart(ctx, {
    //        type: 'bar',
    //        data: {
    //            labels: Labele,
    //            datasets: [{
    //                label: 'Minuta',
    //                data: PrivatnoOdsustvo,
    //                backgroundColor: [
    //                    'rgba(255, 99, 132, 0.2)',
    //                    'rgba(54, 162, 235, 0.2)',
    //                    'rgba(255, 206, 86, 0.2)',
    //                    'rgba(75, 192, 192, 0.2)',
    //                    'rgba(153, 102, 255, 0.2)',
    //                    'rgba(255, 159, 64, 0.2)'
    //                ],
    //                borderColor: [
    //                    'rgba(255,99,132,1)',
    //                    'rgba(54, 162, 235, 1)',
    //                    'rgba(255, 206, 86, 1)',
    //                    'rgba(75, 192, 192, 1)',
    //                    'rgba(153, 102, 255, 1)',
    //                    'rgba(255, 159, 64, 1)'
    //                ],
    //                borderWidth: 1
    //            }]
    //        },
    //        options: {
    //            legend: {
    //                display: false
    //            },
    //            scales: {
    //                yAxes: [{
    //                    ticks: {
    //                        beginAtZero: true
    //                    }
    //                }],
    //                xAxes: [{
    //                    barPercentage: 0.3
    //                }]
    //            }
    //        }
    //    });
    //}

    //function VratiLabele() {
    //    var podaci = {
    //        RadnikID: 0,
    //        Datum: $('#dateOsnovneInformacije').val()
    //    }
    //    $.get('/Pocetna/VrijemeNaSluzbenomPutu', podaci, function (result, status) {
    //        var Labele = result.ListaLabela;
    //        var SluzbeniPut = result.ListaMinutaNaSluzbenomPutu;
    //        VratiSluzbeniPut(Labele, SluzbeniPut);
    //    })

    //}
    //function VratiSluzbeniPut(Labele, SluzbeniPut) {
    //    $("#Sluzbeni").html("");
    //    $("#Sluzbeni").append('<canvas id="myChart"></canvas>');


    //    var ctx = document.getElementById("myChart").getContext('2d');
    //    var myChart = new Chart(ctx, {
    //        type: 'bar',
    //        data: {
    //            labels: Labele,
    //            datasets: [{
    //                label: 'Minuta',
    //                data: SluzbeniPut,
    //                backgroundColor: [
    //                    'rgba(255, 99, 132, 0.2)',
    //                    'rgba(54, 162, 235, 0.2)',
    //                    'rgba(255, 206, 86, 0.2)',
    //                    'rgba(75, 192, 192, 0.2)',
    //                    'rgba(153, 102, 255, 0.2)',
    //                    'rgba(255, 159, 64, 0.2)'
    //                ],
    //                borderColor: [
    //                    'rgba(255,99,132,1)',
    //                    'rgba(54, 162, 235, 1)',
    //                    'rgba(255, 206, 86, 1)',
    //                    'rgba(75, 192, 192, 1)',
    //                    'rgba(153, 102, 255, 1)',
    //                    'rgba(255, 159, 64, 1)'
    //                ],
    //                borderWidth: 1
    //            }],
    //        },
    //        options: {
    //            legend: {
    //                display: false
    //            },
    //            scales: {
    //                yAxes: [{
    //                    ticks: {
    //                        beginAtZero: true
    //                    }
    //                }],
    //                xAxes: [{
    //                    barPercentage: 0.3
    //                }]
    //            }
    //        }
    //    });
    //}

    //function DajPodatke() {
    //    var podaci = {
    //        RadnikID: 0,
    //        Datum: $('#dateOsnovneInformacije').val()
    //    }
    //    $.get('/Pocetna/VrijemeProvedenoNaPauzi', podaci, function (result, status) {
    //        var Dozvoljeno = result.PauzeDo;
    //        var Prekoraceno = result.PauzePreko;
    //        VratiPauzu(Dozvoljeno, Prekoraceno)
    //    })
    //}
    //function VratiPauzu(Dozvoljeno, Prekoraceno) {

    //    $("#MojID").html("");
    //    $("#MojID").append('<canvas id="myChartPauza"></canvas>');

    //    var ctx = document.getElementById("myChartPauza").getContext('2d');
    //    var myChart = new Chart(ctx, {
    //        type: 'bar',
    //        data: {
    //            labels: ["Pauza"],
    //            datasets: [
    //                {
    //                    label: "Dozvoljeno vrijeme na pauzi",
    //                    data: Dozvoljeno,
    //                    backgroundColor: [
    //                        '#A3CB38',
    //                        '#A3CB38'
    //                    ]


    ////                },
    ////                {
    ////                    label: "Prekoraceno vrijeme",
    ////                    data: Prekoraceno,
    ////                    backgroundColor: [
    ////                        'rgba(231, 76, 60, 0.7)',
    ////                        'rgba(231, 76, 60, 0.7)'
    ////                    ]
    ////                }
    ////            ]
    ////        },
    ////        options: {
    ////            scales: {
    ////                yAxes: [{
    ////                    ticks: {
    ////                        beginAtZero: true,
    ////                        max: 60
    ////                    },
    ////                    stacked: true

    ////                }],

    //                xAxes: [{
    //                    stacked: true,
    //                    barPercentage: 0.2
    //                }],
    //            }
    //        }
    //    });
    //}

    ////VratiAktivnosti(0);
    ////VratiRadnoVrijeme();
    ////VratiSluzbeniPut();
    ////DajPodatke();
    ////VratiLabele();
    ////VratiLabeluPrivatnoOdsustvo();
});
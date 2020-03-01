    function modalRefreshAktivnosti(radnik) {
        var podaci = {
            RadnikID: radnik.RadnikID,
            Datum: $('#datumModalKalendar').val()
        };
     
        $.get("/Pocetna/VratiAktivnosti", podaci, function (result, status) {

           
            $("#listaAktivnosti").html(result);

        });
    }

    var radnik = {};

    function Radnici() {
        var data = {
            GrupaId: $("#dropDownGrupe").data('grupaid'),
            StatusId: $("#dropDownStatusi").data('statusid'),
            Datum: $('#date').val()
        };

        $.get("/Pocetna/_SviRadnici", data, function (result, status) {
            $('#results').html(result);

            var table = $('#example').DataTable({
                select: true,
                "searching": true,
                "paging": true,
                "pageLength": 5,
                "aLengthMenu": [[5, 10, 20], [5, 10, 20]],
                "language": {
                    "sProcessing": "Procesiranje u toku...",
                    "sLengthMenu": "Prikaži _MENU_ elemenata",
                    "sZeroRecords": "Nije pronađen nijedan rezultat",
                    "sInfo": "Prikaz _START_ do _END_ od ukupno _TOTAL_ elemenata",
                    "sInfoEmpty": "Prikaz 0 do 0 od ukupno 0 elemenata",
                    "sInfoFiltered": "(filtrirano od ukupno _MAX_ elemenata)",
                    "sInfoPostFix": "",
                    "sSearch": "Pretraga:",
                    "sUrl": "",
                    "oPaginate": {
                        "sFirst": "Početna",
                        "sPrevious": "Prethodna",
                        "sNext": "Sledeća",
                        "sLast": "Poslednja"
                    }
                },
                'columnDefs': [{
                    'targets': [5, 6], /* column index */
                    'orderable': false, /* true or false */
                }]

            });

            $('#example tbody').on('click', 'tr', function () {
                ucitajProfilRadnika(this.children['item_RadnikID'].value);
            });

            $('#example').on('click', 'tbody td .btnModal', function () {

                $("#exampleModal").modal();

                $("#krofnaChartModal").html('<p class ="aktivnosti-poruka pt-4">Odaberite vremenski period za prikaz statistike...</p>')

                //pravi novi kalendar
                radnik = {
                    RadnikID: this.dataset['radnikid']
                }

                VratiLabeluPrivatnoOdsustvo(radnik);
                VratiLabeleSluzbeniPut(radnik);
                VratiLabelePauza(radnik);

                $("#daterangeaco").flatpickr({
                    mode: "range",
                    minDate: "1990-01-01",
                    maxDate: "today",
                    dateFormat: "Y-m-d"
                });


                //brise stari kalendar
                $("#mycalendar").html("");

                $.get("/Pocetna/PrikaziDetalje", radnik, function (result, status) {
                    $("#SlikaModal").attr("src", "https://www.jeancoutu.com/globalassets/revamp/photo/conseils-photo/20160302-01-reseaux-sociaux-profil/image-principale-919b9d.png"/*result[0].LinkSlike*/ );
                    $('#ImeKorisnika').html(" ");
                    $('#Email').html(" ");
                    $('#BrojTelefona').html(" ");
                    var ImePrezime = result[0].Ime + " " + result[0].Prezime;
                    var Email = result[0].EmailAdresa;
                    var BrojTelefona = result[0].BrojTelefona;
                    $('#ImeKorisnika').append(ImePrezime);
                    $('#Email').append(Email);
                    $('#BrojTelefona').append(BrojTelefona);

                });

                $.get('/Pocetna/GodisnjiOdmorPreostaloDana', radnik, function (result, status) {
                    var preostaliDani = $("#brojDanaGodisnjegOdmora").html(result);
                    $("#brojDanaGodisnjegOdmora").append(preostaliDani);

                });

                $.get("/Pocetna/EvidencijaRadaPoDatumu", radnik, function (result, status) {

                    let lista = {
                        "datumi": result
                    };

                    var sampleEvents = { "monthly": [] };

                    $('#mycalendar').monthly({
                        mode: 'event',
                        dataType: 'json',
                        events: sampleEvents,
                        listaDatuma: lista
                    });

                });

                modalRefreshAktivnosti(radnik);
            });

            $("#datumModalKalendar").daterangepicker({
                singleDatePicker: true,
                maxDate: moment()
            });

            $("#datumModalKalendar").on('change', function () {
                VratiLabeluPrivatnoOdsustvo(radnik);
                VratiLabeleSluzbeniPut(radnik);
                VratiLabelePauza(radnik);
                modalRefreshAktivnosti(radnik);
            });

            //search za DataTable
            $("#example_filter").css('display', 'none');
            $('#text').keyup(function () {
                table.search(this.value).draw('page');
            });


            $('[data-toggle="popover"]').popover();
            function VratiLabeleSluzbeniPut(radnik) {
                var podaci = {
                    RadnikID: radnik.RadnikID,
                    Datum: $('#datumModalKalendar').val()
                }
                $.get('/Pocetna/VrijemeNaSluzbenomPutu', podaci, function (result, status) {
                    var Labele = result.ListaLabela;
                    var SluzbeniPut = result.ListaMinutaNaSluzbenomPutu;
                    VratiSluzbeniPut(Labele, SluzbeniPut);
                })

            }
            function VratiSluzbeniPut(Labele, SluzbeniPut) {
                $("#Sluzbeni").html("");
                $("#Sluzbeni").append('<canvas id="modalSluzbeniPutRadnik"></canvas>');


                var ctx = document.getElementById("modalSluzbeniPutRadnik").getContext('2d');
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
            function VratiLabeluPrivatnoOdsustvo(radnik) {
                var podaci = {
                    RadnikID: radnik.RadnikID,
                    Datum: $('#datumModalKalendar').val()
                }
                $.get('/Pocetna/VrijemeNaPrivatnomOdsustvu', podaci, function (result, status) {
                    var Labela = result.ListaLabela;
                    var PrivatnoOdsustvo = result.ListaMinutaNaSluzbenomPutu;
                    VratiPrivatnoOdsustvo(Labela, PrivatnoOdsustvo);
                })
            }
            function VratiPrivatnoOdsustvo(Labele, PrivatnoOdsustvo) {
                var ctx = document.getElementById("modalPrivatnoOdsustvoRadnik").getContext('2d');
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

            function VratiLabelePauza(radnik) {
                var podaci = {
                    RadnikID: radnik.RadnikID,
                    Datum: $('#datumModalKalendar').val()
                }
                $.get('/Pocetna/VrijemeProvedenoNaPauzi', podaci, function (result, status) {
                    var Dozvoljeno = result.PauzeDo;
                    var Prekoraceno = result.PauzePreko;
                    VratiPauzu(Dozvoljeno, Prekoraceno)
                })
            }
            function VratiPauzu(Dozvoljeno, Prekoraceno) {

                $("#MojID").html("");
                $("#MojID").append('<canvas id="modalPauzaRadnik"></canvas>');

                var ctx = document.getElementById("modalPauzaRadnik").getContext('2d');
                var myChart = new Chart(ctx, {
                    type: 'bar',
                    data: {
                        labels: ["Pauza"],
                        datasets: [
                            {
                                label: "Vrijeme provedeno na pauzi",
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
                                barPercentage: 0.3
                            }],
                        }
                    }
                });
            }

            function VratiStatistikuSluzbeniIzlazak(radnik) {
                var datumi = $("#daterangeaco").val();
                var listadatuma = datumi.split("do");
                var podaci = {
                    Datum1: listadatuma[0],
                    Datum2: (listadatuma[1] === undefined ? listadatuma[0] : listadatuma[1]),
                    RadnikID: radnik.RadnikID
                }

                $.get('/Pocetna/VrijemePoDanuNaSluzbenomPutuZaPeriod', podaci, function (result, status) {                  
                    var Labele = result.ListaLabela;
                    var Data = result.ListaMinutaNaSluzbenomPutu;                  
                    StatistikaSluzbeniIzlazak(Labele, Data);
                })                              
            }
            function StatistikaSluzbeniIzlazak(Labele, Data) {
                var ctx = document.getElementById('modalStatistikaSluzbeniIzlazak').getContext("2d");

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
                var datumi = $("#daterangeaco").val();
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
                var ctx = document.getElementById('modalStatistikaPrivatniIzlazak').getContext("2d");

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
                var datumi = $("#daterangeaco").val();
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
                var ctx = document.getElementById('modalStatistikaPauza').getContext("2d");

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
            $("#daterangeaco").on('change', function () {
                VratiStatistikuSluzbeniIzlazak(radnik);
                VratiStatistikuPrivatniIzlazak(radnik);
                VratiStatistikuPauza(radnik);
                PieZaRadnika(radnik.RadnikID);
            });

            $("#exampleModal").bind('scroll', function () {
                $('.daterangepicker').hide();
            });
        });
};

function vratiTekstStatusa(statusID) {

    switch (statusID) {
        case 0:
            return 'Status';
            break;
        case 1:
            return 'Na poslu';
            break;
        case 2:
            return 'Privatno odsutan';
            break;
        case 3:
            return 'Višednevno odsutan';
            break;
        case 4:
            return 'Nepoznato';
            break;
        case 5:
            return 'Službeno odsutan';
            break;
    }

}

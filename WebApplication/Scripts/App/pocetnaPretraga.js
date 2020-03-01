$(document).ready(function () {
    $('#pocetna-active').css("color", "#00bdab");
    var radnik = {};

    // Rjesavanje problema sa dropdownlistom 
    $('.Statusi').on('click', function (e) {
        e.stopPropagation();
        $('.dropStatus').fadeIn();
        $('.dropGrupe').fadeOut();
    });
    $('.Grupe').on('click', function (e) {
        e.stopPropagation();
        $('.dropGrupe').fadeIn();
        $('.dropStatus').fadeOut();
    });

    $(document).on('click', function () {
        $('.dropGrupe').fadeOut();
        $('.dropStatus').fadeOut();
    });

    //Kupljenje podataka za grupe
    $(".dropGrupe li").on('click', function (e) {
        var selText = ($(this).text() + ' ▽');
        $("#dropDownGrupe").data('grupaid', this.id);
        $("#dropDownGrupe").val(selText);
        Radnici();
        GrafPopuniti();
    });

    //Kupljenje podataka za status
    $(".dropStatus li").on('click', function () {
        var selText = ($(this).text() + ' ▽');
        $("#dropDownStatusi").data('statusid', this.id);
        $("#dropDownStatusi").val(selText);
        Radnici();
    });

   
    $('input[type="search"]').on('search', function (e) {
        e.preventDefault();
        $('#text').val("");
        Radnici();
    });

    $('#date').on('change', function (e) {
        Radnici();
        GrafPopuniti();
    })


    function GrafPopuniti() {
        var PodaciZaKontroler = {
            Datum: $('#date').val(),
            GrupaId: $("#dropDownGrupe").data('grupaid')
        }

        $.get("/Pocetna/PopuniGraf", PodaciZaKontroler, function (result, status) {

            var data = result.BrojeviZaGraf;
            var BrojRadnika = result.ukupnoRadnikaUGrupi;
            prisutniRadniciPoDatumu(data, BrojRadnika);
        })

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
                                label: "Dozvoljeno vrijeme na pauzi",
                                data: [Dozvoljeno],
                                backgroundColor: [
                                    '#A3CB38',
                                    '#A3CB38'
                                ]
                            },
                            {
                                label: "Prekoraceno vrijeme",
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

            $(".btnModal").on('click', function () {

                $("#exampleModal").modal();

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
                    $("#SlikaModal").attr("src", result[0].LinkSlike);
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

                //get dobite podatke $("#brojDanaGodisnjegOdmora").text(result);
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

            $("#exampleModal").bind('scroll', function () {
                $('.daterangepicker').hide();
            });
        

    function modalRefreshAktivnosti(radnik) {
        var podaci = {
            RadnikID: radnik.RadnikID,
            Datum: $('#datumModalKalendar').val()
        };
        $.get("/Pocetna/VratiAktivnosti", podaci, function (result, status) {
            $("#listaAktivnosti").html(result);
        });
    }
    Radnici();
    GrafPopuniti();
});



$(document).ready(function () {
    $('#obavjestenje-active').css("color", "#00bdab");
    $('.dropStatusObavjestenja').fadeOut();
    $('.StatusObavjestenja').on('click', function (e) {
        console.log('proslo');
        e.stopPropagation();
        $('.dropStatusObavjestenja').fadeIn();
    });

    $(".dropStatusObavjestenja li").on('click', function () {
        var selText = ($(this).text() + ' ▽');
        $("#dropDownStatusiObavjestenja").data('statusobavjestenjeid', this.id);
        $("#dropDownStatusiObavjestenja").val(selText);
        $('.dropStatusObavjestenja').fadeOut();
        prikaziObavjestenja(true, Number(this.id));
    });

    function pregledObavjestenja(obavjestenjeID) {

        var data = {
            obavjestenjeID: obavjestenjeID
        };


        $.get("/Obavjestenja/_PregledObavjestenja", data, function (result, status) {
            $("#zahtjev").html(result);

            $("#btnOdobri").on('click', function () {
                Obrazlozenje(true, obavjestenjeID);
                $("#btnOdobri").attr('disabled', true);

            });

            $('#btnZabrani').on('click', function () {
                Obrazlozenje(false, obavjestenjeID);
            });

            $("#obrazlozenje").on('change', function () {

                if ($('#obrazlozenje').is(":checked")) {
                    $("#poruka").append(`<div class="form-group mt-3">
                    <h3 for="exampleInputPassword1">Obrazloženje</h3>
                    <input type="password" class="form-control" id="razlog" placeholder="Obrazloženje">
                    </div>

                    `);

                    $('#razlog').summernote({
                        height: 150,
                        placeholder: 'Ukucajte obrazloženje odgovora...'
                    });
                }
                else {
                    $("#poruka").html('');
                }
            });

        });

    };

    function Obrazlozenje(odobrenje, obavjestenjeId) {

        var data = {
            obavjestenjeID: obavjestenjeId,
            odobri: odobrenje,
            imaObrazlozenje: true,
            TextObavjestenja: $('#razlog').code()
        };
        

        $.get("/Obavjestenja/RjesavanjeZahtjeva", data, function (result, status) {
            $('#success_tic').modal('show');
            pregledObavjestenja(obavjestenjeId);
        });
    }


    function prikaziObavjestenja(svaObavjestenja, statusID) {

        var data = {
            IspisiSvaObavjestenja: svaObavjestenja,
            StatusID: statusID
        };

        $.get('/Obavjestenja/_VratiObavjestenja', data, function (result, status) {
            $("#listaObavjestenja").html(result);

            $(".obavjestenje").on('click', function () {
                pregledObavjestenja($(this).data('obavjestenjeid'));

                $(this).css("background-color", "white");

                var data = {
                    obavjestenjeID: $(this).data('obavjestenjeid')
                };
                $.get('/Obavjestenja/ProcitanoObavjestenje', data, function (result, status) {
                });
            });

        });

        $.get('/Obavjestenja/VratiBrojNeprocitanihObavjestenja', function (result, status) {
            $("#neprocitanaObavjestenja").text(`Neprocitano(${result.brojNeprocitanihObavjestenja})`);
        });

    };

    $("#osvjeziObavjestenja").on('click', function () {
        prikaziObavjestenja(true,0);
    });

    $("#neprocitanaObavjestenja").on('click', function () {
        prikaziObavjestenja(false, 0);
    });

    $("#kreirajNoviZahtjev").on('click', function () {



        $.get('/Obavjestenja/_KreirajNoviZahtjev', function (result, status) {

            $("#zahtjev").html(result);
            vrstaPrekovremeni();


            $('#Tekst').summernote({
                height: 150,
                placeholder: 'Ukucajte tekst zahtjeva...'
            });

            $("#vrstaZahtjeva").on('change', function () {
                var vrstaZahtjeva = Number($(this).val());

                switch (vrstaZahtjeva) {

                    case 1: vrstaPrekovremeni();
                        break;
                    case 2: vrstaPrelazakRadnika();
                        break;
                    case 3: vrstaStatistikaRadnika();
                        break;

                }

            });

            var obavjestenjeID = 1;
            var radnikID = null;
            //var datumOdNecega = null;
            $('#vrstaZahtjeva').change(function () {

            });

            obavjestenjeID = $(this).find(':selected').val();
            $('#datumOdNecega').on('change', function () {
                var datumOd = $('#datumOdNecega').val();
            })
            $('#datumDoNecega').on('change', function () {
                var datumDo = $('#datumDoNecega').val();
            })



            //ajax
            $('#btnPosalji').on('click', function () {

                var data = {
                    DatumOd: $('#datumOdNecega').val(),
                    DatumDo: $('#datumDoNecega').val(),
                    TextObavjestenja: $('#Tekst').code(),
                    RadnikID: $('#OdabraniRadnik').val(),
                    PrimalacID: $('#OdabraniPrimalac').val(),
                    TipID: $('#vrstaZahtjeva').val()
                };

                

                $.post("/Obavjestenja/_KreirajNoviZahtjev", data, function (result, status) {
                    $('#success_tic').modal('show');
                    $("#zahtjev").html(result);
                });
                


            });

           

            $("#btnOdustani").on('click', function () {
                $("#zahtjev").html('');
            });



            $('.listaObavjestenja').on('click', function () {
                $("#ListaPrimalaca").empty();
                $('#ListaRadnika').empty();
            });



            $('#primalacID').keyup(function () {
                var value = $('#primalacID').val().toLowerCase();
                PretragaPrimaoci(value);
            });
           
            $('#primalacID').click(function () {
                var value = $('#primalacID').val().toLowerCase();
                if (value !== "") {
                    PretragaPrimaoci(value);
                }
                $('#ListaRadnika').empty();

            });

           

            $('.widget').click(function () {
                $('#ListaPrimalaca').empty();
                $('#ListaRadnika').empty();
            });
           
        });

    });

  

    function PretragaPrimaoci(nazivRadnika) {
        var data = {
            Pretraga: nazivRadnika
        };
        if (data.Pretraga !== "") {
            $.get("/Obavjestenja/PretraziRadnike", data, function (result, status) {
                $("#ListaPrimalaca").empty();
                result.Data.forEach(element => {

                    $("#ListaPrimalaca").append(
                        `<li class="radnici-kartice col-md-12" style="z-index:9999">
                         <div class = "col-md-12 reset row">
                            <div class = "col-md-2 reset">
                                <img src='https://cdn2.iconfinder.com/data/icons/people-3-2/128/Programmer-Avatar-Backend-Developer-Nerd-512.png' class = "slika-kartica" />
                            </div>                            

                            <div class = "col-md-10 reset" >
                                <span class="ime-i-prezime" id=${element.RadnikID} radnikIme=${element.Ime} radnikPrezime=${element.Prezime}>${element.Ime + ' ' + element.Prezime}</span><br>
                                ${element.EmailAdresa}
                            </div>
                         </div>
                    </li>`);
                });

                


               

                $('#ListaPrimalaca li').click(function () {
                    $("#ListaPrimalaca").empty();
                    var ime = $(this).find('span').attr('radnikIme');
                    var prezime = $(this).find('span').attr('radnikPrezime');
                    $("#primalacID").val(ime + ' ' + prezime);

                    $('#OdabraniPrimalac').val($(this).find('span').attr('id'));

                    
                });


            });
        }
    }

    


    function vrstaPrekovremeni() {
        $("#VrstaZahtjevaIzgled").html('');
        $("#VrstaZahtjevaIzgled").append(`<div class="form-group">
                            <label for="exampleFormControlSelect1">Izaberi datum od</label>
                            <input type="text" class="form-control" id="datumOdNecega" placeholder="Izaberite datum">
                        </div>`);

        $('#datumOdNecega').daterangepicker({
            locale: {
                format: 'DD-MM-YYYY'
            },
            singleDatePicker: true
        });
    };


    function PretragaRadnika(nazivRadnika) {
        var data = {
            Pretraga: nazivRadnika
        };
        if (data.Pretraga !== "") {
            $.get("/Obavjestenja/PretraziRadnike", data, function (result, status) {
                $("#ListaRadnika").empty();
                result.Data.forEach(element => {

                    $("#ListaRadnika").append(
                        `<li class="radnici-kartice col-md-12" style="z-index:9999">
                         <div class = "col-md-12 reset row">
                            <div class = "col-md-2 reset">
                                <img src='https://cdn2.iconfinder.com/data/icons/people-3-2/128/Programmer-Avatar-Backend-Developer-Nerd-512.png' class = "slika-kartica" />
                            </div>                            

                            <div class = "col-md-10 reset" >
                                <span class="ime-i-prezime" id=${element.RadnikID} radnikIme=${element.Ime} radnikPrezime=${element.Prezime}>${element.Ime + ' ' + element.Prezime}</span><br>
                                ${element.EmailAdresa}
                            </div>
                         </div>
                    </li>`);
                });
                

                $('#ListaRadnika li').click(function () {
                    $("#ListaRadnika").empty();
                    var ime = $(this).find('span').attr('radnikIme');
                    var prezime = $(this).find('span').attr('radnikPrezime');
                    $("#radnikID").val(ime + ' ' + prezime);

                    $('#OdabraniRadnik').val($(this).find('span').attr('id'));
                });

            });
        }
    }




    function vrstaPrelazakRadnika() {
        $("#VrstaZahtjevaIzgled").html('');
        $("#VrstaZahtjevaIzgled").append(`<div class="form-group">
                            <label for="exampleInputEmail1">Izaberi radnika</label>
                            <input type="text" class="form-control" id="radnikID" placeholder="Pronadji radnika">
                            <input type="hidden" id="OdabraniRadnik"/>
                            <div class="row col-md-11 search-primalac  rezultat-pretraga pl-4 pr-4 reset" id="ListaRadnika">
                                <ul></ul>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="exampleFormControlSelect1">Izaberi datum od</label>
                            <input type="text" class="form-control" id="datumOdNecega" placeholder="Izaberite datum">
                        </div>

                        <div class="form-group">
                            <label for="exampleFormControlSelect1">Izaberi datum do</label>
                            <input type="text" class="form-control" id="datumDoNecega" placeholder="Izaberite datum">
                        </div>`);

        $('#radnikID').keyup(function () {
            var value = $('#radnikID').val().toLowerCase();
            PretragaRadnika(value);
        });

       

        $('#radnikID').on('click', function () {
            var value = $('#radnikID').val().toLowerCase();
            if (value !== "") {
                PretragaRadnika(value);
            }
            $('#ListaPrimalaca').empty();

        });


        $('#datumOdNecega').daterangepicker({
            locale: {
                format: 'DD-MM-YYYY'
            },
            singleDatePicker: true
        });

        $('#datumDoNecega').daterangepicker({
            locale: {
                format: 'DD-MM-YYYY'
            },
            singleDatePicker: true
        });

    };

    function vrstaStatistikaRadnika() {
        $("#VrstaZahtjevaIzgled").html('');
        $("#VrstaZahtjevaIzgled").append(`<div class="form-group">
                            <label for="exampleInputEmail1">Izaberi radnika</label>
                            <input type="text" class="form-control" id="radnikID" placeholder="Pronađi radnika">
                            <input type="hidden" id="OdabraniRadnik"/>
                            <div class="widget-body row col-md-11 search-primalac  rezultat-pretraga pl-4 pr-4 reset" id="ListaRadnika">
                                <ul></ul>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="exampleFormControlSelect1">Izaberi datum od</label>
                            <input type="text" class="form-control" id="datumOdNecega" placeholder="Izaberite datum">
                        </div>

                        <div class="form-group">
                            <label for="exampleFormControlSelect1">Izaberi datum do</label>
                            <input type="text" class="form-control" id="datumDoNecega" placeholder="Izaberite datum">
                        </div>`);

        $('#radnikID').keyup(function () {
            var value = $('#radnikID').val().toLowerCase();
            PretragaRadnika(value);
        });

        $('#datumOdNecega').daterangepicker({
            locale: {
                format: 'DD-MM-YYYY'
            },
            singleDatePicker: true
        });

        $('#datumDoNecega').daterangepicker({
            locale: {
                format: 'DD-MM-YYYY'
            },
            singleDatePicker: true
        });

    };

    prikaziObavjestenja(true, 0);



});
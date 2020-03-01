$("#notifikacije").on('click', function () {
    $.get("/Obavjestenja/Last5notifications", function (result, status) {
        $('#Notifikacije').empty();

        result.forEach(ele => {
            $('#Notifikacije').append('<li class="notification-box notifikacija-hover NotifikacijeClass" id="' + ele.ObavjestenjeID + '"><div class="row"><div class="col-lg-3 col-sm-3 col-3 text-center"><img src="https://www.jeancoutu.com/globalassets/revamp/photo/conseils-photo/20160302-01-reseaux-sociaux-profil/image-principale-919b9d.png" id="NotifikacijaSlike"/> </div> <div class="col-lg-8 col-sm-8 col-8"> <strong class="text-info">' + ele.PosiljalacIme +
                '</strong> <div> ' + ele.ObavjestenjeNaziv + '</div> <small class="text-warning">' + ele.DatumObavjestenjaString + '</small></div></li>');
        });

        $('.NotifikacijeClass').on('click', function () {
            $('#modalObavjestenja').modal();

            var data = {
                id : $(this).attr('id')
            };
            
            
            $.get("/Obavjestenja/VratiObavjestenje", data, function (result, status) {
                $('#Posiljalac').empty();
                $('#TipObavjestenja').empty();
                $('#sadrzajObavjestenja').empty();
                $('#Pregledano').empty();
                $('#Odobreno').empty();
                $('#Posiljalac').append(result.PosiljalacIme);
                $('#TipObavjestenja').append(result.ObavjestenjeNaziv);
                $('#sadrzajObavjestenja').append(result.SadrzajObavjestenja);
                if (result.Pregledano) {
                    $('#Pregledano').append("Da");
                } else {
                    $('#Pregledano').append("Ne");
                }
                if (result.Odobreno) {
                    $('#Odobreno').append("Da");
                } else {
                    $('#Odobreno').append("Ne");
                }


            });
        });

        $('#BrojObavjestenja').empty();
        $('#BrojObavjestenja').html(result.length);
    });

    if ($("#listaNotifikacija").hasClass('show')) {
        $("#listaNotifikacija").removeClass('show');
        $("#notifikacije").removeClass('notifikacije-selektovan');
    }
    else {
        $("#listaNotifikacija").addClass('show');
        $("#notifikacije").addClass('notifikacije-selektovan');
    }
});

//$('.NotifikacijeClass').on('click',function () {
//    console.log('jea');
//});


    



//function GrafPopuniti() {
//    var PodaciZaKontroler = {
//        Datum: $('#date').val(),
//        GrupaId: $("#dropDownGrupe").data('grupaid')
//    }

//    $.get("/Pocetna/PopuniGraf", PodaciZaKontroler, function (result, status) {

//        var data = result;
//        prisutniRadniciPoDatumu(data);
//    })

//}
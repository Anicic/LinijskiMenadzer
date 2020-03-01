$(document).ready(function () {
    $('#administracija-active').css("color", "#00bdab");
    function pretragaGrupa(nazivGrupe) {
        var data = {
            Pretraga: nazivGrupe
        };

        $.get("/GrupaRadnik1/GetGrupe", data, function (result, status) {
            $("#sveGrupe").empty();
            result.Data.forEach(element => {
                $("#sveGrupe").append(`<div class="styled-radio">
                           <input type="radio" class="radio-button" name="radio" value="${element.GrupaID}" id="rad-${element.GrupaID}">
                           <label for="rad-${element.GrupaID}" class="text-primary">${element.NazivGrupe}</label>
                       </div>`);
                $('.radio-button').on('click', function () {
                    $("#pretragaGrupe").val("");
                    $(`.radio-button`).attr('checked', false);
                    $(this).attr('checked', true);
                });
            });


            $('.radio-button').on('change', function () {
                IspisiRadnikeGrupe();
            });
        });
    }




    $(function () {
        $("#pretragaRadnika").on("keyup", function () {
            var value = $('#pretragaRadnika').val().toLowerCase();
            PretragaRadnika(value);
            $('#izabraniRadnici').hide();
        });
    });


    function PretragaRadnika(nazivRadnika) {
        var data = {
            Pretraga: nazivRadnika
        };
        if (data.Pretraga !== "") {
            $.get("/GrupaRadnik1/PretraziRadnike", data, function (result, status) {
                $("#svi-Radnici").empty();
                result.Data.forEach(element => {

                    $("#svi-Radnici").append(
                        `<li class="radnici-kartice col-md-12" style="z-index:9999">
                         <div class = "col-md-12 reset row">
                            <div class = "col-md-2 reset">
                                <img src='https://cdn2.iconfinder.com/data/icons/people-3-2/128/Programmer-Avatar-Backend-Developer-Nerd-512.png' class = "slika-kartica" />
                            </div>                            

                            <div class = "col-md-10 reset" >
                                <span class="ime-i-prezime" id=${element.RadnikID}>${element.Ime + ' ' + element.Prezime}</span><br>
                                ${element.EmailAdresa}
                            </div>
                         </div>
                    </li>`);
                });

                $('#svi-Radnici li').click(function () {
                    $('#izabraniRadnici').show();
                    var data = {
                        RadnikID: $(this).find('span').attr('id'),
                        GrupaID: $(".radio-button:checked").val()
                    }
                    $.get("/GrupaRadnik1/IspisiListu", data, function (result, status) {
                        $("#svi-Radnici").empty();
                        IspisiRadnikeGrupe();

                        $('.close').click(function () {
                            var RadnikID = $(this).find('i').attr('radnikId');
                            var GrupaID = $(".radio-button:checked").val();
                            ObrisiRadnika(RadnikID, GrupaID);
                        });
                    });
                    $(this).show(); // Hide move button
                });
            });
        }
    }

    function ObrisiRadnika(RadnikID, GrupaID) {
        var data = {
            RadnikId: RadnikID,
            GrupaId: GrupaID
        };

        $.get("/GrupaRadnik1/UkloniRadnika", data, function (result, status) {
            IspisiRadnikeGrupe();
        });
    }



    $('input[type="search"]').on('search', function () {
        $("#svi-Radnici").empty();
        $('#izabraniRadnici').show();
    });


    $('.left-grupe').on('click', function () {
        $("#svi-Radnici").empty();
        $('#izabraniRadnici').show();
    });



    $('#pretragaRadnika').on('focus', function () {
        var value = $('#pretragaRadnika').val().toLowerCase();
        PretragaRadnika(value);
        $('#izabraniRadnici').hide();
    });




    $("#btnDodaj").on('click', function () {

        $("#myModal").modal();
        $("#Naziv").val("");
        $("#poruka").html("");
    });

    $(".nekaklasa").on('click', function () {

        var data = {
            naziv: $("#Naziv").val(),
            tip: $(".Statusi").val().replace(' ▽', ''),
            datumKraj: $("#datumKrajaGrupe").val()
        }
        $.get("/GrupaRadnik1/CreateNaziv", data, function (result, status) {
            $("#poruka").html("");
            $("#poruka").html(result.Message);
            if (result.Result == "OK") {
                pretragaGrupa();

                $('#myModal').modal('hide');
            }

        });

    });

    $('.Statusi').on('click', function (e) {
        e.stopPropagation();
        $('.dropStatus').fadeIn();
        $('.dropGrupe').fadeOut();
    });


    $(document).on('click', function () {
        $('.dropStatus').fadeOut();
    });

    $(".dropStatus li").on('click', function () {
        var selText = ($(this).text() + ' ▽');
        $("#dropDownStatusi").data('statusid', this.id);
        $("#dropDownStatusi").val(selText);
    });


    function IspisiRadnikeGrupe() {
        $('#izabraniRadnici').html("");
        var data = {
            GrupaID: $(".radio-button:checked").val()
        }
        $.get("/GrupaRadnik1/IspisiRadnikeGrupe", data, function (result, status) {
            result.Data.forEach(element => {
                $("#izabraniRadnici").append(
                    `<li class="radnici-kartice col-md-12 radnici-u-grupi">
                         <div class = "col-md-12 reset row">
                            <div class = "col-md-2 reset">
                                <img src='https://cdn2.iconfinder.com/data/icons/people-3-2/128/Programmer-Avatar-Backend-Developer-Nerd-512.png' class = "slika-kartica" />
                            </div>                            

                            <div class = "col-md-8 reset" >
                                <span class="ime-i-prezime" id=${element.RadnikID}>${element.Ime + ' ' + element.Prezime}</span><br>
                                ${element.EmailAdresa}
                            </div>

                            <div class = "col-md-2">
                                <button type="button" class="close" >
                                    <i class="fas fa-times delete" radnikId=${element.RadnikID}></i>
                                  </button>
                            </div>
                         </div>
                    </li>`);

                
            });


            $('.close').click(function () {
                var RadnikID = $(this).find('i').attr('radnikId');
                var GrupaID = $(".radio-button:checked").val();
                ObrisiRadnika(RadnikID, GrupaID);
            });

        });

    }

    pretragaGrupa();
});
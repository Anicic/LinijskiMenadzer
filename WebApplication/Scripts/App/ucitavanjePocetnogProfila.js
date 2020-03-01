function ucitajProfilRadnika(radnikID) {

    var data = {
        RadnikID: radnikID,
        Datum: $('#date').val()
    };

    $.get("/Pocetna/VratiPocetnaProfilRadnika", data, function (result, status) {

        $("#pocetnaProfil").html(result);

        $('[data-toggle="popover"]').popover();

        var datum = new Date($("#date").val());

        var podaci = {
            RadnikID: radnikID,
            Mjesec: datum.getMonth() + 1,
            Godina: datum.getFullYear()
        };

        var listaPrisutnosti = [];

        $.get('/Pocetna/GetPrisutnost', podaci, function (result, status) {
            listaPrisutnosti.push(result.NaPoslu);
            listaPrisutnosti.push(result.PrivatnoOdsutan);
            listaPrisutnosti.push(result.VisednevnoOdsutan);
            listaPrisutnosti.push(result.Nepoznato);
            listaPrisutnosti.push(result.PreostaloDana);
            listaPrisutnosti.push(result.SluzbenoOdsutan);


            var ctxD = document.getElementById("prisutnostUMjesecu").getContext('2d');
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
                            fontColor: "#FFF"
                        }
                    },
                    responsive: true,
                    maintainAspectRatio: true,
                }
            });


        });

    });

};


$('#date').change(function () {
    ucitajProfilRadnika(0);
});


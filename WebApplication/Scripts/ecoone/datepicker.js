(function ($) {

    'use strict';

    // ------------------------------------------------------- //
    // Datepicker
    // ------------------------------------------------------ //	
    $(function () {
        //default date range picker
        $('#daterange').daterangepicker({
            autoApply: true
        });

        //date time picker
        $('#datetime').daterangepicker({
            timePicker: true,
            timePickerIncrement: 30,
            locale: {
                format: 'MM/DD/YYYY h:mm A'
            }
        });

        
       

    

        $('#date').daterangepicker({
            singleDatePicker: true
        });

        $('#datumPocetkaGrupe').daterangepicker({
            singleDatePicker: true
        });
        $('#datumKrajaGrupe').daterangepicker({
            singleDatePicker: true
        });
      
    });

})(jQuery);
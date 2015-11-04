$(document).ready(function () {
    // Default settings
    $('.table-stats').responsiveTable();
    // Custom settings
    $('.myTable2').responsiveTable({
        staticColumns: 2,
        scrollRight: true,
        scrollHintEnabled: true,
        scrollHintDuration: 2000
    });
});
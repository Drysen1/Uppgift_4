$(document).ready(function () {
    var $window = $(window);

    function checkScreenSize() {
        var windowsize = $window.width();
        if (windowsize < 1024) {
            alert("Det verkar som om att du använder en mobiltelefon eller en liten surfplatta. \n \n "+
                "Vi rekommenderar att du använder en surfplatta av storlek Ipad liggandes eller en dator för optimal upplevelse av statistiksidan.");
        }
    }

    checkScreenSize();
    $(window).resize(checkScreenSize);
});
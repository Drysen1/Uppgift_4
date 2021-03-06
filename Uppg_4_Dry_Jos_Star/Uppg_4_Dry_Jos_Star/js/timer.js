//timer.js javascript for timer to be used on test page.
//2015-10-28 Erik Drys�n
//Revised 2015-11-02 Erik Drys�n

window.onload = function () {

    //$('.page-overlay').hide();
    if (sessionStorage.getItem("is_reloaded"))
    {
        //alert('Reloaded!');
        $(".timer-holder").css("display", "none");
    }
    else
    {
        //alert('Not reloaded');
        countDown();
    }

    sessionStorage.setItem("is_reloaded", true);
}

function countDown() {
    var countdown = 30 * 60 * 1000;
    var timerId = setInterval(countDownTimer, 1000);

    function countDownTimer() {
        countdown -= 1000;
        var min = Math.floor(countdown / (60 * 1000));
        var sec = Math.floor((countdown - (min * 60 * 1000)) / 1000);
        if (sec < 10) {
            $("#timer").html(min + " : 0" + sec);

        }
        else {
            $("#timer").html(min + " : " + sec);
        }
        if(min > 1 && min < 5)
        if (min < 1) {
            $(".timer-holder").css("background", "red");
        }

        if (countdown <= 0) {
            //alert("30 min!");
            clearInterval(timerId);
            //$('.page-overlay').css("display", "block");
            //$('.page-overlay').hide();
            $('.page-overlay').fadeIn('slow');
        }
    }
}
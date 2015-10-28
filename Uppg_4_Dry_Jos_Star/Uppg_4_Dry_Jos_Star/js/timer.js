//timer.js javascript for timer to be used on test page.
//2015-10-28 Erik Drysén

window.onload = function() {
    var countdown = 2 * 60 * 1000;
    var timerId = setInterval(countDownTimer, 1000);
        $('.page-overlay').hide();   

    function countDownTimer(){
      countdown -= 1000;
      var min = Math.floor(countdown / (60 * 1000));
      var sec = Math.floor((countdown - (min * 60 * 1000)) / 1000);  
      if (sec < 10)
      {
         $("#clock").html(min + " : 0" + sec);

      }
      else
      {
          $("#clock").html(min + " : " + sec);
      }
      if (min < 1)
      {
        $("#clock").css("background", "red");
      }

      if (countdown <= 0) {
         alert("30 min!");
         clearInterval(timerId);
        $('.page-overlay').fadeIn('slow');
      } 
    } 
}
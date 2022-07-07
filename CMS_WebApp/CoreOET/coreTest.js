// mozfullscreenerror event handler
function errorHandler() {
    alert('mozfullscreenerror');
}
document.documentElement.addEventListener('mozfullscreenerror', errorHandler, false);

// toggle full screen
function toggleFullScreen() {
    if (!document.fullscreenElement &&    // alternative standard method
        !document.mozFullScreenElement && !document.webkitFullscreenElement) {  // current working methods
        if (document.documentElement.requestFullscreen) {
            document.documentElement.requestFullscreen();
        } else if (document.documentElement.mozRequestFullScreen) {
            document.documentElement.mozRequestFullScreen();
        } else if (document.documentElement.webkitRequestFullscreen) {
            document.documentElement.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        }
    }
}
function getTimeRemaining(endtime) {
    var t = Date.parse(endtime) - Date.parse(new Date());
    var seconds = Math.floor((t / 1000) % 60);
    var minutes = Math.floor((t / 1000 / 60) % 60);
    var hours = Math.floor((t / (1000 * 60 * 60)) % 24);
    var days = Math.floor(t / (1000 * 60 * 60 * 24));
    return {
        'total': t,
        'days': days,
        'hours': hours,
        'minutes': minutes,
        'seconds': seconds
    };
}

function initializeClock(id, endtime) {
    var clock = document.getElementById(id);
    var daysSpan = clock.querySelector('.days');
    var hoursSpan = clock.querySelector('.hours');
    var minutesSpan = clock.querySelector('.minutes');
    var secondsSpan = clock.querySelector('.seconds');

    function updateClock() {
        var t = getTimeRemaining(endtime);        
       // daysSpan.innerHTML = t.days;
        hoursSpan.innerHTML = ('0' + t.hours).slice(-2);
        minutesSpan.innerHTML = ('0' + t.minutes).slice(-2);
        secondsSpan.innerHTML = ('0' + t.seconds).slice(-2);
        if (t.total <= 0) {
            clearInterval(timeinterval);
            $('.testContent').hide();
            DumpData(true);
        }
    }

    updateClock();
    var timeinterval = setInterval(updateClock, 1000);
}
function HideTabs() {
    $('#paperTesting a').hide();
}
function ShowTabs() {
    $('#paperTesting a').show();
}
function HideAudio(controlID)
{
    var audio = $('#' + controlID);
    audio.removeAttr("controls");
    $("#" + controlID).prop("disabled", true);
    $('#l' + controlID).html('You have played it once.');
    $('#a' + controlID).attr('onclick', 'alert("You have played it once. You cannot play it again.");');
    $('#a' + controlID).css('background-color', '#EC7063');
}
//lock the audio for once play
function playAudio(controlID)
{
    var result_s = new TestCollection();
    if (result_s.IsListened(controlID))
    {
        HideAudio(controlID);
        $('#a' + controlID).hide();
        alert("You have played it once. You cannot play it again.");        
        return;
    }
    HideTabs();
    document.getElementById(controlID).play();
    HideAudio(controlID);
    result_s.Listened(controlID);
    /*if (control.hasAttribute("controls")) {
        control.removeAttribute("controls")
    } else {
        control.setAttribute("controls", "controls")
    }*/
    var audio = $('#' + controlID);
    audio.on('ended', function () {
        //hide the control
        //var results = new TestCollection();        
        $('#a' + controlID).hide();
        ShowTabs();        
    });
}
function Security() {
    //lock right click
    document.addEventListener('contextmenu', function (e) {
        e.preventDefault();
    });
    //forcing no close the window
    //Full screen
    toggleFullScreen();
    //toggleFullScreen();
}
//startDate
//var newDate = new Date(startDate);
var deadline = new Date(Date.parse(endDate));
initializeClock('clockdiv', deadline);
Security();
function InitSecs() {
    var tc = new TestCollection();
    tc.ParseResult(isListen);
    //auto save
    setInterval(function () { tc.SetCache(tc.Answer(), isListen); }, 2000);
    Security();
}
InitSecs();
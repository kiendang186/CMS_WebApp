function setCookie(cname, cvalue, exdays) {
    var d = new Date();
    d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));
    var expires = "expires=" + d.toUTCString();
    document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
}
function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function Question(questionid)
{
    this.QuestionID = questionid;
    this.Answer = function () {
        var selectedVal = $('input[name=ops_' + this.QuestionID.toString() + ']:checked').val();
        if (selectedVal!=null)
            return selectedVal;
        return '';
    };
    this.setState = function (state)
    {
        var div = $('#question' + this.QuestionID);
        if(div.length>0)
        {
            if (state == 1) {
                div.attr('style', 'background-color:#FAFAD2;');
            }
            else {
                div.attr('style', '');
            }
        }
    };
}
function TestCollection()
{
    this.IsListened = function (controlID) {
        var ck = getCookie("listenC");
        if (ck.indexOf(controlID) > -1) return true;
        return false;
    };
    this.Listened = function (controlID) {
        var ck = getCookie("listenC");
        if (ck == null)
        {
            ck = '';
        }
        setCookie("listenC", ck+'#'+controlID, { expires: 10 });//10 days
    };
    this.ClearListened = function () {
        setCookie("listenC", '', { expires: 10 });//10 days
    };
    this.CheckEmpty = function () {
        var questions = $('.question');
        var answer = '';
        var result = false;
        if (questions.length > 0) {
            for (var i = 0; i < questions.length; i++) {
                var obj = $(questions[i]).attr('data-id');
                var qobj = new Question(obj);
                if(qobj.Answer()=='')
                {
                    result = true;
                    qobj.setState(1);
                }
                else {
                    qobj.setState(0);
                }
            }
        }
        return result;
    };
    this.Answer = function () {
        var questions = $('.question');
        var answer = '';
        if(questions.length>0)
        {
            for(var i=0;i<questions.length;i++)
            {
                var obj = $(questions[i]).attr('data-id');
                var qobj = new Question(obj);
                answer += obj + '!' + qobj.Answer() + '|';
            }
        }
        return answer;

    };
    this.SetCache=function(answers,isListening)
    {
        if (isListening)
            setCookie("answersL", answers, { expires: 10 });//10 days
        else
            setCookie("answersR", answers, { expires: 10 });
    }
    this.PullCache = function (isListening) {
        if (isListening)
            return getCookie("answersL");
        else
            return getCookie("answersR");
    }
    this.ParseResult = function (isListening)
    {
        var cache = cache = this.PullCache(isListening);
        var qs = cache.split('|');
        if(qs.length>0)
        {
            for(var i=0;i<qs.length;i++)
            {
                var elem = qs[i].split('!');
                if (elem.length > 0) {
                    if (elem[0].length > 0 && elem[1].length > 0)
                        $("input[name=ops_" + elem[0] + "][value=" + elem[1] + "]").attr('checked', 'checked');
                }
            }
        }
    }
}
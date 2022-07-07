var absolute_URL = 'http://oets.com/Archived/';
var container = 'imgs';
function ImageTag(imageName)
{
    var html = '<div class="imgTag" title="' + imageName + '" ondblclick="AddImg2Editor(\'' + absolute_URL + imageName + '\')">';
    html += '<img src="'+absolute_URL+imageName+'">';
    html += '</div>';
    return html;
}
function LoadImages(images)
{
    if (images === null) return;
    var html = '';
    var s = images.split(';');
    for(var i=0; i<s.length; i++)
    {
        var t = s[i];
        if(t.length>0)
        {
            html += ImageTag(t);
        }

        if ((i+1) % 5 === 0) {
            html += "<div class='clear'></div>";
        }
    }
    $('#' + container).html(html);
}
function initImg()
{
    LoadImages(imgs);
}
function AddImg2Editor(url)
{
    window.opener.InsertImg(url, 'content');
}
initImg();
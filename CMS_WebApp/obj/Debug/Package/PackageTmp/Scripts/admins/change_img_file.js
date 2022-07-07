function onFileChange(f) {
    if (f.files && f.files[0]) {
        var file = f.files[0];
        var fileType = file["type"];
        var validImageTypes = ["image/gif", "image/jpeg", "image/png"];
        if ($.inArray(fileType, validImageTypes) >= 0) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $("#photoPreview").attr("src", e.target.result);
            };
            reader.readAsDataURL(f.files[0]);
        }
        else {
            $("#photoPreview").attr("src", '/Areas/ad/Upload/no-image.png');
        }        
    }
    else {
        $("#photoPreview").attr("src", '/Areas/ad/Upload/no-image.png');
    }
}
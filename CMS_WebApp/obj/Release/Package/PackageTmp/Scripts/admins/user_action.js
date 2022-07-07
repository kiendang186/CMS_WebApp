$('#UploadFile').change(function () {
    onFileChange(this);
});

function onFileChange(f) {
    if (f.files && f.files[0]) {
        var file = f.files[0];
        var fileType = file.type;//file["type"];
        var fileSize = Math.round((file.size / 1024));
        var validImageTypes = ["image/gif", "image/jpeg", "image/png"];
        if ($.inArray(fileType, validImageTypes) >= 0) {
            if (fileSize <= 1024) {
                $('#file-msg').html('').attr('data-error', '');
                var reader = new FileReader();
                reader.onload = function (e) {
                    $("#photoPreview").attr("src", e.target.result);
                };
                reader.readAsDataURL(f.files[0]);
            }
            else {
                $('#file-msg').html('Dung lượng file không lớn hơn 1MB').attr('data-error', 'error');
                $("#photoPreview").attr("src", $("#photoPreview").attr('data-path'));
                $('#UploadFile').val('');
            }
        }
        else {
            $('#file-msg').html('Vui lòng chọn file ảnh').attr('data-error', 'error');
            $("#photoPreview").attr("src", $("#photoPreview").attr('data-path'));
            $('#UploadFile').val('');
        }
    }   
}
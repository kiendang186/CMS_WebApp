$('#fileimage').change(function () {
    if (this.files && this.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $("#photoPreview").attr("src", e.target.result);
        };
        reader.readAsDataURL(this.files[0]);
    }
    else {
        if ($('#empId').length) {
            $("#photoPreview").attr("src", $('#empImgProfile').val());
        }
        else {
            $("#photoPreview").attr("src", '/Area/ad/Upload/img-emp/default-user.png');
        }
    }
});
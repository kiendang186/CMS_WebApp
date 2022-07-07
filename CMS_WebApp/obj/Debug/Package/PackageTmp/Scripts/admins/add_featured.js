$(function () {

    $.validator.setDefaults({
        submitHandler: function () {
            callAddPost();
        }
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param * 1000000)
    }, 'Dung lượng file không được quá 1 MB');

    $.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
    }, "Please enter a value with a valid extension.");

    $('#add-fi-form').validate({
        rules: {
            title: {
                required: true,
                maxlength: 300
            },
            desc: {
                required: true,
                maxlength: 500
            },
            imgfile: {
                required: true,
                extension: "png|jpg|jpeg",
                filesize: 1
            }
        },
        messages: {
            title: {
                required: "Vui lòng cung cấp tiêu đề tin",
                maxlength: "Tiêu đề chấp nhận tối đa 300 ký tự"
            },
            desc: {
                required: "Vui lòng cung cấp nội dung tin",
                maxlength: "Nội dung chấp nhận tối đa 500 ký tự"
            },
            imgfile: {
                required: "Vui lòng cung cấp ảnh đại diện cho thông tin",
                extension: "Vui lòng chỉ chọn file ảnh"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('text-danger');
            element.closest('.form-group').append(error);
        },
        highlight: function (element, errorClass, validClass) {
            $(element).addClass('is-invalid');
        },
        unhighlight: function (element, errorClass, validClass) {
            $(element).removeClass('is-invalid');
        }
    });

    // closing error dialog
    $('#btn-error-close').click(function () {
        window.location.href = '/ad/featured';
    });
});

function callAddPost() {
    if (window.FormData !== undefined) {
        var title = $('#title').val();
        var desc = $('#desc').val();
        var url = '#';
        if ($('#url').val().length > 0)
            url = $('#url').val();
        var enable = $('input[name="enable"]:checked').val();
        var file = $('#imgfile')[0].files[0];

        var formData = new FormData();
        formData.append("Id", 0);
        formData.append("Title", title);
        formData.append("Description", desc);
        formData.append("URL", url);
        formData.append("Enable", enable);
        formData.append("UploadFile", file);

        $.ajax({
            url: '/ad/featured/addfeatured',
            type: "POST",
            contentType: false, 
            processData: false, 
            data: formData,
            success: function (data) {
                if (data.status === true) {
                    window.location.href = '/ad/featured';
                }
                else {
                    showErrorDialog();
                }
            },
            error: function (xhr, status, error) {
                showErrorDialog();
            }
        });
    }
    else {
        showInfoDialog();
    }
}

function showErrorDialog() {
    $('#modal-default-error').modal({
        backdrop: 'static',
        keyboard: false
    });
}

function showInfoDialog() {
    $('#message').html('Chức năng không thể thực hiện vì dạng thức dữ liệu không được hỗ trợ');
    $('#modal-info').modal();
}
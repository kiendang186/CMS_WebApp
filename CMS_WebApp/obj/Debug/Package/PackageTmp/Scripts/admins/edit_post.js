$(function () {

    $.validator.setDefaults({
        submitHandler: function () {
            callEditPost();
        }
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param * 1048576)
    }, 'Dung lượng file không được quá 1 MB');   

    $('#edit-post-form').validate({
        rules: {
            title: {
                required: true,
                maxlength: 200
            },
            excerpt: {
                required: true,
                maxlength: 500
            },
            imgfile: {                
                extension: "png|jpg|jpeg|gif",
                filesize: 1
            },
            attachfile: {                
                extension: "doc|docx|xls|xlsx|ppt|pptx|rar|zip|rtf|png|jpg|jpeg|gif|pdf",
                filesize: 10
            }
        },
        messages: {
            title: {
                required: "Vui lòng cung cấp tên bài viết",
                maxlength: "Tên bài viết chấp nhận tối đa 300 ký tự"
            },
            excerpt: {
                required: "Vui lòng cung cấp nội dung tóm tắt",
                maxlength: "Tóm tắt chấp nhận tối đa 500 ký tự"
            },
            imgfile: {                
                extension: "Vui lòng chỉ chọn file ảnh",                
            },
            attachfile: {
                extension: "Hệ thống không hỗ trợ dạng file này",
                filesize: "Dung lượng file không quá 10MB"
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
        window.location.href = '/ad/post';
    });
});

function callEditPost() {
    if (window.FormData !== undefined) { 
        var enable = $('input[name="enable"]:checked').val();
        var imgfile = $('#imgfile')[0].files[0];
        var attfile = $('#attachfile')[0].files[0];

        var formData = new FormData();
        formData.append("Id", $('#postId').val());
        formData.append("Title", $('#title').val());
        formData.append("Excerpt", $('#excerpt').val());
        formData.append("Content", tinyMCE.get('content').getContent());
        formData.append("Enable", enable);
        formData.append("ImageFile", imgfile);
        formData.append("CategoryId", $('#category').val());
        formData.append("AttachmentFile", attfile);

        $.ajax({
            url: '/ad/post/editpost',
            type: "POST",
            contentType: false, 
            processData: false, 
            data: formData,
            success: function (data) {
                if (data.status === true) {
                    window.location.href = '/ad/post';
                }
                else {
                    showErrorDialog();
                }
            },
            error: function (xhr, status, error) {
                console.log(xhr.responseText);
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
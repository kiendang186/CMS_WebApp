var absolute_URL = 'http://demo.dthu.edu.vn/Archived/';
var gallery_container = 'gallery-container';
tinymce.init({
    selector: '#content',
    height: 300,
    theme: 'modern',
    plugins: 'print preview searchreplace autolink directionality visualblocks visualchars fullscreen link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern',
    toolbar1: 'csimage | formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
    image_advtab: true,    
    content_css: [
        '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
        '//www.tinymce.com/css/codepen.min.css'
    ],

    setup: function (editor) {
        editor.addButton('csimage', {
            text: 'Thêm hình',
            icon: false,
            onclick: function () {
                ShowImgMnt(true);
            }
        });
    }
});

function ShowImgMnt(isShowDialog) { 
    $.ajax({
        type: 'GET',
        url: '/ad/editor/imagemnt',
        dataType: 'json',
        success: function (images) {            
            LoadImages(images);
            if(isShowDialog)
                showGalleryDialog();
        },
        error: function (xhr, status, error) {
            window.location.href = '/error';
        }
    });
}

function ImageTag(imageName) {
    var html = '<div class="col-md-2"><div class="thumbnail" title="' + imageName + '" ondblclick="AddImg2Editor(\'' + absolute_URL + imageName + '\')"><img src="/Archived/' + imageName + '" alt="' + imageName +'" style="width:100%"></div></div>';    
    return html;
}

function LoadImages(images) {    
    if (images === null) {
        $('#' + gallery_container).html('Chưa có hình ảnh');
    }
    else {
        var html = '';
        var s = images.split(';');
        var isNewLine = true;
        var i = 0;
        for (i = 0; i < s.length; i++) {
            if (isNewLine) {
                html += '<div class="row">';
            }
            var t = s[i];
            if (t.length > 0) {
                html += ImageTag(t);
            }

            if ((i + 1) % 6 === 0) {
                html += "</div>";
                isNewLine = true;
            }
            else {
                isNewLine = false;
            }
        }

        if (i < 6) {
            html += '</div>';
        }

        $('#' + gallery_container).html(html);
    }
}

function showGalleryDialog() {
    $('#uploading-container').hide();
    $('#modal-gallery').modal({
        backdrop: 'static',
        keyboard: false
    });
}

function AddImg2Editor(url) {
    InsertImg(url, 'content');
    $('#modal-gallery').modal('hide');
}

function InsertImg(url, editorID) {
    var ed = tinyMCE.get(editorID);                // get editor instance
    var range = ed.selection.getRng();                  // get range
    var newNode = ed.getDoc().createElement("img");  // create img node
    newNode.src = url;                           // add src attribute
    range.insertNode(newNode);                          // insert Node
}

//============ upload file handle ==============
$(function () {
    // submit form
    $.validator.setDefaults({
        submitHandler: function () {
            uploadFile();
        }
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param * 1000000)
    }, 'Dung lượng file không được quá 1 MB');

    $.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, '|') : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp(".(" + param + ")$", "i"));
    }, "Please enter a value with a valid extension.");

    $('#form-gallery').validate({
        rules: {            
            uploadFile: {
                required: true,
                extension: "png|jpg|jpeg",
                filesize: 1
            }
        },
        messages: {            
            uploadFile: {
                required: "Vui lòng chọn một ảnh để tải lên",
                extension: "Hệ thống chỉ hỗ trợ tải lên dạng file ảnh"
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
})

function uploadFile() {
    if (window.FormData !== undefined) {
        var file = $('#uploadFile')[0].files[0];
        var formData = new FormData();        
        formData.append("file", file);

        $('#uploading-container').show();
        $('#div-progress').show();
        $('#uploading-text').html('Đang tải ảnh...');

        $.ajax({
            url: '/ad/editor/uploadfile',
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (result) {
                if (result === true) {
                    setTimeout(function () {
                        $('#uploading-container').hide();
                        ShowImgMnt(false);
                    }, 3000);                    
                }
                else {
                    $('#div-progress').hide();
                    $('#uploading-text').html('Không tải ảnh bạn vừa chọn lên hệ thống');
                }
            },
            error: function (xhr, status, error) {
                $('#div-progress').hide();
                $('#uploading-text').html('Có lỗi xảy ra khi thực hiện tải ảnh bạn vừa chọn lên hệ thống');
            }
        });
        
    }
    else {
        $('#uploading-container').show();
        $('#div-progress').hide();
        $('#uploading-text').html('Chức năng không thể thực hiện vì có vấn đề xảy ra');
    }
}
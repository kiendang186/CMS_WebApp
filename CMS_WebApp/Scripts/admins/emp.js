$(document).ready(function () {

    $('#dtp').datetimepicker({
        format: 'DD/MM/YYYY'
    });

    tinymce.init({
        selector: '#biography',
        height: 300,
        theme: 'modern',
        plugins: 'print preview searchreplace autolink directionality visualblocks visualchars fullscreen link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists textcolor wordcount imagetools contextmenu colorpicker textpattern',
        toolbar1: 'csimage | formatselect | bold italic strikethrough forecolor backcolor | link | alignleft aligncenter alignright alignjustify  | numlist bullist outdent indent  | removeformat',
        image_advtab: true,
        content_css: [
            '//fonts.googleapis.com/css?family=Lato:300,300i,400,400i',
            '//www.tinymce.com/css/codepen.min.css'
        ]
    });    

    $.validator.setDefaults({
        submitHandler: function () {
            doEmp();
        }
    });

    $.validator.addMethod('filesize', function (value, element, param) {
        return this.optional(element) || (element.files[0].size <= param * 1048576)
    }, 'Dung lượng file không được quá 1 MB');

    if ($('#empId').length) {
        editValidation();
    }
    else {
        addValidation();
    }

    // closing error dialog
    $('#btn-error-close').click(function () {
        window.location.href = '/ad/employee';
    });

});

function addValidation() {
    $('#frm-emp').validate({
        rules: {
            fullname: {
                required: true,
                maxlength: 100
            },
            position: {
                required: true,
                maxlength: 100
            },
            degree: {
                required: true,
                maxlength: 50
            },
            email: {
                required: true,
                maxlength: 50,
                email: true
            },
            dob: {
                required: true
            },
            fileimage: {
                required: true,
                extension: "png|jpg|jpeg|gif",
                filesize: 1
            }
        },
        messages: {
            fullname: {
                required: "Vui lòng nhập họ tên",
                maxlength: "Họ tên chấp nhận tối đa 100 ký tự"
            },
            position: {
                required: "Vui lòng nhập chức vụ",
                maxlength: "Chức vụ chấp nhận tối đa 100 ký tự"
            },
            degree: {
                required: "Vui lòng nhập học vị",
                maxlength: "Học vị chấp nhận tối đa 50 ký tự"
            },
            email: {
                required: "Vui lòng nhập email",
                maxlength: "Email chấp nhận tối đa 50 ký tự",
                email: "Vui lòng nhập một email hợp lệ"
            },
            dob: {
                required: "Vui lòng cung cấp ngày sinh"
            },
            fileimage: {
                required: "Vui lòng cung cấp ảnh đại diện",
                extension: "Vui lòng chỉ chọn file ảnh",
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
}

function editValidation() {
    $('#frm-emp').validate({
        rules: {
            fullname: {
                required: true,
                maxlength: 100
            },
            position: {
                required: true,
                maxlength: 100
            },
            degree: {
                required: true,
                maxlength: 50
            },
            email: {
                required: true,
                maxlength: 50,
                email: true
            },
            dob: {
                required: true
            },
            fileimage: {                
                extension: "png|jpg|jpeg|gif",
                filesize: 1
            }
        },
        messages: {
            fullname: {
                required: "Vui lòng nhập họ tên",
                maxlength: "Họ tên chấp nhận tối đa 100 ký tự"
            },
            position: {
                required: "Vui lòng nhập chức vụ",
                maxlength: "Chức vụ chấp nhận tối đa 100 ký tự"
            },
            degree: {
                required: "Vui lòng nhập học vị",
                maxlength: "Học vị chấp nhận tối đa 50 ký tự"
            },
            email: {
                required: "Vui lòng nhập email",
                maxlength: "Email chấp nhận tối đa 50 ký tự",
                email: "Vui lòng nhập một email hợp lệ"
            },
            dob: {
                required: "Vui lòng cung cấp ngày sinh"
            },
            fileimage: {                
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
}

function doEmp() {

    var id = 0;
    if ($('#empId').length) {
        id = $('#empId').val();
    }

    if (window.FormData !== undefined) {        
        var imgfile = $('#fileimage')[0].files[0];       

        var formData = new FormData();
        formData.append("Id", id);
        formData.append("FullName", $('#fullname').val());
        formData.append("Position", $('#position').val());
        formData.append("Degree", $('#degree').val());
        formData.append("DOB", $('#dob').val());
        formData.append("POB", $('#pob').val());
        formData.append("Email", $('#email').val());
        formData.append("Biography", tinyMCE.get('biography').getContent());        
        formData.append("FileImage", imgfile);

        $.ajax({
            url: '/ad/employee/doemp',
            type: "POST",
            contentType: false,
            processData: false,
            data: formData,
            success: function (data) {
                if (data.act == 0) {
                    if (data.status === true) {
                        sessionStorage.isShowAddMsg = true;
                        sessionStorage.addMsg = data.message;

                        sessionStorage.isShowEditMsg = false;
                        sessionStorage.editMsg ='';

                        sessionStorage.isShowDelMsg = false;
                        sessionStorage.delMsg = '';

                        window.location.href = '/ad/employee';
                    }
                } else if (data.act == 1) {
                    if (data.status === true) {
                        sessionStorage.isShowEditMsg = true;
                        sessionStorage.editMsg = data.message;

                        sessionStorage.isShowAddMsg = false;
                        sessionStorage.addMsg = '';

                        sessionStorage.isShowDelMsg = false;
                        sessionStorage.delMsg = '';

                        window.location.href = '/ad/employee';
                    } else {
                        showNofityMessage(data.message);
                    }
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

function showConfimDialog(button, id) {
    var firstCell = $(button).closest('tr').find('td:first');
    var strongTag = $(firstCell).find('strong')[0];
    var name = $(strongTag).text();
    $("#employee_name").html(name);
    $(".btnOK").bind("click", function () {
        deleteEmp(id);
    });

    $("#delModal").modal("show");
}

function deleteEmp(id) {
    $.ajax({
        type: 'POST',
        url: "/ad/employee/deleteemp",
        data: { id: id },
        success: function (data) {
            $("#delModal").modal("hide");
            if (data.result === 1) {
                window.location.href = '/ad/employee';             
            } else if (data.result === 0) {
                showNofityMessage(data.responseText);
            } else if (data.result === -1) {
                showNofityMessage(data.responseText);
            }
        },
        error: function (jqXHR, exception) {
            $("#delModal").modal("hide");
            showErrorDialog();
        }
    });
}

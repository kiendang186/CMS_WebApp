$(function () {

    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');
    });

    $.validator.setDefaults({
        submitHandler: function () {
            callAddMenu();
        }
    });

    $('#menu_form').validate({
        rules: {
            title: {
                required: true,
                maxlength: 200
            }
        },
        messages: {
            title: {
                required: "Vui lòng cung cấp tên menu",
                maxlength: "Tên menu chấp nhận tối đa 200 ký tự"
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

function callAddMenu() {
    var title = $('#title').val();
    var url = $("#url").val();
    var status = $('#status').is(':checked');

    var menu = {
        Id: 0,
        Title: title,
        URL: url,
        IsShow: status,
        Parent: 0
    };

    $.ajax({
        url: '/ad/menu/addmenu',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(menu),
        success: function (response) {
            clearForm();            
            showNofityMessage(response.message);
            setTimeout(function () {
                window.location.href = "/ad/menu";
            }, 700);            
        },
        error: function (xhr, status, error) {
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });            
        }
    });
}

function clearForm() {
    $('#menu_form').find('input[type=text]').val('');
}

function showConfimDialog(button, id) {
    var menu_name = $(button).closest('tr').find('td:first').text();
    $("#str-menuid").html(menu_name);
    $(".btnOK").bind("click", function () {
        DeleteMenu(id);
    });

    $("#delMenuModal").modal("show");
}

function DeleteMenu(mid) {
    $.ajax({
        type: "POST",
        url: "/ad/menu/deletemenu",
        data: { id: mid },
        success: function (data) {
            $("#delMenuModal").modal("hide");
            if (data.result === 1) {                
                showNofityMessage(data.responseText);
                setTimeout(function () {
                    window.location.href = "/ad/menu";
                }, 700);
            } else if (data.result === 0) {
                showNofityMessage(data.responseText);
            } else if (data.result === -1) {
                showNofityMessage(data.responseText);
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.responseText);
            $("#delMenuModal").modal("hide");
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });            
        }
    });
}
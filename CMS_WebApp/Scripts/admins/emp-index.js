$(document).ready(function () {
    
    if (sessionStorage.isShowAddMsg) {
        sessionStorage.isShowAddMsg = false;
        showNofityMessage(sessionStorage.addMsg);
        sessionStorage.addMsg = '';
    }

    if (sessionStorage.isShowEditMsg) {
        sessionStorage.isShowEditMsg = false;
        showNofityMessage(sessionStorage.editMsg);
        sessionStorage.editMsg = '';
    }

    if (sessionStorage.isShowDelMsg) {
        sessionStorage.isShowDelMsg = false;
        showNofityMessage(sessionStorage.delMsg);
        sessionStorage.delMsg = '';
    }

    // closing error dialog
    $('#btn-error-close').click(function () {
        window.location.href = '/ad/employee';
    });

    $('#btnSaveOrder').click(function () {
        saveOrder();
    });

});


function saveOrder() {
    var orderList = [];
    var count = 0;
    $.each($('input[name=order]'), function (i, e) {
        if ($(e).val().length > 0) {
            orderList.push({
                Id: $(e).attr('data-id'),
                Order: $(e).val()
            });
        }
        else {
            count++;
        }
    });

    if (count > 0) {
        $('#message').text('Thứ tự không được để trống. Vui lòng kiểm tra trước khi tiếp tục');
    }
    else {
        $('#message').text('');
        $.ajax({
            type: 'POST',
            url: "/ad/employee/updateorder",
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            data: JSON.stringify({ 'orders': orderList }),
            success: function (data) {
                if (data.status) {
                    showNofityMessage(data.message);
                    setTimeout(function () {
                        window.location.href = '/ad/employee';
                    }, 700);
                }
            },
            error: function (jqXHR, exception) {                
                showErrorDialog();
            }
        });
    }
}

function showErrorDialog() {
    $('#modal-default-error').modal({
        backdrop: 'static',
        keyboard: false
    });
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
                sessionStorage.isShowDelMsg = true;
                sessionStorage.delMsg = data.responseText;

                sessionStorage.isShowAddMsg = false;
                sessionStorage.addMsg = '';

                sessionStorage.isShowEditMsg = false;
                sessionStorage.editMsg = '';

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

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

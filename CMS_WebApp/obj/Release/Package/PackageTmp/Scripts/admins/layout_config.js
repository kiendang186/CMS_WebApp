$(document).ready(function () {
    $('#btnSave').click(function () {
        if (!validateOnSubmit()) {
            saveChanges();
        }
    });    
});

function validateOnSubmit() {
    var count = 0;
    $.each($('[id^=num_item]'), function (i, e) {
        if ($(e).val().length <= 0) {
            var id = $(e).attr('id');
            id = id.substr(id.lastIndexOf('_') + 1);
            $('#msg-error-' + id).html('Vui lòng nhập số lượng');
            count++;
        }
    });

    return (count > 0);
}

function clearErrors() {
    $('[id^=msg-error]').text('');
}

function validate(input, id) {
    if ($(input).val().length <= 0) {
        $('#msg-error-' + id).text('Vui lòng nhập số lượng');
    }
    else {
        $('#msg-error-' + id).text('');
    }
}

function saveChanges() {
    var configs = [];
    $.each($('#tblHomeView tr'), function (i, e) {
        var tdFirst = $(e).find('td')[0];
        var id = $(tdFirst).text();

        configs.push({
            Id: id,
            ViewName: '',
            Category: $('#category-' + id).val(),
            Items: $('#num_item_' + id).val()
        });
    });

    $.ajax({
        url: '/ad/Config/Layout',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(configs),
        success: function (response) {
            clearErrors();
            showNofityMessage(response.message);
        },
        error: function (xhr, status, error) {
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });            
        }
    });
}
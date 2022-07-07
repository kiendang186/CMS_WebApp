$(function () {

    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');            
    });

    $.validator.setDefaults({
        submitHandler: function () {
            callAddLocation();
        }
    });

    $('#location_form').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100
            }
        },
        messages: {
            name: {
                required: "Vui lòng cung cấp tên địa điểm/dãy nhà học",
                maxlength: "Vui lòng nhập không quá 100 ký tự"
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

var table = $("#tblLocation").DataTable({
    "ajax": {
        "url": "/ad/location/GetLocationList",
        "type" : "GET",
        "datatype": "json",
        "error": function (xhr) {
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
    },
    "columns": [       
        { "data": "Name", "width" : "30%" },
        { "data": "Description", "width": "auto" },        
        { "data": "Id", "width": "20%" }
    ],
    "columnDefs": [        
        {
            "targets": 2,
            "data": "Id",
            "className" : "text-center",
            "render": function (data, type, row, meta) {
                return "<a data-toggle='tooltip' data-placement='top' title='Sửa' class='btn btn-primary btn-sm' href='location/editlocation/" + data + "'><i class='fa fa-pencil-square-o'></i></a>&nbsp;<button data-toggle='tooltip' data-placement='top' title='Xóa' class='btn btn-sm btn-danger' onclick='showConfimDialog(this, \"" + data + "\")'><i class='fa fa-trash-o'></i></button>";
            },
            "orderable" : false
            
        }
    ],
    "language": {
        "emptyTable": "Không có dữ liệu, bạn có thể chọn <strong>Thêm địa điểm</strong> để đưa dữ liệu vào hệ thống",
        "lengthMenu": "Hiển thị _MENU_ mẫu tin",
        "info": "Trang _PAGE_/_PAGES_",
        "infoEmpty": "",
        "search": "Tìm kiếm",
        "paginate": {
            first: "Trang đầu",
            previous: "Trang trước",
            next: "Trang sau",
            last: "Trang cuối"
        }
    }
});

function callAddLocation() {
    var name = $('#name').val();
    var desc = $("#desc").val();

    var loc = {
        Id: 0,
        Name: name,
        Description: desc
    };

    $.ajax({        
        url: '/ad/location/addlocation',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(loc),
        success: function (response) {
            clearForm();
            table.ajax.reload();
            showNofityMessage(response.message)
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
    $('#location_form').find('input[type=text],textarea').val('');
}

function showConfimDialog(button, id) {
    var name = $(button).closest('tr').find('td:first').text();    
    $("#str_loc_name").html(name);
    $(".btnOK").bind("click", function () {
        DeleteLocation(id);
    });

    $("#delLocationModal").modal("show");
}

function DeleteLocation(lid) {
    $.ajax({
        type: "POST",
        url: "/ad/location/deletelocation",
        data: { id: lid },
        success: function (data) {
            $("#delLocationModal").modal("hide");
            if (data.result === 1) {
                table.ajax.reload();
                showNofityMessage(data.responseText);
            } else if (data.result === 0) {
                showNofityMessage(data.responseText);
            } else if (data.result === -1) {
                showNofityMessage(data.responseText);
            }
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.responseText);
            $("#delLocationModal").modal("hide");
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });            
        }
    });
}
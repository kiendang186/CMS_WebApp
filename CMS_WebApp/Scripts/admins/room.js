$(function () {

    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');            
    });

    $.validator.setDefaults({
        submitHandler: function () {
            callAddRoom();
        }
    });

    $('#room_form').validate({
        rules: {
            name: {
                required: true,
                maxlength: 100
            }
        },
        messages: {
            name: {
                required: "Vui lòng cung cấp tên phòng học/thực hành",
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

var table = $("#tblRoom").DataTable({
    "ajax": {
        "url": "/ad/room/GetRoomList",
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
        { "data": "Name", "width": "20%" },
        { "data": "Status", "width": "15%" },
        { "data": "LocationName", "width": "15%" },
        { "data": "Description", "width": "30%" },        
        { "data": "Id", "width": "20%" }
    ],
    "columnDefs": [
        {
            "targets": 1,
            "data": "Status",
            "className": "text-center",
            "render": function (data, type, row, meta) {
                var status;
                if (data === true) {
                    status = "<p data-toggle='tooltip' data-placement='top' title='Được sử dụng' class='text-primary'><i class='fa fa-fw fa-check-square'></i></p>";
                }
                else {
                    status = "<p data-toggle='tooltip' data-placement='top' title='Không thể sử dụng'><i class='fa fa-fw fa-lock'></i></p>";
                }

                return status;
            },
            "orderable": false

        },
        {
            "targets": 4,
            "data": "Id",
            "className" : "text-center",
            "render": function (data, type, row, meta) {
                return "<a data-toggle='tooltip' data-placement='top' title='Sửa' class='btn btn-primary btn-sm' href='room/editroom/" + data + "'><i class='fa fa-pencil-square-o'></i></a>&nbsp;<button data-toggle='tooltip' data-placement='top' title='Xóa' class='btn btn-sm btn-danger' onclick='showConfimDialog(this, \"" + data + "\")'><i class='fa fa-trash-o'></i></button>";
            },
            "orderable" : false
            
        }
    ],
    "language": {
        "emptyTable": "Không có dữ liệu, bạn có thể chọn <strong>Thêm phòng</strong> để đưa dữ liệu vào hệ thống",
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

function callAddRoom() {
    var name = $('#name').val();
    var desc = $("#desc").val();
    var locationid = $('#location').val();
    var status = $('#status').is(':checked');

    var room = {
        Id: 0,
        Name: name,
        Status: status,
        Description: desc,
        LocationId: locationid
    };

    $.ajax({        
        url: '/ad/room/addroom',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(room),
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
    $('#room_form').find('input[type=text],textarea').val('');
}

function showConfimDialog(button, id) {
    var name = $(button).closest('tr').find('td:first').text();    
    $("#room_name").html(name);
    $(".btnOK").bind("click", function () {
        DeleteRoom(id);
    });

    $("#delRoomModal").modal("show");
}

function DeleteRoom(lid) {
    $.ajax({
        type: "POST",
        url: "/ad/room/deleteroom",
        data: { id: lid },
        success: function (data) {
            $("#delRoomModal").modal("hide");
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
            $("#delRoomModal").modal("hide");
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });            
        }
    });
}
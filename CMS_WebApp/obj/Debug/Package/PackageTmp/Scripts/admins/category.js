var ERROR_SRC = 0; //0: OTHERS; 1: LOADING DATA TABLE;
$(function () {

    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');
        if (ERROR_SRC === 1)
            window.location.href = "/ad/home";        
    });

    $.validator.setDefaults({
        submitHandler: function () {
            callAddLocation();
        }
    });

    $('#category_form').validate({
        rules: {
            category_name: {
                required: true,
                maxlength: 200
            }
        },
        messages: {
            category_name: {
                required: "Vui lòng cung cấp tên thể loại",
                maxlength: "Tên thể loại chấp nhận tối đa 200 ký tự"
            }
        },
        errorElement: 'span',
        errorPlacement: function (error, element) {
            error.addClass('invalid-feedback');
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

var table = $("#tblCategory").DataTable({
    "ajax": {
        "url": "/ad/Category/GetCategoryList",
        "type" : "GET",
        "datatype": "json",
        "error": function (xhr) {
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });
            ERROR_SRC = 1;
        }
    },
    "columns": [       
        { "data": "Name", "width" : "30%" },
        { "data": "Description", "width": "auto" },        
        { "data": "ExtCode", "width": "20%" }
    ],
    "columnDefs": [        
        {
            "targets": 2,
            "data": "ExtCode",
            "className" : "text-center",
            "render": function (data, type, row, meta) {
                var codes = data.split('-');
                if (codes[1] !== 'NO_ACTION')
                    return "<a data-toggle='tooltip' data-placement='top' title='Sửa' class='btn btn-primary btn-sm' href='category/edit/" + codes[0] + "'><i class='fa fa-pencil-square-o'></i></a>&nbsp;<button data-toggle='tooltip' data-placement='top' title='Xóa' class='btn btn-sm btn-danger' onclick='showConfimDialog(this, \"" + codes[0] + "\")'><i class='fa fa-trash-o'></i></button>";
                else
                    return "";
            },
            "orderable" : false
            
        }
    ],
    "language": {
        "emptyTable": "Không có dữ liệu, bạn có thể chọn <strong>Thêm thể loại</strong> để đưa dữ liệu vào hệ thống",
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
    var name = $('#category_name').val();
    var desc = $("#category_desc").val();

    var category = {
        Id: 0,
        Name: name,
        Description: desc
    };

    $.ajax({        
        url: '/ad/Category/AddCategory',
        type: 'POST',
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(category),
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
            ERROR_SRC = 0;
        }
    });
}

function clearForm() {
    $('#category_form').find('input[type=text],textarea').val('');
}

function showConfimDialog(button, cateid) {
    var category_name = $(button).closest('tr').find('td:first').text();    
    $("#str-cateid").html(category_name);
    $(".btnOK").bind("click", function () {
        DeleteCategory(cateid);
    });

    $("#delCategoryModal").modal("show");
}

function DeleteCategory(cateid) {
    $.ajax({
        type: "POST",
        url: "/ad/Category/Delete",
        data: { id : cateid },
        success: function (data) {
            $("#delCategoryModal").modal("hide");
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
            $("#delCategoryModal").modal("hide");
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });
            ERROR_SRC = 0;
        }
    });
}
$(function () {
    init();
});

var table = $("#tblPost").DataTable({
    "ajax": {
        "url": "/ad/post/GetPostList",
        "type": "GET",
        "datatype": "json"
    },
    "columns": [
        { "data": "MetaTitle", "width": "0%" },
        { "data": "Title", "width": "35%" },       
        { "data": "Enable", "width": "10%" },
        { "data": "Category", "width": "15%" },
        { "data": "Date", "width": "10%" },
        { "data": "Author", "width": "15%" },
        { "data": "Id", "width": "15%" }
    ],
    "columnDefs": [
        {
            "targets": 0,
            "data": "MetaTitle",
            "visible": false
        },
        {
            "targets": 1,
            "data": "Title",
            "render": function (data, type, row, meta) {
                return "<a href='/bai-viet/"+ row["MetaTitle"] + "-" + row["Id"] + "' target='_blank'>" + data + "</a>";
            }
        },
        {
            "targets": 2,
            "data": "Enable",
            "render": function (data, type, row, meta) {
                return (data === true ? "<p class='text-center'><span data-toggle='tooltip' data-placement='right' title='Hiển thị' class='badge bg-light-blue'><i class='fa fa-fw fa-eye'></i></span></p>" : "<p class='text-center'><span data-toggle='tooltip' data-placement='right' title='Ẩn' class='badge bg-red'><i class='fa fa-fw fa-eye-slash'></i></span></p>");
            }
        },
        {
            "targets": 6,
            "data": "Id",
            "className": "text-center",
            "render": function (data, type, row, meta) {
                return "<a data-toggle='tooltip' data-placement='top' title='Sửa' class='btn btn-primary btn-sm' href='post/editpost/" + data + "'><i class='fa fa-pencil-square-o'></i></a>&nbsp;<button data-toggle='tooltip' data-placement='top' title='Sao chép' class='btn btn-sm btn-success' onclick='DuplicatePost(\"" + data + "\")'><i class='fa fa-fw fa-copy'></i></button>&nbsp;&nbsp;&nbsp;<button data-toggle='tooltip' data-placement='top' title='Xóa' class='btn btn-sm btn-danger' onclick='showConfimDialog(this,\"" + data + "\")'><i class='fa fa-trash-o'></i></button>";
            },
            "orderable": false
        }
    ],
    "language": {
        "emptyTable": "Không có dữ liệu, bạn có thể chọn <strong>Thêm bài viết</strong> để thêm dữ liệu vào hệ thống",
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

function showConfimDialog(button, id) {
    var title = $(button).closest('tr').find('td:first').text();
    $("#str-title").html(title);
    $(".btnOK").bind("click", function () {
        DeletePost(id);
    });

    $("#delModal").modal("show");
}

function DeletePost(id) {
    $.ajax({
        type: 'POST',
        url: "/ad/post/deletepost",       
        data: { id : id },
        success: function (data) {
            $("#delModal").modal("hide");
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
            $("#delModal").modal("hide");
            showErrorDialog();
        }
    });
}

function DuplicatePost(id) {
    $.ajax({
        type: 'POST',
        url: "/ad/post/duplicatepost",
        data: { id: id },
        success: function (data) {            
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
            showErrorDialog();
        }
    });
}

function init() {
    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal("hide");
    });
}

function showErrorDialog() {
    $('#modal-default-error').modal({
        backdrop: 'static',
        keyboard: false
    });
}
var table = $("#tblUser").DataTable({
    "ajax": {
        "url" : "/ad/User/GetUserList",
        "type" : "GET",
        "datatype" : "json"
    },
    "columns": [       
        { "data": "Username", "width" : "auto" },
        { "data": "Fullname", "width": "auto" },
        { "data": "Email", "width": "auto" },
        { "data": "Role", "width": "auto" },
        { "data": "IsActived", "width": "auto" },
        { "data": "LastLogin", "width": "auto" },
        { "data": "Username", "width": "auto" }
    ],
    "columnDefs": [
        {
            "targets": 3,
            "data": "Role",
            "render": function (data, type, row, meta) {
                var items = data.split('-');
                var rcode = items[0];
                var rname = items[1];
                var text = '';
                if (rcode === 'ADMIN') {
                    text = '<i class="fa fa-fw fa-user"></i> ' + rname;
                } else if (rcode === 'EDITOR') {
                    text = '<i class="fa fa-fw fa-edit"></i> ' + rname;
                }
                return text;
            }
        },
        {
            "targets": 4,
            "data": "IsActived",
            "render": function (data, type, row, meta) {
                return "<span class='label label-" + (data === true ? "primary" : "danger") + "'>" + (data === true ? "<i class='fa fa-fw fa-unlock'></i> Hoạt động" : "<i class='fa fa-fw fa-lock'></i> Đang khóa") + "</span>";
            }
        },
        {
            "targets": 6,
            "data": "Username",
            "className" : "text-center",
            "render": function (data, type, row, meta) {
                return "<a data-toggle='tooltip' data-placement='top' title='Sửa' class='btn btn-primary btn-sm' href='/ad/user/edituser/" + data + "'><i class='fa fa-pencil-square-o'></i></a>&nbsp;<button data-toggle='tooltip' data-placement='top' title='Xóa' class='btn btn-sm btn-danger' onclick='showConfimDialog(\"" + data + "\")'><i class='fa fa-trash-o'></i></button>";
            },
            "orderable" : false
            
        }
    ],
    "language": {
        "emptyTable": "Không có dữ liệu người dùng, bạn có thể chọn <strong>Thêm người dùng</strong> để thêm dữ liệu vào hệ thống",
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

function showConfimDialog(userid) {
    $("#str-userid").html(userid);
    $(".btnOK").bind("click", function () {
        DeleteUser(userid);
        $("#delUserModal").modal("hide");
    });

    $("#delUserModal").modal("show");
}

function DeleteUser(username) {
    $.ajax({
        type: "POST",
        url: "/ad/User/Delete",
        data: { userid: username },
        success: function (data) {
            if (data.success) {
                table.ajax.reload();                
                $.notify(data.message, {
                    globalPosition: 'top center',
                    className: 'success'
                });
            }          
        },
        error: function (xhr) {           
            $('#modal-default-error').modal({
                backdrop: 'static',
                keyboard: false
            });
        }
    });
}

$(function () {
    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');        
    });
});
$(function () {
    $(".overlay-loader").hide();

    $('#btn-error-close').click(function () {
        $('#modal-default-error').modal('hide');
    });
});

function submitLogin(form) {    
    if ($(form).valid()) {    
        // show loading
        $(".overlay-loader").show();
        $.ajax({
            type: "POST",
            url: "/ad/Auth/Login",
            data: $(form).serialize(),
            success: function (data) {          
                if (data.Success) {                    
                    window.location.href = "/ad";                    
                }
                else {
                    $("#loginmsg").html(data.Msg);                    
                }
                // hide loading
                $(".overlay-loader").hide();
            },
            error: function (jqXHR, exception) {
                // hide loading
                $(".overlay-loader").hide();
                $('#modal-default-error').modal({
                    backdrop: 'static',
                    keyboard: false
                });                
            }
        }); 
    }
    return false;
}
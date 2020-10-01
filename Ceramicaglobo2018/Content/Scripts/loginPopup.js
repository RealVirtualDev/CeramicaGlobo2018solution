var loginPopup = {
    
    show: function (prov,ig) {
      
        $(document).on("keyup",function (e) {
            if (e.keyCode == 27) { // escape key maps to keycode `27`
                galleryPopup.close();
            }
            if (e.keyCode == 13) { // enter
                loginPopup.login();
            }
        });

    },

    close:function() {
        $(".loginPopup").fadeOut(function() {
            $(".oscuragallery").remove();
            $(this).remove();
        });
        $(document).off("keyup");
    },
    reloadpage: function () {
        window.location.reload();
    },
    togglemode: function () {
        if ($("#bttoggle").attr("data-type") == "recuperapassword") {
            // trasformo in login
            $("#bttoggle").attr("data-type", "login");
            $("#bttoggle").html($("#bttoggle").attr("data-logintext"));
            $(".passwordrow").hide();
            $(".btlogin").hide();
            $(".btrecuperapassword").show();
        }
        else {
            // trasformo in recuperapassword
            $("#bttoggle").attr("data-type", "recuperapassword");
            $("#bttoggle").html($("#bttoggle").attr("data-recuperapasswordtext"));
            $(".passwordrow").show();
            $(".btlogin").show();
            $(".btrecuperapassword").hide();
        }
    },
    login: function () {
        var _email = $("#tbemaillogin").val();
        var _pass = $("#tbpasswordlogin").val();
        var _returl = $(".loginPopup").attr("data-returl");

        $("#btlogin").addClass("btn-loading");

        //$.post("/utentireg/login?email=" + email + "&password=" + pass + "&r=" + returl, function (result) {
        $.post("/utentireg/login", { email:_email, password:_pass, r:_returl }, function (result) {
            if (result.Success == false) {
                $(".messagelogin").removeClass("text-secondary").addClass("text-danger").html(result.Message).fadeIn();
            }
            else {
                // loggato
                $(".messagelogin").removeClass("text-danger").addClass("text-secondary").html(result.Message).fadeIn();
                $(".btlogin").hide();
                $(".loginform").hide();
                $(".btchiudi").fadeIn();

            }
        })
    },
    recuperapassword: function () {
        var email = $("#tbemaillogin").val();
        $("#btrecupera").addClass("btn-loading");

        $.post("/utentireg/PasswordRecovery?email=" + email, function (result) {

            if (result.Success == false) {
                $(".messagelogin").removeClass("text-secondary").addClass("text-danger").html(result.Message).fadeIn();
            }
            else {
                // loggato
                $(".messagelogin").removeClass("text-danger").addClass("text-secondary").html(result.Message).fadeIn();
                $(".btlogin").hide();
                $(".btrecuperapassword").hide();

                $(".loginform").hide();
                $(".btchiudi").fadeIn();

            }

        });

    }
}
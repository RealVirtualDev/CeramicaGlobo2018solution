var getToken = function () {
    return $('input[name=__RequestVerificationToken]').val();
}
var ajaxSend = function () {

    // valido i dati inseriti
    let validateStatus = jsvalidator.validateContainer(".registerForm");
    let returl = $(".btn-invia").attr("data-returl");
    //console.info(returl);
    //console.info(validateStatus.isvalid)
    // inserisco a mano
    if ($("#tbpassword").val().indexOf("&") > -1) {
        validateStatus.isvalid = 0;
        var c = { fieldName: "", langKey: $("#tbpassword").attr("data-validationchars"), controlId: "tbpassword" }
        validateStatus.rawError.push(c);
    }
    if ($("#cbaccetta").prop("checked") == false) {
        validateStatus.isvalid = 0;
        var c = { fieldName: "", langKey: $("#cbaccetta").attr("data-validationerror"), controlId:"cbaccetta"}
        validateStatus.rawError.push(c);
    }
    //console.info(validateStatus.isvalid);
    if (validateStatus.isvalid == 0) {

        var langerror = "";
        $.each(validateStatus.rawError, function (i, v) {
            langerror += v.langKey.replace("-1", v.fielName) + "<br/>";
        })


        //console.info(validateStatus);
        let bt1 = {
            onclick: function () {
                mbox.hide();
            }
        }
        // scrollo sul primo controllo trovato con errore
        $('html, body').animate({
            scrollTop: $("#" + validateStatus.rawError[0].controlId).offset().top - 50
        }, 1000);
        //console.info($("#" + validateStatus.rawError[0].controlId).offset().top);
        mbox.reset();
        mbox.show("error", langerror, bt1);
        return false;
    }


    $(".btn-invia").addClass("btn-loading disabled");

    $.post("/utentireg/RegistraUtente?r=" + returl, {
        ragionesociale: $("#tbragionesociale").val(),
        nome: $("#tbnome").val(),
        cognome: $("#tbcognome").val(),
        email: $("#tbemail").val(),
        nazione: $("#tbnazione").val(),
        citta: $("#tbcitta").val(),
        provincia: $("#tbprovincia").val(),
        password: $("#tbpassword").val(),
        professione: $("#tbprofessione").val(),
        indirizzo: $("#tbindirizzo").val(),
        __RequestVerificationToken: getToken() // Token is posted.
    }, function (data) {
        //$btn.button('reset')
        // console.info(data);
        console.info(data)

        if (data.Success) {
            // console.info(data)
            let bt1 = {
                onclick: function () {
                    mbox.hide();
                    window.location.href = data.RedirectUrl;
                }
            }
            mbox.reset();
            mbox.show("info", data.Message, bt1);
            //window.location.href = data.RedirectUrl;
        }
        else {
            // credenziali errate
            //console.info(data);
            let bt1 = {
                onclick: function () {
                    mbox.hide();
                }
            }
            mbox.show("alert", data.Message, bt1);
           // $(".error-div").html(data.Message).fadeIn();
           $(".btn-login").removeClass("btn-loading disabled");
        }
    });
}

function keypressHandler(e) {

    if (e.which == 13) {

        e.preventDefault(); //stops default action: submitting form
        $(this).blur();
        $('.btn-invia').focus().click();//give your submit an ID
    }
}

$('body').on("keyup", "#registerform", function (e) { keypressHandler(e) });

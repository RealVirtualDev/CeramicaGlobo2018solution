getToken = function () {
    return $('input[name=__RequestVerificationToken]').val();
}
ajaxSend = function () {

    // valido i dati inseriti
    let validateStatus = jsvalidator.validateContainer(".registerForm");
    //console.info(validateStatus);
    //console.info(validateStatus.isvalid)

    
    if ($("#cbaccetta").prop("checked") == false) {
        validateStatus.isvalid = 0;
        var c = { fieldName: "", langKey: $("#cbaccetta").attr("data-validationerror"), controlId: "cbaccetta" }
        validateStatus.rawError.push(c);
    }


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

    $.post("/utentireg/RegistraNewsletter", {
        ragionesociale: $("#tbragionesociale").val(),
        email: $("#tbemail").val(),
        professione: $("#tbprofessione").val(),
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
            $(".btn-invia").removeClass("btn-loading disabled");
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

$('body').on("keyup", "#registerform", (e) => keypressHandler(e));

getToken = function () {
    return $('input[name=__RequestVerificationToken]').val();
}
ajaxSend = function () {

    // valido i dati inseriti
    let validateStatus = jsvalidator.validateContainer(".paymentform");
    //console.info(validateStatus);
    //console.info(validateStatus.isvalid)
    if (validateStatus.isvalid == false) {

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
    

    $(".btn-inviapagamento").addClass("btn-loading disabled");

    $.post("/pagamenti/SendPayment", {
        lingua: $("#ddllang").val(),
        importo: $("#tbimporto").val(),
        ragionesociale: $("#tbragione").val(),
        riferimento: $("#tbriferimento").val(),
      
        __RequestVerificationToken: getToken() // Token is posted.
    }, function (data) {
        //$btn.button('reset')
        // console.info(data);
        //console.info(data)
        //console.info(data.success)

        if (data.success) {
            //console.info("ok")
            //let bt1 = {
            //    onclick: function () {
            //        mbox.hide();
            //    }
            //}
            //mbox.reset();
            //mbox.show("info", data.message, bt1);
            //window.location.href = data.RedirectUrl;
            $("body").append(data.message);
            $("#formpay").submit();
        }
        else {
            // credenziali errate
            //console.info(data);
            let bt1 = {
                onclick: function () {
                    mbox.hide();
                }
            }
            mbox.show("alert", data.message, bt1);
            // $(".error-div").html(data.Message).fadeIn();
            $(".btn-login").removeClass("btn-loading disabled");
        }
    });
}

function keypressHandler(e) {

    if (e.which == 13) {

        e.preventDefault(); //stops default action: submitting form
        $(this).blur();
        $('.btn-inviamail').focus().click();//give your submit an ID
    }
}

$('body').on("keyup", "#signupform", (e) => keypressHandler(e));

var json;


var initEditor = function (jsonData) {

    json = JSON.parse(jsonData);

    // salva e annulla
    $(".btn-annulla").click(() => closeEditNew());
    $(".btn-salva").click(() => savepage());


}

var updatePageModel = function () {

    // valido i dati inseriti
    let validateStatus = jsvalidator.validateContainer(".validateWrapper");

    if (validateStatus.isvalid == false) {
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
        mbox.show("error", validateStatus.messageError, bt1);
        return false;
    }


    $.each(json, function (name, value) {
        
        // ciclo gli elementi sdandard
        if ($('#tb' + name).length > 0) {

            json[name] = $('#tb' + name).val();
            
        }

    });
    json["attivo"] = $("#ddlattivo").val()==1 ? true : false;

    return true;

}

var savepage = function () {
    // apro il loading
    mbox.show("loading", "Salvataggio dei dati in corso...");

    // AGGIORNO IL MODELLO
    if (!updatePageModel())
        return; // esco se modello non valido lato client


    //var urlsave = '@Url.Action("savePage", "pages", new { area = "admin" })';
    var urlsave = "/admin/utentinewsletter/savePage";
    $.post(urlsave, { "pmodel": json }, function (data) {

        // messageBox
        //let btok = { onclick: function () { alert('bt1 clicked') } };
        //let btcancel = {
        //    text: "CANCEL", hide: false, onclick:
        //    function () {
        //        hidePopup()
        //    }
        //};
        if (data == "OK") {
            var bt1 = {

                onclick: function () {
                    mbox.hide();
                    closeEditNew();
                }

            }
            grid.rebind();
            mbox.show("info", "I dati sono stati salvati", bt1);
        }
        else {
            // errore
            var bt1 = {

                onclick: function () {
                    mbox.hide();
                 }

            }
            mbox.show("alert", data, bt1);
        }


    });
};




var json;


var initEditor = function (jsonData) {
    
    json = jsonData;

    // SETTO CKEDITOR
    $("[data-type='ckeditor']").each(function () {
        CKEDITOR.replace($(this).attr("id"), ckeditorConfig);
    })

    // salva e annulla
    $(".btn-annulla").click(() => closeEditNew());
    $(".btn-salva").click(() => savepage());

    // APRO FILESELECTOR
    $(".buttonfileselector,.buttonimgselector").on("click", function () {

        let urlCall = "/admin/FileSelector/render"
        let id = $(this).attr("data-modelid");
        $.post(urlCall, { mode: "resource", modelId: id }, function (data) {

            $('#fileselectorPlaceHolder').html(data);

        })

    });

    // RESET
    $(".buttonfilereset,.buttonimgreset").on("click", function () {
        let propertyname = $(this).attr("data-name");
        let resetval = $("#modelcontrol" + propertyname).attr("data-resetvalue");
        $("#modelcontrol" + propertyname).val(resetval);
       // console.info(resetval);
        if ($("#modelcontrolimg" + propertyname)) {


            let defaultimg = $("#modelcontrolimg" + propertyname).attr("data-imgdefault");
            let defaultfolder = $("#modelcontrolimg" + propertyname).attr("data-folder");
            $("#modelcontrolimg" + propertyname).attr("src", resetval);
        }

    });

}

var updatePageModel = function () {

    // valido i dati inseriti
    let validateStatus = jsvalidator.validateContainer(".validateWrapper");
    //console.info(validateStatus)
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


    //$.each(json, function (name, value) {

    //    // ciclo gli elementi sdandard
    //    if ($('#tb' + name).length > 0) {

    //        json[name] = $('#tb' + name).val();
    //        // CKEDITOR
    //        if ($('#tb' + name).attr("data-type") == "ckeditor") {
    //            json[name] = CKEDITOR.instances["tb" + name].getData();
    //        }

    //    }
    //});


    // aggiorno modello aggiuntivo
    $.each(json.Model, function (i, v) {

        let el = $("#modelcontrol" + v["propertyname"]);
        let controltype = el.attr("data-type");
        let inputVal = "";
        let par = v.jsonadminparams != "" ? JSON.parse(v.jsonadminparams) : null;

        //console.info(controltype);

        switch (controltype) {
            case "text":
            case "date":
            case "file":
            case "img":
            case "textarea":
            case "dropdown":
                inputVal = el.val();
               // console.info(el.val());
                break;
            case "ckeditor":
                inputVal = CKEDITOR.instances["modelcontrol" + v["propertyname"]].getData();
                break;
            case "res_dropdownlist":
                inputVal = ResourceSelector.DropDownList.getValue("modelcontrol" + v["propertyname"]);
                break;
            case "customcomponent":
                //console.info("Custom", ResourceSelector[par['name']]);
                inputVal = CustomComponents.getValue(v["propertyname"]);
                //console.info(inputVal);
                break;
        }
        //console.info("ok", inputVal)
        if (inputVal!=undefined)
            json.Model[i]["value"] = inputVal;

   
    })

    return true;

}

var savepage = function () {
    // apro il loading
    mbox.show("loading", "Salvataggio dei dati in corso...");

    // AGGIORNO IL MODELLO
    if (!updatePageModel()) {
        //console.info("non valido")
        return; // esco se modello non valido lato client

    }

    // aggiorno la gallery
    if (typeof updateGalleryModel !== "undefined") {
        updateGalleryModel();
    }

    // aggiorno i files
    if (typeof updateFilesModel !== "undefined") {
        updateFilesModel();
    }

    //var urlsave = '@Url.Action("savePage", "pages", new { area = "admin" })';
    var urlsave = "/admin/resources/savePage";
    //console.info(json)
    $.post(urlsave, { "resource": json }, function (data) {

        // messageBox
        //let btok = { onclick: function () { alert('bt1 clicked') } };
        //let btcancel = {
        //    text: "CANCEL", hide: false, onclick:
        //    function () {
        //        hidePopup()
        //    }
        //};
        var bt1;

        if (data !== "OK") {
             bt1 = {

                onclick: function () {
                    mbox.hide();

                }

            }
            mbox.show("error", data, bt1);
        }
        else {
             bt1 = {

                onclick: function () {
                    mbox.hide();
                    closeEdit();
                }

            }
            grid.rebind();
            mbox.show("info", "I dati sono stati salvati", bt1);
        }
    });
};




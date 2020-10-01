$(".btn-deleteFileItem").click(function () {
    // sicuri di voler eliminare l'elemento?

    var idx = parseInt($(this).attr("data-idx"));

    let btsi = {
        text: "Si",
        onclick: function () {
            mbox.hide();
            json.Files[idx].status = "deleted";
            $(".adminFileItem[data-idx='" + idx + "']").addClass("hide");
            $(".adminFileItem[data-idx='" + idx + "']").attr("data-status", "deleted");
        }
    }
    let btno = {
        text: "No",
        onclick: function () {
            mbox.hide();
        }
    }

    mbox.reset();
    mbox.show("info", "Sicuri di voler eliminare l\'elemento selezionato?", btsi, btno);


    //console.info(json.Files[idx]);
});

var updateFilesModel = function () {
    // aggiorno il modello gallery con i dati dei forms

    $(".adminFileItem").each(function (i, v) {
        let idx = $(this).attr("data-idx");
        let displayname = $(".tbdisplayname", this).val();

        json.Files[idx].displayname = displayname;

    })
}

var updateFile = function (files) {

    updateFilesModel();

    for (file of files) {
        let item = {
            folder: "/public/temp/",
            file: file,
            pname: json.pname,
            lang: json.lang,
            displayname: file,
            ico: file.split(".").slice(-1)[0] + ".png",
            status: "new"
        }
        json.Files.push(item)
    }
    let urlpost = "/admin/resources/getFiles";
    $.post(urlpost, { pfiles: json.Files }, function (data) {
        $(".filesview").html(data);
    });
    //console.info(json.Gallery)

};

$("#uploadfile").jlupload({
    buttonlabel: "SFOGLIA",
    fileprefix: '',
    filter: '',
    multiselect: true,
    uploadfolder: '/public/temp/',
    connector: '/admin/uploader/upload',

    uploadcompleted: function (dataresult) {
        //$('#log').append('<span>ID controllo: '+ _f.controlid +' File: '+ _f.name +' caricato in '+_f.folder+'</span><br/>')

        var _filesname = [];

        $(dataresult).each(function (i, v) {
            _filesname.push(this.file.name);
            //console.info(this.file.name);
        })
        // attendo 10 secondi prima di aggiornare
        $(".fileLoading").show();
        setTimeout(() => updateFile(_filesname), 5000);
        // updateGallery(_filesname);
    }, multiselect: true
});
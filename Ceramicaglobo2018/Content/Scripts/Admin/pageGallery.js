$(".btn-deleteGalleryItem").click(function () {
    // sicuri di voler eliminare l'elemento?
    var idx = parseInt($(this).attr("data-idx"));

    let btsi = {
        text:"Si",
        onclick: function () {
            mbox.hide();
            json.Gallery[idx].status = "deleted";
            $(".adminGalleryItem[data-idx='" + idx + "']").addClass("hide");
            $(".adminGalleryItem[data-idx='" + idx + "']").attr("data-status", "deleted");
        }
    }
    let btno = {
        text: "No",
        onclick: function () {
            mbox.hide();
        }
    }

    mbox.reset();
    mbox.show("alert", "Sicuri di voler eliminare l\'elemento selezionato?", btsi, btno);
    

    //console.info(pageControl.jsonModel.Gallery);
});


var updateGalleryModel = function () {
    // aggiorno il modello gallery con i dati dei forms
    $(".adminGalleryItem").each(function (i, v) {
        let idx = $(this).attr("data-idx");
        let titolo = $(".tbtitolo", this).val();
        let descrizione = $(".descrizione", this).val();
        let urlvideo = $(".tburlvideo", this).val();

        json.Gallery[idx].titolo = titolo;
        json.Gallery[idx].descrizione = descrizione;
        json.Gallery[idx].urlvideo = urlvideo;


    })
}


var updateGallery = function (files) {

    updateGalleryModel();


    for (file of files) {
        let item = {
            folder: "/public/temp/",
            img: file,
            lang: json.lang,
            pname: json.pname,
            itemgroupcontent: json.itemgroup,
            status: "new"
        }
        json.Gallery.push(item)
    }
    let urlpost = "/admin/gallerymanager/getGallery";
    $.post(urlpost, { pgallery: json.Gallery }, function (data) {
        $(".galleryview").html(data);
    });
    //console.info(json.Gallery)

};


$("#uploadgallery").jlupload({
    buttonlabel: "SFOGLIA",
    fileprefix: '',
    filter: 'jpg,JPG',
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
        $(".galleryLoading").show();
        setTimeout(() => updateGallery(_filesname), 5000);
        // updateGallery(_filesname);
    }, multiselect: true,

    filerefused: function (ext, file) {
        // file extension not allowed
    }
});

var fileSelector = {
    setting: {},
    selected: {},
    show: function () {
        /* setting={
            folder:"",
            hfallonewfolder:true,
            parentControlId:0,
            parentImgId:0,
            imgw:0,
            imgh:0,

        }*/
  
        $("#tbaddress").val(fileSelector.setting.startFolder);
        if (fileSelector.setting.hallonewfolder===false) {
            $("#divnewfolder").hide();
        }
        else {
            $("#divnewfolder").show();
        }

        fileSelector.loadFiles();
        $("#fileselectorpopup").fadeIn();

        fileSelector.setUpload();
        fileSelector.registerEvents();
    },
    hide: function () {
          
          $("#fileselectorpopup").fadeOut();
          fileSelector.detachEvents();
          //$('#fufilesel').html($('#fufilesel').html());

    },
    loadFiles:function() {
        let postUrl = "/admin/FileSelector/ListContent";
        let currentFolder = fileSelector.setting.currentFolder;
        let currentId = fileSelector.setting.modelId;
        fileSelector.loading(true);
        $.post(postUrl, { "path": currentFolder, "mode": fileSelector.setting.source, "modelId": currentId }, function (data) {
            $("#fileselectorlist").html(data);
            fileSelector.loading(false);
        })
    },
    processPath: {
        addFile: function (fname) { // per l'anteprima'
            var _path = $("#tbaddress").val();
            var _pathArray = _path.split("/");
            _pathArray.push(fname);
           
            var _newPathname = "";
            for (i = 0; i < _pathArray.length; i++) {
                if (_pathArray[i] != '') {
                    _newPathname += "/";
                    _newPathname += _pathArray[i];
                }

            }
            
            return _newPathname;
           
        },

        addFolder: function (fname) {
            var _path = $("#tbaddress").val();
            var _pathArray = _path.split("/");
            _pathArray.push(fname);
            var result;


            var _newPathname = "";
            for (i = 0; i < _pathArray.length; i++) {
                if (_pathArray[i] != '') {
                    _newPathname += "/";
                    _newPathname += _pathArray[i];
                }

            }
            
            if (_newPathname.indexOf(fileSelector.setting.startFolder) != -1) {
                fileSelector.setting.currentFolder = _newPathname;
                result= _newPathname;
            }
            else {
                fileSelector.setting.currentFolder = fileSelector.setting.startFolder;

                result= fileSelector.setting.startFolder;
            }
            // ripristino i setting giusti col la folder corrente
            fileSelector.setUpload();
            
            return result;


        },

        goBack: function () {
            var _path = $("#tbaddress").val();
            var _pathArray = _path.split("/");
            var result;

            var _newPathname = "";
            for (i = 0; i < _pathArray.length - 1; i++) {
                if (_pathArray[i] != '') {
                    _newPathname += "/";
                    _newPathname += _pathArray[i];
                }
            }
           

            if (_newPathname.indexOf(fileSelector.setting.startFolder) != -1) {
                fileSelector.setting.currentFolder = _newPathname;
                result = _newPathname;
            }
            else {
                fileSelector.setting.currentFolder = fileSelector.setting.startFolder;
                result = fileSelector.setting.startFolder;
            }
            fileSelector.setUpload();
            return result;
        }

    },
    setUpload: function () {

        // workaround per unbindare il plugin jluploader
        var uptemplate = '<input type="file" id="fufilesel">';
        $("#fileselectorwrapper .uploadwrapper").html(uptemplate);
       // console.info(fileSelector.setting.currentFolder)

        /****************** PLUGIN UPLOAD FILES ********************/
      var pp= $("#fufilesel").jlupload({
            buttonlabel: "SFOGLIA",
            multiselect: true,
            connector: '/admin/uploader/upload',
            filter: fileSelector.setting.uploadMode =="image" ? 'jpg,JPG,bmp,png' : '',
            additionalParams: [ 
                { "name": "targetfolder", "val": fileSelector.setting.currentFolder },
                { "name": "modelId", "val": fileSelector.setting.modelId },
                { "name": "mode", "val": fileSelector.setting.source }
           ],
            uploadfolder: fileSelector.setting.currentFolder,
            uploadcompleted: function (dataresult) {
                fileSelector.loading(true);

                var _newval = '';

                $(dataresult).each(function () {
                    _newval += this.file.name + ';'
                });
                setTimeout(function () {
                    // attendo un secondo
                    // aggiorno
                    fileSelector.loadFiles();
                }, 2000)
             

            },
            filerefused: function (ext, file) {
                // file extension not allowed
            }
        });

    },
    registerEvents: function () {

        /****************** MENU CONTESTUALE ********************/
        // If the document is clicked somewhere
        $(document).on("contextmenu", ".fileitem", function (e) {

            e.preventDefault();
            if ($(this).attr("data-ftype") == "back") {
                // non faccio aprire il menu contestuale per  elemento back
                return
            };
            if ($(this).attr("data-ftype") == "folder") {
                // nascondo il tasto apri
                $(".custommenu li:first-child").hide();

            }
            else {
                // mostro il tasto apri
                $(".custommenu li:first-child").show();
            }

            $(".fileitem").removeClass("itemselected");
            $(this).addClass("itemselected");
            var _xoffset = parseInt($("#fileselectorpopup").position().left) - parseInt($("#fileselectorpopup").width() / 2);
            var _yoffset = parseInt($("#fileselectorpopup").position().top) - parseInt($("#fileselectorpopup").height() / 2);
           
            $(".custommenu").hide();
            
            $(".custommenu").stop(true, true).toggle(100).css({

                //top: e.pageY - _yoffset + "px",
                //left: e.pageX - _xoffset + "px"

                top: e.clientY - _yoffset + "px",
                left: e.clientX - _xoffset+ "px"
            });
            fileSelector.selected.fname = $(this).attr("data-fname");
            fileSelector.selected.ftype = $(this).attr("data-ftype");

        });

        // bindo il pulsante chiudi popup
        $("#btchiudipopup").on("click", function () {
            fileSelector.hide();
        });


       
        /****************** MENU CONTESTUALE CLICK SU ELEMENTO ********************/
        // If the menu element is clicked
        $(".custommenu li").on("click",function () {

            // This is the triggered action name
            switch ($(this).attr("data-action")) {

                // A case for each action. Your actions here
                case "apri":
                    if (selected.ftype != 'folder') {
                        window.open(processPath.addFolder(selected.fname));
                    };
                    break
                case "elimina":
                    
                    let btok = {
                        text: "SI", onclick: function () {
                           // Elimino
                           fileSelector.delete();
                           mbox.hide();
                        }
                    };
                    let btcancel = {
                        text: "NO", onclick:
                        function () {
                            mbox.hide();
                        }
                    };

                    mbox.show("alert", "Sicuri di voler eliminare l'elemento selezionato?", btok,btcancel);



                   
                    break

            }

            // Hide it AFTER the action was triggered
            $(".custommenu").hide(100);
            $(".fileitem").removeClass("itemselected");
        });

        /****************** GESTISCO IL DOPPIO CLICK ********************/
        $("#fileselectorlist").on("dblclick", ".fileitem", function () {
            var _fname = $(this).attr("data-fname");
            var _ftype = $(this).attr("data-ftype");
           
            if (_ftype == "folder") {

                //$("#tbaddress").val($("#tbaddress").val() + "/" + _fname);
                $("#tbaddress").val(fileSelector.processPath.addFolder(_fname));
                //$("#btupdate").click();
                fileSelector.loadFiles();
            };

            if (_ftype == 'back' ) {
                $("#tbaddress").val(fileSelector.processPath.goBack());
                fileSelector.loadFiles();
            };

            if (_ftype == 'img' || _ftype == 'file') {
                // FILE SELEZIONATO CHIUDO POPUP
               
                //fileselected($("#hfcontrolname").val(), fileSelector.processPath.addFolder(_fname))
                let control = $("#hfcontrolname").val();
                let filepath = fileSelector.processPath.addFile(_fname);
                //console.info(fileSelector.setting.parentControlId);


                $("#" + fileSelector.setting.parentControlId).val(filepath);
                if (fileSelector.setting.parentImgId != "") {
                    $("#" + fileSelector.setting.parentImgId).attr("src", filepath);
                };

                fileSelector.hide();
            };


        });

        /****************** HIGHLIGHT SU UN FILE QUANDO MENU CONTESTUALE APERTO ********************/
        $("#fileselectorlist").on("mouseenter", ".fileitem", function () {

            $(".custommenu").hide();
            $(".fileitem").removeClass("itemselected");

            var _fname = $(this).attr("data-fname");
            var _ftype = $(this).attr("data-ftype");

            if (_ftype == "img") {

                // visualizzo l'alteprima
                var _path = fileSelector.processPath.addFile(_fname)
                $("#imgpreview").html('<img style="max-width:350px;max-height:150px;" src="' + _path + '"/><br/>' + _fname)
            } else {
                $("#imgpreview").html('');
            }


        }).on("mouseout", ".fileitem", function () {
            $("#imgpreview").html('');
            });

        // pulsante nuova cartella
        $("#btnewfolder").on("click", function () {
            let fname = $("#tbnewfolder").val();
            if (fname) {
                fileSelector.createfolder(fname);
            }
        });

        // pulsante cambia indirizzo
        $("#btgoaddress").on("click", function () {
            let address = $("#tbaddress").val();
        });

    },
    detachEvents: function () {
        

        /****************** MENU CONTESTUALE ********************/
        // If the document is clicked somewhere
        $(document).off("contextmenu", ".fileitem");
        // bindo il pulsante chiudi popup
        $("#btchiudipopup").off("click");
        /****************** GESTISCO IL DOPPIO CLICK ********************/
        $("#fileselectorlist").off("dblclick", ".fileitem");
        /****************** HIGHLIGHT SU UN FILE QUANDO MENU CONTESTUALE APERTO ********************/
        $("#fileselectorlist").off("mouseenter", ".fileitem").off("mouseout", ".fileitem");
        // click menu contestuale
        $(".custommenu li").off("click");
        $("#btnewfolder").off("click");
    },
    delete: function () {
        let fname = fileSelector.selected.fname;
        let currentPath = fileSelector.setting.currentFolder;

        // aggiorno view files
        let urlPost = "/admin/FileSelector/delete";
        $.post(urlPost, { "path": currentPath, "fname": fname, "mode": fileSelector.setting.source, "modelId":fileSelector.setting.modelId },
            function (data) {
                $("#fileselectorlist").html(data);
            }
        )

    },
    createfolder: function (foldername) {
        let postUrl = "/admin/fileselector/createfolder";
        let par = { "path": fileSelector.setting.currentFolder, "fname": foldername, "mode": fileSelector.setting.source,"modelId":fileSelector.setting.modelId}
        $.post(postUrl, par, function (data) {
            $("#fileselectorlist").html(data);
        })
    },
    goto: function (address) {
        let postUrl = "/admin/FileSelector/changeFolder";
        let currentId = fileSelector.setting.modelId;

        $.post(postUrl, { "path": address, "mode": fileSelector.setting.source, "modelId": currentId }, function (data) {
            fileSelector.setting.currentFolder = data;
            fileSelector.loadFiles();
        })
    },
    loading: function (ison) {
        if (ison) {
            $(".floading").fadeIn();
        }
        else {
            $(".floading").fadeOut();
        }
    }
}





   

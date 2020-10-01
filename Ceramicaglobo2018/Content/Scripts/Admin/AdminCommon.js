var defaultlang = 'it';

function querystring(key) {
    var re = new RegExp('(?:\\?|&)' + key + '=(.*?)(?=&|$)', 'gi');
    var r = [], m;
    while ((m = re.exec(document.location.search)) != null) r.push(m[1]);
    return r;
}

//function fiximg() {
//    // reimposto le immagini di anteprima
//    // FIX per immagini di anteprima che spariscono con i postback ajax
//    $("[data-previewcontrol]").each(function () {
//        var tbname = $(this).attr("data-previewcontrol");
//        $(this).attr("src", $("#" + tbname).val());

//    });
//}

// NASCONDI IL FORM
$('.contenteditor').css("display", "none")

 // apre l'editor
var scrollUp = function () {
    $('.contenteditor').slideDown("slow");
    // scrolla su
    $('html, body').animate({ scrollTop: 0 }, 'slow');
}
// chiude l'editor
var closeEdit = function () {
   // $('.contenteditor').slideUp("slow");
    // disattivo per globo
}
var closeEditNew = function () {
    $('.contenteditor').slideUp("slow");
}

// mostra nasconde la gallery
function galleryvisibility(isvisible) {
    //console.info(isvisible)
    if (isvisible === "true") {

        $("#divgallery").show();
    }
    else {
        $("#divgallery").hide();
    }
};

// mostra nasconde i files
function filesvisibility(isvisible) {
    if (isvisible === "true") {
        $("#divfiles").show();
    }
    else {
        $("#divfiles").hide();
    }
};

// valida il form
function validaform() {
    _ret1 = false;
    _ret1 = validateall('', '.roundboxlang', '')
    if (_ret1 == false) {
        malert("Controllare i campi contrassegnati in giallo!")
    };
    return _ret1;
}


    // NASCONDI IL FORM
    // $('#contenteditor').css("display", "none")

   
    // setto la lingua italiana di default
    function resetlang() {
        $(".langtab").removeClass("langtabactive");
        $(".langtab").first().addClass("langtabactive");
    }

    resetlang();

    $(".langtab").on("click", function () {

        let mode = localStorage.mode;
        let lang = localStorage.lang;
        let itemgroup = localStorage.itemgroup;
        let rname = localStorage.rname;
        let pname = localStorage.pname;


        if (itemgroup=="0") {
            alert('Prima di cambiare lingua è necessario salvare i contenuti in Italiano.')
            return
        }

        if (confirm("Attenzione, i dati non salvati per la lingua corrente verranno persi, continuare?") == false) {
            return
        }

        $(".langtab").removeClass("langtabactive");
        $(this).addClass("langtabactive");

        localStorage.lang=$(this).attr("data-lang");
        // ricarica i dati della nuova lingua selezionata
        if (itemgroup) {
            // sto editando una risorsa
            editPage(rname, itemgroup, localStorage.lang);

        }
        else {
            // sto editando una pagina
            editPage(pname, localStorage.lang);
        }

        

    })


var ckeditorConfig = {
    // Define the toolbar: http://docs.ckeditor.com/#!/guide/dev_toolbar
    // The standard preset from CDN which we used as a base provides more features than we need.
    // Also by default it comes with a 2-line toolbar. Here we put all buttons in a single row.
    toolbar: [
        { name: 'document', items: ['Source'] },
        { name: 'clipboard', items: ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Undo', 'Redo'] },
        { name: 'basicstyles', items: ['Bold', 'Italic', 'Strike', '-', 'RemoveFormat'] },
        { name: 'paragraph', items: ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', '-', 'CreateDiv', '-', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'] },
        { name: 'links', items: ['Link', 'Unlink'] },
        { name: 'insert', items: ['Image', 'EmbedSemantic', 'Table'] },
        { name: 'tools', items: ['Maximize'] }
    ],
    // Since we define all configuration options here, let's instruct CKEditor to not load config.js which it does by default.
    // One HTTP request less will result in a faster startup time.
    // For more information check http://docs.ckeditor.com/#!/api/CKEDITOR.config-cfg-customConfig
    //customConfig: '',
    // Enabling extra plugins, available in the standard-all preset: http://ckeditor.com/presets-all
    extraPlugins: 'autoembed,embedsemantic,image2,uploadimage,uploadfile,justify',
    /*********************** File management support ***********************/
    // In order to turn on support for file uploads, CKEditor has to be configured to use some server side
    // solution with file upload/management capabilities, like for example CKFinder.
    // For more information see http://docs.ckeditor.com/#!/guide/dev_ckfinder_integration
    // Uncomment and correct these lines after you setup your local CKFinder instance.
    filebrowserBrowseUrl: '/ckfinder/ckfinder.html',
    //filebrowserBrowseUrl: 'http://localhost:3622/contents/ckfinder/testbrowser.html',
    filebrowserUploadUrl: '/ckfinder/connector',
    

    //filebrowserImageUploadUrl: 'http://localhost:3622/ckfinder/connector?command=QuickUpload&type=Images',
    //filebrowserImageBrowseUrl: 'http://localhost:3622/contents/ckfinder/ckfinder.html&type=Images',
   


   // imageUploadUrl: '/ckfinder/connector?command=QuickUpload&type=Images',

    //filebrowserBrowseUrl: 'http://localhost:3622/contents/ckfinder/ckfinder.html',
    //filebrowserImageBrowseUrl: 'http://localhost:3622/contents/ckfinder/ckfinder.html?type=Images',
    //filebrowserFlashBrowseUrl: 'http://localhost:3622/contents/ckfinder/ckfinder.html?type=Flash',
    //filebrowserUploadUrl: 'http://localhost:3622/contents/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files',
    //filebrowserImageUploadUrl: 'http://localhost:3622/contents/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images',
   
    

    /*********************** File management support ***********************/
    // Remove the default image plugin because image2, which offers captions for images, was enabled above.
    removePlugins: 'image',
    // Make the editing area bigger than default.
    height: 461,
    // An array of stylesheets to style the WYSIWYG area.
    // Note: it is recommended to keep your own styles in a separate file in order to make future updates painless.
    contentsCss: ['https://cdn.ckeditor.com/4.6.1/standard-all/contents.css', '/content/css/ckeditorarticle.css'],
    // This is optional, but will let us define multiple different styles for multiple editors using the same CSS file.
    bodyClass: 'article-editor',
    // Reduce the list of block elements listed in the Format dropdown to the most commonly used.
    format_tags: 'p;h1;h2;h3;pre',
    // Simplify the Image and Link dialog windows. The "Advanced" tab is not needed in most cases.
    removeDialogTabs: 'image:advanced;link:advanced',
    // Define the list of styles which should be available in the Styles dropdown list.
    // If the "class" attribute is used to style an element, make sure to define the style for the class in "mystyles.css"
    // (and on your website so that it rendered in the same way).
    // Note: by default CKEditor looks for styles.js file. Defining stylesSet inline (as below) stops CKEditor from loading
    // that file, which means one HTTP request less (and a faster startup).
    // For more information see http://docs.ckeditor.com/#!/guide/dev_styles
    stylesSet: [
        /* Inline Styles */
        { name: 'Marker', element: 'span', attributes: { 'class': 'marker' } },
        { name: 'Cited Work', element: 'cite' },
        { name: 'Inline Quotation', element: 'q' },
        /* Object Styles */
        {
            name: 'Special Container',
            element: 'div',
            styles: {
                padding: '5px 10px',
                background: '#eee',
                border: '1px solid #ccc'
            }
        },
        {
            name: 'Compact table',
            element: 'table',
            attributes: {
                cellpadding: '5',
                cellspacing: '0',
                border: '1',
                bordercolor: '#ccc'
            },
            styles: {
                'border-collapse': 'collapse'
            }
        },
        { name: 'Borderless Table', element: 'table', styles: { 'border-style': 'hidden', 'background-color': '#E6E6FA' } },
        { name: 'Square Bulleted List', element: 'ul', styles: { 'list-style-type': 'square' } },
        /* Widget Styles */
        // We use this one to style the brownie picture.
        { name: 'Illustration', type: 'widget', widget: 'image', attributes: { 'class': 'image-illustration' } },
        // Media embed
        { name: '240p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-240p' } },
        { name: '360p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-360p' } },
        { name: '480p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-480p' } },
        { name: '720p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-720p' } },
        { name: '1080p', type: 'widget', widget: 'embedSemantic', attributes: { 'class': 'embed-1080p' } }
    ]
}
 $(".tronca").each(function(i,v){
     
     var lines = $(this).attr("data-lines");
        $(this).trunk8({
            lines: lines
        });
})

function querystring(key) {
    var re = new RegExp('(?:\\?|&)' + key + '=(.*?)(?=&|$)', 'gi');
    var r = [], m;
    while ((m = re.exec(document.location.search)) != null) r.push(m[1]);
    return r;
}
// MENU MOBILE

$("a[data-frame]").on("click", function () {
    var df = $(this).attr("data-frame");
    $("#" + df).animate({ "right": 1 });
})

$(".menumobileframeback").on("click", function () {
    $(".menumobilesubframe").animate({ "right": "-450px" });
})

$(".btn-menumobile").on("click", function () {
    var stato = $(".tendinamenu").attr("data-status");
    if (stato == "closed") {
        // apro
        $(".tendinamenu").attr("data-status", "opened");
        $("body").addClass("noscroll");
        $(".tendinamenu").animate({ "right": "0px" });
    }
    else {
        // chiudo
        $(".tendinamenu").attr("data-status", "closed");
        $("body").removeClass("noscroll");
        $(".tendinamenu").animate({ "right": "-450px" });
        $(".menumobilesubframe").animate({ "right": "-450px" });
    }
})



//$(".fancybox-thumb").fancybox({
//    prevEffect: 'none',
//    nextEffect: 'none',
//    helpers: {
//        title: {
//            type: 'outside'
//        },
//        thumbs: {
//            width: 50,
//            height: 50
//        }
//    }
//});


// adatto eventuali video
//$(".video").fitVids();

var mobileToggle = function () {
    $(".menumobile").slideToggle();
}

function load_css_async(filename) {


    var cb = function () {
        var l = document.createElement('link'); l.rel = 'stylesheet';
        l.href = filename;
        var h = document.getElementsByTagName('head')[0]; h.parentNode.insertBefore(l, h);
       

    };
    var raf = requestAnimationFrame || mozRequestAnimationFrame ||
        webkitRequestAnimationFrame || msRequestAnimationFrame;
    if (raf) raf(cb);
    else window.addEventListener('load', cb);

}

$(".btn-goup").on("click", function () {
    $('html, body').animate({ scrollTop: 0 }, 'slow');

   
})
$(window).scroll(function (event) {
    var scroll = $(window).scrollTop();

    // faccio apparire il pulsante rapido vai su
    if (scroll > 800) {
        $(".btn-goup").fadeIn();
    }
    else {
        $(".btn-goup").fadeOut();
    }

    let stack = $(".homeslider").length >0 ? parseInt($(".homeslider").css("height")) : 400;
    // 
    if (scroll > stack) {
        $(".header").css({ position: "absolute", top: stack + "px" });
    }
    else {
        $(".header").css({ position: "fixed", top: "0" });
    }
});

$(".readmorelink").on("click", function () {
    $(this).next(".readmorecontent").slideDown();
    $(this).hide();
})


// NEWSLETTER

var readnewsletter = function (lang) {
    window.location.href = lang == "it" ? "/azienda/comunicazione" : lang + "/company/communication";
}

var joinnewsletter = function (lang) {
    var email = $("#tbnewsletterfooter").val();
    window.location.href = (lang == "it" ? "/registrazionenewsletter" : lang + "/newsletterjoin") + "?e=" + email;
}


var joinnewslettermobile = function (lang) {
    var email = $("#tbnewsletterfootermobile").val();
    window.location.href = (lang == "it" ? "/registrazionenewsletter" : lang + "/newsletterjoin") + "?e=" + email;
}


// CAMBIA LINGUA


var changelanguage = function () {
    $.post("/popup/languageselector", function (data) {
        $("body").append(data);
    })
}

// RICERCA PRODOTTI

var searchproduct = function (lang) {
    var searchstring = $("#tbsearchheader").val();
    //console.info((lang == "it" ? "" : "/" + lang) + "/search?q=" + searchstring);
    if (searchstring.length >= 3) {
        window.location.href =(lang=="it" ? "" : "/" + lang) + "/search?q=" + searchstring; 
    }
}

var searchproductadv = function (lang) {
    var searchstring = $("#tbsearchheader").val();
    //console.info((lang == "it" ? "" : "/" + lang) + "/search?q=" + searchstring);
    if (searchstring.length >= 3) {
        window.location.href = (lang == "it" ? "" : "/" + lang) + "/AdvancedSearch?q=" + searchstring;
    }
    else {
        window.location.href = (lang == "it" ? "" : "/" + lang) + "/AdvancedSearch";
    }
}

$("#tbsearchheader").on("keyup", function (e) {
    if (e.keyCode == 13) { // escape key maps to keycode `27`
        searchproduct($(this).attr("data-lang"));
    }
});

// COMPARATORE

$(".cbcompara").on("change", function () {
    var igprodotto = $(this).attr("data-igprodotto");
    var icoprodotto = $(this).attr("data-icoprodotto");
    var numelementi = $(".comparewrapper .compareicon img[data-igprodotto]").length;
    var giapresente = $(".comparewrapper .compareicon img[data-igprodotto='" + igprodotto + "']").length > 0;

    //console.info($(this).prop("checked"));

    if ($(this).prop("checked")  ) {
        if (numelementi < 5 && giapresente == false) {
            // aggiungo
            $(".comparewrapper .compareicon").append('<img  data-igprodotto="' + igprodotto + '" src="' + icoprodotto + '"/>')
        }
        else {
            $(this).prop("checked", false);
        }
    }
    else {
        // rimuovo
        $(".comparewrapper .compareicon img[data-igprodotto='" + igprodotto + "']").remove();
    }


    if ($(".comparewrapper .compareicon img[data-igprodotto]").length > 0) {
        $(".comparewrapper").show();
    }
    else {
        $(".comparewrapper").hide();
    }

});

$(".compareicon").on("click", "img", function () {
    var igprodotto = $(this).attr("data-igprodotto");
    $(".comparewrapper .compareicon img[data-igprodotto='" + igprodotto + "']").remove();
    $(".cbcompara[data-igprodotto='" + igprodotto + "']").prop("checked", false);

    if ($(".comparewrapper .compareicon img[data-igprodotto]").length > 0) {
        $(".comparewrapper").show();
    }
    else {
        $(".comparewrapper").hide();
    }

});

var emptyCompare = function () {
    $(".comparewrapper .compareicon img[data-igprodotto]").remove();
    $(".comparewrapper").hide();
    $(".cbcompara").prop("checked", false);
}
var Compare = function (lang) {
    var ig = "";
    if ($(".comparewrapper .compareicon img[data-igprodotto]").length > 0) {
        $(".comparewrapper .compareicon img[data-igprodotto]").each(function () {
            ig += $(this).attr("data-igprodotto") + "-";
        })
        window.location.href = (lang == "it" ? "" : "/" + lang) + "/compare?ig=" + ig.slice(0,-1);
    }

}

var showLogin = function (returl) {
   // console.info(returl);
    $.post("/popup/loginpopup", { "retUrl":returl}, function (data) {
        $("body").append(data);
    });
}

// FINITURE

$(".finitura-item").on("mouseover", function () {

    if ($(this).attr("data-pianta")=="true") {

        var imgpianta = $(this).attr("data-path");
        $(".finiturapopover .imgpiantafiniura").attr("src", imgpianta);
        var pop = $(".finiturapopover");
        $(".finiturapopover").remove();
        $(this).append(pop);
        $(".finiturapopover").show();
    }

}).on("mouseout", function () {
    $(".finiturapopover").hide();
   
    }).on("click", function () {
        //console.info($(this).attr("data-pianta"));
        if ($(this).attr("data-pianta") == "true") {
            if ($(".finiturapopover").is(":visible")) {
                $(".finiturapopover").hide();
            }
            else {
                var imgpianta = $(this).attr("data-path");
                $(".finiturapopover .imgpiantafiniura").attr("src", imgpianta);
                var pop = $(".finiturapopover");
                $(".finiturapopover").remove();
                $(this).append(pop);
                $(".finiturapopover").show();
            }
        }
       
    })
var galleryPopup= {
    show: function (prov,ig) {
        var url = "";
        switch (prov) {
            case "prodotto":
                url = "/Liste/getGalleryProdotto?ig=" + ig;
                break;

            case "evento":
                url = "/Liste/getGalleryEvento?ig=" + ig;
                break;
        }

        $.post(url, function (data) {
            $("body").append(data);
        });

        $(document).on("keyup",function (e) {
            if (e.keyCode == 27) { // escape key maps to keycode `27`
                galleryPopup.close();
            }
        });

    },

    close:function() {
        $(".galleryPopup").fadeOut(function() {
            $(".oscuragallery").remove();
            $(this).remove();
        });
        $(document).off("keyup");
    },
    changeImage: function (ig) {
        var src = $("#img" + ig).attr("data-imgbig");
        $(".imggallerycontent img").attr("src", src);
    }
}
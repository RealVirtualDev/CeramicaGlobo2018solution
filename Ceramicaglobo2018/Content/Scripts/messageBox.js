var mbox = {

    hide: function () {
        $(".btn1messagebox").off("click");
        $(".btn2messagebox").off("click");
        $(".oscura").fadeOut();
        $(".messagebox").fadeOut();

    },

    show: function (icon, content, button1setting, button2setting) {

        // icon=alert | error | info | loading | confirm
        /* buttonsetting={
        text:"",
        onclick:function(),
        hide:false
        }
        */

        $(".messageboxicon").removeClass(function (index, className) {
            return (className.match(/(^|\s)icon\S+/g) || []).join('');
        })

        if (icon) {
            $(".messageboxicon").addClass("icon" + icon);
        };

        if (!button1setting || (button1setting.hide && button1setting.hide == true)) {
            $(".btn1messagebox").hide();
        }
        else {
            $(".btn1messagebox").html(button1setting.text || "OK");
            $(".btn1messagebox").on("click", function () { button1setting.onclick() });
            $(".btn1messagebox").show();
        }


        if (!button2setting || (button2setting.hide && button2setting.hide == true)) {
            // nascondi bt2
            $(".btn2messagebox").hide();
        }
        else {

            $(".btn2messagebox").html(button2setting.text || "ANNULLA");
            $(".btn2messagebox").on("click", function () { button2setting.onclick() });
            $(".btn2messagebox").show();
        }

        $(".messageboxcontent").html(content);

        $(".oscura").fadeIn();
        $(".messagebox").fadeIn();

    },

    reset: function () {
        $(".messageboxcontent").html('');
        $(".btn1messagebox").off("click");
        $(".btn2messagebox").off("click");

    }
}
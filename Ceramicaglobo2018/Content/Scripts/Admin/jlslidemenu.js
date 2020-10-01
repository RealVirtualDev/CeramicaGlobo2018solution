$(document).ready(function () {
    var intv;
    var mainint;

    //$('#jlslidemenu li').hover(
        
    //function () {
    //    clearInterval(mainint);
    //    clearInterval(intv);
    //    console.info(0)
    //    $(this).find('ul:first').slideDown('fast', function () {
            
    //    });
    //}, function () {
    //    console.info(1)
    //    $this = this;
    //    intv= setTimeout(function () { $($this).find('ul').slideUp('fast') }, 200);
    //});

    //$('#jlslidemenu li').on("mouseout", function () {
    //    console.info(1);
    //    $this = this;
    //    mainint = setTimeout(function () {
    //        $($this).find('ul').slideUp()
    //    }, 500)
    //})

    //$('ul#jlslidemenu').on(
    //    {
            
    //        "mouseout": function () {
    //            console.info(1)
    //            $('#jlslidemenu ul').slideUp('fast') 

    //        }
    //    })

    $('#jlslidemenu li').on(
        {
            "mouseenter": function () {
                
                $(this).find('>ul').stop(true,true).slideDown('fast');
            },
            "mouseleave": function () {
                
                $(this).find('>ul').stop(true, true).slideUp('fast')
                                      
            }
    })
});
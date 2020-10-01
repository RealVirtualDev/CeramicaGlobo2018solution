
var ResourceSelector = {
    DataSource: [],
    setDataSource: function (controlname, items) {
        //console.info(items);
        ResourceSelector.DataSource[controlname] = items;
    },
    DropDownList: {
        getValue: function (controlname) {

            return $("#" + controlname).val()!="0" ? $("#" + controlname).val() : "";
        },
        fillByParent: function (parentname,controlname) {
            var parentvalue = $('[data-controlname="' + parentname + '"]').val();
            //console.info(parentvalue);
            $('select[data-controlname="' + controlname + '"] option').remove();

            $.each(ResourceSelector.DataSource[controlname], function (i,v) {
               
                //console.info(parentvalue,v);
                if (v["parentValue"] == parentvalue)
                {
                   // console.info(v["selected"] == true);
                    $('select[data-controlname="' + controlname + '"]').append('<option ' + (v["selected"]==true ? "selected" : "") + ' value="' + v["resultValue"] + '">' + v["displayValue"] + '</option>');
                }
            });
        }
    }
}
// BINDO I DROPDOWN A CASCATA
$("body").on("change","select", function () {
   
    if ($(this).attr("data-groupname") != "") {
        let cname = $("select[data-groupparent='" + $(this).attr("data-controlname") + "']").attr("data-controlname");
        let pname = $(this).attr("data-controlname");
       //console.info($(this).attr("data-controlname"),cname,pname);
        ResourceSelector.DropDownList.fillByParent(pname,cname);
    }
});

var CustomComponents = {
    DataSource: [],
    setDataSource: function (controlname, items) {
        //console.info(items);
        ResourceSelector.DataSource[controlname] = items;
    },
    getValue: function (propertyname) {
        switch (propertyname) {
            case "accessori":
                return CustomComponents.AccessoriSelector.getValue();
                break;
            case "finiture":
                return CustomComponents.FinitureSelector.getValue();
                break;
        }
    },
    FinitureSelector: {
        getValue: function () {
            var result = "";
            $(".FinitureSelectorItem[data-selected='true']").each(function () {
                result += $(this).attr("data-element") + "|" + $(this).attr("data-testo") + "|" + $(this).attr("data-groupraw") + ";"
            });
            return result.slice(0, -1);
        },
        resetAll: function () {

          
               
                    // nascondo tutto
                    $(".FinitureSelectorGroupDiv").hide();
                    $(".FinitureSelectorContentDiv").hide();
                    $(".FinitureSelectorItem").hide();
                    // deseleziono tutto
                    $(".FinitureSelectorGroupDiv").attr("data-selected","false");
                    $(".FinitureSelectorContentDiv").attr("data-selected", "false");
                    $(".FinitureSelectorItem").attr("data-selected", "false");
                    // deseleziono tutti i checkbox
                    $(".cbfinituraitem").removeAttr("checked");
                    $(".cbfinituragruppo").removeAttr("checked");
                    // ripristino i check originali
                    $(".cbfinituraitem").each(function () {
                        //console.info($(this));
                        if ($(this).attr("data-original").toLowerCase() == "true") {
                            $(this).prop("checked", true);
                            
                        }
                    })
                    CustomComponents.FinitureSelector.init();
                  
          

        },
        resetGroup: function (gname) {
            // FinitureSelectorGroupDiv e FinitureSelectorContentDiv sono già visibili
            $(".cbfinituraitem[data-group='" + gname + "']").each(function () {

                var el = $(this).attr("data-element");

                if ($(this).attr("data-original").toLowerCase() == "true") {
                    $(this).prop("checked", true);
                    $(".FinitureSelectorItem[data-element='" + el + "']").attr("data-selected", "true");
                    $(".FinitureSelectorItem[data-element='" + el + "']").show();
                }
                else {
                    $(this).prop("checked", false);
                    $(".FinitureSelectorItem[data-element='" + el + "']").attr("data-selected", "false");
                    $(".FinitureSelectorItem[data-element='" + el + "']").hide();

                }

                
            })

            // se il gruppo non ha elementi selezionati lo nascondo tutto
            if ($(".FinitureSelectorItem[data-group='" + gname + "'][data-selected='true']").length == 0) {

                $(".FinitureSelectorGroupDiv[data-group='" + gname + "']").attr("data-selected", "false");
                $(".FinitureSelectorContentDiv[data-group='" + gname + "']").attr("data-selected", "false");
                $(".FinitureSelectorItem[data-group='" + gname + "']").attr("data-selected", "false");

                $(".FinitureSelectorGroupDiv[data-group='" + gname + "']").hide();
                $(".FinitureSelectorContentDiv[data-group='" + gname + "']").hide();
                $(".FinitureSelectorItem[data-group='" + gname + "']").hide();
            }

            
        },

        init: function () {
            $(".cbfinituraitem:checked").each(function () {

                var g = $(this).attr("data-group");
                var el = $(this).attr("data-element");

                $(".FinitureSelectorGroupDiv[data-group='" + g + "']").attr("data-selected", "true");
                $(".FinitureSelectorContentDiv[data-group='" + g + "']").attr("data-selected", "true");
                $(".FinitureSelectorItem[data-element='" + el + "']").attr("data-selected", "true");

                $(".FinitureSelectorGroupDiv[data-group='" + g + "']").show();
                $(".FinitureSelectorContentDiv[data-group='" + g + "']").show();
                $(".FinitureSelectorItem[data-element='" + el + "']").show();

            });
            
        }
    },
    AccessoriSelector: {
        
        getValue: function() {
            var result = "";
            $(".accessorioselected .accessorioitemwrapper").each(function () {
                result += $(this).attr("data-uidaccessorio") + ";"
            });
            return result.slice(0, -1);
        },
        resetView: function(propertyname) {
            val = CustomComponents.DataSource["accessorireset"];
            if (!(val === undefined || val == "")) {
                $.each(val.split(";"), function (i,v) {
                    let ig = v.split("|")[0];
                    console.info(ig);
                })
            }
            else {
                $(".accessorioitemwrapper").remove();
            }
        },
        search: function () {
            $(".btn-addaccessorio").removeClass("btn-loading");
            let txt = $(".accessoriosearch").val();
            if (txt.length >= 3) {
                $(".btn-addaccessorio").addClass("btn-loading");
                $.post("/Admin/Component/SearchAccessorio", { "stext": txt }, function (data) {
                    $(".accessoriosearchresult").html(data);
                    $(".btn-addaccessorio").removeClass("btn-loading");
                    // scrollUp();
                });

            }
        },
        collegaProdotto: function (uid) {
            var acc = $(".accessoriosearchresult .accessorioitemwrapper[data-uidaccessorio='" + uid + "']");
            $(".accessoriosearchresult .accessorioitemwrapper[data-uidaccessorio='" + uid + "']").remove();
            $(".accessorioselected").append(acc);

            if ($(".accessorioselected .accessorioitemwrapper").length == 0) {
                $(".divaccessoriselezionati").html("Nessun Accessorio Selezionato");
            }
            else {
                $(".divaccessoriselezionati").html("Accessori Selezionati:");
            }
        },
        scollegaProdotto: function (uid) {
            var acc = $(".accessorioselected .accessorioitemwrapper[data-uidaccessorio='" + uid + "']");
            $(".accessorioselected .accessorioitemwrapper[data-uidaccessorio='" + uid + "']").remove();
            $(".accessoriosearchresult").append(acc);

            if ($(".accessorioselected .accessorioitemwrapper").length == 0) {
                $(".divaccessoriselezionati").html("Nessun Accessorio Selezionato");
            }
            else {
                $(".divaccessoriselezionati").html("Accessori Selezionati:");
            }
        },
        init: function (val) {
            // chiamo il metodo per popolare i selezionati
            if (val != "") {
                CustomComponents.DataSource["accessorireset"] = val;
                $(".btn-addaccessorio").addClass("btn-loading");
                $.post("/Admin/Component/FillAccessorio", { "currentValue": val }, function (data) {
                    $(".accessorioselected").html(data);
                    $(".btn-addaccessorio").removeClass("btn-loading");
                    // scrollUp();

                    if ($(".accessorioselected .accessorioitemwrapper").length == 0) {
                        $(".divaccessoriselezionati").html("Nessun Accessorio Selezionato");
                    }
                    else {
                        $(".divaccessoriselezionati").html("Accessori Selezionati:");
                    }
                });
            }
           
        }
    }

}
// BINDO LA RICERCA DELLE FINITURE
$("body").on("keyup",".finituresearch", function () {

    var stext = $(this).val().toLowerCase();
    $(".FinitureSelectorGroupDiv[data-selected='false']").hide();
    $(".FinitureSelectorItem[data-selected='false']").hide();
    $(".FinitureSelectorContentDiv[data-selected='false']").hide()

    if (stext.length > 3) {

       // var gruppi = $(".FinitureSelectorGroupDiv[data-group*='" + stext + "']");
        var elem = $(".FinitureSelectorItem[data-group*='" + stext + "'],.FinitureSelectorItem[data-codice*='" + stext + "'],.FinitureSelectorItem[data-testo*='" + stext + "']");

        // gruppi
        $(".FinitureSelectorGroupDiv[data-group*='" + stext + "']").show()
        $(".FinitureSelectorContentDiv[data-group*='" + stext + "']").show()
        $(".FinitureSelectorItem[data-group*='" + stext + "']").show()
        
        $.each(elem, function (i, v) {
           var g= $(this).attr("data-group");
          $(".FinitureSelectorGroupDiv[data-group='" + g + "']").show()
          $(".FinitureSelectorContentDiv[data-group='" + g + "']").show()
          // $(".FinitureSelectorItem[data-group='" + g + "']").show()
           //console.info(g);
          $(this).show();
           //$(".FinitureSelectorItem[data-testo*='" + $(this).attr("data-testo") + "']").show();
        });


    } 

});

// BINDO I CHECKBOX DELLE FINITURE
$("body").on("change", ".cbfinituraitem", function () {
    //console.info($(this).prop("checked"));
    var selected = $(this).prop("checked");
    var g = $(this).attr("data-group");
    var elementig = $(this).attr("data-element");

    if (selected) {

        $(".FinitureSelectorGroupDiv[data-group='" + g + "']").attr("data-selected", "true");
        $(".FinitureSelectorContentDiv[data-group='" + g + "']").attr("data-selected", "true");
        $(".FinitureSelectorItem[data-element='" + elementig + "']").attr("data-selected", "true");
    }
    else {
        //console.info(elementig); 
        $(".FinitureSelectorItem[data-element='" + elementig + "']").attr("data-selected", "false");

        if ($("FinitureSelectorItem[data-group='" + g + "'][data-selected='true']").length > 0) {
            // ci sono altri elementi selezionati nel gruppo
            $(".FinitureSelectorGroupDiv[data-group='" + g + "']").attr("data-selected", "true");
            $(".FinitureSelectorContentDiv[data-group='" + g + "']").attr("data-selected", "true");
        }
        else {
            // non ci sono altri elementi selezionati nel gruppo
            $(".FinitureSelectorGroupDiv[data-group='" + g + "']").attr("data-selected", "false");
            $(".FinitureSelectorContentDiv[data-group='" + g + "']").attr("data-selected", "false");
        }

    }
});

// BINDO I CHECKBOX DI TUTTO IL GRUPPO
$("body").on("change", ".cbfinituragruppo", function () {
    var g = $(this).attr("data-group");
    var selected = $(this).prop("checked");
    //console.info(g, selected, selected.toLowerCase);
    $(".FinitureSelectorGroupDiv[data-group='" + g + "']").attr("data-selected", selected);
    $(".FinitureSelectorContentDiv[data-group='" + g + "']").attr("data-selected", selected);
    $(".FinitureSelectorItem[data-group='" + g + "']").attr("data-selected", selected);
    $(".cbfinituraitem[data-group='" + g + "']").prop("checked", selected);

    $(".FinitureSelectorGroupDiv[data-selected='true']").show();
    $(".FinitureSelectorContentDiv[data-selected='true']").show();
    $(".FinitureSelectorItem[data-selected='true']").show();
});
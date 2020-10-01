// struttura la griglia js
var grid;


var editPage = function (rname,itemgroup, editLang) {

    let purl = "/admin/resources/getEditor";
    localStorage.clear();
    localStorage.itemgroup = itemgroup;
    localStorage.lang = lang;
    localStorage.rname = rname;

    // compilo pageeditor
    $.post(purl, { "rname": rname, "itemgroup": itemgroup, "lang": editLang }, function (data) {
        $(".pageeditor").html(data);
        scrollUp();
    })


};

//function endEdit() {

//    //$('#contenteditor').slideUp("slow");
//    malert("I dati sono stati salvati con successo.");
//    // se ci sono altre lingue tengo aperto l'edit
//    if ($(".langtab").length == 1) {
//        $('.contenteditor').slideUp("slow");
//    }
//    grid.rebind()
//}

//function cancel() {
//    resettaform()
//    $('#hfmode').val('')
//    $('.contenteditor').slideUp("slow");

//}



grid = $("#grid").jltable(
    {

        columns: [

            // COLONNA 0
            { title: "Pagina", fieldName: "titolo", columnType: "string", width: "auto", visible: true, columnName: "colnome", style: { "text-align": "left", "padding-left": "20px" } },
            // COLONNA 6 -- COLONNA BUTTON -- SOLO PULSANTI
            {
                title: "",
                width: "200px",
                visible: true,
                columnName: "col5",
                columnType: "button",
                buttons: [
                    // PULSANTE 2 DI TIPO LINK SENZA COMMAND
                    {
                        text: 'MODIFICA',
                        commandName: 'modifica',
                        commandArgument: 'modifica',
                        style: { 'padding': '6px', 'float': 'right', 'margin-right': '10px' },

                    }
                ],

                style: { "text-align": "center", "min-width": "150px", "padding-right": "10px" },
                allowSearch: false,
                allowSort: false
            }


        ], // FINE COLONNE

        dataSource: [],// non impostare
        // PARAMETRO PER CHIAMARE IL WEBSERVICE CHE RITORNA UN OGGETTO JSON
        connector: {
            serverPath: '/admin/resources/' + rname,
            source: {
                serviceName: 'getData',
                preload: true
                
            },
            deleterow: {
                serviceName: 'deletePage'
               
            }


        },
        // VALORI DI DEFAULT -- SONO OPZIONALI

        width: "100%", //LARGHEZZA RSPETTO AL DIV CONTENITORE
        headerGridColor: "#b7b7b7", // COLORE DELLA GRIGLIA NELL'HEADER
        showGrid: true, // MOSTRA LA GRIGLIA
        gridColor: "#b7b7b7", // COLORE DELLA GRIGLIA
        oddBackground: "#fffcf6", // BACKGROUND DELLE RIGHE DISPARI
        evenBackground: "#fff", // BACKGROUND DELLE RIGHE DISPARI
        currencySymbol: "€", // SIMBOLO PER IL SIMBOLO DEL CURRENCY
        headerBorderRadius: 10, // BORDO ARROTONDATO DELL'HEADER DA IMPOSTARE IN CONCOMITANZA DEI BORDI ARROTONDATI DEL DIV CONTENITORE
        pageSize: 100, // DIMENSIONE DELLA PAGINA
        showtoolbar: false,
        // TOOBAR
        toolbar: {

            buttons: [
                // PULSANTE 1 DI TIPO COMMAND -- ESEGUE LA FUNZIONE COMMAND RITORNANDO COMMANDNAME,COMMANDARGUMENT E LA RIGA DEL DATASOURCE
                {
                    text: 'NUOVO PRODOTTO',
                    commandName: 'nuovo',
                    commandArgument: 'nuovo',
                    style: { 'text-align': 'center', 'width': '170px', 'float': 'right', 'padding': '6px', 'float': 'right' }
                }


            ]

        },
        // VIENE ESEGUITA AD OGNI PULSANTE CON COMMAND PREMUTO
        command: function (cname, carg, row) {
            //CNAME=COMMANDNAME
            //CARG=COMMANDARGUMENT
            //ROW=RIGA DEL DATASOURCE
            //alert(carg);
            switch (carg) {
                case 'elimina':

                    let btsi = {
                        text: "Si",
                        onclick: function () {
                            mbox.hide();
                            grid.deleteRow(row['itemgroup']);
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

                    break;
                case 'modifica':

                    editPage(row["rname"],row.itemgroup, 'it');
                    
                    break;
                case 'duplica':
                    // non implementato
                    break;
                case 'nuovo':
                    // non implementato
                    break;


            }

        },
        onbind: function (row) {
            // SIMILE ALL'EVENTO ONITEMDATABOUND DI .NET
            // SI SCATENA PRIMA DI COMPILARE LA RIGA HTML PER CUI SI POSSONO CAMBIARE I VALORI DELLA RIGA PASSATA
            //ROW=RIGA DEL DATASOURCE PRIMA DEL BIND DELLA GRIGLIA HTML
            row["visualizza"] = row["visualizza"] == true ? "Si" : "No";
            row["prodottocomp"] = "<div class=\"line_titolo\">" + row["titolo"] + "</div><div class=\"line_descrizione\">" + row["descrizione"] + "</div><div class=\"line_scheda\">" + row["scheda"] + "</div>";
            row["metatitle"] = "<div class=\"line_metatitle\"><b>Title:</b> " + row["metatitle"] + "</div><div class=\"line_metadescription\"><b>Description:</b> " + row["metadescription"] + "</div>";
        }
    }
);







﻿// struttura la griglia js
var grid;
var resourceName = "Transazioni";


var editPage = function (rname, itemgroup, editLang) {

    if (editLang === "it") {
        resetlang();
    }

    let purl = "/admin/resources/getEditor"

    // salvo i parametri nello storage
    localStorage.clear();
    localStorage.itemgroup = itemgroup;
    localStorage.lang = editLang;
    localStorage.rname = rname;

    // compilo pageeditor
    $.post(purl, { "rname": resourceName, "itemgroup": itemgroup, "lang": editLang }, function (data) {
        $(".pageeditor").html(data);
        scrollUp();
    })

};

grid = $("#grid").jltable(
    {

        columns: [

            { title: "Data", fieldName: "datastr", columnType: "string", width: "auto", visible: true, columnName: "c0", style: { "text-align": "left", "padding-left": "30px" } },
            // COLONNA 2
            { title: "Riferimento Globo", fieldName: "refglobo", columnType: "string", width: "auto", visible: true, columnName: "c1", style: { "text-align": "left", "padding-left": "30px" } },
            { title: "Ragione Sociale", fieldName: "intestatario", columnType: "string", width: "auto", visible: true, columnName: "c2", style: { "text-align": "left", "padding-left": "30px" } },
            { title: "Importo", fieldName: "importo", columnType: "currency", width: "auto", visible: true, columnName: "c6", style: { "text-align": "right", "padding-right": "30px" } },
            { title: "Esito", fieldName: "esito", columnType: "string", width: "auto", visible: true, columnName: "c3", style: { "text-align": "left", "padding-left": "30px" } },
            { title: "Num. Rif.", fieldName: "refnumber", columnType: "string", width: "auto", visible: true, columnName: "c4", style: { "text-align": "left", "padding-left": "30px" } },
            { title: "Id Ordine", fieldName: "oid", columnType: "string", width: "auto", visible: true, columnName: "c5", style: { "text-align": "left", "padding-left": "30px" } },

            // COLONNA 6 -- COLONNA BUTTON -- SOLO PULSANTI
            {
                title: "",
                width: "80px",
                visible: true,
                columnName: "col5",
                columnType: "button",
                buttons: [
                    // PULSANTE 2 DI TIPO LINK SENZA COMMAND
                    {
                        text: 'ELIMINA',
                        commandName: 'elimina',
                        commandArgument: 'elimina',
                        style: { 'padding': '6px', 'float': 'right' }
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
            serverPath: '/admin/resources/',
            source: {
                serviceName: 'getResource',
                customData: [
                    { name: "rname", val: rname }
                ]

            },
            totalitem: {
                serviceName: 'Count',
                customData: [
                    { name: "rname", val: rname }
                ]
            },
            deleterow: {
                serviceName: 'delete',
                customData: [
                    { name: "rname", val: rname }
                ]
            },
            reorder: {
                serviceName: 'reorder',
                customData: [
                    { name: "rname", val: rname }
                ]
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
        pageSize: 50, // DIMENSIONE DELLA PAGINA
        allowReorder: false,
        showtoolbar: false,
        // TOOBAR
        //toolbar: {

        //    buttons: [
        //        // PULSANTE 1 DI TIPO COMMAND -- ESEGUE LA FUNZIONE COMMAND RITORNANDO COMMANDNAME,COMMANDARGUMENT E LA RIGA DEL DATASOURCE
        //        {
        //            text: 'NUOVA CATEGORIA',
        //            commandName: 'nuovo',
        //            commandArgument: 'nuovo',
        //            style: { 'text-align': 'center', 'width': '170px', 'float': 'right', 'padding': '6px' }
        //        }


        //    ]
        //},
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

                    editPage(resourceName, row.itemgroup, 'it');


                    break;
                case 'duplica':
                    // non implementato
                    break;
                case 'nuovo':
                    editPage(resourceName, 0, 'it');
                    break;


            }

        },
        onbind: function (row) {
            // console.info(row);
            // SIMILE ALL'EVENTO ONITEMDATABOUND DI .NET
            // SI SCATENA PRIMA DI COMPILARE LA RIGA HTML PER CUI SI POSSONO CAMBIARE I VALORI DELLA RIGA PASSATA
            //ROW=RIGA DEL DATASOURCE PRIMA DEL BIND DELLA GRIGLIA HTML
            // row['ismenu'] = row['ismenu'] == '1' ? 'Si' : 'No';
            // row['visibile'] = row['visibile'] == '1' ? 'Si' : 'No';
            //row['pdf'] = '<a target="_blank" href="' + row['pdf'] + '">' + row['pdf'] + '</a>';

        },
        onreordered: function (movedir, movedrow, targetrow) {
            //console.info(movedir);
            grid.reorderRow(movedir, movedrow["itemgroup"], targetrow["itemgroup"], movedrow["ordinamento"], targetrow["ordinamento"]);
        }
    }
);






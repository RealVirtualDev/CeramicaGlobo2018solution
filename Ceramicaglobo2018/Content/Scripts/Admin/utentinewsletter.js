// struttura la griglia js
var grid;



var editPage = function (id) {

    let purl = "/admin/utentinewsletter/getEditor"
    localStorage.clear();
    localStorage.lang = "it"; // x amministratori è indifferente
    localStorage.pname = "";

    // compilo pageeditor
    $.post(purl, { "id": id}, function (data) {
        $(".pageeditor").html(data);
        scrollUp();
    })


};


grid = $("#grid").jltable(
    {

        columns: [

            // COLONNA 1
            { title: "Ragione Sociale", fieldName: "ragionesociale", columnType: "string", width: "auto", visible: true, columnName: "c1", style: { "text-align": "center" } },
            { title: "Professione", fieldName: "professione", columnType: "string", width: "auto", visible: true, columnName: "c2", style: { "text-align": "center" } },
            { title: "Email", fieldName: "email", columnType: "string", width: "auto", visible: true, columnName: "c4", style: { "text-align": "center" } },
            { title: "Attivo", fieldName: "attivostr", columnType: "string", width: "auto", visible: true, columnName: "c5", style: { "text-align": "center" } },
            { title: "Lingua", fieldName: "lingua", columnType: "string", width: "auto", visible: true, columnName: "c7", style: { "text-align": "center" } },
            // COLONNA 6 -- COLONNA BUTTON -- SOLO PULSANTI
            {
                title: "",
                width: "200px",
                visible: true,
                columnName: "col5",
                columnType: "button",
                buttons: [
                    // PULSANTE 1 DI TIPO COMMAND -- ESEGUE LA FUNZIONE COMMAND RITORNANDO COMMANDNAME,COMMANDARGUMENT E LA RIGA DEL DATASOURCE
                    {
                        text: 'ELIMINA',
                        commandName: 'elimina',
                        commandArgument: 'elimina',
                        style: { 'padding': '6px', 'float': 'right' }
                    },
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
            serverPath: '/admin/utentinewsletter/',
            source: {
                serviceName: 'getData'
                //,tablename: 'pageinfo',
                // where: ''
            },
            deleterow: {
                serviceName: 'delete'
                
                //tableName: 'pageinfo'
            },
            totalitem: {
                serviceName: 'Count',
                customData: [
                    { name: "lang", val: "it" }
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
        pageSize: 100, // DIMENSIONE DELLA PAGINA
        showtoolbar: true,
        // TOOBAR
        toolbar: {

            buttons: [
                // PULSANTE 1 DI TIPO COMMAND -- ESEGUE LA FUNZIONE COMMAND RITORNANDO COMMANDNAME,COMMANDARGUMENT E LA RIGA DEL DATASOURCE
                {
                    text: 'NUOVO UTENTE',
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
                            grid.deleteRow(row['id']);
                           
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
                    editPage(row["id"]);

                    break;
                case 'duplica':
                    // non implementato
                    break;
                case 'nuovo':
                    editPage(0);
                    // non implementato
                    break;


            }

        },
        onbind: function (row) {
            // SIMILE ALL'EVENTO ONITEMDATABOUND DI .NET
            // SI SCATENA PRIMA DI COMPILARE LA RIGA HTML PER CUI SI POSSONO CAMBIARE I VALORI DELLA RIGA PASSATA
            //ROW=RIGA DEL DATASOURCE PRIMA DEL BIND DELLA GRIGLIA HTML
            row["attivostr"] = row["attivo"] == "1" ? "Si" : "No";
        }
    }
);







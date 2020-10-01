/*! latest update 17/06/2014
 * jQuery JLGRID ovvero jlusigrid by LuSi
 * Copyright 2013-2016, Ludovici Simone
 * You need to buy a license if you want use this script.
 * write at web@lusilab.it
 * Version: 2.0.0 (Jan 05 2013)
 * last update 25/05/2016
 */



(function ($) {
    $.fn.jltable = function (options) {


        // SEMPLICISSIMA FORMATTAZIONE DELLA DATA
        Date.prototype.format = function (formatstring, _datestr) {

            var result;

            var d;

            if (_datestr) {

                // MYSQL RITORNA UNA DATA CON AAAA-MM-GGT00.00.00
                var _d2 = _datestr.split("T")[0].substring(0, 10).replace(/\//g, "-");
                var matches = _d2.match('^(\\d{4})-(\\d{2})-(\\d{2})$');
                var matches2 = _d2.match('^(\\d{2})-(\\d{2})-(\\d{4})$');

                // formato yyyy-mm-dd
                if (matches) {

                    d = new Date();

                    d.setFullYear(parseInt(matches[1]));
                    d.setMonth(parseInt(matches[2]) - 1);
                    d.setDate(parseInt(matches[3]));

                }
                // formato dd-mm-yyyy
                if (matches2) {

                    d = new Date();

                    d.setFullYear(parseInt(matches2[3]));
                    d.setMonth(parseInt(matches2[2]) - 1);
                    d.setDate(parseInt(matches2[1]));
                }


            }
            else {
                d = this;
            }

            _m = '00' + (d.getMonth() + 1);
            _a = d.getFullYear();
            _d = '00' + d.getDate();

            _m = _m.substring(_m.length - 2, _m.length);
            _d = _d.substring(_d.length - 2, _d.length);


            switch (formatstring) {

                case "dd-mm-yyyy":
                    result = _d + '-' + _m + '-' + _a;
                    break;
                case "dd/mm/yyyy":
                    result = _d + '/' + _m + '/' + _a;
                    break;
                case "yyyy-mm-dd":
                    result = _a + '-' + _m + '-' + _d;
                    break;
                case "yyyy/mm/dd":
                    result = _a + '/' + _m + '/' + _d;
                    break;

            }

            return result;
        };

        // ARRAY CLONE prototype
        if (!Array.prototype.clone) {
            Array.prototype.clone = function () {
                var arr1 = new Array();
                for (var property in this) {
                    arr1[property] = typeof (this[property]) == 'object' ? this[property].clone() : this[property]
                }
                return arr1;
            };
        };



        // AGGIUNGE UN ALEMENTO ALL'ARRAY SE NON ESISTE GIA
        Array.prototype.mergeUnique = function (arraytomerge, comparer) {
            var a = this;

            for (var i = 0; i < arraytomerge.length; ++i) {
                var toadd = true;
                for (var j = 0; j < this.length; ++j) {
                    var el = this[j];

                    if (comparer(el, arraytomerge[i])) {
                        toadd = false;
                    }

                }

                if (toadd) {
                    a.push(arraytomerge[i])

                }

            }

            return a;
        };

        // ESEMPIO pushIfNotExist

        //var res = a1.mergeUnique(a2, function (el,e) {
        //    return el["a"] === e["a"];
        //});

        // CLONA DATASOURCE
        function clonadatasource(source) {
            var result = new Array;
            var i = 0;
            $(source).each(function () {

                result[i] = this;
                i++;
            })

            return result
        }


        var myself = this; // mi serve per chiamare le api internamente

        // valori di default
        var config = {
            gridName: "lusigrid",

            // COLONNE *******************************************************************************
            // title: titolo della colonna
            // fieldName: nome del campo del datasource
            // width: larghezza della colonna
            // visible: true|false
            // columnName: nome univoco della colonna
            // style: oggetto key,value con gli attributi di stile da passare alla cella della colonna
            // link: link per il dato
            // linktarget: target del link
            // imagepath: path base in caso di colonna immagine
            // columnType: image|currency|integer|decimal|string|date|button
            // buttons: array di pulsanti: commandName: nome del command,commandArgument: argomento del command,text: label del pulsante,style:stili
            // text: testo della colonna, se passato il fieldName non viene considerato
            // dataField: lista field (array) da esporre in td come html5 data- (es. data-[nomefield]=[valorefield])
            // ***************************************************************************************
            //{
            //    title: "Colonna 3", fieldName: "campo3", width: "100px", visible: false, columnName: "col3", columnType: "image",
            //    images:[
            //        { alt: "", fieldName: "campo3", path: '/public/products/min/', link: { url: 'http://www.lusilab.it', target: '_blank'}, style: { "border": "solid 1px black" } }
            //    ],
            //    style: { "text-align": "center" },
            //  dataField:{db:['dbcol1','dbcol2'],fixed:{"f1":"v1","f2":"v2"}}, [OGGETTO] aggiunge data-dbcol1="valdb1" ...
            //  addClass:['class1','class2'], aggiunge classi al td
            //},
            columns: [],
            dataSource: [],
            dataField: [], // lista field (array) da esporre in tr come html5 data- (es. data-[nomefield]=[valorefield])
            addClass: [], // classi aggiuntive sul TR
            dataSourceCount: 0,
            toolbar: [],
            showtoolbar: false,
            connector: {

                //serverPath: 'services/datab.asmx/',
                //source: {
                //    serviceName: 'getarticoli',
                //    preload: true
                //},
                //deleterow: {
                //    serviceName: 'delete',
                //    tableName: 'articoli'
                //},
                //reorder: {
                //    serviceName: 'reorder',
                //    tableName: 'articoli'
                //}
            },
            width: "100%",
            headerGridColor: "#dcdcdc",
            showGrid: true,
            gridColor: "#eeeeee",
            oddBackground: "#f2fbff",
            evenBackground: "#fff",
            currencySymbol: "€",
            headerBorderRadius: 10,
            allowReorder: false,
            reorderIndicatorOffsetY: 9,
            pageSize: 15,
            // EVENTI
            // IL COMMAND
            command: function (commandName, commandArgument, row) { return row; },
            // IL DATABINDING
            onbind: function (row) { return row; },
            onreordered: function (direction, draggedrow, targetrow) { }
        };
        // VARIABILI PERSISTENTI USATE INTERNAMENTE
        var status = {
            sortField: '',
            sortDir: 'none',
            sortColumnType: 'string',
            currentPage: 1,
            currentDataSource: [],
            pages: [],
            totalPages: 0,
            latestFilteredColumn: '',
            curretFilteredColumn: '',
            currentFilteredText1: '',
            currentFilteredText2: '',
            currentFilteredValue1: '',
            currentFilteredValue2: '',
            currentFilteredOperator: '',
            currentFilteredField: '',
            customFilter: false,
            customFilterCommand: [],
            customSearch: false,
            customSearchCommand: [],
            customSearchQuery: '',
            nextPage: 1,
            prevPage: 1,
            fullLoaded: false

        };

        // EFFETTUO IL MERGE DELLE OPZIONI DI DEFAULT CON QUELLE PASSATE
        if (options) $.extend(true, config, options);

        // TABELLA 
        var gridtag = '<table style="border-collapse: collapse;width:100%"><thead><tr class="jlgrid_columns_' + config.gridName + '"></tr>';

        // TOOLBAR TODO DROPDOWN
        if (config.showtoolbar) {
            gridtag += '<tr><td class="jlgrid_toolbar_' + config.gridName + '" style="padding:10px;border-bottom:solid 1px ' + config.gridColor + '" colspan="' + (config.columns.length + (config.allowReorder ? 1 : 0)) + '">';

            if (config.toolbar.buttons) {

                $(config.toolbar.buttons).each(function (btindex, btvalue) {

                    var _vis = (this.visible != undefined ? this.visible : true);
                    if (_vis) {

                        var _btstyle = ''
                        // AGGIUNGE LO STILE DEL PULSANTE SE PASSATO
                        if (this.style) {
                            $.each(this.style, function (i, v) {
                                _btstyle += i + ':' + v + ';';
                            });
                        }

                        _url = '';
                        gridtag += '<a ' + _url + ' data-commandname="' + this.commandName + '" data-commandargument="' + this.commandArgument + '" style="' + _btstyle + '" class="jlgrid_toolbarbuttons_' + config.gridName + ' jlgrid_toolbarbutton_' + config.gridName + '_' + (this.commandName ? this.commandName : 'nocommand') + '">' + this.text + '</a>'

                    };

                });
                // BINDA


            }

            gridtag += '</td></tr>'
        }

        gridtag += '</thead><tbody></tbody></table><div id="jlgrid_paginator_' + config.gridName + '"></div>';

        // CONVERTO IN OGGETTO JQUERY
        var grid = $(gridtag);


        // CARICO SOLO LA PAGINA 1 E POPOLO IL RESTO DEL DATASOURCE IN BACKGROUND

        function loadPage(numpage, callback) {
            // load
            //config.dataSource = [];
            let urlPost = config.connector.serverPath + config.connector.source.serviceName;
            let urlData = { pagesize: config.pageSize, currentpage: numpage };
            // parametri custom
            if (config.connector.source.customData) {
                $.each(config.connector.source.customData, (i, v) => urlData[v["name"]] = v["val"]);
            }
            // carico la pagina passata
            $.post(urlPost, urlData, function (data) {
                $.each(data, (i, el) => config.dataSource.push(el));
                // scateno evento onbind prima di costruire la griglia
                $.each(config.dataSource, function (index, value) { config.onbind(config.dataSource[index]); });

                //console.info(config.dataSource);
                if (numpage == 1) {
                    status.fullLoaded = false;
                    dataBind(status.currentPage);
                    addRowEvents();
                }
                //console.info(status.pages);
                status.pages[parseInt(numpage) - 1].status = "loaded";

                // carico tutte le altre pagine in background
                
                if (numpage < status.totalPages) {

                    loadPage(numpage + 1);
                }
                else {
                   
                    status.fullLoaded = true;
                    buildPaginator();
                    addPaginatorEvents();
                }
                
            })


        }

        // UN WORKAROUND PER VELOCIZZARE IL CARICAMENTO PER GRANDI QUANTITA DI DATI
        function loadData() {

            // ottengo il numero di pagine totali
            let urlPostCount = config.connector.serverPath + config.connector.totalitem.serviceName;
            let urlDataCount = {};
            // parametri custom per il conteggio
            if (config.connector.totalitem.customData) {
                $.each(config.connector.totalitem.customData, function (i, v) {
                    urlDataCount[v["name"]] = v["val"];
                })
            }
            //
            //Conteggio Item Totali;
            $.post(urlPostCount, urlDataCount, function (data) {
                status.totalPages = Math.ceil(parseInt(data) / parseInt(config.pageSize));
                if (status.totalPages == 0) status.totalPages = 1;
                // array per tenere traccia dello stato delle pagine
                for (let num = 0; num < status.totalPages; num++) {
                    status.pages.push({ numpage: num, status: "empty" });
                }
                loadPage(1);


            });



        };

        // COSTRUISCI LA TENDINA DELLA RICERCA
        function createSearchPopup(colname, coltype, colfieldname) {

            if (coltype == undefined) coltype = 'string';

            var _pop = '<div data-fieldname="' + colfieldname + '" style="text-align:left;text-transform:none; padding:10px;position:absolute;width:205px;height:230px;margin-left:-12px;display:none;" class="jlgrid_searchpops_' + config.gridName + ' jlgrid_searchpop_' + colname + '_' + config.gridName + '">';
            _pop += 'Testo da cercare<br/>';
            _pop += '<select id="jlgrid_searchdrop1_' + colname + '_' + config.gridName + '" style="color:gray;font-size:11px;padding:3px;width:100%;border:solid 1px #dcdcdc;">';

            // PER COLONNE STRINGA
            if (coltype == "string" || coltype == "" || coltype == undefined) {
                _pop += '<option value="contains">Contiene</option>';
                _pop += '<option value="notcontains">Non Contiene</option>';
                _pop += '<option value="equal">Uguale a</option>';
                _pop += '<option value="notequal">Diverso da</option>';
                _pop += '<option value="startwith">Inizia con</option>';
                _pop += '<option value="endwith">Finisce con</option>';
            };
            // NUMERICO E DATE
            if (coltype == "integer" || coltype == "currency" || coltype == "decimal" || coltype == "date") {
                _pop += '<option value="equal">Uguale a</option>';
                _pop += '<option value="notequal">Diverso da</option>';
                _pop += '<option value="greater">Maggiore di</option>';
                _pop += '<option value="greaterorequal">Maggiore o uguale a</option>';
                _pop += '<option value="less">Minore di</option>';
                _pop += '<option value="lessorequal">Minore o uguale a</option>';
            };

            _pop += '</select>';
            _pop += '<input id="jlgrid_searchtext1_' + colname + '_' + config.gridName + '" style="margin-top:5px;color:gray;font-size:11px;padding:3px;width:100%;border:solid 1px #dcdcdc;" type="text"></input><br/>';
            _pop += 'Opzionale';
            _pop += '<br/>';
            _pop += '<select id="jlgrid_concatsearchdrop_' + colname + '_' + config.gridName + '" style="color:gray;font-size:11px;padding:3px;width:100%;border:solid 1px #dcdcdc;"><option value="and">E</option><option value="or">OPPURE</option></select>';
            _pop += 'Testo da cercare<br/>';

            _pop += '<select id="jlgrid_searchdrop2_' + colname + '_' + config.gridName + '" style="color:gray;font-size:11px;padding:3px;width:100%;border:solid 1px #dcdcdc;">';
            // PER COLONNE STRINGA
            if (coltype == "string" || coltype == "" || coltype == undefined) {
                _pop += '<option value="contains">Contiene</option>';
                _pop += '<option value="notcontains">Non Contiene</option>';
                _pop += '<option value="uqual">Uguale</option>';
                _pop += '<option value="notequal">Diverso</option>';
                _pop += '<option value="startwith">Inizia con</option>';
                _pop += '<option value="endwith">Finisce con</option>';
            };
            // NUMERICO E DATE
            if (coltype == "integer" || coltype == "currency" || coltype == "decimal" || coltype == "date") {
                _pop += '<option value="equal">Uguale a</option>';
                _pop += '<option value="notequal">Diverso da</option>';
                _pop += '<option value="greater">Maggiore di</option>';
                _pop += '<option value="greaterorequal">Maggiore o uguale a</option>';
                _pop += '<option value="less">Minore di</option>';
                _pop += '<option value="lessorequal">Minore o uguale a</option>';

            };
            _pop += '</select>';
            _pop += '<input id="jlgrid_searchtext2_' + colname + '_' + config.gridName + '" style="margin-top:5px;color:gray;font-size:11px;padding:3px;width:100%;border:solid 1px #dcdcdc;" type="text"></input><br/>';

            // pulsanti di ricerca
            _pop += '<a data-columntype="' + coltype + '" data-columnname="' + colname + '"  class="jlgrid_buttons_' + config.gridName + ' jlgrid_buttoncerca_' + config.gridName + '" style="float:right;margin-top:10px;">CERCA</a>';
            _pop += '<a data-columnname="' + colname + '" class="jlgrid_buttons_' + config.gridName + ' jlgrid_buttoncercaclear_' + config.gridName + '" style="float:right;margin-top:10px;margin-right:10px;">RESETTA</a>';
            _pop += '</div>';


            return _pop;
        };

        // INIZIALIZZA LA GRIGLIA
        function init() {

            loadData();


            var _innerheader, _innerbody, _innerfooter;
            _innerheader = "";

            // INSERISCO IL REORDER?
            if (config.allowReorder) {

                _innerheader += '<th class="jltable_header_th_' + config.gridName + '"  id="jltable_header_th_reorder_' + config.gridName + '" style="width:10px"></th>';

            };

            $(config.columns).each(function (index, value) {
                _vis = this.visible != undefined ? this.visible : true;
                if (_vis) {

                    var _coltype = 'string';
                    var _allowSearch = this.allowSearch != undefined ? this.allowSearch : true;
                    var _allowSort = this.allowSort != undefined ? this.allowSort : true;

                    if (this.columnType) _coltype = this.columnType;

                    // TH DELLA CELLA
                    _innerheader += '<th class="jltable_header_th_' + config.gridName + '" data-allowsearch="' + _allowSearch + '" data-allowsort="' + _allowSort + '" data-fieldname="' + this.fieldName + '" id="jltable_header_th_' + this.columnName + '_' + config.gridName + '" style="border-right:solid 1px ' + config.headerGridColor + ';width:' + this.width + '"><div style="padding:10px;position:relative;">';

                    if (_allowSearch) {
                        // div della ricerca
                        _innerheader += createSearchPopup(this.columnName, this.columnType, this.fieldName);
                        // LINK DEL PULSANTE CERCA
                        _innerheader += '<a id="jlgrid_btsearch_' + this.columnName + '_' + config.gridName + '" class="btSearchHeader" data-columnname="' + this.columnName + '" style="position:absolute;top:50%;margin-top:-10px;left:5px;width:20px;height:20px;display:block;cursor:pointer;"></a>';

                    };

                    // DIV DELLA LABEL DELLA COLONNA
                    _innerheader += '<div id="jlgrid_column_label_' + this.columnName + '_' + config.gridName + '" data-columnname="' + this.columnName + '" data-fieldname="' + this.fieldName + '" data-columntype="' + _coltype + '" data-sort="none"  class="jlgrid_column_label_' + config.gridName + '" style="cursor:pointer;white-space:nowrap;padding-right:22px;padding-left:22px;">' + this.title + '</div></div></th>';


                };
            }
            );

            // AGGIUNGE L'HEADER
            $('thead tr.jlgrid_columns_' + config.gridName, grid).html(_innerheader);
            // ELIMINA L'ULTIMO BORDO DELLE CELLE DELLE COLONNE
            $("th", grid).last().css("border-right", "none");
            // NASCONDI I PULSANTI CERCA
            $(".btSearchHeader", grid).hide();

            // inserisci bordo stondato
            $("th", grid).last().css('border-radius', '0 ' + config.headerBorderRadius + 'px 0 0');
            $("th", grid).first().css('border-radius', config.headerBorderRadius + 'px 0 0 0');


            //if (_preload == true) {
            //    $(".jlpageloading_" + config.gridName).show();
            //    loadData(false);

            //}


        };

        // BINDA GLI EVENTI DELL'grid
        function addEvents() {
            //  BINDA IL CLICK DEL PULSANTE CERCA
            $(".btSearchHeader", grid).on("click", function (event) {

                event.stopPropagation();
                var _colname = $(this).attr("data-columnname")
                $(".jlgrid_searchpops_" + config.gridName + ":not(.jlgrid_searchpop_" + _colname + "_" + config.gridName + ")").slideUp();
                $(".jlgrid_searchpop_" + _colname + "_" + config.gridName).slideToggle();

            });

            // BIND DEGLI EVENTI PER LA LABEL DELLA COLONNA E PER MOSTRARE IL PULSANTE CERCA

            $('[data-allowsort="true"] .jlgrid_column_label_' + config.gridName, grid).on("click", function () {
                // CAMBIA ORDINAMENTO
                var _datafieldname = $(this).attr("data-fieldname");
                var _coltype = $(this).attr("data-columntype");
                var _colsort = $(this).attr("data-sort");
                var _colname = $(this).attr("data-columnname");
                changesort(_colname, _coltype, _colsort, _datafieldname);

            });

            // PULSANTI CERCA ALL'INTERNO DEI POPUP DI RICERCA
            $('[data-allowsort="true"] .jlgrid_buttoncerca_' + config.gridName, grid).on("click", function () {

                var _colname = $(this).attr("data-columnname");
                var _coltype = $(this).attr("data-columntype");
                cerca(_colname, _coltype);
            });


            // PULSANTI CANCELLA RICERCA CERCA ALL'INTERNO DEI POPUP DI RICERCA
            $('[data-allowsort="true"] .jlgrid_buttoncercaclear_' + config.gridName, grid).on("click", function () {

                var _colname = $(this).attr("data-columnname");
                resettacerca(_colname);
            });


            $(".jltable_header_th_" + config.gridName + '[data-allowsearch="true"]', grid).on({

                mouseenter: function () {
                    // MOSTRA IL PULSANTE CERCA
                    $("div", this).children("a.btSearchHeader").show();

                },
                mouseleave: function () {
                    // NASCONDE IL PULSANTE CERCA SOLO SE LA COLONNA NON è FILTRATA
                    var _colname = $("div", this).children("a.btSearchHeader").attr("data-columnname");
                    if (status.curretFilteredColumn != _colname) $("div", this).children("a.btSearchHeader").hide();
                }
            }
            );

            // PULSANTI NELLA TOOLBAR

            if (config.toolbar.buttons) {

                $(config.toolbar.buttons).each(function (i, v) {
                    $button = this;
                    $(".jlgrid_toolbarbutton_" + config.gridName + '_' + this.commandName, grid).on("click", function (event) {

                        event.stopPropagation();
                        var _cname = $(this).attr("data-commandname");
                        var _carg = $(this).attr("data-commandargument");

                        config.command(_cname, _carg)

                    });
                });

            }



        };

        // BINDA GLI EVENTI DEL BODY
        function addRowEvents() {

            // CICLA LE COLONNE PER AGGIUNGERE EVENTUALI COMMAND
            $(config.columns).each(function (index, value) {

                if (this.buttons) {
                    $this = this.buttons
                    //  BINDA IL CLICK DEL PULSANTE CERCA
                    $.each($this, function (bindex, bvalue) {
                        bt = this;

                        $(".jlgrid_button_" + config.gridName + '_' + this.commandName, grid).on("click", function (event) {
                            event.stopPropagation();
                            cname = $(this).attr("data-commandname")
                            carg = $(this).attr("data-commandargument")
                            _rowindex = $(this).attr("data-rowindex")
                            config.command(cname, carg, status.currentDataSource[_rowindex])

                        });
                    });
                };
            });

            // IN LAVORAZIONE SELEZIONA LA RIGA COL CLICK
            $('tbody tr', grid).on("click", function (event) {
                $(this).toggleClass("jlgrid_selectedrow_" + config.gridName)
            });

            // INSERISCO IL REORDER ?
            if (config.allowReorder) {

                // QUANDO PREMO IL TASTO SINISTRO DEL MOUSE SULLA MANIGLIA
                $('td.td_reorder', grid).on("mousedown", function (event) {
                    var _rowindex = $(this).attr("data-idx");
                    // solo se è premuto il tasto sinistro

                    if (event.which == 1) reorder.startDrag(_rowindex, (event.pageX), event.pageY);


                });
                // QUANDO RILASCIO IL TASTO SINISTRO DEL MOUSE SULLA GRIGLIA
                $(myself).on("mouseup", function (event) {


                    var _rowindex = $(this).attr("data-rowindex");
                    // solo se è premuto il tasto sinistro
                    if (event.which == 1) {
                        if (reorder.status.isdragging) reorder.stopDrag();

                    };


                });

            };


        }

        function textSelection(target, enable) {

            if (typeof target.onselectstart != "undefined") //For IE 
                target.onselectstart = function () { return enable }
            else if (typeof target.style.MozUserSelect != "undefined") //For Firefox
            {

                if (enable) { target.style.MozUserSelect = 'text' }
                else { target.style.MozUserSelect = "none" }
            }

            else //All other route (For Opera)
                target.onmousedown = function () { return enable }
            target.style.cursor = enable ? "auto" : "default"

        }

        // OGGETTO PER IL RIORDINAMENTO CON DRAG & DROP
        reorder = {

            draghandle: '<div class="jlgrid_draghandle_' + config.gridName + '"" style="cursor:pointer;display:block;"></div>',
            dragger: '<div id="jlgrid_dragger_' + config.gridName + '" style="cursor:move;display:block;display:none;position:absolute;" >Sposta la Riga</div>',
            imagelayer: '<div id="jlgrid_imagedraglayer_' + config.gridName + '" style="position:absolute;top:0;left:0;width:100%;height:100%;"></div>',
            status: {
                overidx: -1,
                draggeridx: -1,
                isdragging: false,
                releaseidx: -1
            },
            startDrag: function (_rowindex, _x, _y) {

                //document.onselectstart = function () { return false; }; // hack per chrome per il drag
                textSelection(document.body, false);

                reorder.status.isdragging = true;


                $(myself).append(reorder.imagelayer);
                $(myself).append(reorder.dragger);

                _offsetx = $(myself).offset().left;
                _offsety = $(myself).offset().top;
                _dragger = $('#jlgrid_dragger_' + config.gridName)

                _draggeroffsety = Math.ceil(_dragger.height() / 2) + Math.ceil((parseInt(_dragger.css("padding-top").replace('px', '')) + parseInt(_dragger.css("padding-bottom").replace('px', ''))) / 2);

                reorder.status.overidx = _rowindex;
                reorder.status.draggeridx = _rowindex;


                $('#jlgrid_dragger_' + config.gridName).css({ 'left': Math.ceil(_x - _offsetx - 12) + 'px', 'top': Math.ceil(_y - _offsety - _draggeroffsety) + 'px' })


                _theadlenght = $("thead", $(myself)).first().height();
                _trlenght = $("tbody tr", $(myself)).first().height();

                //$("#jlgrid_imagedraglayer_" + config.gridName).css("background-position", "0px " + ((_trlenght * reorder.status.overidx) + _theadlenght - config.reorderIndicatorOffsetY) + "px")
                // nascondo l'indicatore
                $("#jlgrid_imagedraglayer_" + config.gridName).hide();
                var _currtop = 0;

                // BINDO IL MOUSEMOVE SUL DIV PRINCIPALE - SPOSTO IL DRAGGER
                $(myself).on(

                    "mousemove", function (event) {
                        var _showindicator = true;
                        var _offsetcalcolotop = 0;
                        // mostro l'indicatore
                        //console.info(reorder.status.draggeridx < reorder.status.overidx, reorder.status.draggeridx , reorder.status.overidx);

                        if (parseInt(reorder.status.draggeridx) < parseInt(reorder.status.overidx)) {
                            _offsetcalcolotop = 1;
                        }

                        _currtop = 0;
                        // calcolo il top della riga corrente
                        for (var i = 0; i < parseInt(reorder.status.overidx) + parseInt(_offsetcalcolotop); i++) {
                            _trlenght = $($("tbody tr", $(myself))[i]).height();
                            _currtop += _trlenght;
                        }


                        // in aqlcuni casi l'indicatore non va mostrato
                        // non mostro l'indicatore sulla riga trascinata
                        if (reorder.status.draggeridx == reorder.status.overidx) {
                            _showindicator = false;

                        }
                        $('#jlgrid_dragger_' + config.gridName).css({ 'left': Math.ceil(event.pageX - _offsetx - 10) + 'px', 'top': Math.ceil(event.pageY - _offsety - _draggeroffsety) + 'px' })



                        if (_showindicator) {
                            // mostro la freccia del drag
                            //$("#jlgrid_imagedraglayer_" + config.gridName).css("background-position", "0px " + ((_trlenght * reorder.status.overidx) + _theadlenght - config.reorderIndicatorOffsetY) + "px")
                            $("#jlgrid_imagedraglayer_" + config.gridName).css("background-position", "0px " + ((_currtop) + _theadlenght - config.reorderIndicatorOffsetY) + "px")
                            $("#jlgrid_imagedraglayer_" + config.gridName).show();
                        }
                        else { $("#jlgrid_imagedraglayer_" + config.gridName).hide(); }


                    });


                reorder.createOverlayer();


                // 

                $(".jlgrid_dragoverdiv_" + config.gridName).on(
                    {
                        mouseenter: function (event) {
                            _idx = $(this).attr("data-idx")
                            $('tbody tr[data-idx=' + _idx + '] td', myself).addClass("jlgrid_dragoverdiv_active_" + config.gridName)
                            reorder.status.overidx = _idx;

                        }
                        ,
                        mouseleave: function (event) {
                            _idx = $(this).attr("data-idx")
                            $('tbody tr[data-idx=' + _idx + '] td', myself).removeClass("jlgrid_dragoverdiv_active_" + config.gridName)
                            //$(this).removeClass("jlgrid_dragoverdiv_active_" + config.gridName)
                            reorder.status.overidx = -1;
                        }
                    }
                )



                $('#jlgrid_dragger_' + config.gridName).show();
            },
            stopDrag: function (_rowindex) {

                //document.onselectstart = function () { return true; }; // hack per chrome per il drag
                textSelection(document.body, true)


                reorder.status.releaseidx = reorder.status.overidx;

                // qui il codice della chiamata alla funzione di ritorno degli indici
                // ...


                reorder.status.rowidx = -1;

                // bindo il mousemove sul div principale
                $(myself).off("mousemove");
                $('#jlgrid_dragger_' + config.gridName).hide().remove();

                $(".jlgrid_dragoverdiv_" + config.gridName).off("mouseenter")
                $(".jlgrid_dragoverdiv_" + config.gridName).off("mouseleave")
                $('tbody tr td', myself).removeClass("jlgrid_dragoverdiv_active_" + config.gridName)

                reorder.removeOverlayer();
                reorder.status.isdragging = false;

                if (parseInt(reorder.status.overidx) <= status.currentDataSource.length - 1)
                    _rigaover = status.currentDataSource[reorder.status.overidx];
                else
                    _rigaover = { 'id': -1, 'ordinamento': status.currentDataSource.length };


                _rigadragged = status.currentDataSource[reorder.status.draggeridx];
                _dir = (parseInt(reorder.status.overidx) < parseInt(reorder.status.draggeridx)) ? "up" : "down";

                // SCATENA L'EVENTO

                // se sto spostando una riga all'ultimo posto la riga target è null


                if (reorder.status.overidx != reorder.status.draggeridx) config.onreordered(_dir, _rigadragged, _rigaover)

            },
            createOverlayer: function () {

                _theadlenght = $("thead", $(myself)).first().height();
                _trlenght = $("tbody tr", $(myself)).first().height();
                var _currtop = 0; // indice posizione top

                var _numvoci = (status.currentDataSource.length > config.pageSize ? config.pageSize : status.currentDataSource.length);

                for (var i = 0; i <= _numvoci; i++) {
                    // calcolo l'altezza della riga attuale
                    _trlenght = $($("tbody tr", $(myself))[i]).height();
                    //if (i == 0) {
                    //    // il primo div deve prendere le prime 2 righe
                    //    _trlenght += $($("tbody tr", $(myself))[1]).height();

                    //}
                    $(myself).append('<div data-idx="' + i + '" class="jlgrid_dragoverdiv_' + config.gridName + '" style="cursor:move;position:absolute;top:' + (_theadlenght + (_currtop)) + 'px;left:0;width:' + $(myself).width() + 'px;height:' + (_trlenght == 0 ? "10" : _trlenght) + 'px;" ></div>')
                    _currtop += _trlenght;
                }



            },
            removeOverlayer: function () {
                $(".jlgrid_dragoverdiv_" + config.gridName).remove()
                $("#jlgrid_imagedraglayer_" + config.gridName).remove()
            }

        }

        // BINDA GLI EVENTI DEL PAGINATORE
        function addPaginatorEvents() {

            $(".jlgrid_pagelink_" + config.gridName, grid).on("click", function (event) {

                var _pagenumber = $(this).attr("data-pagenumber");
                myself.gotoPage(_pagenumber);

            });
        };

        // CERCA CON POPUP INTEGRATO
        function cerca(colname, coltype) {

            // ESCI SE NON È STATO SPECIFICATO ALCUN VALORE DA CERCARE
            if ($('#jlgrid_searchtext1_' + colname + '_' + config.gridName).val() == '') return;


            // SE PRECEDENTE RICERCA DIVERSA DALL'ATTUALE AZZERA I CAMPI DEL PRECEDENTE POPUP
            if (status.latestFilteredColumn != colname && status.latestFilteredColumn != '') {
                resettacerca(status.latestFilteredColumn, true);
            };

            status.latestFilteredColumn = colname;
            status.curretFilteredColumn = colname;
            status.currentFilteredField = $('.jlgrid_searchpop_' + colname + '_' + config.gridName).attr("data-fieldname")
            status.currentFilteredText1 = $('#jlgrid_searchtext1_' + colname + '_' + config.gridName).val();
            status.currentFilteredValue1 = $('#jlgrid_searchdrop1_' + colname + '_' + config.gridName).val();
            status.currentFilteredText2 = $('#jlgrid_searchtext2_' + colname + '_' + config.gridName).val();
            status.currentFilteredValue2 = $('#jlgrid_searchdrop2_' + colname + '_' + config.gridName).val();
            status.currentFilteredOperator = $('#jlgrid_concatsearchdrop_' + colname + '_' + config.gridName).val();
            status.sortColumnType = coltype;

            // TIRA SU IL POPUP CERCA  
            $('.jlgrid_searchpop_' + colname + '_' + config.gridName).slideUp();


            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        };

        // CLEAR CERCA CON POPUP INTEGRATO
        function resettacerca(colname, fromcerca) {

            if (colname != status.latestFilteredColumn) return;

            // resetta i campi
            $('#jlgrid_searchdrop1_' + status.latestFilteredColumn + '_' + config.gridName).prop('selectedIndex', 0);
            $('#jlgrid_searchdrop2_' + status.latestFilteredColumn + '_' + config.gridName).prop('selectedIndex', 0);
            $('#jlgrid_concatsearchdrop_' + status.latestFilteredColumn + '_' + config.gridName).prop('selectedIndex', 0);
            $('#jlgrid_searchtext1_' + status.latestFilteredColumn + '_' + config.gridName).val('');
            $('#jlgrid_searchtext2_' + status.latestFilteredColumn + '_' + config.gridName).val('');


            status.curretFilteredColumn = '';
            status.latestFilteredColumn = '';
            status.currentFilteredField = '';
            status.currentFilteredText1 = '';
            status.currentFilteredText2 = '';
            status.currentFilteredValue1 = '';
            status.currentFilteredValue2 = '';
            status.currentFilteredOperator = '';
            status.sortColumnType = '';

            // NASCONDI IL PULSANTE CERCA
            $('#jlgrid_btsearch_' + colname + '_' + config.gridName).hide();
            // TIRA SU IL POPUP CERCA  
            $('.jlgrid_searchpop_' + colname + '_' + config.gridName).slideUp();

            if (fromcerca != true) {
                dataBind(1);
                addRowEvents();
                buildPaginator();
                addPaginatorEvents();
            }
        };

        // CAMBIA IL SORTING DELLA COLONNA CLICCATA
        function changesort(colname, coltype, colsort, colfield) {

            // TIPO DI COLONNA
            status.sortColumnType = coltype;// obj.attr("data-columntype");
            var _oldsort = colsort;// obj.attr("data-sort");
            var _newsort = "";

            // AZZERA L'ORDINAMENTO
            $(".jlgrid_column_label_" + config.gridName).attr("data-sort", "none");
            $(".jlgrid_column_label_" + config.gridName).removeClass("asc desc none");

            switch (_oldsort) {
                case "":
                    _newsort = "asc";
                    break;
                case "none":
                    _newsort = "asc";
                    break;
                case "asc":
                    _newsort = "desc";
                    break;
                case "desc":
                    _newsort = "none";
                    break;
            }

            // stato
            status.sortField = colfield;
            status.sortDir = _newsort;

            // ASSEGNA CLASSI DELL'ORDINAMENTO ATTUALE
            $('#jlgrid_column_label_' + colname + '_' + config.gridName).attr("data-sort", _newsort);
            $('#jlgrid_column_label_' + colname + '_' + config.gridName).removeClass("asc desc none").addClass(_newsort)

            // RIBINDA
            dataBind(status.currentPage);
            addRowEvents();

        };

        // FUNZIONE CUSTOM PER ORDINARE I DATI
        function sourceSort(dato1, dato2) {


            // if (status.sortField == '') return -1;

            // CONVERTE IL DATO NEL GIUSTO TIPO ALTRIMENTI ORDINA TUTTO COME FOSSERO STRINGHE, ANCHE I NUMERI
            var _d1, _d2
            if (status.sortColumnType == 'decimal' || status.sortColumnType == 'currency') {
                _d1 = parseFloat(dato1[status.sortField]);
                _d2 = parseFloat(dato2[status.sortField]);
            };
            if (status.sortColumnType == 'string' || status.sortColumnType == 'date') {
                _d1 = dato1[status.sortField];
                _d2 = dato2[status.sortField];
            };
            if (status.sortColumnType == 'integer') {
                _d1 = parseInt(dato1[status.sortField]);
                _d2 = parseInt(dato2[status.sortField]);

            };

            if (_d1 == null) _d1 = '';
            if (_d2 == null) _d2 = '';


            if (status.sortDir == 'asc') {

                // LA SEGUENTE SINTASSI È PIÙ O MENO COME IIF
                return (_d1 < _d2) ? -1 : ((_d1 > _d2) ? 1 : 0);
            };
            if (status.sortDir == 'desc') {
                return (_d1 > _d2) ? -1 : ((_d1 < _d2) ? 1 : 0);

            };
            if (status.sortDir == 'none') {
                return 0;
            };

        };

        // TIPO DI FILTRO STRINGA
        function textfilter(_ds) {

            var rex1 = new RegExp();
            var rex2 = new RegExp();

            switch (status.currentFilteredValue1) {
                case 'contains':
                    rex1 = '.*' + status.currentFilteredText1.toLowerCase() + '.*'
                    break;
                case 'notcontains':
                    rex1 = '^((?!' + status.currentFilteredText1.toLowerCase() + ').)*$'
                    break;
                case 'equal':
                    rex1 = '^(' + status.currentFilteredText1.toLowerCase() + '$)'
                    break;
                case 'notequal':
                    rex1 = '^(?!' + status.currentFilteredText1.toLowerCase() + '$)'
                    break;
                case 'startwith':
                    rex1 = '^(' + status.currentFilteredText1.toLowerCase() + ').*'
                    break;
                case 'endwith':
                    rex1 = '.*(' + status.currentFilteredText1.toLowerCase() + ')$'
                    break;

            }

            // secondo valore
            switch (status.currentFilteredValue2) {
                case 'contains':
                    rex2 = '.*' + status.currentFilteredText2.toLowerCase() + '.*'
                    break;
                case 'notcontains':
                    rex2 = '^((?!' + status.currentFilteredText2.toLowerCase() + ').)*$'
                    break;
                case 'equal':
                    rex2 = '^(' + status.currentFilteredText2.toLowerCase() + '$)'
                    break;
                case 'notequal':
                    rex2 = '^(?!' + status.currentFilteredText2.toLowerCase() + '$)'
                    break;
                case 'startwith':
                    rex2 = '^(' + status.currentFilteredText2.toLowerCase() + ').*'
                    break;
                case 'endwith':
                    rex2 = '.*(' + status.currentFilteredText2.toLowerCase() + ')$'
                    break;

            }

            _find1 = false;
            _find2 = false;

            var temparr = $.grep(_ds, function (value) {

                if (value[status.currentFilteredField] == null) {
                    value[status.currentFilteredField] = "";
                }

                _find1 = false;
                _find2 = false;

                // VALORE 1
                if (value[status.currentFilteredField].toLowerCase().match(rex1)) _find1 = true;

                // VEDI SE C'è IL SECONDO PARAMETRO DI CONFRONTO
                if (status.currentFilteredText2 != '') {

                    // VALORE 2
                    if (value[status.currentFilteredField].toLowerCase().match(rex2)) _find2 = true;

                    // A SECONDA DI QUALE OPERATORE è STATO SELEZIONATO
                    if (status.currentFilteredOperator == 'and') {
                        // and

                        if (_find1 && _find2) return value;
                    }
                    else {
                        if (_find1 || _find2) return value;
                    };

                }
                else {

                    if (_find1) return value;
                };

            });

            return temparr;

        }

        // TIPO DI FILTRO NUMERICO
        function numberfilter(_ds) {

            var controllanumero = new RegExp('(([0-9]*)+)([\.]?|[\,]?)([0-9]*)?')

            var _t1 = status.currentFilteredText1.replace(config.currencySymbol, '').replace(',', '.').trim();
            var _t2 = status.currentFilteredText2.replace(config.currencySymbol, '').replace(',', '.').trim();

            // CONTROLLO SE I VALORI INSERITI COME RICERCA SIANO NUMERICI
            if (!_t1.match(controllanumero)) return _ds;

            var _ft1 = parseFloat(_t1);
            var _ft2;

            if (!_t1.match(controllanumero)) {
                _ft2 = NaN;
            }
            else {
                _ft2 = parseFloat(_t2);
            };

            var _val1 = status.currentFilteredValue1;
            var _val2 = status.currentFilteredValue2;

            _find1 = false;
            _find2 = false;

            var temparr = $.grep(_ds, function (value) {

                _find1 = false;
                _find2 = false;

                switch (_val1) {
                    case 'equal':
                        if (parseFloat(value[status.currentFilteredField]) == _ft1) _find1 = true;
                        break;
                    case 'notequal':
                        if (parseFloat(value[status.currentFilteredField]) != _ft1) _find1 = true;
                        break;
                    case 'greater':
                        if (parseFloat(value[status.currentFilteredField]) > _ft1) _find1 = true;
                        break;
                    case 'greaterorequal':
                        if (parseFloat(value[status.currentFilteredField]) >= _ft1) _find1 = true;
                        break;
                    case 'less':
                        if (parseFloat(value[status.currentFilteredField]) < _ft1) _find1 = true;
                        break;
                    case 'lessorequal':
                        if (parseFloat(value[status.currentFilteredField]) <= _ft1) _find1 = true;
                        break;

                };

                // valore 2
                if (!isNaN(_ft2)) {

                    switch (_val2) {
                        case 'equal':
                            if (parseFloat(value[status.currentFilteredField]) == _ft2) _find2 = true;
                            break;
                        case 'notequal':
                            if (parseFloat(value[status.currentFilteredField]) != _ft2) _find2 = true;
                            break;
                        case 'greater':
                            if (parseFloat(value[status.currentFilteredField]) > _ft2) _find2 = true;
                            break;
                        case 'greaterorequal':
                            if (parseFloat(value[status.currentFilteredField]) >= _ft2) _find2 = true;
                            break;
                        case 'less':
                            if (parseFloat(value[status.currentFilteredField]) < _ft2) _find2 = true;
                            break;
                        case 'lessorequal':
                            if (parseFloat(value[status.currentFilteredField]) <= _ft2) _find2 = true;
                            break;

                    };

                    // A SECONDA DI QUALE OPERATORE è STATO SELEZIONATO
                    if (status.currentFilteredOperator == 'and') {
                        // and

                        if (_find1 && _find2) return value;
                    }
                    else {
                        if (_find1 || _find2) return value;
                    };

                }
                else {
                    if (_find1) return value;
                };

            });

            return temparr;

        };

        function datefilter(_ds) {
            // gg/mm/aaaa oppure gg-mm-aaaa oppure gg/mm/aa oppure gg-mm-aa
            var controlladata1 = new RegExp('((([0]{1}[1-9]{1})|([1-2]{1}[0-9]{1})|([3]{1}[0-1]{1}))|([1-9]{1})){1}([-]|[/])((([0]{1}[1-9]{1})|([1]{1}[0-2]{1}))|([1-9]{1})){1}([-]|[/])(([0-9]{4})|([0-9]{2}){1})')
            // aaaa/mm/gg oppure aaaa-mm-gg oppure aa/mm/gg oppure aa-mm-gg
            var controlladata2 = new RegExp('(([0-9]{4})|([0-9]{2}){1})([-]|[/])((([0]{1}[1-9]{1})|([1]{1}[0-2]{1}))|([1-9]{1})){1}([-]|[/])((([0]{1}[1-9]{1})|([1-2]{1}[0-9]{1})|([3]{1}[0-1]{1}))|([1-9]{1})){1}')

            var _t1 = status.currentFilteredText1.trim();
            var _t2 = status.currentFilteredText2.trim();

            // CONTROLLO SE I VALORI INSERITI COME RICERCA SIANO NUMERICI
            if (!_t1.match(controlladata1)) return _ds;

            var _ft1 = new Date().format('yyyy-mm-dd', _t1);
            var _ft2 = new Date().format('yyyy-mm-dd', _t1);

            var _val1 = status.currentFilteredValue1;
            var _val2 = status.currentFilteredValue2;

            _find1 = false;
            _find2 = false;

            var temparr = $.grep(_ds, function (value) {

                _find1 = false;
                _find2 = false;

                if (isNaN(new Date(value[status.currentFilteredField]))) return value;
                // in realta comparo le string in formato internazionale
                _tempdate = new Date().format('yyyy-mm-dd', value[status.currentFilteredField]);

                switch (_val1) {
                    case 'equal':
                        if (_tempdate == _ft1) _find1 = true;
                        break;
                    case 'notequal':
                        if (_tempdate != _ft1) _find1 = true;
                        break;
                    case 'greater':
                        if (_tempdate > _ft1) _find1 = true;
                        break;
                    case 'greaterorequal':
                        if (_tempdate >= _ft1) _find1 = true;
                        break;
                    case 'less':
                        if (_tempdate < _ft1) _find1 = true;
                        break;
                    case 'lessorequal':
                        if (_tempdate <= _ft1) _find1 = true;
                        break;

                };

                // valore 2
                if (!isNaN(_ft2)) {

                    switch (_val2) {
                        case 'equal':
                            if (_tempdate == _ft2) _find2 = true;
                            break;
                        case 'notequal':
                            if (_tempdate != _ft2) _find2 = true;
                            break;
                        case 'greater':
                            if (_tempdate > _ft2) _find2 = true;
                            break;
                        case 'greaterorequal':
                            if (_tempdate >= _ft2) _find2 = true;
                            break;
                        case 'less':
                            if (_tempdate < _ft2) _find2 = true;
                            break;
                        case 'lessorequal':
                            if (_tempdate <= _ft2) _find2 = true;
                            break;

                    };

                    // A SECONDA DI QUALE OPERATORE è STATO SELEZIONATO
                    if (status.currentFilteredOperator == 'and') {
                        // and

                        if (_find1 && _find2) return value;
                    }
                    else {
                        if (_find1 || _find2) return value;
                    };

                }
                else {
                    if (_find1) return value;
                };

            });

            return temparr;
        };

        // FILTRA IL DATASOURCE
        function filtercolumn(_ds) {

            var temparr;

            switch (status.sortColumnType) {
                case 'string': case '': case undefined:
                    temparr = textfilter(_ds)
                    break;
                case 'integer': case 'decimal': case 'currency':
                    temparr = numberfilter(_ds)
                    break;
                case 'date':
                    temparr = datefilter(_ds)
                    break;
            }

            return temparr;

        };

        // FILTRO CUSTOM DA API
        function filtercolumncustom(_ds, _filtercommand, _searchmode) {
            // _filtercommand è un oggetto che contiene:
            // fieldname,fieldtype,searchtype,value
            var tempresult = _ds.slice();
            var tempsource = _ds.slice();
            var searchresult = []; // inserisce tutti i risultati in modalità OR
            var searchtemp = []; // inserisce tutti i risultati in modalità OR

            var rex1 = new RegExp();
            var controllanumero = new RegExp('(([0-9]*)+)([\.]?|[\,]?)([0-9]*)?')
            var controlladata = new RegExp('((([0]{1}[1-9]{1})|([1-2]{1}[0-9]{1})|([3]{1}[0-1]{1}))|([1-9]{1})){1}([-]|[/])((([0]{1}[1-9]{1})|([1]{1}[0-2]{1}))|([1-9]{1})){1}([-]|[/])(([0-9]{4})|([0-9]{2}){1})')


            $(_filtercommand).each(function (i, v) {
                switch (v.fieldtype) {
                    case 'string': case '': case undefined:

                        switch (v.searchtype) {
                            case 'contains':
                                rex1 = '.*' + v.value.toLowerCase() + '.*'
                                break;
                            case 'notcontains':
                                rex1 = '^((?!' + v.value.toLowerCase() + ').)*$'
                                break;
                            case 'equal':
                                rex1 = '^(' + v.value.toLowerCase() + '$)'
                                break;
                            case 'notequal':
                                rex1 = '^(?!' + v.value.toLowerCase() + '$)'
                                break;
                            case 'startwith':
                                rex1 = '^(' + v.value.toLowerCase() + ').*'
                                break;
                            case 'endwith':
                                rex1 = '.*(' + v.value.toLowerCase() + ')$'
                                break;

                        }


                        if (_searchmode) {
                            // modalità cerca
                            tempresult = $.grep(tempsource, function (value) {
                                if (value[v.fieldname].toLowerCase().match(rex1)) return value;
                            });

                            searchresult.mergeUnique(tempresult, function (el, e) {

                                return el.id === e.id;
                            });

                        }
                        else {
                            // modalità filtro
                            tempresult = $.grep(tempresult, function (value) {
                                if (value[v.fieldname].toLowerCase().match(rex1)) return value;
                            });
                        }


                        break;
                    case 'integer': case 'decimal': case 'currency':


                        var _t1 = parseFloat(v.value.replace(config.currencySymbol, '').replace(',', '.').trim());
                        if (_searchmode) { // CERCA
                            tempresult = $.grep(tempsource, function (value) {



                                switch (v.searchtype) {
                                    case 'equal':
                                        if (parseFloat(value[v.fieldname]) == _t1) return value;
                                        break;
                                    case 'notequal':
                                        if (parseFloat(value[v.fieldname]) != _t1) return value;
                                        break;
                                    case 'greater':
                                        if (parseFloat(value[v.fieldname]) > _t1) return value;
                                        break;
                                    case 'greaterorequal':
                                        if (parseFloat(value[v.fieldname]) >= _t1) return value;
                                        break;
                                    case 'less':
                                        if (parseFloat(value[v.fieldname]) < _t1) return value;
                                        break;
                                    case 'lessorequal':
                                        if (parseFloat(value[v.fieldname]) <= _t1) return value;
                                        break;

                                };
                            });

                            searchresult.mergeUnique(tempresult, function (el, e) {
                                return el.id === e.id;
                            });
                        }
                        else { // FILTRA
                            tempresult = $.grep(tempresult, function (value) {



                                switch (v.searchtype) {
                                    case 'equal':
                                        if (parseFloat(value[v.fieldname]) == _t1) return value;
                                        break;
                                    case 'notequal':
                                        if (parseFloat(value[v.fieldname]) != _t1) return value;
                                        break;
                                    case 'greater':
                                        if (parseFloat(value[v.fieldname]) > _t1) return value;
                                        break;
                                    case 'greaterorequal':
                                        if (parseFloat(value[v.fieldname]) >= _t1) return value;
                                        break;
                                    case 'less':
                                        if (parseFloat(value[v.fieldname]) < _t1) return value;
                                        break;
                                    case 'lessorequal':
                                        if (parseFloat(value[v.fieldname]) <= _t1) return value;
                                        break;

                                };
                            });
                        }




                        break;

                    case 'date':

                        var _t1 = v.value.trim();

                        // CONTROLLO SE I VALORI INSERITI COME RICERCA SIANO NUMERICI
                        if (!_t1.match(controlladata)) return _tempresult;

                        var _ft1 = new Date().format('yyyy-mm-dd', _t1);

                        if (_searchmode) {
                            // CERCA
                            tempresult = $.grep(tempsource, function (value) {



                                if (isNaN(new Date(value[v.fieldname]))) return value;
                                // in realta comparo le string in formato internazionale
                                _tempdate = new Date().format('yyyy-mm-dd', value[v.fieldname]);

                                switch (v.searchtype) {
                                    case 'equal':
                                        if (_tempdate == _ft1) return value;
                                        break;
                                    case 'notequal':
                                        if (_tempdate != _ft1) return value;
                                        break;
                                    case 'greater':
                                        if (_tempdate > _ft1) return value;
                                        break;
                                    case 'greaterorequal':
                                        if (_tempdate >= _ft1) return value;
                                        break;
                                    case 'less':
                                        if (_tempdate < _ft1) return value;
                                        break;
                                    case 'lessorequal':
                                        if (_tempdate <= _ft1) return value;
                                        break;

                                };


                            });

                            searchresult.mergeUnique(tempresult, function (el, e) {
                                return el.id === e.id;
                            });
                        }
                        else {
                            // FILTRA
                            tempresult = $.grep(tempresult, function (value) {



                                if (isNaN(new Date(value[v.fieldname]))) return value;
                                // in realta comparo le string in formato internazionale
                                _tempdate = new Date().format('yyyy-mm-dd', value[v.fieldname]);

                                switch (v.searchtype) {
                                    case 'equal':
                                        if (_tempdate == _ft1) return value;
                                        break;
                                    case 'notequal':
                                        if (_tempdate != _ft1) return value;
                                        break;
                                    case 'greater':
                                        if (_tempdate > _ft1) return value;
                                        break;
                                    case 'greaterorequal':
                                        if (_tempdate >= _ft1) return value;
                                        break;
                                    case 'less':
                                        if (_tempdate < _ft1) return value;
                                        break;
                                    case 'lessorequal':
                                        if (_tempdate <= _ft1) return value;
                                        break;

                                };




                            });
                        }



                        break;
                }



            })
            if (_searchmode) {
                return searchresult;

            }
            else {
                return tempresult;

            }
        };

        // RICERCA CUSTOM DA CHIAMATA API
        function searchcustom(_ds, _searchcommand, _searchquery) {

        }

        // ESEGUE IL DATABIND
        function dataBind(pageNumber) {

            if (!pageNumber) { pageNumber = 1 };
            status.currentPage = pageNumber;

            var _riga = new String('');
            // svuoto la view corrente
            $("tbody", grid).html('');

            // RICAVO L'INDICE DI INIZIO IN BASE AL NUMERO DI PAGINA PASSATO
            var _startindex = (pageNumber - 1) * config.pageSize;

            // questo rimischia il datasource
            var _dataSource = config.dataSource.slice();

            // FILTRO CUSTOM
            if (status.customFilter) {
                _dataSource = filtercolumncustom(_dataSource, status.customFilterCommand, false).slice();

            }

            // E' IMPOSTATO QUALCHE FILTRO?
            if (status.curretFilteredColumn != '') {
                _dataSource = filtercolumn(_dataSource).slice();
            };

            // RICERCA CUSTOM DA CHIAMATA API
            if (status.customSearch) {

                _dataSource = filtercolumncustom(_dataSource, status.customSearchCommand, true).slice();

            }

            if (!(status.sortField == '' || status.sortDir == 'none')) {
                _dataSource.sort(sourceSort);
            }


            // SALVA LO STATO DEL DATASOURCE
            status.currentDataSource = _dataSource



            // ottiene un nuovo oggetto datasource con le sole righe da mostrare
            var _datasubset = $(_dataSource).slice(_startindex, _startindex + config.pageSize)

            // CICLA LE RIGHE
            $(_datasubset).each(function (index, value) {

                $this = this;
                $index = index;
                $rowindex = (index + _startindex);
                $data = ""
                $class = ""

                // DATA ASSOCIATO AL TR ************

                if (config.dataField) {

                    $.each(config.dataField, function (i, v) {

                        $data += "data-" + v + "=\"" + $this[v] + "\" ";
                    });
                }

                // CLASSI AGGIUNTIVE


                if (config.addClass) {

                    $.each(config.addClass, function (i, v) {

                        $class += " " + v;
                    });
                }

                _riga += '<tr ' + $data + ' data-idx="' + $rowindex + '" class="jltable_body_tr_' + config.gridName + $class + '">';



                // INSERISCO IL REORDER ?
                if (config.allowReorder) {

                    _grid = '';

                    // GRIGLIA INFERIORE
                    if ($index < _datasubset.length - 1) {
                        if (config.showGrid) _grid += 'border-bottom:solid 1px ' + config.gridColor + ';';
                    };

                    _riga += '<td data-idx="' + $rowindex + '" class="td_reorder" style="padding:0px;margin:0;width:' + this.width + ';' + _grid + ';">' + reorder.draghandle + '</td>';

                };

                // CICLA LE COLONNE
                $(config.columns).each(function (index, value) {

                    _vis = this.visible != undefined ? this.visible : true;
                    if (_vis) {

                        _grid = '';
                        _style = '';
                        _data = '';
                        _class = '';

                        // griglia

                        // GRIGLIA LATERALE
                        if (index < config.columns.length - 1) {
                            if (config.showGrid) _grid = 'border-right:solid 1px ' + config.gridColor + ';';
                        };
                        // GRIGLIA INFERIORE
                        if ($index < _datasubset.length - 1) {
                            if (config.showGrid) _grid += 'border-bottom:solid 1px ' + config.gridColor + ';';
                        };
                        // STYLE
                        if (this.style) {
                            $.each(this.style, function (i, v) {
                                _style += i + ':' + v + ';';
                            });
                        }

                        // DATA ASSOCIATO ALLE SINGOLE CELLE TD ********

                        if (this.dataField) {
                            if (this.dataField.db) {
                                $.each(this.dataField.db, function (i, v) {
                                    _data += "data-" + v + "=\"" + $this[v] + "\" ";
                                });
                            }
                            if (this.dataField.fixed) {
                                $.each(this.dataField.fixed, function (i, v) {
                                    _data += "data-" + i + "=\"" + v + "\" ";
                                });
                            }

                        }

                        // CLASSI AGGIUNTIVE SINGOLE CELLE TD ********

                        if (this.addClass) {

                            $.each(this.addClass, function (i, v) {
                                _class += " " + v;
                            });
                        }


                        _riga += '<td ' + _data + ' class="' + this.columnName + _class + '" style="padding:5px;margin:0;width:' + this.width + ';' + _grid + _style + '">';

                        // SEMPLICE FORMATTAZIONE

                        if (this.link) {
                            _riga += '<a ' + (this.link.linktarget ? 'target="' + this.linktarget + '"' : '') + ' href="' + (this.link.basepath ? this.link.basepath : '') + $this[this.link.fieldname] + '">';
                        };

                        if (this.columnType) {

                            switch (this.columnType) {

                                case "string": case "":

                                    if (this.text) {
                                        _riga += this.text;
                                    }
                                    else {

                                        if ($this[this.fieldName] != null)
                                        { _riga += $this[this.fieldName] }
                                    };

                                    break;
                                case "decimal":
                                    _riga += parseFloat(this.text ? this.text : $this[this.fieldName]).toFixed(2)
                                    break;
                                case "integer":
                                    _riga += parseInt(this.text ? this.text : $this[this.fieldName])
                                    break;
                                case "currency":
                                    _riga += config.currencySymbol + ' ' + parseFloat(this.text ? this.text : $this[this.fieldName]).toFixed(2)
                                    break;
                                case "date":
                                    var d = new Date($this[this.fieldName]);

                                    //alert(new Date())
                                    var d = new Date().format('dd/mm/yyyy', $this[this.fieldName]);
                                    _riga += d

                                    break;

                            }
                        }
                        else {
                            _riga += $this[this.fieldName];
                        };


                        if (this.link) {
                            _riga += '</a>';
                        };

                        // BUTTONS
                        if (this.buttons) {

                            $(this.buttons).each(function (btindex, btvalue) {
                                var _vis = (this.visible != undefined ? this.visible : true);

                                if (_vis) {
                                    var _btstyle = ''

                                    // AGGIUNGE LO STILE DEL PULSANTE SE PASSATO
                                    if (this.style) {
                                        $.each(this.style, function (i, v) {

                                            _btstyle += i + ':' + v + ';';
                                        });
                                    }

                                    _url = '';
                                    // SE C'E' IL LINK
                                    if (this.link) {
                                        _url = ' href="';
                                        _pars = '';

                                        // PARAMETRI DA AGGIUNGERE
                                        if (this.link.nameValueCollection) {
                                            $.each(this.link.nameValueCollection, function (parindex, parvalue) {
                                                _pars += (parindex == 0 ? '?' : '&') + this.name + '=' + $this[this.fieldName]
                                            });
                                        };

                                        _url += this.link.url + _pars + '" '

                                    };
                                    _riga += '<a ' + _url + ' style="' + _btstyle + '" data-rowindex="' + $rowindex + '" data-commandname="' + this.commandName + '" data-commandargument="' + this.commandArgument + '" class="jlgrid_buttons_' + config.gridName + ' jlgrid_button_' + config.gridName + '_' + (this.commandName ? this.commandName : 'nocommand') + '">' + (this.text ? this.text : $this[this.fieldName]) + '</a>'

                                };
                            });

                        };
                        // IMAGES
                        if (this.images) {

                            $(this.images).each(function (btindex, btvalue) {

                                var _imgstyle = ''

                                // AGGIUNGE LO STILE DEL PULSANTE SE PASSATO
                                if (this.style) {
                                    $.each(this.style, function (i, v) {

                                        _imgstyle += i + ':' + v + ';';
                                    });
                                }
                                // SE C'E' IL LINK
                                if (this.link) _riga += '<a target="' + (this.link.target ? this.link.target : '_self') + '" href="' + this.link.url + '">'

                                _riga += '<img style="' + _imgstyle + '" src="' + (this.text ? this.path + this.text : this.path + $this[this.fieldName]) + '" alt="' + this.alt + '"/>';

                                // CHIUDO IL LINK
                                if (this.link) _riga += '</a>'
                            });

                        };

                        _riga += '</td>';

                    };


                }); /*FINE CICLE cLONNE*/
                _riga += '</tr>';


                $("tbody", grid).append(_riga);
                _riga = "";
            });

            // ASYNC ****************** AGGIUNGO LE RIGHE UNA ALLA VOLTA

            // AGGIUNGE L'grid


            $("tbody tr:odd td", grid).css("background-color", config.oddBackground);
            $("tbody tr:even td", grid).css("background-color", config.evenBackground);



        };

        // COSTRUISCE IL PAGINATORE
        function buildPaginator() {

            var _numerator = '';
            var _prevpage = (parseInt(status.currentPage) - 1) >= 1 ? (parseInt(status.currentPage) - 1) : 1;
            var _nextpage = (parseInt(status.currentPage) + 1) < status.totalPages ? (parseInt(status.currentPage) + 1) : status.totalPages;

            var _pagine = [];
            for (i = 1; i <= status.totalPages; i++) _pagine.push(i);
            var _maxinterval = status.totalPages >= 8 ? 8 : status.totalPages;
            var _offsetpage = 2;
            var _gruppi = Math.ceil(status.totalPages / (_maxinterval + _offsetpage));


            var _currentgroup = Math.ceil((parseInt(status.currentPage) + parseInt(_offsetpage)) / parseInt(_maxinterval))
            if (status.currentPage < ((_currentgroup - 1) * _maxinterval) && _currentgroup > 1) _currentgroup -= 1;

            var _startindex

            if (_gruppi == 1) {
                _startindex = 0;
            }
            else {
                _startindex = ((_currentgroup - 1) * _maxinterval) - _offsetpage;

            };


            if (_startindex < 0) _startindex = 0;


            var _stopindex = (((_currentgroup - 1) * _maxinterval) + _maxinterval) + _offsetpage;

            if (_stopindex > (status.totalPages * config.pageSize)) _stopindex = (status.totalPages * config.pageSize);

            if (_currentgroup > _gruppi) _currentgroup = _gruppi;

            _numerator += '<a  data-pagenumber="1" class="jlgrid_pagelink_' + config.gridName + '">|<</a>'
            _numerator += '<a  data-pagenumber="' + _prevpage + '" class="jlgrid_pagelink_' + config.gridName + '"><</a>'

            // METTE I PUNTINI
            if (_currentgroup > 1) {
                _numerator += '<a  class="jlgrid_pagenolink_' + config.gridName + '">...</a>'
            }

            if (_stopindex == undefined) _stopindex = 1;

            _currentpagegroup = _pagine.slice(_startindex, _stopindex)


            $.each(_currentpagegroup, function (i, v) {
                _numerator += '<a  data-pagenumber="' + (v) + '" class="jlgrid_pagelink_' + (v == status.currentPage ? 'selected_' : '') + config.gridName + '">' + (v) + '</a>'
            });


            // METTE I PUNTINI
            if (_currentgroup < _gruppi && _gruppi > 1) {
                _numerator += '<a class="jlgrid_pagenolink_' + config.gridName + '">...</a>'
            }

            _numerator += '<a data-pagenumber="' + _nextpage + '" class="jlgrid_pagelink_' + config.gridName + '">></a>'
            _numerator += '<a data-pagenumber="' + status.totalPages + '" class="jlgrid_pagelink_' + config.gridName + '">>|</a>'
            _numerator += '<div id="jlpaginator_label_' + config.gridName + '" style="color:gray;">Pagina ' + status.currentPage + ' di ' + (status.totalPages == '0' ? 1 : status.totalPages) + '</div>'
            if (status.fullLoaded == false) {
                _numerator += '<div class="jlpageloading_' + config.gridName + '" style="float:left;padding:5px 10px 5px 25px;"><img src="/images/ajax/progress_bar2.gif"/></div>'
            }

            $(grid).last().html(_numerator);

            status.prevPage = _prevpage;
            status.nextPage = _nextpage;

        };

        // *******************************************
        // API
        // *******************************************

        this.gotoPage = function (pageNumber) {

            status.currentPage = pageNumber;
            dataBind(pageNumber);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        };
        this.gotoNextPage = function () {

            status.currentPage = status.nextPage;
            dataBind(status.nextPage);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();


        };
        this.gotoPrevPage = function () {

            status.currentPage = status.prevPage;
            dataBind(status.prevPage);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        };
        this.gotoFirstPage = function () {
            status.currentPage = 1;
            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        };

        this.gotoLastPage = function () {
            status.currentPage = status.totalPages;
            dataBind(status.totalPages);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        };

        this.deleteRow = function (itemgroup) {



            let urlPostCount = config.connector.serverPath + config.connector.deleterow.serviceName;
            let urlDataCount = {};
           
            // parametri custom per il conteggio
            if (config.connector.deleterow.customData) {
                $.each(config.connector.totalitem.customData, function (i, v) {
                    urlDataCount[v["name"]] = v["val"];
                })
            }
            urlDataCount["itemgroup"] = itemgroup;
           // console.info(urlDataCount);
            //
            //Conteggio Item Totali;
            $.post(urlPostCount, urlDataCount, function (data) {
                myself.rebind();
            });



            //var _data = "{'table':'" + config.connector.deleterow.tableName + "','id':'" + id + "'}"
            //$.ajax({
            //    type: "POST",
            //    url: config.connector.serverPath + config.connector.deleterow.serviceName,
            //    data: _data,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    success: function (msg) {

            //        return $.parseJSON(msg.d);

            //    },
            //    error: function (e) {
            //        return false;

            //    }

            //});


        };

        this.reorderRow = function (movedir, igtomove, igtarget,ordtomove,ordtarget) {

            let urlPostCount = config.connector.serverPath + config.connector.reorder.serviceName;
            let urlDataCount = {};

            // parametri custom per il conteggio
            if (config.connector.deleterow.customData) {
                $.each(config.connector.totalitem.customData, function (i, v) {
                    urlDataCount[v["name"]] = v["val"];
                })
            }
            urlDataCount["movedirection"] = movedir;
            urlDataCount["igtomove"] = igtomove;
            urlDataCount["igtarget"] = igtarget;
            urlDataCount["ordtomove"] = ordtomove;
            urlDataCount["ordtarget"] = ordtarget;

           // console.info(urlDataCount);
            //
            //Conteggio Item Totali;
            $.post(urlPostCount, urlDataCount, function (data) {
                myself.rebind();
            });


            //var _data = "{'table':'" + config.connector.reorder.tableName + ",'movedirection':'" + movedir + "','movedrow':'" + rowtomove + "','targetrow':'" + targetrow + "'}";

            //$.ajax({
            //    type: "POST",
            //    url: config.connector.serverPath + config.connector.reorder.serviceName,
            //    data: _data,
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    async: false,
            //    success: function (msg) {
            //        return $.parseJSON(msg.d);

            //    },
            //    error: function (e) {
            //        return false;

            //    }

            //});

            //myself.rebind();

        };

        // cambia il datasource
        this.changeBindingConnector = function (serviceName, where) {
            config.connector.source.serviceName = serviceName;
            config.connector.source.where = where;
            myself.rebind();
        };

        this.search = function (query, searchcommand) {
            // search command array: { fieldname: "nomefield", fieldtype: "string,date,number,decimal", searchtype: "equal,notequal,contains,notcontains,startwith,notstartwith,endwith,notendwith", value: ""}

            if (query != "" & searchcommand.length > 0) {
                status.customSearch = true;
                status.customSearchCommand = searchcommand;
                status.customSearchQuery = query;
            }
            else {
                status.customSearch = false;
            }

            loadData();
            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

        }
        this.searchreset = function () {
            status.customSearch = false;
            loadData();
            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();
        }
        this.filter = function (filtercommand) {
            //Filter Command array: { fieldname: "nomefield", fieldtype: "string,date,number,decimal", searchtype: "equal,notequal,contains,notcontains,startwith,notstartwith,endwith,notendwith", value: ""}
            status.customFilter = true;
            status.customFilterCommand = filtercommand;
            loadData();
            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();
        }
        this.resetfilter = function () {
            status.customFilter = false;
            loadData();
            dataBind(1);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();
        }

        this.rebind = function () {
           // console.info(config.dataSource);
            config.dataSource = [];

            status.currentPage = 1;

            loadData();
            dataBind(status.currentPage);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();
        };
        // come il rebind ma non ricompila il datasource
        this.rebuild = function () {
            dataBind(status.currentPage);
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();
        };
        // ritorna il datasource corrente

        this.getCurrentSource = function () {
            return status.currentDataSource;
        }

        // esporta in csv
        this.exportCSV = function (folder, filename, callback) {


            

            var tosend = escape(JSON.stringify(status.currentDataSource));
            var result;

            $.ajax({
                type: "POST",
                url: 'services/datab.asmx/exportCSV',
                data: "{'source':'" + tosend + "','folder':'" + folder + "','filename':'" + filename + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: true,
                success: function (msg) {
                    callback(msg.d);
                },
                error: function (e) {

                    callback("error");
                }

            });


        }

        // codice da eseguire su ogni elemento
        this.each(function () {


            h = init();
            //b = dataBind(1);
            addEvents();
            addRowEvents();
            buildPaginator();
            addPaginatorEvents();

            $(this).append(grid);

            //setta le altezze top delle finestre del popup cerca
            var _thheight = $("th", grid).first().height()
            $(".jlgrid_searchpops_" + config.gridName).css("top", (_thheight + 3) + "px")

        });

        // PERMETTE L'ATTACH
        return this;
    };
})(jQuery);
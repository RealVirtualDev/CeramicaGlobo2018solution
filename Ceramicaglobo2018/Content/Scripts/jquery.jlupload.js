// ********************************* JLUPLOAD ***************************************
// v 1.0.0 19/01/2013
// Plugin ideato e scritto da Simone Ludovici
// **********************************************************************************
// v 1.0.0 on 2013-01-19
// Created by Simone Ludovici
// **********************************************************************************


// the semi-colon before the function invocation is a safety 
// net against concatenated scripts and/or other plugins 
// that are not closed properly.
; (function ($, window, document, undefined) {


    // Defaults config
    var pluginName = 'jlupload',

        config = {
            buttonlabel: "SELECT",
            multiselectedlabel: 'files selected',
            multiselect: true,
            uploadfolder: '/upload/',
            fileprefix:'',
            connector: '/connectors/jluploader.ashx',
            filter: '',
            fileuploadprogress: function (_file, _percentprogress) { },
            totaluploadprogress: function (_controlid, _percentprogress) { },
            fileuploaded: function (_file) { },
            uploadcompleted: function (queque) { },
            filerefused: function () {_allowedext,_file},
            fileaborted: function (_file) { },
            additionalParams:[],
            cidc: 1
        };

    // Plugin constructor
    function Plugin(element, options) {
        this.element = element;
        this.options = $.extend({}, config, options);
        this._opt = options;
        this._config = config;
        this._name = pluginName;
        this.init();
    }

    Plugin.prototype.init = function () {
        

        myopt = this.options;

        this.getProgressTemplate = function (row) {
           
            return progresstemplate;
        };

        myself = this;
        

        if (typeof current == 'undefined') current = [];
        if (typeof currentcontrolid == 'undefined') currentcontrolid = [];

       
        inputtype = ('html5');
        
        htmltemplate = '<div data-sourcecontrolid="%currentcontrolid%" id="jlupload_%cid%" data-count="%cid%" class="jlupload_outer">';
        htmltemplate += '<input type="file" name="%currentcontrolid%" id="upload%cid%" ' + (myopt.multiselect ? 'multiple="multiple"' : '') + '/>';
        htmltemplate += '<div id="jlupload_label_%cid%" class="jlupload_label"></div>';
        htmltemplate += '<div id="jlupload_selectbutton_%cid%" class="jlupload_button">%selectlabel%</div>';
        htmltemplate += '<div style="clear:both;"></div>';
        htmltemplate += '</div>';

        
        progresstemplate = '<div  data-controlid="%cid%" id="jlupload_progress_%cid%_%cfid%" class="jlupload_progress_outer">';
        progresstemplate += '<div id="jlupload_progressbar_%cid%_%cfid%" class="jlupload_progressbar"></div>';
        progresstemplate += '<div id="jlupload_progressinfolabel_%cid%_%cfid%" class="jlupload_progressinfolabel">0%</div>';
        progresstemplate += '<div id="jlupload_progressfilelabel_%cid%_%cfid%" class="jlupload_progressfilelabel">%fname%</div>';
        progresstemplate += '<div class="jlupload_progresscancel" ><a id="jlupload_calcelfile_%cid%_%cfid%" style="cursor:pointer;">X</a></div>';
        progresstemplate += '</div>';



        // HTML5 ONLY
        // PROGRESS FOR EACH SELECTED FILE
        progress = {
            add: function (witchid, idx) {


                $(current[idx].queque).each(function (i, v) {
                    if (this.status == "queque") {

                        $("#jlupload_" + witchid).after(progresstemplate.replace("%fname%", this.name).replace(/%cid%/g, witchid).replace(/%cfid%/g, (i + 1 + _offset)))
                        $("#jlupload_calcelfile_" + witchid + "_" + (i + 1)).on("click", function () { current[idx].queque[i].status = "aborted" })

                    };

                });

                // GET MAX PROGRESSBAR VALUE
                current[idx].barwidth = $(".jlupload_progressbar").first().css("width").replace("px", "")
                // SET PROGRESSBAR TO 0
                $(".jlupload_progressbar").css("width", "0px")


                if (inputtype == 'html5') {

                    // FILES TO PROCESS

                    $(current[idx].queque).each(function (quequeindex, value) {
                        if (this.status == "queque") {
                            progress.sendFile(this['controlid'], this['fileid'], this['file'], quequeindex, idx);
                        };
                    })


                };

            },

            sendFile: function (witchid, fileid, file, quequeidx, idx) {

                // SET QUEQUE STATUS TO UPLOADING
                current[idx].queque[quequeidx].status = 'uploading'
                var data = new FormData()
                data.append("name", file.name);
                data.append("fileprefix", current[idx].options.fileprefix);
                data.append("size", file.size);
                data.append("type", file.type);
                data.append("file", file);
                data.append("folder", current[idx].options.uploadfolder);

                if (current[idx].options.additionalParams) {
                    $.each(current[idx].options.additionalParams, function (i, v) {
                        data.append(v["name"], v["val"]);
                       
                    })
                } 

                //console.info(data);

                $.ajax(
                    {
                        url: current[idx].options.connector,
                        dataType: "json",
                        type: "POST",
                        data: data,
                        cache: false,
                        contentType: false,
                        processData: false,
                        success: function () { },
                        error: function (e) { },

                     

                        xhr: function () {
                            
                            myXhr = $.ajaxSettings.xhr();
                                                        
                            current[idx].globalxhr.push(myXhr)
                           
                            if (myXhr.upload) {
                                myXhr.upload.addEventListener('progress', function (evt) {

                                    if (evt.lengthComputable) {
                                        var percentComplete = (evt.loaded / evt.total) * 100;

                                        // CANCELING
                                        // END REQUEST
                                        if (current[idx].queque[quequeidx]['status'] == 'aborted') {
                                            current[idx].globalxhr[quequeidx].abort();
                                            $("[data-controlid=" + witchid + "] #jlupload_progressbar_" + witchid + "_" + fileid).css("background-color", "red")
                                            $("[data-controlid=" + witchid + "] #jlupload_progressfilelabel_" + witchid + "_" + fileid).css("color", "white")
                                            $("#jlupload_progress_" + witchid + '_' + fileid).delay(1000).fadeOut("slow", function () { $(this).remove() })//.delay(2000).remove();
                                            current[idx].queque[quequeidx].status = "canceled"
                                            current[idx].options.fileaborted(current[idx].queque[quequeidx], idx)
                                           

                                        }

                                        // FILE PROGRESS
                                        progress.fileprogress(witchid, quequeidx, percentComplete, idx);
                                       

                                        // UPLOAD COMPLETED                                               
                                        if (percentComplete == 100) {
                                            current[idx].queque[quequeidx]['status'] = 'completed'
                                            
                                            // CANCEL PROGRESS DIV
                                            progress.remove(witchid + "_" + fileid, quequeidx, idx)

                                        };

                                    };


                                }, false);

                                myXhr.upload.addEventListener('load', function (evt) {
                                    // FIREFOX FIX FOR LOSTING SOME PROGRESS EVENTS
                                    if (current[idx].queque[quequeidx] && parseInt(current[idx].queque[quequeidx].progress) < 100)
                                    {
                                        current[idx].queque[quequeidx].progress = 100;
                                        // FILE PROGRESS
                                        progress.fileprogress(witchid, quequeidx, 100, idx);
                                        current[idx].queque[quequeidx]['status'] = 'completed'
                                        // CANCEL PROGRESS DIV
                                        progress.remove(witchid + "_" + fileid, quequeidx, idx)
                                    }
                                    
                                })

                            } else {
                                $('#progress').html("Upload progress is not supported.");
                            };
                            return myXhr;
                        }

                    });

            },
            totalprogress: function (witchid, idx) {
                // TOTAL PROGRESS FOR ALL FILES
                _totalpercent = 0;
                _filecount = 0;

                $.each(current[idx].queque, function (i, el) {
                    if (el['status'] !== 'abort' && el['status'] !== 'canceled') {
                        _filecount++;
                        _totalpercent += parseFloat(el['progress']);
                    }
                });

                if (_filecount != 0) {

                    _progress = Math.ceil(_totalpercent / _filecount);

                }
                else {
                    _progress = 0;

                    // FILES IN QUEQUE CANCELLED BY USER
                                       
                        // svuota la coda
                        current[idx].queque = new Array();
                        current[idx].queque.length = 0;
                        current[idx].globalxhr = new Array
                        current[idx].globalxhr.length = 0;
                        $("#upload" + (idx + 1)).replaceWith($("#upload" + (idx + 1)).val('').clone(true));
                    

                }
                    

                current[idx].options.totaluploadprogress(witchid, _progress)

            },
            fileprogress: function (witchid, quequeidx, percentComplete, idx) {
               
                barw = Math.floor((current[idx].barwidth / 100) * Math.ceil(percentComplete))

                // PROGRESSBAR
                $("#jlupload_progressbar_" + witchid + "_" + current[idx].queque[quequeidx].fileid).css("width", barw)
                // % PROGRESS
                $("#jlupload_progressinfolabel_" + witchid + "_" + current[idx].queque[quequeidx].fileid).html(Math.ceil(percentComplete) + '%')
                current[idx].queque[quequeidx]['progress'] = percentComplete;

                current[idx].options.fileuploadprogress(current[idx].queque[quequeidx], percentComplete)
                // UPDATE TOTAL PROGRESS
                progress.totalprogress(witchid, idx)


            },

            remove: function (controlid, quequeidx, idx) {
                $("#jlupload_progress_" + controlid).delay(1000).fadeOut("slow", function () { $(this).remove() })//.delay(2000).remove();
                _finiti = jQuery.grep(current[idx].queque, function (el) {
                    return el['status'] == 'completed' || el['status'] == 'canceled' || el['status'] == 'aborted'
                }).length;

                // FILE COMPLETED
               
                current[idx].options.fileuploaded(current[idx].queque[quequeidx]);
                // LATEST FILE IN QUEQUE
                if (_finiti == current[idx].queque.length) {
                    current[idx].options.uploadcompleted(current[idx].queque);
                    // EMPTY QUEQUE
                    current[idx].queque = new Array();
                    current[idx].queque.length = 0;
                    current[idx].globalxhr = new Array
                    current[idx].globalxhr.length = 0;
                    // RESET UPLOADER
                    // FIX FOR OPERA (OPERA doesn't clear selected files list so reset input field by replace it)
                    $("#upload" + (idx + 1)).replaceWith($("#upload" + (idx + 1)).val('').clone(true));

                }
            }

        }


    };


    $.fn[pluginName] = function (options) {

        return this.each(function () {

            currentjqel = $(this);

            if (!$.data(this, 'plugin_' + pluginName)) {
                $.data(this, 'plugin_' + pluginName,
                new Plugin(this, options));
            }

            // INCREMENTAL ID
            if (typeof cid == 'undefined') cid = 0;// sta per current id


            if (current[cid]) {

                // CURRENT[IDX] ALREADY EXISTS
                current[cid] = {
                    barwidth: 0,
                    queque: new Array(),
                    // STORAGE FOR ALL XHTMLREQUEST HTML5 MODE
                    globalxhr: new Array,
                    options: myopt
                };
            }
            else {
                current.push({
                    barwidth: 0,
                    queque: new Array(),
                    // STORAGE FOR ALL XHTMLREQUEST HTML5 MODE
                    globalxhr: new Array(),
                    options: myopt
                });


            }
            
            currentcontrolid[cid] = $(this).attr('id')
            cid++

           
                 
              
            // use HTML5
            setHTML5(cid, (cid - 1));
              
           

            $(this).remove();


            //  SET CONTROLS TO WORK WITH HTML5
            function setHTML5(witchid, idx) {
               // console.info(myself);
                currentjqel.after(htmltemplate.replace(/%cid%/g, cid).replace("%selectlabel%", options.buttonlabel).replace(/%currentcontrolid%/g, currentcontrolid[idx]))

                $("#jlupload_selectbutton_" + witchid).on("click", function () {
                    
                    
                    $("#upload" + witchid).trigger("click");
                });
                
                // FILESELECTED
                $("#upload" + witchid).on("change", function () {
                   
                   
                    // OFSET IF QUEQUE IS NOT EMPTY
                    _offset = (current[idx].queque.length ? current[idx].queque.length : 0);
                    
                    

                    $(this.files).each(function (i, v) {
                        
                        


                        // FILTER
                        if (current[idx].options.filter != '') {
                            allowedext = current[idx].options.filter.toLowerCase().split(",")
                            
                            poinposition = this.name.lastIndexOf(".");
                            totallength = this.name.length;
                            ext = this.name.substring(poinposition + 1, totallength).toLowerCase();
                            
                            // EXT ON FILTER?
                            if ($.inArray(ext, allowedext) > -1)
                                current[idx].queque.push({ 'idx': witchid + '_' + (i + 1 + _offset), 'fileprefix': current[idx].options.fileprefix, 'name': this.name, 'status': 'queque', 'controlid': witchid, 'fileid': (i + 1 + _offset), 'folder': current[idx].options.uploadfolder, 'progress': 0, 'file': this })
                            else
                                current[idx].options.filerefused(current[idx].options.filter, this);

                        }
                        else
                            current[idx].queque.push({ 'idx': witchid + '_' + (i + 1 + _offset), 'fileprefix': current[idx].options.fileprefix, 'name': this.name, 'status': 'queque', 'controlid': witchid, 'fileid': (i + 1 + _offset), 'folder': current[idx].options.uploadfolder, 'progress': 0, 'file': this })
                        
                    });
                    
                       
                    if (current[idx].queque.length > 0)
                    {
                        
                        // ONE FILE SELECTED
                        if (this.files.length == 1) {
                            $("#jlupload_label_" + witchid).html(this.files[0].name)
                        } // MULTIPLE FILE SELECTED
                        else { $("#jlupload_label_" + witchid).html(this.files.length + ' ' + current[idx].options.multiselectedlabel) };
                        progress.add(witchid, idx)
                    }
                    

                });
            };

        });
    }



})(jQuery, window, document);
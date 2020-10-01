
jsvalidator = {
    state: {
        isvalid: true,
        messageError: "",
        rawError: []
    },
    reset: function () {
        $(".validationerror").removeClass("validationerror");
        jsvalidator.state.isvalid = true;
        jsvalidator.state.messageError = "";
        jsvalidator.state.rawError = [];
    },
    rex: {
        date: new RegExp(/^(((0[1-9]|[12]\d|3[01])\/(0[13578]|1[02])\/((19|[2-9]\d)\d{2}))|((0[1-9]|[12]\d|30)\/(0[13456789]|1[012])\/((19|[2-9]\d)\d{2}))|((0[1-9]|1\d|2[0-8])\/02\/((19|[2-9]\d)\d{2}))|(29\/02\/((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))))$/),
        cf: new RegExp(/^[A-Z]{6}[0-9]{2}[A-Z][0-9]{2}[A-Z][0-9]{3}[A-Z]$/),
        pass: new RegExp(/(^[a-zA-Z0-9\_\*\-\+\!\?\,\:\;\.\xE0\xE8\xE9\xF9\xF2\xEC\x27]{6,12})$/),
        mail: new RegExp(/^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/),
        cforpiva: new RegExp(/^([A-Za-z]{6}[0-9lmnpqrstuvLMNPQRSTUV]{2}[abcdehlmprstABCDEHLMPRST]{1}[0-9lmnpqrstuvLMNPQRSTUV]{2}[A-Za-z]{1}[0-9lmnpqrstuvLMNPQRSTUV]{3}[A-Za-z]{1})|([0-9]{11})$/),
        piva: new RegExp(/^([0-9]{11})$/),
        int: new RegExp(/^\d+$/),
        dec: new RegExp(/(^\d+)((([\.]|[\,])(\d+$))|$){1}/),
        cap: new RegExp(/^[0-9]{5}$/),
        tel: new RegExp(/^[\+]{0,1}[0-9 \.]+$/),
        req: null

    },
    init: function () {
        String.prototype.validate = function (rex) {

            let _val = this.replace(/^\s+|\s+$/, '');

            if (rex) {
                if (_val.length > 0 && !_val.match(rex))
                    return false
                else
                    return true;
            }
            else {
                // field required
                return _val.length > 0;
            }


        };

        $("body").on("blur", "[data-validation]", function () { jsvalidator.validateField(this) })

    },
    destroy: function () {

    },
    validateField: function (control, fromValidateAll) {

        // resetto il messaggio di errore
        if (fromValidateAll == false) {
            jsvalidator.reset();
        }

        let _dacontrollare = $(control).attr("data-validation").split(",");
        let _fieldname = $(control).attr("data-validationfield");
        let _tipo = $(control).attr("data-type");
        let _valorecampo = $(control).val();
        if (_tipo == "ckeditor") {
            _valorecampo = CKEDITOR.instances[$(control).attr("id")].getData();
        }
        let error = false;

        // valida un campo specifico
        $(_dacontrollare).each(function (idx, el) {

            let _currentfielderror = false;
            let _errMsg = "";
            let _errMsgLangKey = "";
            let rex;

            switch (el) {

                case "required":
                    // rimuove tutti gli spazi
                    // error == false && _valorecampo.replace(/^\s+|\s+$/, '').length == 0 ? _currentfielderror = true : _currentfielderror = (false | error);
                    rex = jsvalidator.rex.req;
                    _errMsg = "Il campo -1 è obbligatorio";
                    break;
                case "date":
                    //_currentfielderror = _valorecampo.validate(jsvalidator.rex.date) | error;
                    _errMsg = "La data inserita per il campo -1 non è corretta";
                    rex = jsvalidator.rex.date;
                    break;
                case "email":
                    rex = jsvalidator.rex.mail;
                    _errMsg = "L\'email inserita per il campo -1 non è corretta";

                    break;
                case "codicefiscale":
                    rex = jsvalidator.rex.cf;
                    _errMsg = "Il codice fiscale inserito non è corretto";

                    break;
                case "partitaiva":
                    rex = jsvalidator.rex.piva;
                    _errMsg = "La partita iva inserita non è corretta";

                    break;
                case "partitaivaorcodicefiscale":
                    rex = jsvalidator.rex.cforpiva;
                    _errMsg = "Inserire almeno un valore tra la partita iva e il codice fiscale";

                    break;
                case "decimal":
                    rex = jsvalidator.rex.dec;
                    _errMsg = "Il valore inserito per il campo -1 deve essere numerico";

                    break;
                case "integer":
                    rex = jsvalidator.rex.int;
                    _errMsg = "Il valore inserito per il campo -1 deve essere numerico intero";
                    break;
                case "password":
                    rex = jsvalidator.rex.pass;
                    _errMsg = "La password inserita deve essere di almeno 6 caratteri e al massimo 12";
                    break;
                case "cap":
                    rex = jsvalidator.rex.cap;
                    _errMsg = "Il cap inserito non è corretto";
                    break;
                case "telefono":
                    rex = jsvalidator.rex.tel;
                    _errMsg = "Il telefono inserito non è corretto";
                    break;
                case "file":
                    rex = jsvalidator.rex.req;
                    _errMsg = "Inserire il file";
                    break;
            }

            if (el) {
                // verifica file ed estensione
                // ***************************
                if (el == "file") {
                    // verifico file obbligatorio ed estensione ammessa
                    var files = $(control).get(0).files;

                    if (files.length == 0) {
                        _currentfielderror = false;
                    }
                    else {

                        var allext = $(control).attr("data-fileextension");
                        var fname = files[0].name;

                        if (allext) {
                            var extfound = false;
                            $.each(allext.split(","), function (i, v) {

                                if (fname.match(v + "$")) {
                                    extfound = true;
                                }
                            })
                            _currentfielderror = extfound;

                        }
                        else {
                            _currentfielderror = true;
                        }
                        // _currentfielderror = $(control).get(0).files > 0;
                    }


                }
                else
                    _currentfielderror = _valorecampo.validate(rex); // true se valido

            }
            else {
                _currentfielderror = true;
            }


            if (!_currentfielderror) {
                // errore
                jsvalidator.state.messageError += _errMsg.replace("-1", _fieldname) + "<br/>";
                jsvalidator.state.rawError.push({ fielName: _fieldname, langKey: $(control).attr("data-validationlangkey"), error: _errMsg.replace("-1", _fieldname), controlId: $(control).attr("id") });

                $(control).addClass("validationerror")
                error = true;
            }
            else {
                //$(control).removeClass("validationerror")
            }


        });

        if (error) {
            return true
        }
        else {
            return false
        }
    },
    validateContainer: function (containerSelector) {

        jsvalidator.reset();

        var _isvalid = true;
        var _c = $(containerSelector + " [data-validation]")

        $(_c).each(function (i, el) {

            let check = jsvalidator.validateField(el, true);
            _isvalid = !check & _isvalid;

        });

        jsvalidator.state.isvalid = _isvalid;


        return jsvalidator.state;
    }

}

jsvalidator.init();





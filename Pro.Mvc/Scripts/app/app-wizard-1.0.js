
//wizard ====================================

function app_wizard(showSteps, showBorder, height) {
    var slf = this;
    this.ShowSteps = showSteps;
    this.$tabs = $('#wizard').tabs();

    if (height)
        $('#wizard').css('height', height);
    if (!showBorder) {
        $('#wizard').css('border', 'none');
        //$(".ui-tabs-panel").css('padding', '0');
    }
    if (showSteps) {
        //$('.next-tab').hide();
        //$('#wizard-validation').hide();

        $('.prev-tab').hide();
        $('.end-tab').hide();
    }

    $('.next-tab').click(function () {
        var totalSize = $(".ui-tabs-panel").length;
        var active = $('#wizard').tabs("option", "active");

        if (!wizard_validate_step(active)) {
            return;
        }

        var next = active + 1;

        if (slf.ShowSteps) {
            if (next >= totalSize - 1) {
                $('.next-tab').hide();
                $('.end-tab').show();
            }
            $('.prev-tab').show();
        }

        
        slf.displayStep(next, totalSize);
        return false;
    });
    $('.prev-tab').click(function () {

        var active = $('#wizard').tabs("option", "active");
        var prev = active - 1;

        if (slf.ShowSteps) {
            if (prev <= 0) {
                $('.prev-tab').hide();
            }
            $('.end-tab').hide();
            $('.next-tab').show();
            $('#wizard-validation').val("");
        }

        //$('#wizard').tabs({ active: prev });
        slf.displayStep(prev);
        //$tabs.tabs('select', $(this).attr("rel"));
        return false;
    });

    this.displayStep = function (step, totalSize) {

        $('#wizard').tabs({ active: step });
        
        if (totalSize === undefined)
            totalSize = $(".ui-tabs-panel").length;
        //totalSize++;
        step++;
        $('#wizard-validation').val("שלב " + step + " מתוך " + totalSize);
    }
    this.displayWizardValidation = function (message) {
        $('#wizard-validation').val(message);
        return false;
    }
    //var wizard_validate_step = function (step) {

    //    return false;
    //}
    //displayStep(0);

    //end wizard =====================================

};

function app_wiztabs() {

    var slf = this;

    this.getCurrent = function () {
        return $("#wiz-container").find('.active');
    }
    this.getIframe = function () {
        var iframe = this.getCurrent().find('.wiz-partial').find("iframe");
        if (iframe && iframe.length>0) {
            return iframe[0].contentWindow;
        }
        return null;
        //$(wizard.getCurrent().find('.wiz-partial')[0].childNodes['0'])[0].contentWindow.def.doSubmit

        //return cur.find("iframe")[0].contentWindow;
    }
    //this.doIframeCallback = function (callback) {
    //    var iframe = this.getIframe();
    //    if (iframe && iframe.def && callback) {
    //        iframe.def.callback();
    //    }
    //}
    this.wizHome = function () {
        this.displayStep(1);
    }
    this.displayStep = function (step) {
        var current = $("#wiz-container").find('.active');
        current.removeClass('active')
        $("#wiz-" + step).addClass('active');
    }
    this.clearStep = function (step) {
        var part = (step) ? $("#divPartial" + step) : this.getCurrent().find('.wiz-partial');
        if (part.length > 0)
            part.children().remove();
    }

    this.changeIframe = function (src) {
        var iframe = this.getCurrent().find('.wiz-partial').find("iframe");
        app_iframe.changeIframe(iframe,src);
    }
    this.loadIframe = function (step, src, width, height, scrolling) {
        //var part = $("#divPartial" + step);
        //if (part.length > 0)
        //    part.children().remove();
        app_iframe.removeIframe("divPartial" + step);
        this.displayStep(step);
        app_iframe.loadIframe("divPartial" + step, src, width, height, scrolling);
    }
    this.appendIframe = function (step, src, width, height, scrolling,loaderTag) {
        //var part = $("#divPartial" + step);
        //if (part.length > 0)
        //    part.children().remove();
        app_iframe.removeIframe("divPartial" + step);
        this.displayStep(step);
        app_iframe.appendIframe("divPartial" + step, src, width, height, scrolling,loaderTag);
    }
    this.attachIframe = function (step, src, width, height, scrolling) {
        this.displayStep(step);
        app_iframe.attachIframe("divPartial" + step, src, width, height, scrolling);
    }
    this.removeIframe = function (step) {
        var part = (step) ? $("#divPartial" + step) : this.getCurrent().find('.wiz-partial');
        if (part.length > 0)
            part.children().remove();
    }
    this.existsIframe = function (step) {
        var part = (step) ? $("#divPartial" + step) : this.getCurrent().find('.wiz-partial');
        return (part.children().length > 0);
    }
    this.closeIframe = function () {

        this.displayStep(1);
    }

    this.displayWizardValidation = function (message) {
        $('#wizard-validation').val(message);
        return false;
    }
    //var wizard_validate_step = function (step) {

    //    return false;
    //}
    //displayStep(0);

    //end wizard =====================================
};

function wiz_control(controlName,tagWindow) {
    //this.html,
    //this.control_sync,
    this.control_name = controlName,
    this.tagWindow = tagWindow;
    this.init = function (html, data, syncCallback) {
        //var pasive = dataModel.Option == "a" ? " pasive" : "";
        //this.html = html;
        $(this.tagWindow).html(html).hide();

        if (syncCallback)
            syncCallback(data);

        //var slf = this;

        //if (this.control_sync == null)
        //    this.control_sync = syncCallback();

        //this.control_sync.init(dataModel, userInfo);
        //this.load();
    },
    this.clearDataForm = function (form) {
        if (form === undefined)
            form = "fcForm";
        app_jqxform.clearDataForm(form);
    },
    this.load = function (form, source, loadCallback) {

        if (form === undefined || form === null)
            form = "fcForm";

        //if (this.RecordId > 0) {

        if (this.viewAdapter === undefined || this.viewAdapter === null) {

            this.viewAdapter = new $.jqx.dataAdapter(source, {
                loadComplete: function (record) {
                    if (record) {
                        if (loadCallback) {
                            loadCallback(record);
                        }
                        else {
                            app_form.loadDataForm(form, record);
                        }
                    }
                },
                loadError: function (jqXHR, status, error) {
                },
                beforeLoadComplete: function (records) {
                }
            });
        }
        else {
            this.viewAdapter._source.data = source.data;
        }
        this.viewAdapter.dataBind();
        //}
        //else {
        //    $('#RecordId').val(this.RecordId);
        //    $('#UserId').val(this.UserInfo.UserId);
        //    $('#AccountId').val(this.UserInfo.AccountId);
        //}
    },
    this.loadData = function (form, record, loadCallback) {

        if (form === undefined || form === null)
            form = "fcForm";

        if (record) {
            if (loadCallback) {
                loadCallback(record);
            }
            else {
                app_form.loadDataForm(form, record);
            }
        }
        else {
            console.log('load new');
        }
    },
    this.display = function () {
        $(this.tagWindow).show();
    },
    this.doCancel = function () {
        window.parent.triggerWizControlCancel(this.control_name);
    },
    this.doSubmit = function (preCallback, successCallback) {
        //e.preventDefault();
        var slf = this;
        var actionurl = $('#fcForm').attr('action');
        if (preCallback)
            preCallback(this.control_name);
        var validationResult = function (isValid) {
            if (isValid) {
                $.ajax({
                    url: actionurl,
                    type: 'post',
                    dataType: 'json',
                    data: $('#fcForm').serialize(),
                    success: function (data) {
                        if (successCallback)
                            successCallback(data);
                        else {
                            app_dialog.alert(data.Message);
                            if (data.Status >= 0) {
                                //if (slf.IsDialog) {
                                window.parent.triggerWizControlCompleted(slf.control_name,data.OutputId);
                                //    //$('#fcForm').reset();
                                //}
                                //else {
                                //    app.refresh();
                                //}
                                //$('#RecordId').val(data.OutputId);
                            }
                        }
                    },
                    error: function (jqXHR, status, error) {
                        app_dialog.alert(error);
                    }
                });
            }
        }
        $('#fcForm').jqxValidator('validate', validationResult);
    };
};

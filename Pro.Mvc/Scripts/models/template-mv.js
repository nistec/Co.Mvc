'use strict';

class template_mv {

    constructor($element, dataModel) {
        this.$element = $element;
        this.Title = "";
        this.Name = "";
        this.Tag = "#";

        if (dataModel)
            this.init(dataModel);
    }

    _loadHtml(dataModel) {

        const tagMenu = "liMain";

         var html=`
<!-- html body -->
`
      
        if (dataModel.Inline == true) {
            app_panel.appendPanelAfter(this.$element, html, this.Title, '800px');
        }
        else
            $(this.$element).html(html);

        this.Rule = app_perms.renderRule(this.Name, dataModel.Option);

        //buttons
        var _slf = this;

        $(this.Tag + "-Submit").on("click", function () {
            _slf.doSubmit();
        });
        $(this.Tag + "-Cancel").on("click", function () {
            _slf.doCancel();
        });
        $(this.Tag + "-Clear").on("click", function () {
            _slf.doClear();
        });

        app_menu.activeLayoutMenu(tagMenu);
        app_menu.breadcrumbs("Main", "Members", 'en');
    }

    _loadData() {

    }

    _loadRules() {

    }

    init(dataModel, userInfo) {

        this._loadHtml(dataModel);

        this.Inline = dataModel.Inline;
        this.RecordId = dataModel.Id;
        this.UserInfo = userInfo;
        this.AccountId = userInfo.AccountId;

        //add control here


        this._loadData();
        this._loadRules();
    }

    doDisplay() {

        if (this.Inline)
            app_panel.panelAfterShow(this.$element);
        else
            $(this.$element).show();
    }

    doSubmit() {

        var _slf = this;

        app_form.doFormPost(this.TagForm, function (data) {

            _slf.doClose(true);

        });//, preSubmit, validatorTag);
    }

    doClose(refresh) {

        if (this.Inline) {
            if (refresh)
                app_members_base.triggerMembersRefresh();
            app_panel.panelAfterClose(this.$element);
        }
        else {
            app.redirectTo("????");
        }
    }

    doClear() {
        app_form.clearDataForm(this.TagForm, ["AccountId","UserId"]);
    }

}

    

 


    

 

 

  

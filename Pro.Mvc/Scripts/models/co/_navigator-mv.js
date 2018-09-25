'use strict';

const TagSpa = '#SPA';
//var members_grid;
var wizard;
var current_mv;

var navigator_mv = {

    navigate: function (model,args) {
        
        $('#spa_dynamic_style').remove();

        //if (current_mv)
        //    delete current_mv;

        switch (model) {
            case "dashboard":
                current_mv = new dashboard_mv(TagSpa);
                current_mv.init();
                break;
            case "members":
                current_mv = new members_mv(TagSpa);
                var dataModel = new members_query(userInfo);
                current_mv.init(dataModel, userInfo);
                break;
        }

    },
    getRule:function(name) {

        var rule = userInfo.DefaultRule;
        var claims = userInfo.Claims;
        if (claims) {
            var r = $.grep(records, function (item) { return item["ItemName"] == name; });
            if (r)
                return r;
        }
        return rule;
    }
}


var co_navigator2 = function () {

    var model;

    $('#co_members').on('click', function (e) {

        model = new model_members();
        model.init(dataModel, userInfo);

    });





}
    

 


    

 

 

  

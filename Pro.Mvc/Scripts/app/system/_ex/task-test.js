

function app_task_demo()
{
    var task = new app_task();
    task.
   
}

var app_task = (function () {

    var app_task = function (element, TaskId, userInfo, taskModel) {

        this.$element = $(element);
        //this.$element = document.querySelectorAll(element);

        //if (!this.$element.is('div')) {
        //    $.error('app_wiztabs should be applied to DIV element');
        //    return;
        //}
        //// ensure to use the `new` operator
        //if (!(this instanceof app_task))
        //    return new app_task(element, TaskId, userInfo, taskModel);

        //this.TaskId = dataModel.PId;
        //this.Model = dataModel;
        //this.TaskModel = taskModel;
        //this.UserInfo = userInfo;
        //this.AccountId = userInfo.AccountId;
        //this.UserRole = userInfo.UserRole;
        //this.AllowEdit = (this.UserRole > 4) ? 1 : 0;
        //this.Title = (this.TaskModel == 'E') ? 'כרטיס' : 'משימה';
        //this.uploader;
        //var slf = this;
        //var exp1_Inited = false;

    
        return this;
    };

    app_task.prototype.doCancel = function () {
        app.redirectTo("/System/TaskUser");
        //app_messenger.Notify("הודעה");
        //parent.triggerCancelEdit();
        return this;
    }

    app_task.prototype.doComment = function (id) {
        //wizard.displayStep(2);
        $.ajax({
            type: 'GET',
            url: '/System/_TaskComment',
            data: { "id": id },
            success: function (data) {
                $('#divPartial2').html(data);
            }
        });
        return this;
    };

 

    return app_task;

}());

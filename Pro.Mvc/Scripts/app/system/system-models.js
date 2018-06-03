
//============================================================================================ app_task_form_template

var app_task_model = {

    taskBoard: function(tag) {

        var html = (function () {/*
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">משימות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/ReminderNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> תזכורת </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/TaskNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> משימה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול משימות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportSubTask">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> דוח פעילות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=task_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי משימות </span>
                        </a>
                    </li>
                </ul>
            </div>
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">סוגיות</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TopicNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגיה חדשה </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTopics">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול סוגיות </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=topic_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי סוגיות </span>
                        </a>
                    </li>
                </ul>
            </div>
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">תיעוד</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/DocNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> מסמך חדש </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportDocs">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול מסמכים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=doc_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי מסמכים </span>
                        </a>
                    </li>
                </ul>
            </div>
            <div class="panel-area">
                <div class="panel-area-title" id="hxp-title">כרטיסים</div>
                <ul class="rtl">
                    <li class="padding-r10">
                        <a href="/System/TicketNew?pid=0">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> כרטיס חדש </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/ReportTasks">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> ניהול כרטיסים </span>
                        </a>
                    </li>
                    <li class="padding-r10">
                        <a href="/System/DefEntity?entity=ticket_type">
                            <i class="fa fa-angle-double-left" style="font-size:20px"></i><span> סוגי כרטיסים </span>
                        </a>
                    </li>
                </ul>
            </div>*/}).toString().match(/[^]*\/\*([^]*)\*\/\}$/)[1];

        $(tag).html(html);

    }

};

'use strict';

class model_main {

    constructor($element, userModel) {
        this.$element = $element;
        this.load(userModel);
    }

    load(userModel) {
        var html =
 `
<div class="global indent">

    <!--content-->
    <div class="container rtl">
        <h2 id="cms_1-text-main-1" class="indent center">מערכת ניהול</h2>
        <div class="spacer-10"></div>
        <div class="row">
            <div class="services-box">
                <div class="col-lg-4 col-md-4 col-sm-12">
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">מערכת מנויים</div>
                                <a  href="/Co/Spa"><img src="/Images/main/members.jpg" style="width:220px;height:152px" /></a>

                            </div>
                        </div>
                    </div>
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">אוטומציה שיווקית</div>
                                <a  href="#"><img src="/Images/main/automation.jpg" style="width:220px;height:152px" /></a>

                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-12">
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">מערכת משימות</div>
                                <a href="/System/SystemBoard"><img src="/Images/main/tasks-mng.jpg" style="width:220px;height:152px" /></a>
                            </div>
                        </div>
                    </div>
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">עמודי נחיתה</div>
                                <a href="#"><img src="/Images/main/web-design.jpg" style="width:220px;height:152px" /></a>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-lg-4 col-md-4 col-sm-12">
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">מנהל מערכת</div>
                                <a href="/Admin/Main"><img src="/Images/main/admin.jpg" style="width:220px;height:152px" /></a>
                            </div>
                        </div>
                    </div>
                    <div class="thumb-pad4">
                        <div class="thumbnail">
                            <div class="caption">
                                <div class="center">מערכת דיוור</div>
                                <a href="/Netcell/NetcellBoard"><img src="/Images/main/reports.jpg" style="width:220px;height:152px" /></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

`
        $(this.$element).html(html);

        //app_menu.activeLayoutMenu("liMain");
        //app_menu.breadcrumbs("Main", "Members", 'en');
    }

    init(dataModel,userInfo) {

    }
}

    

 


    

 

 

  

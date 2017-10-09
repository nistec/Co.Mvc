//============================================================================================ app_control

var app_common = {
  
    refreshEntity: function (entity, callback) {
        app_query.doPost("/Common/RefreshEntity", {'entity': entity }, callback);
    }

};

//============================================================================================ app_rout

var app_rout = {

    isAllowEdit: function (allowEdit) {
        if (allowEdit == 0) {
            app_dialog.alert('You have no permission for this action.');
        }
    },

    redirectToMembers: function () {
        app.redirectTo("/Main/Members");
    }

};
//============================================================================================ app_lookups

app_lookups = {

    lookup:function(url,id,tag){

        if (id > 0)
            app_query.doLookup(url, { 'id': id }, function (content) {
            $(tag).val(content);
        });
   },
    member_name: function (id, tag) {
        app_lookups.lookup('/Common/Lookup_MemberDisplay', id, tag);
    },
    project_name: function (id, tag) {
        app_lookups.lookup('/System/Lookup_ProjectName', id, tag);
    },

};

//============================================================================================ app_ComboDlg


//function app_ComboMembersDlg(tagName) {

//    var dlg= new app_ComboDlg(tagName);
//    dlg.init("בחירת לקוח\\מנוי", "RecordId", "DisplayName", '/Common/GetMemberDlg');
//    return dlg;
//}

function app_ComboDlg(tagName,tagDisplay,callback) {

    this.inputTag = tagName + '-combo';
    this.windowTag = tagName + '-window';
    this.tagId = tagName.replace('#', '');
    if (tagDisplay === undefined || tagDisplay == null)
        tagDisplay = tagName + '-display';
    else

    this.title = '';

    this.initList = function (url, hideArrow) {
        this.init("Value", "Label", url, hideArrow);
    },
    this.init = function (valueField, displayField, url,type, hideArrow) {
        var slf = this;
        var selectedItem;
        this.title = $(tagName + "-name").text();
        var container = $(tagName + "-container");
        var window = $('<div id="' + this.tagId + '-window"></div>');
        //window.append('<div id="' + this.tagId + '-title"></div>');
        window.append('<div id="' + this.tagId + '-combo"></div>');
        container.append(window);

        var source =
        {
            datatype: "json",
            type: 'POST',
            data:{'type':type},
            url: url
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        app_jqxcombos.setComboSourceAdapter(valueField, displayField, this.inputTag, dataAdapter, '100%');//, dropDownHeight, async) {
        if (hideArrow)
            $(slf.inputTag).jqxComboBox({ showArrow: false });
        //$(tagName + "-title").text(title);

        $(this.inputTag).on('keypress', function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode == '13') {
                slf.select();
            }
        })

        $(this.inputTag).on('select', function (event) {
            var args = event.args;
            if (args && args.index >= 0) {
                var type = args.type; // keyboard, mouse or null depending on how the item was selected.
                slf.selectedItem = args.item;
                if (type == 'mouse') {
                    slf.select();
                }
                //else if (type == 'keyboard') {
                //    slf.select();
                //}
            }
        });
        //$(this.windowTag).on('open', function (event) {
        //    var value = $(tagName).val();
        //    if (value) {
        //        setTimeout(function () {
        //            var item = $(slf.inputTag).jqxComboBox('getItemByValue', value);
        //            $(slf.inputTag).jqxComboBox('selectItem', item);
        //        }, 1000);
        //    }
        //});
    },
    this.display = function () {
        var value = $(tagName).val();
        if (value) {
            var slf = this;
            setTimeout(function () {
                var item = $(slf.inputTag).jqxComboBox('getItemByValue', value);
                $(slf.inputTag).jqxComboBox('selectItem', item);
            }, 1000);
        }
        app_jqx.displayPopover(this.windowTag, tagDisplay, this.title);

        //$(this.windowTag).jqxWindow('bringToFront');
        //$(this.windowTag).jqxWindow('focus');
    },
     this.select = function () {
         if (this.selectedItem) {
             $(tagName).val(this.selectedItem.value);
             $(tagDisplay).val(this.selectedItem.label);
         }
         else {
             $(tagName).val(0);
             $(tagDisplay).val(null);
         }
         //app_jqx.closeWindow(this.windowTag);
         $(this.windowTag).jqxPopover('close');
         if (callback)
             callback(this.selectedItem);
     },
    this.toggle = function () {
        if ($(this.windowTag).is(':visible'))
            $(this.windowTag).jqxPopover('close');//app_jqx.closeWindow(this.windowTag);
        else {
            this.display();
        }
    }
};

//============================================================================================ app_members

var app_members = {

    displayMemberFields: function (url,data) {
	    if(url===undefined || url==null)
	        url='/Common/GetMemberFieldsView';

		$.ajax({
		    url: url,
			type: 'post',
			dataType: 'json',
			data: data,
			success: function (data) {

			    $(".field-ex").hide();

				if (data) {
					$("#ExType").val(data.ExType);

					app_members.displayExField(data.ExId, "ExId");
					app_members.displayExField(data.ExEnum1, "ExEnum1");
					app_members.displayExField(data.ExEnum2, "ExEnum2");
					app_members.displayExField(data.ExEnum3, "ExEnum3");
					app_members.displayExField(data.ExField1, "ExField1");
					app_members.displayExField(data.ExField2, "ExField2");
					app_members.displayExField(data.ExField3, "ExField3");
					app_members.displayExField(data.ExRef1, "ExRef1");
					app_members.displayExField(data.ExRef2, "ExRef2");
					app_members.displayExField(data.ExRef3, "ExRef3");

				    //app.hideOrData("#divEnum1", data.ExEnum1, "");

					//app_members.displayExField(data.ExId, "#divExId", "#lblExId");
					//app_members.displayExField(data.ExEnum1, "#divEnum1", "#lblEnum1");
					//app_members.displayExField(data.ExEnum2, "#divEnum2", "#lblEnum2");
					//app_members.displayExField(data.ExEnum3, "#divEnum3", "#lblEnum3");
					//app_members.displayExField(data.ExField1, "#divField1", "#lblExField1");
					//app_members.displayExField(data.ExField2, "#divField2", "#lblExField2");
					//app_members.displayExField(data.ExField3, "#divField3", "#lblExField3");
					//app_members.displayExField(data.ExRef1, "#divExRef1", "#lblExRef1");
					//app_members.displayExField(data.ExRef2, "#divExRef2", "#lblExRef2");
					//app_members.displayExField(data.ExRef3, "#divExRef3", "#lblExRef3");

                    /*
					if (data.ExEnum1 == "")
						$("#divEnum1").hide();
					else
						$("#lblEnum1").text(data.ExEnum1);

					if (data.ExEnum2 == "")
						$("#divEnum2").hide();
					else
						$("#lblEnum2").text(data.ExEnum2);

					if (data.ExEnum3 == "")
						$("#divEnum3").hide();
					else
						$("#lblEnum3").text(data.ExEnum3);

					if (data.ExField1 == "")
						$("#divField1").hide();
					else
						$("#lblExField1").text(data.ExField1);

					if (data.ExField2 == "")
						$("#divField2").hide();
					else
						$("#lblExField2").text(data.ExField2);

					if (data.ExField3 == "")
						$("#divField3").hide();
					else
						$("#lblExField3").text(data.ExField3);

					if (data.ExId == "")
						$("#divExId").hide();
					else
					    $("#lblExId").text(data.ExId);

					if (data.ExRef1 == "")
					    $("#divExRef1").hide();
					else
					    $("#lblExRef1").text(data.ExRef1);

					if (data.ExRef2 == "")
					    $("#divExRef2").hide();
					else
					    $("#lblExRef2").text(data.ExRef2);

					if (data.ExRef3 == "")
					    $("#divExRef3").hide();
					else
					    $("#lblExRef3").text(data.ExRef3);
                    */
				}
			},
			error: function (jqXHR, status, error) {
				app_dialog.alert(error);
			}
		});
    },
    displayExField: function (exval,fieldname,match) {

        if (exval == "")
            $("#div" + fieldname).hide();
        else {
            $("#lbl" + fieldname).text(exval);
            $("#div" + fieldname).show();
        }
    },
    initMembersDlg: function (tagName) {

        if (tagName === undefined)
            return;

        //var source = $('#' + tagName + '-combo').jqxComboBox('source');

        //if (source != null)
        //{
        //    app_jqx.displayWindow('#' + tagName + '-window', '#' + tagName + '-button');
        //    return;
        //}

        var selectedItem;
        var source =
        {
            datatype: "json",
            datafields: [
                { name: 'RecordId', type: 'number' },
                { name: 'DisplayName', type: 'string' },
                { name: 'AccountId', type: 'number' }
            ],
            url: '/Common/GetMemberDlg',
        };
        var dataAdapter = new $.jqx.dataAdapter(source);

        app_jqxcombos.setComboSourceAdapter("RecordId", "DisplayName", tagName + '-combo', dataAdapter, 240);//, dropDownHeight, async) {

        $('#' + tagName + '-combo').on('select', function (event) {
            var args = event.args;
            if (args) {
                // index represents the item's index.                       
                var index = args.index;
                var item = args.item;
                // get item's label and value.
                var label = item.label;
                var value = item.value;
                var type = args.type; // keyboard, mouse or null depending on how the item was selected.

                selectedItem = item;
                $('#' + tagName).val(item.value);
                $('#' + tagName + '-display').val(item.label);
                app_jqx.closeWindow('#' + tagName + '-window');
            }
        });

        $('#' + tagName + '-button').on('click', function (event) {
            app_jqx.displayWindow('#' + tagName + '-window', '#' + tagName + '-button');
        });
    }

};

//============================================================================================ app_member_edit

var app_member_edit = {

	getrowId: function (gridTag) {

		var selectedrowindex = $(grid).jqxGrid('getselectedrowindex');
		if (selectedrowindex < 0)
			return -1;
		var id = $(gridTag).jqxGrid('getrowid', selectedrowindex);
		return id;
	},
	//load: function (id, userInfo) {
	//	this.TaskId = id;
	//	this.AllowEdit = userInfo.UserRole > 4 ? 1 : 0;
	//	this.grid(id);
	//	return this;
	//},
	add: function (wizard,wizTab, updateTag) {
		if (updateTag)
			$(updateTag).show();
		wizard.appendIframe(wizTab, app.appPath() + "/Main/_MemberAdd", "100%", "500px");
	},
	edit: function (wizard, recordId, wizTab, updateTag) {
		if (updateTag)
			$(updateTag).show();
		if (recordId > 0)
			wizard.appendIframe(wizTab, app.appPath() + "/Main/_MemberEdit?id=" + recordId, "100%", "500px");
	},
	view: function (wizard, recordId, wizTab, updateTag) {
		if (updateTag)
			$(updateTag).hide();
		if (recordId > 0)
			wizard.appendIframe(wizTab, app.appPath() + "/Main/_MemberView?id=" + id, "100%", "500px");
	},
	remove: function (gridTag) {
		var id = app_member_edit.getrowId(gridTag);
		if (id > 0) {
			if (confirm('האם למחוק מנוי ' + id)) {
				app_query.doPost(app.appPath() + "/Main/TaskAssignDelete", { 'id': id });
				$('#jqxgrid2').jqxGrid('source').dataBind();
			}
		}
	},
	refresh: function (gridTag) {
		$(gridTag).jqxGrid('source').dataBind();
	},
	cancel: function (wizard) {
		wizard.wizHome();
	},
	end: function (wizard,data) {
		wizard.wizHome();
		//wizard.removeIframe(2);
		if (data && data.Status > 0) {
			app_member_edit.refresh();
			app_jqxnotify.Info(data, true);
		}
	}
}
//============================================================================================ app_validator

var app_validator = {
    signupValididty: function (AccountId, MemberId, CellPhone, Email, ExId,funk) {

        $.ajax({
            url: "/Registry/SignupValidity",
            type: 'POST',
            dataType: 'json',
            data: { 'AccountId': AccountId, 'MemberId': MemberId, 'CellPhone': CellPhone, 'Email': Email, 'ExId': ExId },
            success: function (data) {
                funk(data);
            }
        });
    }
};

//============================================================================================ app_popup

var app_popup = {

    memberEdit: function (id) {
        //return popupIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600");
        
        return app_dialog.dialogIframe(app.appPath() + "/Common/_MemberEdit?id=" + id, "500", "600","מנוי "+ id);
    },
    managementEdit: function (id) {
        //return popupIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600");
        return app_dialog.dialogIframe(app.appPath() + "/Common/_ManagementEdit?id=" + id, "500", "600", "מנוי " + id);
    },
    cmsHtmlEditor: function (extid) {
        if (app_validation.notForMobile())
            return;
        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

        return app_dialog.dialogIframe("/Cms/CmsHtmlEdit?extid=" + extid, "850", "600", "Cms Html Editor")
    },
    cmsTextEditor: function (extid) {
        if (app_validation.notForMobile())
            return;
        //return popupIframe("/Cms/CmsContentEdit?extid=" + extid, "850", "600");

        return app_dialog.dialogIframe("/Cms/CmsTextEdit?extid=" + extid, "850", "600", "Cms Text Editor")
    },
    cmsPageSettings: function (pageType) {
        if (app_validation.notForMobile())
            return;
        return app_dialog.dialogIframe("/Cms/CmsPageSettings?pageType=" + pageType, "850", "600", "Cms Text Editor")
    },
    cmsAdminPageSettings: function (accountId,pageType) {
        if (app_validation.notForMobile())
            return;
        return app_dialog.dialogIframe("/Cms/CmsAdminPageSettings?accountId=" + accountId + "&pageType=" + pageType, "850", "600", "Cms Text Editor")
    },
    cmsPreview: function (folder, pageType) {
        if (app_validation.notForMobile())
            return;
        var path = "/Preview/" + pageType + "/" + folder;
        return app_dialog.dialogIframe(path, "850", "600", "Cms Preview",true)
    },
    batchMessageView: function (id) {
       
        return app_dialog.dialogIframe(app.appPath() + "/Common/_BatchMessageView?id=" + id, "400", "400", "נוסח הודעה " + id);
    },
    gridView: function (src,title) {
        return app_dialog.dialogIframe(app.appPath() + src, "850", "600", title);
    }
}

//============================================================================================ app_const

var app_const = {

    adminLink: '<a href="/Admin/Manager">מנהל מערכת</a>',
    accountsLink: '<a href="/Admin/DefAccount">ניהול לקוחות</a>'
};

//============================================================================================ app_menu

var app_menu = {

     activeLayoutMenu: function (li) {
        //$("#cssmenu>ul>li.active").removeClass("active");
        //$("#cssmenu>ul>li#" + li).addClass("active");

         $("#mainnav>ul>li.active").removeClass("active");
         $("#mainnav>ul>li#" + li).addClass("active");

    },

    printObject: function (obj) {
        //debugObjectKeys(obj);
        var o = obj;
    },

    breadcrumbs: function (section, page, lang) {

        var breadcrumbs = $(".breadcrumbs");
        breadcrumbs.text('');
        var b = $('<div style="text-align:left;direction:ltr;"></div>')

        if (lang === undefined || lang == 'en') {
            b.append($('<a href="/home/index">Home</a>'));
            b.append($('<span> >> </span>'));
            b.append($('<a href="/main/dashboard">Dashboard</a>'));
            b.append($('<span> >> </span>'));
            if (section.substr(0, 1) == '/') {
                b.append($('<a href="' + section + '">' + page + '</a>'));
                b.append($('<span> | </span>'));
            }
            else
                b.append('' + section + ' >> ' + page + ' |  ');


            b.append('<a href="javascript:parent.history.back()">Back</a>');

            //var path = document.referrer;
            //var page = app.getUrlPage(path);
            //b.append($('<a href="' + path + '">' + page.split('?')[0] + '</a>'));
            //b.append($('<span> >> </span>'));
            //var curPage = app.getUrlPage(location.href);
            //b.append($('<span> ' + curPage.split('?')[0] + ' </span>'));
        }
        else {
            b.append($('<a href="/home/index">דף הבית</a>'));
            b.append($('<span> >> </span>'));
            b.append($('<a href="/main/dashboard">ראשי</a>'));
            b.append($('<span> >> </span>'));
            if (section.substr(0, 1) == '/') {
                b.append($('<a href="' + section + '">' + page + '</a>'));
                b.append($('<span> | </span>'));
            }
            else
                b.append('' + section + ' >> ' + page + ' |  ');
            b.append('<a href="javascript:parent.history.back()">חזרה</a>');

        }
        b.appendTo(breadcrumbs);
    }
};


//============================================================================================ app_jqx_list

var app_jqx_list = {

    branchComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Branch" : tag, '/Common/GetBranchView', 0, 120, false) },

    //placeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "PlaceOfBirth" : tag, '/Common/GetPlaceView', 0, 120, false) },

    statusComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Status" : tag, '/Common/GetStatusView', 0, 0, false) },

    chargeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Charge" : tag, '/Common/GetChargeView', 0, 0, false) },

    regionComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Region" : tag, '/Common/GetRegionView', 0, 120, false) },

    cityComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "City" : tag, '/Common/GetCityView', 150, 120, false) },

    genderComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Gender" : tag, '/Common/GetGenderView', 150, 0, false) },

    categoryCheckListAdapter: function (tag, output) {
        return app_jqxcombos.createCheckListAdapter("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView", 240, 140, false, output === undefined ? "Categories" : output)
    },
    categoryComboCheckAdapter: function (tag, output) { return app_jqxcombos.createComboCheckAdapter("PropId", "PropName", tag === undefined ? "listCategory" : tag, "/Common/GetCategoriesView", 240, 140, false, output === undefined ? "Categories" : output) },

    categoryComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Category" : tag, '/Common/GetCategoriesView', 0, 120, false) },

    entityEnumComboAdapter: function (tag) { return app_jqxcombos.createDropDownAdapter("FieldName", "FieldValue", tag === undefined ? "EntityEnumType" : tag, '/Common/GetMembersEnumFields', 0, 120, false) },

    enum1ComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum1" : tag, '/Common/GetEnum1View', 0, 120, false) },
    enum2ComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum2" : tag, '/Common/GetEnum2View', 0, 120, false) },
    enum3ComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "ExEnum3" : tag, '/Common/GetEnum3View', 0, 120, false) },
    userRoleComboAdapter: function (tag) { return app_jqxcombos.createDropDownAdapter("RoleId", "RoleName", tag === undefined ? "UserRole" : tag, '/Admin/GetUsersRoles', 0, 120, false) },
    campaignComboAdapter: function (tag) { return app_jqxcombos.createDropDownAdapterTag("PropId", "PropName", tag === undefined ? "#Campaign" : tag, '/Common/GetCampaignView', 200, 0, false, 'נא לבחור קמפיין') },
    taskTypeComboAdapter: function (tag) { return app_jqxcombos.createComboAdapter("PropId", "PropName", tag === undefined ? "Task_Type" : tag, '/System/GetTaskTypeList', 0, 120, false) },
    taskStatusComboAdapter: function (tag) { return app_jqxcombos.createDropDownAdapter("PropId", "PropName", tag === undefined ? "TaskStatus" : tag, '/System/GetTaskStatusList', 150, 120, false) }

};

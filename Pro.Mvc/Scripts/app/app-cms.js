//mc-view version 20150418.1

var cmsUrl = function (url) {
    //var abspath = window.location.protocol + "//" + window.location.host + url;
    return $('meta[name=cms]').attr('data-virtual') + url;// $('base').attr('href') + url;
};

var cmsActionLink = function (action, conroller) {
    //var link = window.location.protocol + "//" + window.location.host + "/" + conroller + "/" + action;
    return $('meta[name=cms]').attr('data-virtual') + "/" + conroller + "/" + action;
};

var cmsSiteId = function () {
    return $('meta[name=cms]').attr("data-site");
};

function cmsDisplayPage(pageName) {
    var src = cmsUrl(pageName);

    popupIframe(src, 900, 500, 'yes')
}

var htmlDecode = function (val) {
    if (!val) return "";
    return val.replace(/&gt;/g, ">").replace(/&lt;/g, "<");
};

var htmlEncode = function (val) {
    if (!val) return "";
    return val.replace(/</g, "&lt;").replace(/>/g, "&gt;");
}

var textDecode = function (val) {
    if (!val) return "";
    return val.replace(/\n/g, "<br/>");
};

var textEncode = function (val) {
    if (!val) return "";
    return val.replace(/<br\/>/g, "\n").replace(/<br \/>/g, "\n");
}

function nl2br(str, is_xhtml) {
    var breakTag = (is_xhtml || typeof is_xhtml === 'undefined') ? '<br />' : '<br>';
    return (str + '').replace(/([^>\r\n]?)(\r\n|\n\r|\r|\n)/g, '$1' + breakTag + '$2');
}
var menuItemFormat = function (id, href, text) {
    return '<li id="' + id + '"><a href="' + href + '">' + text + '</a></li>';
};

var getCmsPageCategory = function (catid) {
    switch (catid) {
        case 1:
            return "Master";
        case 2:
            return "Dynamic";
    }
    return "Inline";
};


function renderCmsMenu() {

    var sid = cmsSiteId();
    // prepare the data
    var view_source =
    {
        datatype: "json",
        datafields: [
                { name: 'PageId', type: 'number' },
                { name: 'PageName', type: 'string' },
                { name: 'PageTitle', type: 'string' },
                { name: 'TopMenu', type: 'string' },
                { name: 'FooterMenu', type: 'string' }
        ],
        id: 'PageId',
        data: { 'SiteId': sid },
        type: 'POST',
        url: cmsActionLink('ViewCmsMenu', 'Cms')
    };
    var viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (records) {
            var length = records.length;
            for (var i = 0; i < length; i++) {
                var record = records[i];
                if (record) {
                    var pid = record.PageId;
                    var topmenu = record.TopMenu;
                    if (topmenu) {
                        var id = 'menutop-pid-' + pid;
                        var href = cmsUrl('/Cms/Page?i=' + pid);
                        var mi = menuItemFormat(id, href, record.PageTitle);

                        $('#' + id).remove();
                        $('#' + topmenu).append(mi);
                    }
                    var footermenu = record.FooterMenu;
                    if (footermenu) {
                        var id = 'menufooter-pid-' + pid;
                        var href = cmsUrl('/Cms/Page?i=' + pid);
                        var mi = menuItemFormat(id, href, record.PageTitle);

                        $('#' + id).remove();
                        $('#' + footermenu).append(mi);
                    }
                }
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });
    viewAdapter.dataBind();
}

function renderCmsPage(pid) {

    var sid = cmsSiteId();
    // prepare the data
    var view_source =
    {
        datatype: "json",
        datafields: [
                { name: 'PageId', type: 'number' },
                { name: 'PageName', type: 'string' },
                { name: 'PageContent', type: 'string' }
        ],
        id: 'PageId',
        data: { 'SiteId': sid, 'PageId': pid },
        type: 'POST',
        url: cmsActionLink('ViewCmsPage', 'Cms')
    };
    var viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (record) {
            if (record) {
                var content = record.PageContent;
                if (content) {
                    $('#cms_container').html(htmlDecode(content));
                }
            }
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });
    viewAdapter.dataBind();
}

function renderCmsItems(pid) {

    var sid = cmsSiteId();

    // prepare the data
    var view_source =
    {
        datatype: "json",
        datafields: [
                { name: 'ItemId', type: 'string' },
                { name: 'Section', type: 'string' },
                { name: 'PageId', type: 'number' },
                { name: 'ItemContent', type: 'string' },
                { name: 'ItemType', type: 'string' },
                { name: 'ItemAttr', type: 'string' }
        ],
        id: 'ItemId',
        data: { 'SiteId': sid, 'PageId': pid },
        type: 'POST',
        url: cmsActionLink('ViewCmsPageItems', 'Cms')
    };
    var viewAdapter = new $.jqx.dataAdapter(view_source, {
        loadComplete: function (records) {
            renderCmsRecords(records);
        },
        loadError: function (jqXHR, status, error) {
        },
        beforeLoadComplete: function (records) {
        }
    });
    viewAdapter.dataBind();
}

var renderCmsRecords = function (records) {
    var length = records.length;
    for (var i = 0; i < length; i++) {
        var record = records[i];
        if (record) {

            var itemid = record.ItemId;
            var content = record.ItemContent;
            var attribs = record.ItemAttr;
            var itemtype = record.ItemType;

            if (content) {

                if (itemtype == 'json') {
                    if (content) {
                        $.each(JSON.parse(content), function (idx, obj) {
                            $('#' + obj.id).html(htmlDecode(obj.val));
                        });
                    }
                }
                else if (itemtype == 'link') {
                    if (content != '*')
                        $('#' + itemid).html(content);
                    $('#' + itemid).attr('href', attribs);
                    //if (attribs) {
                    //    $.each(JSON.parse(attribs), function (idx, obj) {
                    //        $('#cms_' + itemid).attr(obj.id, obj.val);
                    //    });
                    //}
                }
                else if (itemtype == 'href') {
                    $('#' + itemid).attr('href', content);
                    if (attribs) {
                        $.each(JSON.parse(attribs), function (idx, obj) {
                            $('#' + itemid).attr(obj.id, obj.val);
                        });
                    }
                }
                else if (itemtype == 'image') {
                    $('#' + itemid).attr('src', content);
                    if (attribs) {
                        $.each(JSON.parse(attribs), function (idx, obj) {
                            $('#' + itemid).attr(obj.id, obj.val);
                        });
                    }
                }
                else if (itemtype == 'background') {
                    $('#' + itemid).css("background-image", "url(" + content + ")");
                    if (attribs) {
                        $.each(JSON.parse(attribs), function (idx, obj) {
                            $('#' + itemid).attr(obj.id, obj.val);
                        });
                    }
                }
                else if (itemtype == 'text') {
                    if (content != '*')
                        $('#' + itemid).html(content);
                    if (attribs) {
                        $.each(JSON.parse(attribs), function (idx, obj) {
                            $('#' + itemid).attr(obj.id, obj.val);
                        });
                    }
                }
                else if (itemtype == 'input') {
                    if (content != '*')
                        $('#' + itemid).val(content);
                    if (attribs) {
                        $.each(JSON.parse(attribs), function (idx, obj) {
                            $('#' + itemid).attr(obj.id, obj.val);
                        });
                    }
                }
                else if (itemtype == 'style') {
                    if (attribs) {
                        $('#' + itemid).css(JSON.parse(attribs));
                    }
                }
                else {
                    $('#' + itemid).html(htmlDecode(content));
                }
            }
        }
    }

}


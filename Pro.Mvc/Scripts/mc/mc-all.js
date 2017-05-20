

function iframeDialog(src, width, height, title) {
    var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
    var dialog = $("<div></div>").append(iframe).appendTo("body").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: "auto",
        height: "auto",
        title:title,
        //create: function (event, ui) {
        //    $(".ui-widget-header").hide();
        //},
        close: function () {
            iframe.attr("src", "");
        }
    });

    iframe.attr({
        width: width,
        height: height,
        src: src
    });
    dialog.dialog("open");
    //dialog.dialog("option", "title", title).dialog("open");
    //$(".ui-dialog-titlebar").hide();
};

/*
function iframeDialog(src, width, height, title) {
    var iframe = $('<iframe frameborder="0" marginwidth="0" marginheight="0" allowfullscreen></iframe>');
    iframe.attr({
        width: width,
        height: height,
        src: src
    });
    //var dialog = $("<div id='popup'><span class='button b-close'><span>X</span></span></div>").append(iframe).appendTo("body").bPopup();//({
    //    modalClose: true,
    //    position:['400','auto'],
    //    opacity: 0.6
    //    //positionStyle: 'fixed' //'fixed' or 'absolute'
    //});

    var dialog = $("<div></div>").append(iframe).appendTo("body").dialog({
        autoOpen: false,
        modal: true,
        resizable: false,
        width: "auto",
        height: "auto",
        title: title,
        close: function () {
            iframe.attr("src", "");
        }
    });//.dialog("open");//dialog("option", "title", title).dialog("open");
    //iframe.attr({
    //    width: width,
    //    height: height,
    //    src: src
    //});
    dialog.dialog("option", "title", title).dialog("open");
    $(".ui-dialog-titlebar").hide();

};
*/
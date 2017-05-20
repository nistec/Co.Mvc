
$(document).ready(function () {
    $('a.Wmlink').attr('target', '_self');
    goWPage(0);
});

function goWPage(pageId) {

    var pid = "mwp-" + pageId;

    $('.WPage').each(function () {
        if ($(this).attr('id') === pid)
            $(this).show();
        else
            $(this).hide();
    });
}
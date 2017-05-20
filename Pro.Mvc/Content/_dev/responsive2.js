var resizing = false;
moveNavigation();
$(window).on('resize', function () {
    if (!resizing) {
        window.requestAnimationFrame(moveNavigation);
        resizing = true;
    }
});

function moveNavigation() {
    var mq = checkMQ(); //this function returns mobile,tablet or desktop 

    if (mq == 'mobile' && topNavigation.parents('.cd-side-nav').length == 0) { //topNavigation = $('.cd-top-nav')
        detachElements();
        topNavigation.appendTo(sidebar); //sidebar = $('.cd-side-nav')
        searchForm.prependTo(sidebar);
    } else if ((mq == 'tablet' || mq == 'desktop') && topNavigation.parents('.cd-side-nav').length > 0) {
        detachElements();
        searchForm.insertAfter(header.find('.cd-logo')); //header = $('.cd-main-header')
        topNavigation.appendTo(header.find('.cd-nav'));
    }
    resizing = false;
}

function detachElements() {
    topNavigation.detach();//topNavigation = $('.cd-top-nav')
    searchForm.detach();//searchForm = $('.cd-search')
}
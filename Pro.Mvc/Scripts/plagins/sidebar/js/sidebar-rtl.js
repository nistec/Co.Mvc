(function ($) {
    $(document).ready(function () {

//jQuery(document).ready(function(){
	//cache DOM elements
	var mainContent = $('.nbr-main-content'),
		header = $('.nbr-main-header'),
		sidebar = $('.nbr-side-nav'),
		sidebarTrigger = $('.nbr-nav-trigger'),
		topNavigation = $('.nbr-top-nav'),
		searchForm = $('.nbr-search'),
		accountInfo = $('.account');

	//on resize, move search and top nav position according to window width
	var resizing = false;
	moveNavigation();
	$(window).on('resize', function(){
		if( !resizing ) {
			(!window.requestAnimationFrame) ? setTimeout(moveNavigation, 300) : window.requestAnimationFrame(moveNavigation);
			resizing = true;
		}
	});

	//on window scrolling - fix sidebar nav
	var scrolling = false;
	//checkScrollbarPosition();
	//$(window).on('scroll', function(){
	//	if( !scrolling ) {
	//		(!window.requestAnimationFrame) ? setTimeout(checkScrollbarPosition, 300) : window.requestAnimationFrame(checkScrollbarPosition);
	//		scrolling = true;
	//	}
	//});

	//mobile only - open sidebar when user clicks the hamburger menu
	sidebarTrigger.on('click', function(event){
		event.preventDefault();
		$([sidebar, sidebarTrigger]).toggleClass('nav-is-visible');
	});

	//click on item and show submenu
	$('.nbr-has-children > a').on('click', function (event) {
	   
		var mq = checkMQ(),
			selectedItem = $(this);
		if( mq == 'mobile' || mq == 'tablet' ) {
			event.preventDefault();
			if( selectedItem.parent('li').hasClass('selected')) {
				selectedItem.parent('li').removeClass('selected');
			} else {
			    sidebar.find('.nbr-has-children.selected').removeClass('selected');
				accountInfo.removeClass('selected');
				selectedItem.parent('li').addClass('selected');
			}
		}
	});

	//click on account and show submenu - desktop version only
	accountInfo.children('a').on('click', function(event){
		var mq = checkMQ(),
			selectedItem = $(this);
		if( mq == 'desktop') {
			event.preventDefault();
			accountInfo.toggleClass('selected');
			sidebar.find('.nbr-has-children.selected').removeClass('selected');
		}
	});

	//$(document).on('click', function (event) {
	   
        //    if (!$(event.target).is('.nbr-has-children a')) {
        //		sidebar.find('.nbr-has-children.selected').removeClass('selected');
	//		accountInfo.removeClass('selected');
	//	}
	//});
	var activeRow;
	$('#mainnav li').on('click', function (event) {
	    //console.log('mainnav');
	    if ($(event.target).is('.nbr-has-children a')) {
	        if (activeRow) {
                $(activeRow).removeClass("active");
	        }
	        else {
	            sidebar.find('.nbr-has-children.active').removeClass('active');
	        }
	        $(this).addClass("active");
	        activeRow = this;
	    }
	    else {
	        sidebar.find('.nbr-has-children.selected').removeClass('selected');
	        accountInfo.removeClass('selected');
	    }
    });
	
	//$("ul.submeunav a").on('click', function (e) {
	//    //e.preventDefault(); 
	//    var url = $(this).attr("href");
	//    $('#body').load(url);
	//});

	/**
    * Activate a menu row.
    */
	//var activate = function (row) {
	//    //if (row == activeRow) {
	//    //    return;
	//    //}
	//    if (activeRow) {
	//        $(activeRow).removeClass("active")
	//    }
	//    $(row).addClass("active")
	//    activeRow = row;
	//};

	//on desktop - differentiate between a user trying to hover over a dropdown item vs trying to navigate into a submenu's contents
	//sidebar.children('ul').menuAim({
    //    activate: function(row) {
    //    	$(row).addClass('hover');
    //    },
    //    deactivate: function(row) {
    //    	$(row).removeClass('hover');
    //    },
    //    exitMenu: function() {
    //    	sidebar.find('.hover').removeClass('hover');
    //    	return true;
    //    },
        //    submenuSelector: ".nbr-has-children",
    //});

	function checkMQ() {
		//check if mobile or desktop device
		return window.getComputedStyle(document.querySelector('.nbr-main-content'), '::before').getPropertyValue('content').replace(/'/g, "").replace(/"/g, "");
	}

	function moveNavigation(){
  		var mq = checkMQ();
        
        if ( mq == 'mobile' && topNavigation.parents('.nbr-side-nav').length == 0 ) {
        	detachElements();
            $('.nbr-side-nav').css('z-index', 99200);
            topNavigation.appendTo(sidebar);
			searchForm.removeClass('is-hidden').prependTo(sidebar);
		} else if ( ( mq == 'tablet' || mq == 'desktop') &&  topNavigation.parents('.nbr-side-nav').length > 0 ) {
			detachElements();
            $('.nbr-side-nav').css('z-index', 100);
			searchForm.insertAfter(header.find('.nbr-logo'));
            topNavigation.appendTo(header.find('.nbr-nav'));
		}
		checkSelected(mq);
		resizing = false;
	}

	function detachElements() {
		topNavigation.detach();
		searchForm.detach();
	}

	function checkSelected(mq) {
		//on desktop, remove selected class from items selected on mobile/tablet version
	    if (mq == 'desktop') $('.nbr-has-children.selected').removeClass('selected');
	}

	function checkScrollbarPosition() {
		var mq = checkMQ();
		
		if( mq != 'mobile' ) {
			var sidebarHeight = sidebar.outerHeight(),
				windowHeight = $(window).height(),
				mainContentHeight = mainContent.outerHeight(),
				scrollTop = $(window).scrollTop();

			( ( scrollTop + windowHeight > sidebarHeight ) && ( mainContentHeight - sidebarHeight != 0 ) ) ? sidebar.addClass('is-fixed').css('bottom', 0) : sidebar.removeClass('is-fixed').attr('style', '');
		}
		scrolling = false;
	}
        //});
    });
})(jQuery);

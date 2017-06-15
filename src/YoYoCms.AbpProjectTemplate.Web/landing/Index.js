var $ = jQuery.noConflict();

$.fn.inlineStyle = function (prop) {
	return this.prop("style")[$.camelCase(prop)];
};

$.fn.doOnce = function( func ) {
	this.length && func.apply( this );
	return this;
}

if( $().infinitescroll ) {

	$.extend($.infinitescroll.prototype,{
		_setup_portfolioinfiniteitemsloader: function infscr_setup_portfolioinfiniteitemsloader() {
			var opts = this.options,
				instance = this;
			// Bind nextSelector link to retrieve
			$(opts.nextSelector).click(function(e) {
				if (e.which == 1 && !e.metaKey && !e.shiftKey) {
					e.preventDefault();
					instance.retrieve();
				}
			});
			// Define loadingStart to never hide pager
			instance.options.loading.start = function (opts) {
				opts.loading.msg
					.appendTo(opts.loading.selector)
					.show(opts.loading.speed, function () {
						instance.beginAjax(opts);
					});
			}
		},
		_showdonemsg_portfolioinfiniteitemsloader: function infscr_showdonemsg_portfolioinfiniteitemsloader () {
			var opts = this.options,
				instance = this;
			//Do all the usual stuff
			opts.loading.msg
				.find('img')
				.hide()
				.parent()
				.find('div').html(opts.loading.finishedMsg).animate({ opacity: 1 }, 2000, function () {
					$(this).parent().fadeOut('normal');
				});
			//And also hide the navSelector
			$(opts.navSelector).fadeOut('normal');
			// user provided callback when done
			opts.errorCallback.call($(opts.contentSelector)[0],'done');
		}
	});

} else {
	console.log('Infinite Scroll not defined.');
}

(function() {
	var lastTime = 0;
	var vendors = ['ms', 'moz', 'webkit', 'o'];
	for(var x = 0; x < vendors.length && !window.requestAnimationFrame; ++x) {
		window.requestAnimationFrame = window[vendors[x]+'RequestAnimationFrame'];
		window.cancelAnimationFrame = window[vendors[x]+'CancelAnimationFrame']
									|| window[vendors[x]+'CancelRequestAnimationFrame'];
	}

	if (!window.requestAnimationFrame)
		window.requestAnimationFrame = function(callback, element) {
			var currTime = new Date().getTime();
			var timeToCall = Math.max(0, 16 - (currTime - lastTime));
			var id = window.setTimeout(function() { callback(currTime + timeToCall); },
			  timeToCall);
			lastTime = currTime + timeToCall;
			return id;
		};

	if (!window.cancelAnimationFrame)
		window.cancelAnimationFrame = function(id) {
			clearTimeout(id);
		};
}());



function debounce(func, wait, immediate) {
	var timeout, args, context, timestamp, result;
	return function() {
		context = this;
		args = arguments;
		timestamp = new Date();
		var later = function() {
			var last = (new Date()) - timestamp;
			if (last < wait) {
				timeout = setTimeout(later, wait - last);
			} else {
				timeout = null;
				if (!immediate) result = func.apply(context, args);
			}
		};
		var callNow = immediate && !timeout;
		if (!timeout) {
			timeout = setTimeout(later, wait);
		}
		if (callNow) result = func.apply(context, args);
		return result;
	};
}


var requesting = false;

var killRequesting = debounce(function () {
	requesting = false;
}, 100);

function onScrollSliderParallax() {
	if (!requesting) {
		requesting = true;
		requestAnimationFrame(function(){
			SEMICOLON.slider.sliderParallax();
			SEMICOLON.slider.sliderElementsFade();
		});
	}
	killRequesting();
}



var SEMICOLON = SEMICOLON || {};

(function($){

	// USE STRICT
	"use strict";

	SEMICOLON.initialize = {

		init: function(){

			SEMICOLON.initialize.responsiveClasses();
			SEMICOLON.initialize.imagePreload( '.portfolio-item:not(:has(.fslider)) img' );
			SEMICOLON.initialize.stickyElements();
			SEMICOLON.initialize.goToTop();
			SEMICOLON.initialize.lazyLoad();
			SEMICOLON.initialize.fullScreen();
			SEMICOLON.initialize.verticalMiddle();
			SEMICOLON.initialize.lightbox();
			SEMICOLON.initialize.resizeVideos();
			SEMICOLON.initialize.imageFade();
			SEMICOLON.initialize.pageTransition();
			SEMICOLON.initialize.dataResponsiveClasses();
			SEMICOLON.initialize.dataResponsiveHeights();

			$('.fslider').addClass('preloader2');

		},

		responsiveClasses: function(){

			if( typeof jRespond === 'undefined' ) {
				console.log('responsiveClasses: jRespond not Defined.');
				return true;
			}

			var jRes = jRespond([
				{
					label: 'smallest',
					enter: 0,
					exit: 479
				},{
					label: 'handheld',
					enter: 480,
					exit: 767
				},{
					label: 'tablet',
					enter: 768,
					exit: 991
				},{
					label: 'laptop',
					enter: 992,
					exit: 1199
				},{
					label: 'desktop',
					enter: 1200,
					exit: 10000
				}
			]);
			jRes.addFunc([
				{
					breakpoint: 'desktop',
					enter: function() { $body.addClass('device-lg'); },
					exit: function() { $body.removeClass('device-lg'); }
				},{
					breakpoint: 'laptop',
					enter: function() { $body.addClass('device-md'); },
					exit: function() { $body.removeClass('device-md'); }
				},{
					breakpoint: 'tablet',
					enter: function() { $body.addClass('device-sm'); },
					exit: function() { $body.removeClass('device-sm'); }
				},{
					breakpoint: 'handheld',
					enter: function() { $body.addClass('device-xs'); },
					exit: function() { $body.removeClass('device-xs'); }
				},{
					breakpoint: 'smallest',
					enter: function() { $body.addClass('device-xxs'); },
					exit: function() { $body.removeClass('device-xxs'); }
				}
			]);
		},

		imagePreload: function(selector, parameters){
			var params = {
				delay: 250,
				transition: 400,
				easing: 'linear'
			};
			$.extend(params, parameters);

			$(selector).each(function() {
				var image = $(this);
				image.css({visibility:'hidden', opacity: 0, display:'block'});
				image.wrap('<span class="preloader" />');
				image.one("load", function(evt) {
					$(this).delay(params.delay).css({visibility:'visible'}).animate({opacity: 1}, params.transition, params.easing, function() {
						$(this).unwrap('<span class="preloader" />');
					});
				}).each(function() {
					if(this.complete) $(this).trigger("load");
				});
			});
		},

		verticalMiddle: function(){
			if( $verticalMiddleEl.length > 0 ) {
				$verticalMiddleEl.each( function(){
					var element = $(this),
						verticalMiddleH = element.outerHeight(),
						headerHeight = $header.outerHeight();

					if( element.parents('#slider').length > 0 && !element.hasClass('ignore-header') ) {
						if( $header.hasClass('transparent-header') && ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) ) {
							verticalMiddleH = verticalMiddleH - 70;
							if( $slider.next('#header').length > 0 ) { verticalMiddleH = verticalMiddleH + headerHeight; }
						}
					}

					if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') ) {
						if( element.parents('.full-screen').length && !element.parents('.force-full-screen').length ){
							if( element.children('.col-padding').length > 0 ) {
								element.css({ position: 'relative', top: '0', width: 'auto', marginTop: '0' }).addClass('clearfix');
							} else {
								element.css({ position: 'relative', top: '0', width: 'auto', marginTop: '0', paddingTop: '60px', paddingBottom: '60px' }).addClass('clearfix');
							}
						} else {
							element.css({ position: 'absolute', top: '50%', width: '100%', paddingTop: '0', paddingBottom: '0', marginTop: -(verticalMiddleH/2)+'px' });
						}
					} else {
						element.css({ position: 'absolute', top: '50%', width: '100%', paddingTop: '0', paddingBottom: '0', marginTop: -(verticalMiddleH/2)+'px' });
					}
				});
			}
		},

		stickyElements: function(){
			if( $siStickyEl.length > 0 ) {
				var siStickyH = $siStickyEl.outerHeight();
				$siStickyEl.css({ marginTop: -(siStickyH/2)+'px' });
			}

			if( $dotsMenuEl.length > 0 ) {
				var opmdStickyH = $dotsMenuEl.outerHeight();
				$dotsMenuEl.css({ marginTop: -(opmdStickyH/2)+'px' });
			}
		},

		goToTop: function(){
			var elementScrollSpeed = $goToTopEl.attr('data-speed'),
				elementScrollEasing = $goToTopEl.attr('data-easing');

			if( !elementScrollSpeed ) { elementScrollSpeed = 700; }
			if( !elementScrollEasing ) { elementScrollEasing = 'easeOutQuad'; }

			$goToTopEl.click(function() {
				$('body,html').stop(true).animate({
					'scrollTop': 0
				}, Number( elementScrollSpeed ), elementScrollEasing );
				return false;
			});
		},

		goToTopScroll: function(){
			var elementMobile = $goToTopEl.attr('data-mobile'),
				elementOffset = $goToTopEl.attr('data-offset');

			if( !elementOffset ) { elementOffset = 450; }

			if( elementMobile != 'true' && ( $body.hasClass('device-xs') || $body.hasClass('device-xxs') ) ) { return true; }

			if( $window.scrollTop() > Number(elementOffset) ) {
				$goToTopEl.fadeIn();
			} else {
				$goToTopEl.fadeOut();
			}
		},

		fullScreen: function(){
			if( $fullScreenEl.length > 0 ) {
				$fullScreenEl.each( function(){
					var element = $(this),
						scrHeight = window.innerHeight ? window.innerHeight : $window.height(),
						negativeHeight = element.attr('data-negative-height');

					if( element.attr('id') == 'slider' ) {
						var sliderHeightOff = $slider.offset().top;
						scrHeight = scrHeight - sliderHeightOff;
						if( element.find('.slider-parallax-inner').length > 0 ) {
							var transformVal = element.find('.slider-parallax-inner').css('transform'),
								transformX = transformVal.match(/-?[\d\.]+/g);
							if( !transformX ) { var transformXvalue = 0; } else { var transformXvalue = transformX[5]; }
							scrHeight = ( ( window.innerHeight ? window.innerHeight : $window.height() ) + Number( transformXvalue ) ) - sliderHeightOff;
						}
						if( $('#slider.with-header').next('#header:not(.transparent-header)').length > 0 && ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) ) {
							var headerHeightOff = $header.outerHeight();
							scrHeight = scrHeight - headerHeightOff;
						}
					}
					if( element.parents('.full-screen').length > 0 ) { scrHeight = element.parents('.full-screen').height(); }

					if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') ) {
						if( !element.hasClass('force-full-screen') ){ scrHeight = 'auto'; }
					}

					if( negativeHeight ){ scrHeight = scrHeight - Number(negativeHeight); }

					element.css('height', scrHeight);
					if( element.attr('id') == 'slider' && !element.hasClass('canvas-slider-grid') ) { if( element.has('.swiper-slide') ) { element.find('.swiper-slide').css('height', scrHeight); } }
				});
			}
		},

		maxHeight: function(){
			if( $commonHeightEl.length > 0 ) {
				if( $commonHeightEl.hasClass('customjs') ) { return true; }
				$commonHeightEl.each( function(){
					var element = $(this);
					if( element.find('.common-height').length > 0 ) {
						SEMICOLON.initialize.commonHeight( element.find('.common-height:not(.customjs)') );
					}

					SEMICOLON.initialize.commonHeight( element );
				});
			}
		},

		commonHeight: function( element ){
			var maxHeight = 0;
			element.children('[class*=col-]').each(function() {
				var elementChild = $(this).children();
				if( elementChild.hasClass('max-height') ){
					maxHeight = elementChild.outerHeight();
				} else {
					if (elementChild.outerHeight() > maxHeight)
					maxHeight = elementChild.outerHeight();
				}
			});

			element.children('[class*=col-]').each(function() {
				$(this).height(maxHeight);
			});
		},

		testimonialsGrid: function(){
			if( $testimonialsGridEl.length > 0 ) {
				if( $body.hasClass('device-sm') || $body.hasClass('device-md') || $body.hasClass('device-lg') ) {
					var maxHeight = 0;
					$testimonialsGridEl.each( function(){
						$(this).find("li > .testimonial").each(function(){
						   if ($(this).height() > maxHeight) { maxHeight = $(this).height(); }
						});
						$(this).find("li").height(maxHeight);
						maxHeight = 0;
					});
				} else {
					$testimonialsGridEl.find("li").css({ 'height': 'auto' });
				}
			}
		},

		lightbox: function(){

			if( !$().magnificPopup ) {
				console.log('lightbox: Magnific Popup not Defined.');
				return true;
			}

			var $lightboxImageEl = $('[data-lightbox="image"]'),
				$lightboxGalleryEl = $('[data-lightbox="gallery"]'),
				$lightboxIframeEl = $('[data-lightbox="iframe"]'),
				$lightboxInlineEl = $('[data-lightbox="inline"]'),
				$lightboxAjaxEl = $('[data-lightbox="ajax"]'),
				$lightboxAjaxGalleryEl = $('[data-lightbox="ajax-gallery"]');

			if( $lightboxImageEl.length > 0 ) {
				$lightboxImageEl.magnificPopup({
					type: 'image',
					closeOnContentClick: true,
					closeBtnInside: false,
					fixedContentPos: true,
					mainClass: 'mfp-no-margins mfp-fade', // class to remove default margin from left and right side
					image: {
						verticalFit: true
					}
				});
			}

			if( $lightboxGalleryEl.length > 0 ) {
				$lightboxGalleryEl.each(function() {
					var element = $(this);

					if( element.find('a[data-lightbox="gallery-item"]').parent('.clone').hasClass('clone') ) {
						element.find('a[data-lightbox="gallery-item"]').parent('.clone').find('a[data-lightbox="gallery-item"]').attr('data-lightbox','');
					}

					if( element.find('a[data-lightbox="gallery-item"]').parents('.cloned').hasClass('cloned') ) {
						element.find('a[data-lightbox="gallery-item"]').parents('.cloned').find('a[data-lightbox="gallery-item"]').attr('data-lightbox','');
					}

					element.magnificPopup({
						delegate: 'a[data-lightbox="gallery-item"]',
						type: 'image',
						closeOnContentClick: true,
						closeBtnInside: false,
						fixedContentPos: true,
						mainClass: 'mfp-no-margins mfp-fade', // class to remove default margin from left and right side
						image: {
							verticalFit: true
						},
						gallery: {
							enabled: true,
							navigateByImgClick: true,
							preload: [0,1] // Will preload 0 - before current, and 1 after the current image
						}
					});
				});
			}

			if( $lightboxIframeEl.length > 0 ) {
				$lightboxIframeEl.magnificPopup({
					disableOn: 600,
					type: 'iframe',
					removalDelay: 160,
					preloader: false,
					fixedContentPos: false
				});
			}

			if( $lightboxInlineEl.length > 0 ) {
				$lightboxInlineEl.magnificPopup({
					type: 'inline',
					mainClass: 'mfp-no-margins mfp-fade',
					closeBtnInside: false,
					fixedContentPos: true,
					overflowY: 'scroll'
				});
			}

			if( $lightboxAjaxEl.length > 0 ) {
				$lightboxAjaxEl.magnificPopup({
					type: 'ajax',
					closeBtnInside: false,
					callbacks: {
						ajaxContentAdded: function(mfpResponse) {
							SEMICOLON.widget.loadFlexSlider();
							SEMICOLON.initialize.resizeVideos();
							SEMICOLON.widget.masonryThumbs();
						},
						open: function() {
							$body.addClass('ohidden');
						},
						close: function() {
							$body.removeClass('ohidden');
						}
					}
				});
			}

			if( $lightboxAjaxGalleryEl.length > 0 ) {
				$lightboxAjaxGalleryEl.magnificPopup({
					delegate: 'a[data-lightbox="ajax-gallery-item"]',
					type: 'ajax',
					closeBtnInside: false,
					gallery: {
						enabled: true,
						preload: 0,
						navigateByImgClick: false
					},
					callbacks: {
						ajaxContentAdded: function(mfpResponse) {
							SEMICOLON.widget.loadFlexSlider();
							SEMICOLON.initialize.resizeVideos();
							SEMICOLON.widget.masonryThumbs();
						},
						open: function() {
							$body.addClass('ohidden');
						},
						close: function() {
							$body.removeClass('ohidden');
						}
					}
				});
			}
		},

		modal: function(){

			if( !$().magnificPopup ) {
				console.log('modal: Magnific Popup not Defined.');
				return true;
			}

			var $modal = $('.modal-on-load:not(.customjs)');
			if( $modal.length > 0 ) {
				$modal.each( function(){
					var element				= $(this),
						elementTarget		= element.attr('data-target'),
						elementTargetValue	= elementTarget.split('#')[1],
						elementDelay		= element.attr('data-delay'),
						elementTimeout		= element.attr('data-timeout'),
						elementAnimateIn	= element.attr('data-animate-in'),
						elementAnimateOut	= element.attr('data-animate-out');

					if( !element.hasClass('enable-cookie') ) { $.removeCookie( elementTargetValue ); }

					if( element.hasClass('enable-cookie') ) {
						var elementCookie = $.cookie( elementTargetValue );

						if( typeof elementCookie !== 'undefined' && elementCookie == '0' ) {
							return true;
						}
					}

					if( !elementDelay ) {
						elementDelay = 1500;
					} else {
						elementDelay = Number(elementDelay) + 1500;
					}

					var t = setTimeout(function() {
						$.magnificPopup.open({
							items: { src: elementTarget },
							type: 'inline',
							mainClass: 'mfp-no-margins mfp-fade',
							closeBtnInside: true,
							fixedContentPos: true,
							removalDelay: 500,
							callbacks: {
								open: function(){
									if( elementAnimateIn != '' ) {
										$(elementTarget).addClass( elementAnimateIn + ' animated' );
									}
								},
								beforeClose: function(){
									if( elementAnimateOut != '' ) {
										$(elementTarget).removeClass( elementAnimateIn ).addClass( elementAnimateOut );
									}
								},
								afterClose: function() {
									if( elementAnimateIn != '' || elementAnimateOut != '' ) {
										$(elementTarget).removeClass( elementAnimateIn + ' ' + elementAnimateOut + ' animated' );
									}
									if( element.hasClass('enable-cookie') ) {
										$.cookie( elementTargetValue, '0' );
									}
								}
							}
						}, 0);
					}, Number(elementDelay) );

					if( elementTimeout != '' ) {
						var to = setTimeout(function() {
							$.magnificPopup.close();
						}, Number(elementDelay) + Number(elementTimeout) );
					}
				});
			}
		},

		resizeVideos: function(){

			if( !$().fitVids ) {
				console.log('resizeVideos: FitVids not Defined.');
				return true;
			}

			$("#content,#footer,#slider:not(.revslider-wrap),.landing-offer-media,.portfolio-ajax-modal,.mega-menu-column").fitVids({
				customSelector: "iframe[src^='http://www.dailymotion.com/embed'], iframe[src*='maps.google.com'], iframe[src*='google.com/maps']",
				ignore: '.no-fv'
			});
		},

		imageFade: function(){
			$('.image_fade').hover( function(){
				$(this).filter(':not(:animated)').animate({opacity: 0.8}, 400);
			}, function() {
				$(this).animate({opacity: 1}, 400);
			});
		},

		blogTimelineEntries: function(){
			$('.post-timeline.grid-2').find('.entry').each( function(){
				var position = $(this).inlineStyle('left');
				if( position == '0px' ) {
					$(this).removeClass('alt');
				} else {
					$(this).addClass('alt');
				}
				$(this).find('.entry-timeline').fadeIn();
			});

			$('.entry.entry-date-section').next().next().find('.entry-timeline').css({ 'top': '70px' });
		},

		pageTransition: function(){
			if( $body.hasClass('no-transition') ) { return true; }

			if( !$().animsition ) {
				$body.addClass('no-transition');
				console.log('pageTransition: Animsition not Defined.');
				return true;
			}

			window.onpageshow = function(event) {
				if(event.persisted) {
					window.location.reload();
				}
			};

			var animationIn = $body.attr('data-animation-in'),
				animationOut = $body.attr('data-animation-out'),
				durationIn = $body.attr('data-speed-in'),
				durationOut = $body.attr('data-speed-out'),
				loaderTimeOut = $body.attr('data-loader-timeout'),
				loaderStyle = $body.attr('data-loader'),
				loaderColor = $body.attr('data-loader-color'),
				loaderStyleHtml = $body.attr('data-loader-html'),
				loaderBgStyle = '',
				loaderBorderStyle = '',
				loaderBgClass = '',
				loaderBorderClass = '',
				loaderBgClass2 = '',
				loaderBorderClass2 = '';

			if( !animationIn ) { animationIn = 'fadeIn'; }
			if( !animationOut ) { animationOut = 'fadeOut'; }
			if( !durationIn ) { durationIn = 1500; }
			if( !durationOut ) { durationOut = 800; }
			if( !loaderStyleHtml ) { loaderStyleHtml = '<div class="css3-spinner-bounce1"></div><div class="css3-spinner-bounce2"></div><div class="css3-spinner-bounce3"></div>'; }

			if( !loaderTimeOut ) {
				loaderTimeOut = false;
			} else {
				loaderTimeOut = Number(loaderTimeOut);
			}

			if( loaderColor ) {
				if( loaderColor == 'theme' ) {
					loaderBgClass = ' bgcolor';
					loaderBorderClass = ' border-color';
					loaderBgClass2 = ' class="bgcolor"';
					loaderBorderClass2 = ' class="border-color"';
				} else {
					loaderBgStyle = ' style="background-color:'+ loaderColor +';"';
					loaderBorderStyle = ' style="border-color:'+ loaderColor +';"';
				}
				loaderStyleHtml = '<div class="css3-spinner-bounce1'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-bounce2'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-bounce3'+ loaderBgClass +'"'+ loaderBgStyle +'></div>'
			}

			if( loaderStyle == '2' ) {
				loaderStyleHtml = '<div class="css3-spinner-flipper'+ loaderBgClass +'"'+ loaderBgStyle +'></div>';
			} else if( loaderStyle == '3' ) {
				loaderStyleHtml = '<div class="css3-spinner-double-bounce1'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-double-bounce2'+ loaderBgClass +'"'+ loaderBgStyle +'></div>';
			} else if( loaderStyle == '4' ) {
				loaderStyleHtml = '<div class="css3-spinner-rect1'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-rect2'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-rect3'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-rect4'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-rect5'+ loaderBgClass +'"'+ loaderBgStyle +'></div>';
			} else if( loaderStyle == '5' ) {
				loaderStyleHtml = '<div class="css3-spinner-cube1'+ loaderBgClass +'"'+ loaderBgStyle +'></div><div class="css3-spinner-cube2'+ loaderBgClass +'"'+ loaderBgStyle +'></div>';
			} else if( loaderStyle == '6' ) {
				loaderStyleHtml = '<div class="css3-spinner-scaler'+ loaderBgClass +'"'+ loaderBgStyle +'></div>';
			} else if( loaderStyle == '7' ) {
				loaderStyleHtml = '<div class="css3-spinner-grid-pulse"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '8' ) {
				loaderStyleHtml = '<div class="css3-spinner-clip-rotate"><div'+ loaderBorderClass2 + loaderBorderStyle +'></div></div>';
			} else if( loaderStyle == '9' ) {
				loaderStyleHtml = '<div class="css3-spinner-ball-rotate"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '10' ) {
				loaderStyleHtml = '<div class="css3-spinner-zig-zag"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '11' ) {
				loaderStyleHtml = '<div class="css3-spinner-triangle-path"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '12' ) {
				loaderStyleHtml = '<div class="css3-spinner-ball-scale-multiple"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '13' ) {
				loaderStyleHtml = '<div class="css3-spinner-ball-pulse-sync"><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div><div'+ loaderBgClass2 + loaderBgStyle +'></div></div>';
			} else if( loaderStyle == '14' ) {
				loaderStyleHtml = '<div class="css3-spinner-scale-ripple"><div'+ loaderBorderClass2 + loaderBorderStyle +'></div><div'+ loaderBorderClass2 + loaderBorderStyle +'></div><div'+ loaderBorderClass2 + loaderBorderStyle +'></div></div>';
			}

			$wrapper.animsition({
				inClass : animationIn,
				outClass : animationOut,
				inDuration : Number(durationIn),
				outDuration : Number(durationOut),
				linkElement : '#primary-menu ul li a:not([target="_blank"]):not([href*="#"]):not([data-lightbox]):not([href^="mailto"]):not([href^="tel"]):not([href^="sms"]):not([href^="call"])',
				loading : true,
				loadingParentElement : 'body',
				loadingClass : 'css3-spinner',
				loadingHtml : loaderStyleHtml,
				unSupportCss : [
								 'animation-duration',
								 '-webkit-animation-duration',
								 '-o-animation-duration'
							   ],
				overlay : false,
				overlayClass : 'animsition-overlay-slide',
				overlayParentElement : 'body',
				timeOut: loaderTimeOut
			});
		},

		lazyLoad: function() {
			var lazyLoadEl = $('[data-lazyload]');

			if( !$().appear ) {
				console.log('lazyLoad: Appear not Defined.');
				return true;
			}

			if( lazyLoadEl.length > 0 ) {
				lazyLoadEl.each( function(){
					var element = $(this),
						elementImg = element.attr( 'data-lazyload' );

					element.attr( 'src', 'images/blank.svg' ).css({ 'background': 'url(images/preloader.gif) no-repeat center center #FFF' });

					element.appear(function () {
						element.css({ 'background': 'none' }).removeAttr( 'width' ).removeAttr( 'height' ).attr('src', elementImg);
					},{accX: 0, accY: 120},'easeInCubic');
				});
			}
		},

		topScrollOffset: function() {
			var topOffsetScroll = 0;

			if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !SEMICOLON.isMobile.any() ) {
				if( $header.hasClass('sticky-header') ) {
					if( $pagemenu.hasClass('dots-menu') ) { topOffsetScroll = 100; } else { topOffsetScroll = 144; }
				} else {
					if( $pagemenu.hasClass('dots-menu') ) { topOffsetScroll = 140; } else { topOffsetScroll = 184; }
				}

				if( !$pagemenu.length ) {
					if( $header.hasClass('sticky-header') ) { topOffsetScroll = 100; } else { topOffsetScroll = 140; }
				}
			} else {
				topOffsetScroll = 40;
			}

			return topOffsetScroll;
		},

		defineColumns: function( element ){
			var column = 4;

			if( element.hasClass('portfolio-full') ) {
				if( element.hasClass('portfolio-3') ) column = 3;
				else if( element.hasClass('portfolio-5') ) column = 5;
				else if( element.hasClass('portfolio-6') ) column = 6;
				else column = 4;

				if( $body.hasClass('device-sm') && ( column == 4 || column == 5 || column == 6 ) ) {
					column = 3;
				} else if( $body.hasClass('device-xs') && ( column == 3 || column == 4 || column == 5 || column == 6 ) ) {
					column = 2;
				} else if( $body.hasClass('device-xxs') ) {
					column = 1;
				}
			} else if( element.hasClass('masonry-thumbs') ) {

				var lgCol = element.attr('data-lg-col'),
					mdCol = element.attr('data-md-col'),
					smCol = element.attr('data-sm-col'),
					xsCol = element.attr('data-xs-col'),
					xxsCol = element.attr('data-xxs-col');

				if( element.hasClass('col-2') ) column = 2;
				else if( element.hasClass('col-3') ) column = 3;
				else if( element.hasClass('col-5') ) column = 5;
				else if( element.hasClass('col-6') ) column = 6;
				else column = 4;

				if( $body.hasClass('device-lg') ) {
					if( lgCol ) { column = Number(lgCol); }
				} else if( $body.hasClass('device-md') ) {
					if( mdCol ) { column = Number(mdCol); }
				} else if( $body.hasClass('device-sm') ) {
					if( smCol ) { column = Number(smCol); }
				} else if( $body.hasClass('device-xs') ) {
					if( xsCol ) { column = Number(xsCol); }
				} else if( $body.hasClass('device-xxs') ) {
					if( xxsCol ) { column = Number(xxsCol); }
				}

			}

			return column;
		},

		setFullColumnWidth: function( element ){

			if( !$().isotope ) {
				console.log('setFullColumnWidth: Isotope not Defined.');
				return true;
			}

			element.css({ 'width': '' });

			if( element.hasClass('portfolio-full') ) {
				var columns = SEMICOLON.initialize.defineColumns( element );
				var containerWidth = element.width();
				if( containerWidth == ( Math.floor(containerWidth/columns) * columns ) ) { containerWidth = containerWidth - 1; }
				var postWidth = Math.floor(containerWidth/columns);
				if( $body.hasClass('device-xxs') ) { var deviceSmallest = 1; } else { var deviceSmallest = 0; }
				element.find(".portfolio-item").each(function(index){
					if( deviceSmallest == 0 && $(this).hasClass('wide') ) { var elementSize = ( postWidth*2 ); } else { var elementSize = postWidth; }
					$(this).css({"width":elementSize+"px"});
				});
			} else if( element.hasClass('masonry-thumbs') ) {
				var columns = SEMICOLON.initialize.defineColumns( element ),
					containerWidth = element.innerWidth();

				if( containerWidth == windowWidth ){
					containerWidth = windowWidth*1.004;
					element.css({ 'width': containerWidth+'px' });
				}

				var postWidth = (containerWidth/columns);

				postWidth = Math.floor(postWidth);

				if( ( postWidth * columns ) >= containerWidth ) { element.css({ 'margin-right': '-1px' }); }

				element.children('a').css({"width":postWidth+"px"});

				var firstElementWidth = element.find('a:eq(0)').outerWidth();

				element.isotope({
					masonry: {
						columnWidth: firstElementWidth
					}
				});

				var bigImageNumbers = element.attr('data-big');
				if( bigImageNumbers ) {
					bigImageNumbers = bigImageNumbers.split(",");
					var bigImageNumber = '',
						bigi = '';
					for( bigi = 0; bigi < bigImageNumbers.length; bigi++ ){
						bigImageNumber = Number(bigImageNumbers[bigi]) - 1;
						element.find('a:eq('+bigImageNumber+')').css({ width: firstElementWidth*2 + 'px' });
					}
					var t = setTimeout( function(){
						element.isotope('layout');
					}, 1000 );
				}
			}

		},

		aspectResizer: function(){
			var $aspectResizerEl = $('.aspect-resizer');
			if( $aspectResizerEl.length > 0 ) {
				$aspectResizerEl.each( function(){
					var element = $(this),
						elementW = element.inlineStyle('width'),
						elementH = element.inlineStyle('height'),
						elementPW = element.parent().innerWidth();
				});
			}
		},

		dataResponsiveClasses: function(){
			var $dataClassXxs = $('[data-class-xxs]'),
				$dataClassXs = $('[data-class-xs]'),
				$dataClassSm = $('[data-class-sm]'),
				$dataClassMd = $('[data-class-md]'),
				$dataClassLg = $('[data-class-lg]');

			if( $dataClassXxs.length > 0 ) {
				$dataClassXxs.each( function(){
					var element = $(this),
						elementClass = element.attr('data-class-xxs'),
						elementClassDelete = element.attr('data-class-xs') + ' ' + element.attr('data-class-sm') + ' ' + element.attr('data-class-md') + ' ' + element.attr('data-class-lg');

					if( $body.hasClass('device-xxs') ) {
						element.removeClass( elementClassDelete );
						element.addClass( elementClass );
					}
				});
			}

			if( $dataClassXs.length > 0 ) {
				$dataClassXs.each( function(){
					var element = $(this),
						elementClass = element.attr('data-class-xs'),
						elementClassDelete = element.attr('data-class-xxs') + ' ' + element.attr('data-class-sm') + ' ' + element.attr('data-class-md') + ' ' + element.attr('data-class-lg');

					if( $body.hasClass('device-xs') ) {
						element.removeClass( elementClassDelete );
						element.addClass( elementClass );
					}
				});
			}

			if( $dataClassSm.length > 0 ) {
				$dataClassSm.each( function(){
					var element = $(this),
						elementClass = element.attr('data-class-sm'),
						elementClassDelete = element.attr('data-class-xxs') + ' ' + element.attr('data-class-xs') + ' ' + element.attr('data-class-md') + ' ' + element.attr('data-class-lg');

					if( $body.hasClass('device-sm') ) {
						element.removeClass( elementClassDelete );
						element.addClass( elementClass );
					}
				});
			}

			if( $dataClassMd.length > 0 ) {
				$dataClassMd.each( function(){
					var element = $(this),
						elementClass = element.attr('data-class-md'),
						elementClassDelete = element.attr('data-class-xxs') + ' ' + element.attr('data-class-xs') + ' ' + element.attr('data-class-sm') + ' ' + element.attr('data-class-lg');

					if( $body.hasClass('device-md') ) {
						element.removeClass( elementClassDelete );
						element.addClass( elementClass );
					}
				});
			}

			if( $dataClassLg.length > 0 ) {
				$dataClassLg.each( function(){
					var element = $(this),
						elementClass = element.attr('data-class-lg'),
						elementClassDelete = element.attr('data-class-xxs') + ' ' + element.attr('data-class-xs') + ' ' + element.attr('data-class-sm') + ' ' + element.attr('data-class-md');

					if( $body.hasClass('device-lg') ) {
						element.removeClass( elementClassDelete );
						element.addClass( elementClass );
					}
				});
			}
		},

		dataResponsiveHeights: function(){
			var $dataHeightXxs = $('[data-height-xxs]'),
				$dataHeightXs = $('[data-height-xs]'),
				$dataHeightSm = $('[data-height-sm]'),
				$dataHeightMd = $('[data-height-md]'),
				$dataHeightLg = $('[data-height-lg]');

			if( $dataHeightXxs.length > 0 ) {
				$dataHeightXxs.each( function(){
					var element = $(this),
						elementHeight = element.attr('data-height-xxs');

					if( $body.hasClass('device-xxs') ) {
						if( elementHeight != '' ) { element.css( 'height', elementHeight ); }
					}
				});
			}

			if( $dataHeightXs.length > 0 ) {
				$dataHeightXs.each( function(){
					var element = $(this),
						elementHeight = element.attr('data-height-xs');

					if( $body.hasClass('device-xs') ) {
						if( elementHeight != '' ) { element.css( 'height', elementHeight ); }
					}
				});
			}

			if( $dataHeightSm.length > 0 ) {
				$dataHeightSm.each( function(){
					var element = $(this),
						elementHeight = element.attr('data-height-sm');

					if( $body.hasClass('device-sm') ) {
						if( elementHeight != '' ) { element.css( 'height', elementHeight ); }
					}
				});
			}

			if( $dataHeightMd.length > 0 ) {
				$dataHeightMd.each( function(){
					var element = $(this),
						elementHeight = element.attr('data-height-md');

					if( $body.hasClass('device-md') ) {
						if( elementHeight != '' ) { element.css( 'height', elementHeight ); }
					}
				});
			}

			if( $dataHeightLg.length > 0 ) {
				$dataHeightLg.each( function(){
					var element = $(this),
						elementHeight = element.attr('data-height-lg');

					if( $body.hasClass('device-lg') ) {
						if( elementHeight != '' ) { element.css( 'height', elementHeight ); }
					}
				});
			}
		},

		stickFooterOnSmall: function(){
			var windowH = $window.height(),
				wrapperH = $wrapper.height();

			if( !$body.hasClass('sticky-footer') && $footer.length > 0 && $wrapper.has('#footer') ) {
				if( windowH > wrapperH ) {
					$footer.css({ 'margin-top': ( windowH - wrapperH ) });
				}
			}
		},

		stickyFooter: function(){
			if( $body.hasClass('sticky-footer') && $footer.length > 0 && ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) ) {
				var stickyFooter = $footer.outerHeight();
				$content.css({ 'margin-bottom': stickyFooter });
			} else {
				$content.css({ 'margin-bottom': 0 });
			}
		}

	};

	SEMICOLON.header = {

		init: function(){

			SEMICOLON.header.superfish();
			SEMICOLON.header.menufunctions();
			SEMICOLON.header.fullWidthMenu();
			SEMICOLON.header.overlayMenu();
			SEMICOLON.header.stickyMenu();
			SEMICOLON.header.stickyPageMenu();
			SEMICOLON.header.sideHeader();
			SEMICOLON.header.sidePanel();
			SEMICOLON.header.onePageScroll();
			SEMICOLON.header.onepageScroller();
			SEMICOLON.header.logo();
			SEMICOLON.header.topsearch();
			SEMICOLON.header.topcart();

		},

		superfish: function(){

			if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
				$('#primary-menu ul ul, #primary-menu ul .mega-menu-content').css('display', 'block');
				SEMICOLON.header.menuInvert();
				$('#primary-menu ul ul, #primary-menu ul .mega-menu-content').css('display', '');
			}

			if( !$().superfish ) {
				$body.addClass('no-superfish');
				console.log('superfish: Superfish not Defined.');
				return true;
			}

			$('body:not(.side-header) #primary-menu > ul, body:not(.side-header) #primary-menu > div > ul:not(.dropdown-menu), .top-links > ul').superfish({
				popUpSelector: 'ul,.mega-menu-content,.top-link-section',
				delay: 250,
				speed: 350,
				animation: {opacity:'show'},
				animationOut:  {opacity:'hide'},
				cssArrows: false,
				onShow: function(){
					var megaMenuContent = $(this);
					if( megaMenuContent.find('.owl-carousel.customjs').length > 0 ) {
						megaMenuContent.find('.owl-carousel').removeClass('customjs');
						SEMICOLON.widget.carousel();
					}

					if( megaMenuContent.hasClass('mega-menu-content') && megaMenuContent.find('.widget').length > 0 ) {
						if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
							setTimeout( function(){ SEMICOLON.initialize.commonHeight( megaMenuContent ); }, 200);
						} else {
							megaMenuContent.children().height('');
						}
					}
				}
			});

			$('body.side-header #primary-menu > ul').superfish({
				popUpSelector: 'ul',
				delay: 250,
				speed: 350,
				animation: {opacity:'show',height:'show'},
				animationOut:  {opacity:'hide',height:'hide'},
				cssArrows: false
			});

		},

		menuInvert: function() {

			$('#primary-menu .mega-menu-content, #primary-menu ul ul').each( function( index, element ){
				var $menuChildElement = $(element),
					menuChildOffset = $menuChildElement.offset(),
					menuChildWidth = $menuChildElement.width(),
					menuChildLeft = menuChildOffset.left;

				if(windowWidth - (menuChildWidth + menuChildLeft) < 0) {
					$menuChildElement.addClass('menu-pos-invert');
				}
			});

		},

		menufunctions: function(){

			$( '#primary-menu ul li:has(ul)' ).addClass('sub-menu');
			$( '.top-links ul li:has(ul) > a, #primary-menu.with-arrows > ul > li:has(ul) > a > div, #primary-menu.with-arrows > div > ul > li:has(ul) > a > div, #page-menu nav ul li:has(ul) > a > div' ).append( '<i class="icon-angle-down"></i>' );
			$( '.top-links > ul' ).addClass( 'clearfix' );

			if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
				$('#primary-menu.sub-title > ul > li').hover(function() {
					$(this).prev().css({ backgroundImage : 'none' });
				}, function() {
					$(this).prev().css({ backgroundImage : 'url("images/icons/menu-divider.png")' });
				});

				$('#primary-menu.sub-title').children('ul').children('.current').prev().css({ backgroundImage : 'none' });
			}

			// var responsiveThreshold = $header.attr('data-responsive-under');
			// if( !responsiveThreshold ) { responsiveThreshold = 992; }

			// if( windowWidth < Number( responsiveThreshold ) ) {
			// 	$body.addClass('mobile-header-active');
			// } else {
			// 	$body.removeClass('mobile-header-active');
			// }

			if( SEMICOLON.isMobile.Android() ) {
				$( '#primary-menu ul li.sub-menu' ).children('a').on('touchstart', function(e){
					if( !$(this).parent('li.sub-menu').hasClass('sfHover') ) {
						e.preventDefault();
					}
				});
			}

			if( SEMICOLON.isMobile.Windows() ) {
				if( $().superfish ){
					$('#primary-menu > ul, #primary-menu > div > ul,.top-links > ul').superfish('destroy').addClass('windows-mobile-menu');
				} else {
					$('#primary-menu > ul, #primary-menu > div > ul,.top-links > ul').addClass('windows-mobile-menu');
					console.log('menufunctions: Superfish not defined.');
				}

				$( '#primary-menu ul li:has(ul)' ).append('<a href="#" class="wn-submenu-trigger"><i class="icon-angle-down"></i></a>');

				$( '#primary-menu ul li.sub-menu' ).children('a.wn-submenu-trigger').click( function(e){
					$(this).parent().toggleClass('open');
					$(this).parent().find('> ul, > .mega-menu-content').stop(true,true).toggle();
					return false;
				});
			}

		},

		fullWidthMenu: function(){
			if( $body.hasClass('stretched') ) {
				if( $header.find('.container-fullwidth').length > 0 ) { $('.mega-menu .mega-menu-content').css({ 'width': $wrapper.width() - 120 }); }
				if( $header.hasClass('full-header') ) { $('.mega-menu .mega-menu-content').css({ 'width': $wrapper.width() - 60 }); }
			} else {
				if( $header.find('.container-fullwidth').length > 0 ) { $('.mega-menu .mega-menu-content').css({ 'width': $wrapper.width() - 120 }); }
				if( $header.hasClass('full-header') ) { $('.mega-menu .mega-menu-content').css({ 'width': $wrapper.width() - 80 }); }
			}
		},

		overlayMenu: function(){
			if( $body.hasClass('overlay-menu') ) {
				var overlayMenuItem = $('#primary-menu').children('ul').children('li'),
					overlayMenuItemHeight = overlayMenuItem.outerHeight(),
					overlayMenuItemTHeight = overlayMenuItem.length * overlayMenuItemHeight,
					firstItemOffset = ( $window.height() - overlayMenuItemTHeight ) / 2;

				$('#primary-menu').children('ul').children('li:first-child').css({ 'margin-top': firstItemOffset+'px' });
			}
		},

		stickyMenu: function( headerOffset ){
			if ($window.scrollTop() > headerOffset) {
				if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
					$('body:not(.side-header) #header:not(.no-sticky)').addClass('sticky-header');
					if( !$headerWrap.hasClass('force-not-dark') ) { $headerWrap.removeClass('not-dark'); }
					SEMICOLON.header.stickyMenuClass();
				} else if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ) {
					if( $body.hasClass('sticky-responsive-menu') ) {
						$('#header:not(.no-sticky)').addClass('responsive-sticky-header');
						SEMICOLON.header.stickyMenuClass();
					}
				}
			} else {
				SEMICOLON.header.removeStickyness();
			}
		},

		stickyPageMenu: function( pageMenuOffset ){
			if ($window.scrollTop() > pageMenuOffset) {
				if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
					$('#page-menu:not(.dots-menu,.no-sticky)').addClass('sticky-page-menu');
				} else if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ) {
					if( $body.hasClass('sticky-responsive-pagemenu') ) {
						$('#page-menu:not(.dots-menu,.no-sticky)').addClass('sticky-page-menu');
					}
				}
			} else {
				$('#page-menu:not(.dots-menu,.no-sticky)').removeClass('sticky-page-menu');
			}
		},

		removeStickyness: function(){
			if( $header.hasClass('sticky-header') ){
				$('body:not(.side-header) #header:not(.no-sticky)').removeClass('sticky-header');
				$header.removeClass().addClass(oldHeaderClasses);
				$headerWrap.removeClass().addClass(oldHeaderWrapClasses);
				if( !$headerWrap.hasClass('force-not-dark') ) { $headerWrap.removeClass('not-dark'); }
				SEMICOLON.slider.swiperSliderMenu();
				SEMICOLON.slider.revolutionSliderMenu();
			}
			if( $header.hasClass('responsive-sticky-header') ){
				$('body.sticky-responsive-menu #header').removeClass('responsive-sticky-header');
			}
			if( ( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ) && ( typeof responsiveMenuClasses === 'undefined' ) ) {
				$header.removeClass().addClass(oldHeaderClasses);
				$headerWrap.removeClass().addClass(oldHeaderWrapClasses);
				if( !$headerWrap.hasClass('force-not-dark') ) { $headerWrap.removeClass('not-dark'); }
			}
		},

		sideHeader: function(){
			$("#header-trigger").click(function(){
				$('body.open-header').toggleClass("side-header-open");
				return false;
			});
		},

		sidePanel: function(){
			$(".side-panel-trigger").click(function(){
				$body.toggleClass("side-panel-open");
				if( $body.hasClass('device-touch') && $body.hasClass('side-push-panel') ) {
					$body.toggleClass("ohidden");
				}
				return false;
			});
		},

		onePageScroll: function(){
			if( $onePageMenuEl.length > 0 ){
				var onePageSpeed = $onePageMenuEl.attr('data-speed'),
					onePageOffset = $onePageMenuEl.attr('data-offset'),
					onePageEasing = $onePageMenuEl.attr('data-easing');

				if( !onePageSpeed ) { onePageSpeed = 1000; }
				if( !onePageEasing ) { onePageEasing = 'easeOutQuad'; }

				$onePageMenuEl.find('a[data-href]').click(function(){
					var element = $(this),
						divScrollToAnchor = element.attr('data-href'),
						divScrollSpeed = element.attr('data-speed'),
						divScrollOffset = element.attr('data-offset'),
						divScrollEasing = element.attr('data-easing');

					if( $( divScrollToAnchor ).length > 0 ) {

						if( !onePageOffset ) {
							var onePageOffsetG = SEMICOLON.initialize.topScrollOffset();
						} else {
							var onePageOffsetG = onePageOffset;
						}

						if( !divScrollSpeed ) { divScrollSpeed = onePageSpeed; }
						if( !divScrollOffset ) { divScrollOffset = onePageOffsetG; }
						if( !divScrollEasing ) { divScrollEasing = onePageEasing; }

						if( $onePageMenuEl.hasClass('no-offset') ) { divScrollOffset = 0; }

						onePageGlobalOffset = Number(divScrollOffset);

						$onePageMenuEl.find('li').removeClass('current');
						$onePageMenuEl.find('a[data-href="' + divScrollToAnchor + '"]').parent('li').addClass('current');

						if( windowWidth < 768 || $body.hasClass('overlay-menu') ) {
							if( $('#primary-menu').find('ul.mobile-primary-menu').length > 0 ) {
								$('#primary-menu > ul.mobile-primary-menu, #primary-menu > div > ul.mobile-primary-menu').toggleClass('show', false);
							} else {
								$('#primary-menu > ul, #primary-menu > div > ul').toggleClass('show', false);
							}
							$pagemenu.toggleClass('pagemenu-active', false);
							$body.toggleClass('primary-menu-open', false);
						}

						$('html,body').stop(true).animate({
							'scrollTop': $( divScrollToAnchor ).offset().top - Number(divScrollOffset)
						}, Number(divScrollSpeed), divScrollEasing);

						onePageGlobalOffset = Number(divScrollOffset);
					}

					return false;
				});
			}
		},

		onepageScroller: function(){
			$onePageMenuEl.find('li').removeClass('current');
			$onePageMenuEl.find('a[data-href="#' + SEMICOLON.header.onePageCurrentSection() + '"]').parent('li').addClass('current');
		},

		onePageCurrentSection: function(){
			var currentOnePageSection = 'home',
				headerHeight = $headerWrap.outerHeight();

			if( $body.hasClass('side-header') ) { headerHeight = 0; }

			$pageSectionEl.each(function(index) {
				var h = $(this).offset().top;
				var y = $window.scrollTop();

				var offsetScroll = headerHeight + onePageGlobalOffset;

				if( y + offsetScroll >= h && y < h + $(this).height() && $(this).attr('id') != currentOnePageSection ) {
					currentOnePageSection = $(this).attr('id');
				}
			});

			return currentOnePageSection;
		},

		logo: function(){
			if( ( $header.hasClass('dark') || $body.hasClass('dark') ) && !$headerWrap.hasClass('not-dark') ) {
				if( defaultDarkLogo ){ defaultLogo.find('img').attr('src', defaultDarkLogo); }
				if( retinaDarkLogo ){ retinaLogo.find('img').attr('src', retinaDarkLogo); }
			} else {
				if( defaultLogoImg ){ defaultLogo.find('img').attr('src', defaultLogoImg); }
				if( retinaLogoImg ){ retinaLogo.find('img').attr('src', retinaLogoImg); }
			}
			if( $header.hasClass('sticky-header') ) {
				if( defaultStickyLogo ){ defaultLogo.find('img').attr('src', defaultStickyLogo); }
				if( retinaStickyLogo ){ retinaLogo.find('img').attr('src', retinaStickyLogo); }
			}
			if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') ) {
				if( defaultMobileLogo ){ defaultLogo.find('img').attr('src', defaultMobileLogo); }
				if( retinaMobileLogo ){ retinaLogo.find('img').attr('src', retinaMobileLogo); }
			}
		},

		stickyMenuClass: function(){
			if( stickyMenuClasses ) { var newClassesArray = stickyMenuClasses.split(/ +/); } else { var newClassesArray = ''; }
			var noOfNewClasses = newClassesArray.length;

			if( noOfNewClasses > 0 ) {
				var i = 0;
				for( i=0; i<noOfNewClasses; i++ ) {
					if( newClassesArray[i] == 'not-dark' ) {
						$header.removeClass('dark');
						$headerWrap.addClass('not-dark');
					} else if( newClassesArray[i] == 'dark' ) {
						$headerWrap.removeClass('not-dark force-not-dark');
						if( !$header.hasClass( newClassesArray[i] ) ) {
							$header.addClass( newClassesArray[i] );
						}
					} else if( !$header.hasClass( newClassesArray[i] ) ) {
						$header.addClass( newClassesArray[i] );
					}
				}
			}
		},

		responsiveMenuClass: function(){
			if( $body.hasClass('device-xs') || $body.hasClass('device-xxs') || $body.hasClass('device-sm') ){
				if( responsiveMenuClasses ) { var newClassesArray = responsiveMenuClasses.split(/ +/); } else { var newClassesArray = ''; }
				var noOfNewClasses = newClassesArray.length;

				if( noOfNewClasses > 0 ) {
					var i = 0;
					for( i=0; i<noOfNewClasses; i++ ) {
						if( newClassesArray[i] == 'not-dark' ) {
							$header.removeClass('dark');
							$headerWrap.addClass('not-dark');
						} else if( newClassesArray[i] == 'dark' ) {
							$headerWrap.removeClass('not-dark force-not-dark');
							if( !$header.hasClass( newClassesArray[i] ) ) {
								$header.addClass( newClassesArray[i] );
							}
						} else if( !$header.hasClass( newClassesArray[i] ) ) {
							$header.addClass( newClassesArray[i] );
						}
					}
				}
				SEMICOLON.header.logo();
			}
		},

		topsocial: function(){
			if( $topSocialEl.length > 0 ){
				if( $body.hasClass('device-md') || $body.hasClass('device-lg') ) {
					$topSocialEl.show();
					$topSocialEl.find('a').css({width: 40});

					$topSocialEl.find('.ts-text').each( function(){
						var $clone = $(this).clone().css({'visibility': 'hidden', 'display': 'inline-block', 'font-size': '13px', 'font-weight':'bold'}).appendTo($body),
							cloneWidth = $clone.innerWidth() + 52;
						$(this).parent('a').attr('data-hover-width',cloneWidth);
						$clone.remove();
					});

					$topSocialEl.find('a').hover(function() {
						if( $(this).find('.ts-text').length > 0 ) {
							$(this).css({width: $(this).attr('data-hover-width')});
						}
					}, function() {
						$(this).css({width: 40});
					});
				} else {
					$topSocialEl.show();
					$topSocialEl.find('a').css({width: 40});

					$topSocialEl.find('a').each(function() {
						var topIconTitle = $(this).find('.ts-text').text();
						$(this).attr('title', topIconTitle);
					});

					$topSocialEl.find('a').hover(function() {
						$(this).css({width: 40});
					}, function() {
						$(this).css({width: 40});
					});

					if( $body.hasClass('device-xxs') ) {
						$topSocialEl.hide();
						$topSocialEl.slice(0, 8).show();
					}
				}
			}
		},

		topsearch: function(){

			$(document).on('click', function(event) {
				if (!$(event.target).closest('#top-search').length) { $body.toggleClass('top-search-open', false); }
				if (!$(event.target).closest('#top-cart').length) { $topCart.toggleClass('top-cart-open', false); }
				if (!$(event.target).closest('#page-menu').length) { $pagemenu.toggleClass('pagemenu-active', false); }
				if (!$(event.target).closest('#side-panel').length) { $body.toggleClass('side-panel-open', false); }
				if (!$(event.target).closest('#primary-menu.mobile-menu-off-canvas > ul').length) { $('#primary-menu.mobile-menu-off-canvas > ul').toggleClass('show', false); }
				if (!$(event.target).closest('#primary-menu.mobile-menu-off-canvas > div > ul').length) { $('#primary-menu.mobile-menu-off-canvas > div > ul').toggleClass('show', false); }
			});

			$("#top-search-trigger").click(function(e){
				$body.toggleClass('top-search-open');
				$topCart.toggleClass('top-cart-open', false);
				$( '#primary-menu > ul, #primary-menu > div > ul' ).toggleClass("show", false);
				$pagemenu.toggleClass('pagemenu-active', false);
				if ($body.hasClass('top-search-open')){
					$topSearch.find('input').focus();
				}
				e.stopPropagation();
				e.preventDefault();
			});

		},

		topcart: function(){

			$("#top-cart-trigger").click(function(e){
				$pagemenu.toggleClass('pagemenu-active', false);
				$topCart.toggleClass('top-cart-open');
				e.stopPropagation();
				e.preventDefault();
			});

		}

	};

	SEMICOLON.slider = {

		init: function() {

			SEMICOLON.slider.sliderParallaxDimensions();
			SEMICOLON.slider.sliderRun();
			SEMICOLON.slider.sliderParallax();
			SEMICOLON.slider.sliderElementsFade();
			SEMICOLON.slider.captionPosition();

		},

		sliderParallaxDimensions: function(){
			if( $sliderParallaxEl.find('.slider-parallax-inner').length < 1 ) { return true; }

			if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ) {
				var parallaxElHeight = $sliderParallaxEl.outerHeight(),
					parallaxElWidth = $sliderParallaxEl.outerWidth();

				if( $sliderParallaxEl.hasClass('revslider-wrap') || $sliderParallaxEl.find('.carousel-widget').length > 0 ) {
					parallaxElHeight = $sliderParallaxEl.find('.slider-parallax-inner').children().first().outerHeight();
					$sliderParallaxEl.height( parallaxElHeight );
				}

				$sliderParallaxEl.find('.slider-parallax-inner').height( parallaxElHeight );

				if( $body.hasClass('side-header') ) {
					$sliderParallaxEl.find('.slider-parallax-inner').width( parallaxElWidth );
				}

				if( !$body.hasClass('stretched') ) {
					parallaxElWidth = $wrapper.outerWidth();
					$sliderParallaxEl.find('.slider-parallax-inner').width( parallaxElWidth );
				}
			} else {
				$sliderParallaxEl.find('.slider-parallax-inner').css({ 'width': '', height: '' });
			}

			if( swiperSlider ) { swiperSlider.update( true ); }
		},

		sliderRun: function(){

			if( typeof Swiper === 'undefined' ) {
				console.log('sliderRun: Swiper not Defined.');
				return true;
			}

			if( $slider.hasClass('customjs') ) { return true; }

			if( $slider.hasClass('swiper_wrapper') ) {

				var element = $slider.filter('.swiper_wrapper'),
					elementDirection = element.attr('data-direction'),
					elementSpeed = element.attr('data-speed'),
					elementAutoPlay = element.attr('data-autoplay'),
					elementLoop = element.attr('data-loop'),
					elementEffect = element.attr('data-effect'),
					elementGrabCursor = element.attr('data-grab'),
					slideNumberTotal = element.find('#slide-number-total'),
					slideNumberCurrent = element.find('#slide-number-current'),
					sliderVideoAutoPlay = element.attr('data-video-autoplay');

				if( !elementSpeed ) { elementSpeed = 300; }
				if( !elementDirection ) { elementDirection = 'horizontal'; }
				if( elementAutoPlay ) { elementAutoPlay = Number( elementAutoPlay ); }
				if( elementLoop == 'true' ) { elementLoop = true; } else { elementLoop = false; }
				if( !elementEffect ) { elementEffect = 'slide'; }
				if( elementGrabCursor == 'false' ) { elementGrabCursor = false; } else { elementGrabCursor = true; }
				if( sliderVideoAutoPlay == 'false' ) { sliderVideoAutoPlay = false; } else { sliderVideoAutoPlay = true; }

				if( element.find('.swiper-pagination').length > 0 ) {
					var elementPagination = '.swiper-pagination',
						elementPaginationClickable = true;
				} else {
					var elementPagination = '',
						elementPaginationClickable = false;
				}

				var elementNavNext = '#slider-arrow-right',
					elementNavPrev = '#slider-arrow-left';

				swiperSlider = new Swiper( element.find('.swiper-parent') ,{
					direction: elementDirection,
					speed: Number( elementSpeed ),
					autoplay: elementAutoPlay,
					loop: elementLoop,
					effect: elementEffect,
					slidesPerView: 1,
					grabCursor: elementGrabCursor,
					pagination: elementPagination,
					paginationClickable: elementPaginationClickable,
					prevButton: elementNavPrev,
					nextButton: elementNavNext,
					onInit: function(swiper){
						SEMICOLON.slider.sliderParallaxDimensions();
						element.find('.yt-bg-player').removeClass('customjs');
						SEMICOLON.widget.youtubeBgVideo();
						$('.swiper-slide-active [data-caption-animate]').each(function(){
							var $toAnimateElement = $(this),
								toAnimateDelay = $toAnimateElement.attr('data-caption-delay'),
								toAnimateDelayTime = 0;
							if( toAnimateDelay ) { toAnimateDelayTime = Number( toAnimateDelay ) + 750; } else { toAnimateDelayTime = 750; }
							if( !$toAnimateElement.hasClass('animated') ) {
								$toAnimateElement.addClass('not-animated');
								var elementAnimation = $toAnimateElement.attr('data-caption-animate');
								setTimeout(function() {
									$toAnimateElement.removeClass('not-animated').addClass( elementAnimation + ' animated');
								}, toAnimateDelayTime);
							}
						});
						$('[data-caption-animate]').each(function(){
							var $toAnimateElement = $(this),
								elementAnimation = $toAnimateElement.attr('data-caption-animate');
							if( $toAnimateElement.parents('.swiper-slide').hasClass('swiper-slide-active') ) { return true; }
							$toAnimateElement.removeClass('animated').removeClass(elementAnimation).addClass('not-animated');
						});
						SEMICOLON.slider.swiperSliderMenu();
					},
					onSlideChangeStart: function(swiper){
						if( slideNumberCurrent.length > 0 ){
							if( elementLoop == true ) {
								slideNumberCurrent.html( Number( element.find('.swiper-slide.swiper-slide-active').attr('data-swiper-slide-index') ) + 1 );
							} else {
								slideNumberCurrent.html( swiperSlider.activeIndex + 1 );
							}
						}
						$('[data-caption-animate]').each(function(){
							var $toAnimateElement = $(this),
								elementAnimation = $toAnimateElement.attr('data-caption-animate');
							if( $toAnimateElement.parents('.swiper-slide').hasClass('swiper-slide-active') ) { return true; }
							$toAnimateElement.removeClass('animated').removeClass(elementAnimation).addClass('not-animated');
						});
						SEMICOLON.slider.swiperSliderMenu();
					},
					onSlideChangeEnd: function(swiper){
						element.find('.swiper-slide').each(function(){
							var slideEl = $(this);
							if( slideEl.find('video').length > 0 && sliderVideoAutoPlay == true ) { slideEl.find('video').get(0).pause(); }
							if( slideEl.find('.yt-bg-player.mb_YTPlayer:not(.customjs)').length > 0 ) { slideEl.find('.yt-bg-player.mb_YTPlayer:not(.customjs)').YTPPause(); }
						});
						element.find('.swiper-slide:not(".swiper-slide-active")').each(function(){
							var slideEl = $(this);
							if( slideEl.find('video').length > 0 ) {
								if( slideEl.find('video').get(0).currentTime != 0 ) { slideEl.find('video').get(0).currentTime = 0; }
							}
							if( slideEl.find('.yt-bg-player.mb_YTPlayer:not(.customjs)').length > 0 ) {
								slideEl.find('.yt-bg-player.mb_YTPlayer:not(.customjs)').YTPGetPlayer().seekTo( slideEl.find('.yt-bg-player.mb_YTPlayer:not(.customjs)').attr('data-start') );
							}
						});
						if( element.find('.swiper-slide.swiper-slide-active').find('video').length > 0 && sliderVideoAutoPlay == true ) { element.find('.swiper-slide.swiper-slide-active').find('video').get(0).play(); }
						if( element.find('.swiper-slide.swiper-slide-active').find('.yt-bg-player.mb_YTPlayer:not(.customjs)').length > 0 && sliderVideoAutoPlay == true ) { element.find('.swiper-slide.swiper-slide-active').find('.yt-bg-player.mb_YTPlayer:not(.customjs)').YTPPlay(); }

						element.find('.swiper-slide.swiper-slide-active [data-caption-animate]').each(function(){
							var $toAnimateElement = $(this),
								toAnimateDelay = $toAnimateElement.attr('data-caption-delay'),
								toAnimateDelayTime = 0;
							if( toAnimateDelay ) { toAnimateDelayTime = Number( toAnimateDelay ) + 300; } else { toAnimateDelayTime = 300; }
							if( !$toAnimateElement.hasClass('animated') ) {
								$toAnimateElement.addClass('not-animated');
								var elementAnimation = $toAnimateElement.attr('data-caption-animate');
								setTimeout(function() {
									$toAnimateElement.removeClass('not-animated').addClass( elementAnimation + ' animated');
								}, toAnimateDelayTime);
							}
						});
					}
				});

				if( slideNumberCurrent.length > 0 ) {
					if( elementLoop == true ) {
						slideNumberCurrent.html( Number( element.find('.swiper-slide.swiper-slide-active').attr('data-swiper-slide-index') ) + 1 );
					} else {
						slideNumberCurrent.html( swiperSlider.activeIndex + 1 );
					}
				}
				if( slideNumberTotal.length > 0 ) {
					slideNumberTotal.html( element.find('.swiper-slide:not(.swiper-slide-duplicate)').length );
				}

			}
		},

		sliderParallaxOffset: function(){
			var sliderParallaxOffsetTop = 0;
			var headerHeight = $header.outerHeight();
			if( $body.hasClass('side-header') || $header.hasClass('transparent-header') ) { headerHeight = 0; }
			if( $pageTitle.length > 0 ) {
				var pageTitleHeight = $pageTitle.outerHeight();
				sliderParallaxOffsetTop = pageTitleHeight + headerHeight;
			} else {
				sliderParallaxOffsetTop = headerHeight;
			}

			if( $slider.next('#header').length > 0 ) { sliderParallaxOffsetTop = 0; }

			return sliderParallaxOffsetTop;
		},

		sliderParallax: function(){

			if( $sliderParallaxEl.length < 1 ) { return true; }

			var parallaxOffsetTop = SEMICOLON.slider.sliderParallaxOffset(),
				parallaxElHeight = $sliderParallaxEl.outerHeight();

			if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !SEMICOLON.isMobile.any() ) {
				if( ( parallaxElHeight + parallaxOffsetTop + 50 ) > $window.scrollTop() ){
					$sliderParallaxEl.addClass('slider-parallax-visible').removeClass('slider-parallax-invisible');
					if ($window.scrollTop() > parallaxOffsetTop) {
						if( $sliderParallaxEl.find('.slider-parallax-inner').length > 0 ) {
							var tranformAmount = (($window.scrollTop()-parallaxOffsetTop) *-.4 ).toFixed(0),
								tranformAmount2 = (($window.scrollTop()-parallaxOffsetTop) *-.15 ).toFixed(0);
							$sliderParallaxEl.find('.slider-parallax-inner').css({'transform':'translateY('+ tranformAmount +'px)'});
							$('.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY('+ tranformAmount2 +'px)'});
						} else {
							var tranformAmount = (($window.scrollTop()-parallaxOffsetTop) / 1.5 ).toFixed(0),
								tranformAmount2 = (($window.scrollTop()-parallaxOffsetTop) / 7 ).toFixed(0);
							$sliderParallaxEl.css({'transform':'translateY('+ tranformAmount +'px)'});
							$('.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY('+ -tranformAmount2 +'px)'});
						}
					} else {
						if( $sliderParallaxEl.find('.slider-parallax-inner').length > 0 ) {
							$('.slider-parallax-inner,.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY(0px)'});
						} else {
							$('.slider-parallax,.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY(0px)'});
						}
					}
				} else {
					$sliderParallaxEl.addClass('slider-parallax-invisible').removeClass('slider-parallax-visible');
				}
				if (requesting) {
					requestAnimationFrame(function(){
						SEMICOLON.slider.sliderParallax();
						SEMICOLON.slider.sliderElementsFade();
					});
				}
			} else {
				if( $sliderParallaxEl.find('.slider-parallax-inner').length > 0 ) {
					$('.slider-parallax-inner,.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY(0px)'});
				} else {
					$('.slider-parallax,.slider-parallax .slider-caption,.ei-title').css({'transform':'translateY(0px)'});
				}
			}
		},

		sliderElementsFade: function(){

			if( $sliderParallaxEl.length > 0 ) {
				if( ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) && !SEMICOLON.isMobile.any() ) {
					var parallaxOffsetTop = SEMICOLON.slider.sliderParallaxOffset(),
						parallaxElHeight = $sliderParallaxEl.outerHeight();
					if( $slider.length > 0 ) {
						if( $header.hasClass('transparent-header') || $('body').hasClass('side-header') ) { var tHeaderOffset = 100; } else { var tHeaderOffset = 0; }
						$sliderParallaxEl.find('#slider-arrow-left,#slider-arrow-right,.vertical-middle:not(.no-fade),.slider-caption,.ei-title,.camera_prev,.camera_next').css({'opacity': 1 - ( ( ( $window.scrollTop() - tHeaderOffset ) *1.85 ) / parallaxElHeight ) });
					}
				} else {
					$sliderParallaxEl.find('#slider-arrow-left,#slider-arrow-right,.vertical-middle:not(.no-fade),.slider-caption,.ei-title,.camera_prev,.camera_next').css({'opacity': 1});
				}
			}
		},

		captionPosition: function(){
			$slider.find('.slider-caption:not(.custom-caption-pos)').each(function(){
				var scapHeight = $(this).outerHeight();
				var scapSliderHeight = $slider.outerHeight();
				if( $(this).parents('#slider').prev('#header').hasClass('transparent-header') && ( $body.hasClass('device-lg') || $body.hasClass('device-md') ) ) {
					if( $(this).parents('#slider').prev('#header').hasClass('floating-header') ) {
						$(this).css({ top: ( scapSliderHeight + 160 - scapHeight ) / 2 + 'px' });
					} else {
						$(this).css({ top: ( scapSliderHeight + 100 - scapHeight ) / 2 + 'px' });
					}
				} else {
					$(this).css({ top: ( scapSliderHeight - scapHeight ) / 2 + 'px' });
				}
			});
		},

		swiperSliderMenu: function( onWinLoad ){
			onWinLoad = typeof onWinLoad !== 'undefined' ? onWinLoad : false;
			if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
				var activeSlide = $slider.find('.swiper-slide.swiper-slide-active');
				SEMICOLON.slider.headerSchemeChanger(activeSlide, onWinLoad);
			}
		},

		revolutionSliderMenu: function( onWinLoad ){
			onWinLoad = typeof onWinLoad !== 'undefined' ? onWinLoad : false;
			if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
				var activeSlide = $slider.find('.active-revslide');
				SEMICOLON.slider.headerSchemeChanger(activeSlide, onWinLoad);
			}
		},

		headerSchemeChanger: function( activeSlide, onWinLoad ){
			if( activeSlide.length > 0 ) {
				var darkExists = false;
				if( activeSlide.hasClass('dark') ){
					if( oldHeaderClasses ) { var oldClassesArray = oldHeaderClasses.split(/ +/); } else { var oldClassesArray = ''; }
					var noOfOldClasses = oldClassesArray.length;

					if( noOfOldClasses > 0 ) {
						var i = 0;
						for( i=0; i<noOfOldClasses; i++ ) {
							if( oldClassesArray[i] == 'dark' && onWinLoad == true ) {
								darkExists = true;
								break;
							}
						}
					}
					$('#header.transparent-header:not(.sticky-header,.semi-transparent,.floating-header)').addClass('dark');
					if( !darkExists ) {
						$('#header.transparent-header.sticky-header,#header.transparent-header.semi-transparent.sticky-header,#header.transparent-header.floating-header.sticky-header').removeClass('dark');
					}
					$headerWrap.removeClass('not-dark');
				} else {
					if( $body.hasClass('dark') ) {
						activeSlide.addClass('not-dark');
						$('#header.transparent-header:not(.semi-transparent,.floating-header)').removeClass('dark');
						$('#header.transparent-header:not(.sticky-header,.semi-transparent,.floating-header)').find('#header-wrap').addClass('not-dark');
					} else {
						$('#header.transparent-header:not(.semi-transparent,.floating-header)').removeClass('dark');
						$headerWrap.removeClass('not-dark');
					}
				}
				if( $header.hasClass('sticky-header') ) {
					SEMICOLON.header.stickyMenuClass();
				}
				SEMICOLON.header.logo();
			}
		},

		owlCaptionInit: function(){
			if( $owlCarouselEl.length > 0 ){
				$owlCarouselEl.each( function(){
					var element = $(this);
					if( element.find('.owl-dot').length > 0 ) {
						element.addClass('with-carousel-dots');
					}
				});
			}
		}

	};

	SEMICOLON.portfolio = {

		init: function(){

			SEMICOLON.portfolio.ajaxload();

		},

		gridInit: function( $container ){

			if( !$().isotope ) {
				console.log('gridInit: Isotope not Defined.');
				return true;
			}

			if( $container.length < 1 ) { return true; }
			if( $container.hasClass('customjs') ) { return true; }

			$container.each( function(){
				var element = $(this),
					elementTransition = element.attr('data-transition'),
					elementLayoutMode = element.attr('data-layout'),
					elementStagger = element.attr('data-stagger');

				if( !elementTransition ) { elementTransition = '0.65s'; }
				if( !elementLayoutMode ) { elementLayoutMode = 'masonry'; }
				if( !elementStagger ) { elementStagger = 0; }

				setTimeout( function(){
					if( element.hasClass('portfolio') ){
						element.isotope({
							layoutMode: elementLayoutMode,
							transitionDuration: elementTransition,
							stagger: Number( elementStagger ),
							masonry: {
								columnWidth: element.find('.portfolio-item:not(.wide)')[0]
							}
						});
					} else {
						element.isotope({
							layoutMode: elementLayoutMode,
							transitionDuration: elementTransition
						});
					}
				}, 300);
			});
		},

		filterInit: function(){

			if( !$().isotope ) {
				console.log('filterInit: Isotope not Defined.');
				return true;
			}

			if( $portfolioFilter.length < 1 ) { return true; }
			if( $portfolioFilter.hasClass('customjs') ) { return true; }

			$portfolioFilter.each( function(){
				var element = $(this),
					elementContainer = element.attr('data-container'),
					elementActiveClass = element.attr('data-active-class'),
					elementDefaultFilter = element.attr('data-default');

				if( !elementActiveClass ) { elementActiveClass = 'activeFilter'; }

				element.find('a').click(function(){
					element.find('li').removeClass( elementActiveClass );
					$(this).parent('li').addClass( elementActiveClass );
					var selector = $(this).attr('data-filter');
					$(elementContainer).isotope({ filter: selector });
					return false;
				});

				if( elementDefaultFilter ) {
					element.find('li').removeClass( elementActiveClass );
					element.find('[data-filter="'+ elementDefaultFilter +'"]').parent('li').addClass( elementActiveClass );
					$(elementContainer).isotope({ filter: elementDefaultFilter });
				}
			});
		},

		shuffleInit: function(){

			if( !$().isotope ) {
				console.log('shuffleInit: Isotope not Defined.');
				return true;
			}

			if( $('.portfolio-shuffle').length < 1 ) { return true; }

			$('.portfolio-shuffle').click(function(){
				var element = $(this),
					elementContainer = element.attr('data-container');

				$(elementContainer).isotope('shuffle');
			});
		},

		portfolioDescMargin: function(){
			var $portfolioOverlayEl = $('.portfolio-overlay');
			if( $portfolioOverlayEl.length > 0 ){
				$portfolioOverlayEl.each(function() {
					var element = $(this);
					if( element.find('.portfolio-desc').length > 0 ) {
						var portfolioOverlayHeight = element.outerHeight();
						var portfolioOverlayDescHeight = element.find('.portfolio-desc').outerHeight();
						if( element.find('a.left-icon').length > 0 || element.find('a.right-icon').length > 0 || element.find('a.center-icon').length > 0 ) {
							var portfolioOverlayIconHeight = 40 + 20;
						} else {
							var portfolioOverlayIconHeight = 0;
						}
						var portfolioOverlayMiddleAlign = ( portfolioOverlayHeight - portfolioOverlayDescHeight - portfolioOverlayIconHeight ) / 2
						element.find('.portfolio-desc').css({ 'margin-top': portfolioOverlayMiddleAlign });
					}
				});
			}
		},

		arrange: function(){
			if( $portfolio.length > 0 ) {
				$portfolio.each( function(){
					var element = $(this);
					SEMICOLON.initialize.setFullColumnWidth( element );
				});
			}
		},

		ajaxload: function(){
			$('.portfolio-ajax .portfolio-item a.center-icon').click( function(e) {
				var portPostId = $(this).parents('.portfolio-item').attr('id');
				if( !$(this).parents('.portfolio-item').hasClass('portfolio-active') ) {
					SEMICOLON.portfolio.loadItem(portPostId, prevPostPortId);
				}
				e.preventDefault();
			});
		},

		newNextPrev: function( portPostId ){
			var portNext = SEMICOLON.portfolio.getNextItem(portPostId);
			var portPrev = SEMICOLON.portfolio.getPrevItem(portPostId);
			$('#next-portfolio').attr('data-id', portNext);
			$('#prev-portfolio').attr('data-id', portPrev);
		},

		loadItem: function( portPostId, prevPostPortId, getIt ){
			if(!getIt) { getIt = false; }
			var portNext = SEMICOLON.portfolio.getNextItem(portPostId);
			var portPrev = SEMICOLON.portfolio.getPrevItem(portPostId);
			if(getIt == false) {
				SEMICOLON.portfolio.closeItem();
				$portfolioAjaxLoader.fadeIn();
				var portfolioDataLoader = $('#' + portPostId).attr('data-loader');
				$portfolioDetailsContainer.load(portfolioDataLoader, { portid: portPostId, portnext: portNext, portprev: portPrev },
				function(){
					SEMICOLON.portfolio.initializeAjax(portPostId);
					SEMICOLON.portfolio.openItem();
					$portfolioItems.removeClass('portfolio-active');
					$('#' + portPostId).addClass('portfolio-active');
				});
			}
		},

		closeItem: function(){
			if( $portfolioDetails && $portfolioDetails.height() > 32 ) {
				$portfolioAjaxLoader.fadeIn();
				$portfolioDetails.find('#portfolio-ajax-single').fadeOut('600', function(){
					$(this).remove();
				});
				$portfolioDetails.removeClass('portfolio-ajax-opened');
			}
		},

		openItem: function(){
			var noOfImages = $portfolioDetails.find('img').length;
			var noLoaded = 0;

			if( noOfImages > 0 ) {
				$portfolioDetails.find('img').on('load', function(){
					noLoaded++;
					var topOffsetScroll = SEMICOLON.initialize.topScrollOffset();
					if(noOfImages === noLoaded) {
						$portfolioDetailsContainer.css({ 'display': 'block' });
						$portfolioDetails.addClass('portfolio-ajax-opened');
						$portfolioAjaxLoader.fadeOut();
						var t=setTimeout(function(){
							SEMICOLON.widget.loadFlexSlider();
							SEMICOLON.initialize.lightbox();
							SEMICOLON.initialize.resizeVideos();
							SEMICOLON.widget.masonryThumbs();
							$('html,body').stop(true).animate({
								'scrollTop': $portfolioDetails.offset().top - topOffsetScroll
							}, 900, 'easeOutQuad');
						},500);
					}
				});
			} else {
				var topOffsetScroll = SEMICOLON.initialize.topScrollOffset();
				$portfolioDetailsContainer.css({ 'display': 'block' });
				$portfolioDetails.addClass('portfolio-ajax-opened');
				$portfolioAjaxLoader.fadeOut();
				var t=setTimeout(function(){
					SEMICOLON.widget.loadFlexSlider();
					SEMICOLON.initialize.lightbox();
					SEMICOLON.initialize.resizeVideos();
					SEMICOLON.widget.masonryThumbs();
					$('html,body').stop(true).animate({
						'scrollTop': $portfolioDetails.offset().top - topOffsetScroll
					}, 900, 'easeOutQuad');
				},500);
			}
		},

		getNextItem: function( portPostId ){
			var portNext = '';
			var hasNext = $('#' + portPostId).next();
			if(hasNext.length != 0) {
				portNext = hasNext.attr('id');
			}
			return portNext;
		},

		getPrevItem: function( portPostId ){
			var portPrev = '';
			var hasPrev = $('#' + portPostId).prev();
			if(hasPrev.length != 0) {
				portPrev = hasPrev.attr('id');
			}
			return portPrev;
		},

		initializeAjax: function( portPostId ){
			prevPostPortId = $('#' + portPostId);

			$('#next-portfolio, #prev-portfolio').click( function() {
				var portPostId = $(this).attr('data-id');
				$portfolioItems.removeClass('portfolio-active');
				$('#' + portPostId).addClass('portfolio-active');
				SEMICOLON.portfolio.loadItem(portPostId,prevPostPortId);
				return false;
			});

			$('#close-portfolio').click( function() {
				$portfolioDetailsContainer.fadeOut('600', function(){
					$portfolioDetails.find('#portfolio-ajax-single').remove();
				});
				$portfolioDetails.removeClass('portfolio-ajax-opened');
				$portfolioItems.removeClass('portfolio-active');
				return false;
			});
		}

	};

	SEMICOLON.widget = {

		init: function(){

			SEMICOLON.widget.animations();
			SEMICOLON.widget.youtubeBgVideo();
			SEMICOLON.widget.tabs();
			SEMICOLON.widget.tabsJustify();
			SEMICOLON.widget.tabsResponsive();
			SEMICOLON.widget.tabsResponsiveResize();
			SEMICOLON.widget.toggles();
			SEMICOLON.widget.accordions();
			SEMICOLON.widget.counter();
			SEMICOLON.widget.roundedSkill();
			SEMICOLON.widget.progress();
			SEMICOLON.widget.twitterFeed();
			SEMICOLON.widget.flickrFeed();
			SEMICOLON.widget.instagramPhotos( '36286274.b9e559e.4824cbc1d0c94c23827dc4a2267a9f6b', 'b9e559ec7c284375bf41e9a9fb72ae01' );
			SEMICOLON.widget.dribbbleShots( '01530280af335d298e756ed8ef786c8c4e92a50b88e53a185531b1a639e768b8' );
			SEMICOLON.widget.navTree();
			SEMICOLON.widget.textRotater();
			SEMICOLON.widget.carousel();
			SEMICOLON.widget.linkScroll();
			SEMICOLON.widget.contactForm();
			SEMICOLON.widget.subscription();
			SEMICOLON.widget.quickContact();
			SEMICOLON.widget.stickySidebar();
			SEMICOLON.widget.cookieNotify();
			SEMICOLON.widget.extras();

		},

		parallax: function(){

			if( !$.stellar ) {
				console.log('parallax: Stellar not Defined.');
				return true;
			}

			if( $parallaxEl.length > 0 || $parallaxPageTitleEl.length > 0 || $parallaxPortfolioEl.length > 0 ) {
				if( !SEMICOLON.isMobile.any() ){
					$.stellar({
						horizontalScrolling: false,
						verticalOffset: 150
					});
				} else {
					$parallaxEl.addClass('mobile-parallax');
					$parallaxPageTitleEl.addClass('mobile-parallax');
					$parallaxPortfolioEl.addClass('mobile-parallax');
				}
			}
		},

		animations: function(){

			if( !$().appear ) {
				console.log('animations: Appear not Defined.');
				return true;
			}

			var $dataAnimateEl = $('[data-animate]');
			if( $dataAnimateEl.length > 0 ){
				if( $body.hasClass('device-lg') || $body.hasClass('device-md') || $body.hasClass('device-sm') ){
					$dataAnimateEl.each(function(){
						var element = $(this),
							animationOut = element.attr('data-animate-out'),
							animationDelay = element.attr('data-delay'),
							animationDelayOut = element.attr('data-delay-out'),
							animationDelayTime = 0,
							animationDelayOutTime = 3000;

						if( element.parents('.fslider.no-thumbs-animate').length > 0 ) { return true; }

						if( animationDelay ) { animationDelayTime = Number( animationDelay ) + 500; } else { animationDelayTime = 500; }
						if( animationOut && animationDelayOut ) { animationDelayOutTime = Number( animationDelayOut ) + animationDelayTime; }

						if( !element.hasClass('animated') ) {
							element.addClass('not-animated');
							var elementAnimation = element.attr('data-animate');
							element.appear(function () {
								setTimeout(function() {
									element.removeClass('not-animated').addClass( elementAnimation + ' animated');
								}, animationDelayTime);

								if( animationOut ) {
									setTimeout( function() {
										element.removeClass( elementAnimation ).addClass( animationOut );
									}, animationDelayOutTime );
								}
							},{accX: 0, accY: -120},'easeInCubic');
						}
					});
				}
			}
		},

		loadFlexSlider: function(){

			if( !$().flexslider ) {
				console.log('loadFlexSlider: FlexSlider not Defined.');
				return true;
			}

			var $flexSliderEl = $('.fslider:not(.customjs)').find('.flexslider');
			if( $flexSliderEl.length > 0 ){
				$flexSliderEl.each(function() {
					var $flexsSlider = $(this),
						flexsAnimation = $flexsSlider.parent('.fslider').attr('data-animation'),
						flexsEasing = $flexsSlider.parent('.fslider').attr('data-easing'),
						flexsDirection = $flexsSlider.parent('.fslider').attr('data-direction'),
						flexsReverse = $flexsSlider.parent('.fslider').attr('data-reverse'),
						flexsSlideshow = $flexsSlider.parent('.fslider').attr('data-slideshow'),
						flexsPause = $flexsSlider.parent('.fslider').attr('data-pause'),
						flexsSpeed = $flexsSlider.parent('.fslider').attr('data-speed'),
						flexsVideo = $flexsSlider.parent('.fslider').attr('data-video'),
						flexsPagi = $flexsSlider.parent('.fslider').attr('data-pagi'),
						flexsArrows = $flexsSlider.parent('.fslider').attr('data-arrows'),
						flexsThumbs = $flexsSlider.parent('.fslider').attr('data-thumbs'),
						flexsHover = $flexsSlider.parent('.fslider').attr('data-hover'),
						flexsSheight = $flexsSlider.parent('.fslider').attr('data-smooth-height'),
						flexsTouch = $flexsSlider.parent('.fslider').attr('data-touch'),
						flexsUseCSS = false;

					if( !flexsAnimation ) { flexsAnimation = 'slide'; }
					if( !flexsEasing || flexsEasing == 'swing' ) {
						flexsEasing = 'swing';
						flexsUseCSS = true;
					}
					if( !flexsDirection ) { flexsDirection = 'horizontal'; }
					if( flexsReverse == 'true' ) { flexsReverse = true; } else { flexsReverse = false; }
					if( !flexsSlideshow ) { flexsSlideshow = true; } else { flexsSlideshow = false; }
					if( !flexsPause ) { flexsPause = 5000; }
					if( !flexsSpeed ) { flexsSpeed = 600; }
					if( !flexsVideo ) { flexsVideo = false; }
					if( flexsSheight == 'false' ) { flexsSheight = false; } else { flexsSheight = true; }
					if( flexsDirection == 'vertical' ) { flexsSheight = false; }
					if( flexsPagi == 'false' ) { flexsPagi = false; } else { flexsPagi = true; }
					if( flexsThumbs == 'true' ) { flexsPagi = 'thumbnails'; } else { flexsPagi = flexsPagi; }
					if( flexsArrows == 'false' ) { flexsArrows = false; } else { flexsArrows = true; }
					if( flexsHover == 'false' ) { flexsHover = false; } else { flexsHover = true; }
					if( flexsTouch == 'false' ) { flexsTouch = false; } else { flexsTouch = true; }

					$flexsSlider.flexslider({
						selector: ".slider-wrap > .slide",
						animation: flexsAnimation,
						easing: flexsEasing,
						direction: flexsDirection,
						reverse: flexsReverse,
						slideshow: flexsSlideshow,
						slideshowSpeed: Number(flexsPause),
						animationSpeed: Number(flexsSpeed),
						pauseOnHover: flexsHover,
						video: flexsVideo,
						controlNav: flexsPagi,
						directionNav: flexsArrows,
						smoothHeight: flexsSheight,
						useCSS: flexsUseCSS,
						touch: flexsTouch,
						start: function(slider){
							SEMICOLON.widget.animations();
							SEMICOLON.initialize.verticalMiddle();
							slider.parent().removeClass('preloader2');
							var t = setTimeout( function(){ $('.grid-container').isotope('layout'); }, 1200 );
							SEMICOLON.initialize.lightbox();
							$('.flex-prev').html('<i class="icon-angle-left"></i>');
							$('.flex-next').html('<i class="icon-angle-right"></i>');
							SEMICOLON.portfolio.portfolioDescMargin();
						},
						after: function(){
							if( $('.grid-container').hasClass('portfolio-full') ) {
								$('.grid-container.portfolio-full').isotope('layout');
								SEMICOLON.portfolio.portfolioDescMargin();
							}
						}
					});
				});
			}
		},

		html5Video: function(){
			var videoEl = $('.video-wrap:has(video)');
			if( videoEl.length > 0 ) {
				videoEl.each(function(){
					var element = $(this),
						elementVideo = element.find('video'),
						outerContainerWidth = element.outerWidth(),
						outerContainerHeight = element.outerHeight(),
						innerVideoWidth = elementVideo.outerWidth(),
						innerVideoHeight = elementVideo.outerHeight();

					if( innerVideoHeight < outerContainerHeight ) {
						var videoAspectRatio = innerVideoWidth/innerVideoHeight,
							newVideoWidth = outerContainerHeight * videoAspectRatio,
							innerVideoPosition = (newVideoWidth - outerContainerWidth) / 2;
						elementVideo.css({ 'width': newVideoWidth+'px', 'height': outerContainerHeight+'px', 'left': -innerVideoPosition+'px' });
					} else {
						var innerVideoPosition = (innerVideoHeight - outerContainerHeight) / 2;
						elementVideo.css({ 'width': innerVideoWidth+'px', 'height': innerVideoHeight+'px', 'top': -innerVideoPosition+'px' });
					}

					if( SEMICOLON.isMobile.any() && !element.hasClass('no-placeholder') ) {
						var placeholderImg = elementVideo.attr('poster');

						if( placeholderImg != '' ) {
							element.append('<div class="video-placeholder" style="background-image: url('+ placeholderImg +');"></div>')
						}

						elementVideo.hide();
					}
				});
			}
		},

		youtubeBgVideo: function(){

			if( !$().mb_YTPlayer ) {
				console.log('youtubeBgVideo: YoutubeBG Plugin not Defined.');
				return true;
			}

			var $youtubeBgPlayerEl = $('.yt-bg-player');
			if( $youtubeBgPlayerEl.hasClass('customjs') ) { return true; }

			if( $youtubeBgPlayerEl.length > 0 ){
				$youtubeBgPlayerEl.each( function(){
					var element = $(this),
						ytbgVideo = element.attr('data-video'),
						ytbgMute = element.attr('data-mute'),
						ytbgRatio = element.attr('data-ratio'),
						ytbgQuality = element.attr('data-quality'),
						ytbgOpacity = element.attr('data-opacity'),
						ytbgContainer = element.attr('data-container'),
						ytbgOptimize = element.attr('data-optimize'),
						ytbgLoop = element.attr('data-loop'),
						ytbgVolume = element.attr('data-volume'),
						ytbgStart = element.attr('data-start'),
						ytbgStop = element.attr('data-stop'),
						ytbgAutoPlay = element.attr('data-autoplay'),
						ytbgFullScreen = element.attr('data-fullscreen');

					if( ytbgMute == 'false' ) { ytbgMute = false; } else { ytbgMute = true; }
					if( !ytbgRatio ) { ytbgRatio = '16/9'; }
					if( !ytbgQuality ) { ytbgQuality = 'hd720'; }
					if( !ytbgOpacity ) { ytbgOpacity = 1; }
					if( !ytbgContainer ) { ytbgContainer = 'self'; }
					if( ytbgOptimize == 'false' ) { ytbgOptimize = false; } else { ytbgOptimize = true; }
					if( ytbgLoop == 'false' ) { ytbgLoop = false; } else { ytbgLoop = true; }
					if( !ytbgVolume ) { ytbgVolume = 1; }
					if( !ytbgStart ) { ytbgStart = 0; }
					if( !ytbgStop ) { ytbgStop = 0; }
					if( ytbgAutoPlay == 'false' ) { ytbgAutoPlay = false; } else { ytbgAutoPlay = true; }
					if( ytbgFullScreen == 'true' ) { ytbgFullScreen = true; } else { ytbgFullScreen = false; }

					element.mb_YTPlayer({
						videoURL: ytbgVideo,
						mute: ytbgMute,
						ratio: ytbgRatio,
						quality: ytbgQuality,
						opacity: Number(ytbgOpacity),
						containment: ytbgContainer,
						optimizeDisplay: ytbgOptimize,
						loop: ytbgLoop,
						vol: Number(ytbgVolume),
						startAt: Number(ytbgStart),
						stopAt: Number(ytbgStop),
						autoplay: ytbgAutoPlay,
						realfullscreen: ytbgFullScreen,
						showYTLogo: false,
						showControls: false
					});
				});
			}
		},

		tabs: function(){

			if( !$().tabs ) {
				console.log('tabs: Tabs not Defined.');
				return true;
			}

			var $tabs = $('.tabs:not(.customjs)');
			if( $tabs.length > 0 ) {
				$tabs.each( function(){
					var element = $(this),
						elementSpeed = element.attr('data-speed'),
						tabActive = element.attr('data-active');

					if( !elementSpeed ) { elementSpeed = 400; }
					if( !tabActive ) { tabActive = 0; } else { tabActive = tabActive - 1; }

					element.tabs({
						active: Number(tabActive),
						show: {
							effect: "fade",
							duration: Number(elementSpeed)
						}
					});
				});
			}
		},

		tabsJustify: function(){
			if( !$('body').hasClass('device-xxs') && !$('body').hasClass('device-xs') ){
				var $tabsJustify = $('.tabs.tabs-justify');
				if( $tabsJustify.length > 0 ) {
					$tabsJustify.each( function(){
						var element = $(this),
							elementTabs = element.find('.tab-nav > li'),
							elementTabsNo = elementTabs.length,
							elementContainer = 0,
							elementWidth = 0;

						if( element.hasClass('tabs-bordered') || element.hasClass('tabs-bb') ) {
							elementContainer = element.find('.tab-nav').outerWidth();
						} else {
							if( element.find('tab-nav').hasClass('tab-nav2') ) {
								elementContainer = element.find('.tab-nav').outerWidth() - (elementTabsNo * 10);
							} else {
								elementContainer = element.find('.tab-nav').outerWidth() - 30;
							}
						}

						elementWidth = Math.floor(elementContainer/elementTabsNo);
						elementTabs.css({ 'width': elementWidth + 'px' });

					});
				}
			} else { $('.tabs.tabs-justify').find('.tab-nav > li').css({ 'width': '' }); }
		},

		tabsResponsive: function(){

			if( !$().tabs ) {
				console.log('tabs: Tabs not Defined.');
				return true;
			}

			var $tabsResponsive = $('.tabs.tabs-responsive');
			if( $tabsResponsive.length < 1 ) { return true; }

			$tabsResponsive.each( function(){
				var element = $(this),
					elementNav = $(this).find('.tab-nav'),
					elementContent = $(this).find('.tab-container');

				elementNav.children('li').each( function(){
					var navEl = $(this),
						navElAnchor = navEl.children('a'),
						navElTarget = navElAnchor.attr('href'),
						navElContent = navElAnchor.html();

					elementContent.find(navElTarget).before('<div class="acctitle hide"><i class="acc-closed icon-ok-circle"></i><i class="acc-open icon-remove-circle"></i>'+navElContent+'</div>');
				});
			});
		},

		tabsResponsiveResize: function(){

			if( !$().tabs ) {
				console.log('tabs: Tabs not Defined.');
				return true;
			}

			var $tabsResponsive = $('.tabs.tabs-responsive');
			if( $tabsResponsive.length < 1 ) { return true; }

			$tabsResponsive.each( function(){
				var element = $(this),
					elementAccStyle = element.attr('data-accordion-style');

				if( $('body').hasClass('device-xs') || $('body').hasClass('device-xxs') ) {

					element.find('.tab-nav').addClass('hide');
					element.find('.tab-container').addClass('accordion '+ elementAccStyle +' clearfix');
					element.find('.tab-content').addClass('acc_content');
					element.find('.acctitle').removeClass('hide');
					SEMICOLON.widget.accordions();

				} else if( $('body').hasClass('device-sm') || $('body').hasClass('device-md') || $('body').hasClass('device-lg') ) {

					element.find('.tab-nav').removeClass('hide');
					element.find('.tab-container').removeClass('accordion '+ elementAccStyle +' clearfix');
					element.find('.tab-content').removeClass('acc_content');
					element.find('.acctitle').addClass('hide');
					element.tabs( "refresh" );

				}
			});
		},

		toggles: function(){
			var $toggle = $('.toggle');
			if( $toggle.length > 0 ) {
				$toggle.each( function(){
					var element = $(this),
						elementState = element.attr('data-state');

					if( elementState != 'open' ){
						element.children('.togglec').hide();
					} else {
						element.children('.togglet').addClass("toggleta");
					}

					element.children('.togglet').click(function(){
						$(this).toggleClass('toggleta').next('.togglec').slideToggle(300);
						return true;
					});
				});
			}
		},

		accordions: function(){
			var $accordionEl = $('.accordion');
			if( $accordionEl.length > 0 ){
				$accordionEl.each( function(){
					var element = $(this),
						elementState = element.attr('data-state'),
						accordionActive = element.attr('data-active');

					if( !accordionActive ) { accordionActive = 0; } else { accordionActive = accordionActive - 1; }

					element.find('.acc_content').hide();

					if( elementState != 'closed' ) {
						element.find('.acctitle:eq('+ Number(accordionActive) +')').addClass('acctitlec').next().show();
					}

					element.find('.acctitle').click(function(){
						if( $(this).next().is(':hidden') ) {
							element.find('.acctitle').removeClass('acctitlec').next().slideUp("normal");
							var clickTarget = $(this);
							$(this).toggleClass('acctitlec').next().slideDown("normal", function(){
								$('html,body').stop(true).animate({
									'scrollTop': clickTarget.offset().top - ( SEMICOLON.initialize.topScrollOffset() - 40 )
								}, 800, 'easeOutQuad' );
							});
						}
						return false;
					});
				});
			}
		},

		counter: function(){

			if( !$().appear ) {
				console.log('counter: Appear not Defined.');
				return true;
			}

			if( !$().countTo ) {
				console.log('counter: countTo not Defined.');
				return true;
			}

			var $counterEl = $('.counter:not(.counter-instant)');
			if( $counterEl.length > 0 ){
				$counterEl.each(function(){
					var element = $(this);
					var counterElementComma = $(this).find('span').attr('data-comma');
					if( !counterElementComma ) { counterElementComma = false; } else { counterElementComma = true; }
					if( $body.hasClass('device-lg') || $body.hasClass('device-md') ){
						element.appear( function(){
							SEMICOLON.widget.runCounter( element, counterElementComma );
							if( element.parents('.common-height') ) {
								SEMICOLON.initialize.maxHeight();
							}
						},{accX: 0, accY: -120},'easeInCubic');
					} else {
						SEMICOLON.widget.runCounter( element, counterElementComma );
					}
				});
			}
		},

		runCounter: function( counterElement,counterElementComma ){
			if( counterElementComma == true ) {
				counterElement.find('span').countTo({
					formatter: function (value, options) {
						value = value.toFixed(options.decimals);
						value = value.replace(/\B(?=(\d{3})+(?!\d))/g, ',');
						return value;
					}
				});
			} else {
				counterElement.find('span').countTo();
			}
		},

		roundedSkill: function(){

			if( !$().appear ) {
				console.log('roundedSkill: Appear not Defined.');
				return true;
			}

			if( !$().easyPieChart ) {
				console.log('roundedSkill: EasyPieChart not Defined.');
				return true;
			}

			var $roundedSkillEl = $('.rounded-skill');
			if( $roundedSkillEl.length > 0 ){
				$roundedSkillEl.each(function(){
					var element = $(this);

					var roundSkillSize = element.attr('data-size');
					var roundSkillSpeed = element.attr('data-speed');
					var roundSkillWidth = element.attr('data-width');
					var roundSkillColor = element.attr('data-color');
					var roundSkillTrackColor = element.attr('data-trackcolor');

					if( !roundSkillSize ) { roundSkillSize = 140; }
					if( !roundSkillSpeed ) { roundSkillSpeed = 2000; }
					if( !roundSkillWidth ) { roundSkillWidth = 8; }
					if( !roundSkillColor ) { roundSkillColor = '#0093BF'; }
					if( !roundSkillTrackColor ) { roundSkillTrackColor = 'rgba(0,0,0,0.04)'; }

					var properties = {roundSkillSize:roundSkillSize, roundSkillSpeed:roundSkillSpeed, roundSkillWidth:roundSkillWidth, roundSkillColor:roundSkillColor, roundSkillTrackColor:roundSkillTrackColor};

					if( $body.hasClass('device-lg') || $body.hasClass('device-md') ){
						element.css({'width':roundSkillSize+'px','height':roundSkillSize+'px','line-height':roundSkillSize+'px'}).animate({opacity:0}, 10);
						element.appear( function(){
							if (!element.hasClass('skills-animated')) {
								var t = setTimeout( function(){ element.css({opacity: 1}); }, 100 );
								SEMICOLON.widget.runRoundedSkills( element, properties );
								element.addClass('skills-animated');
							}
						},{accX: 0, accY: -120},'easeInCubic');
					} else {
						SEMICOLON.widget.runRoundedSkills( element, properties );
					}
				});
			}
		},

		runRoundedSkills: function( element, properties ){
			element.easyPieChart({
				size: Number(properties.roundSkillSize),
				animate: Number(properties.roundSkillSpeed),
				scaleColor: false,
				trackColor: properties.roundSkillTrackColor,
				lineWidth: Number(properties.roundSkillWidth),
				lineCap: 'square',
				barColor: properties.roundSkillColor
			});
		},

		progress: function(){

			if( !$().appear ) {
				console.log('progress: Appear not Defined.');
				return true;
			}

			var $progressEl = $('.progress');
			if( $progressEl.length > 0 ){
				$progressEl.each(function(){
					var element = $(this),
						skillsBar = element.parent('li'),
						skillValue = skillsBar.attr('data-percent');

					if( $body.hasClass('device-lg') || $body.hasClass('device-md') ){
						element.appear( function(){
							if (!skillsBar.hasClass('skills-animated')) {
								element.find('.counter-instant span').countTo();
								skillsBar.find('.progress').css({width: skillValue + "%"}).addClass('skills-animated');
							}
						},{accX: 0, accY: -120},'easeInCubic');
					} else {
						element.find('.counter-instant span').countTo();
						skillsBar.find('.progress').css({width: skillValue + "%"});
					}
				});
			}
		},

		twitterFeed: function(){

			if( typeof sm_format_twitter === 'undefined' ) {
				console.log('twitterFeed: sm_format_twitter() not Defined.');
				return true;
			}

			if( typeof sm_format_twitter3 === 'undefined' ) {
				console.log('twitterFeed: sm_format_twitter3() not Defined.');
				return true;
			}

			var $twitterFeedEl = $('.twitter-feed');
			if( $twitterFeedEl.length > 0 ){
				$twitterFeedEl.each(function() {
					var element = $(this),
						twitterFeedUser = element.attr('data-username'),
						twitterFeedCount = element.attr('data-count'),
						twitterFeedLoader = element.attr('data-loader');

					if( !twitterFeedUser ) { twitterFeedUser = 'twitter' }
					if( !twitterFeedCount ) { twitterFeedCount = 3 }
					if( !twitterFeedLoader ) { twitterFeedLoader = 'include/twitter/tweets.php'; }

					$.getJSON( twitterFeedLoader + '?username='+ twitterFeedUser +'&count='+ twitterFeedCount, function(tweets){
						if( element.hasClass('fslider') ) {
							element.find(".slider-wrap").html(sm_format_twitter3(tweets)).promise().done( function(){
								var timer = setInterval(function(){
									if( element.find('.slide').length > 1 ) {
										element.removeClass('customjs');
										var t = setTimeout( function(){ SEMICOLON.widget.loadFlexSlider(); }, 500);
										clearInterval(timer);
									}
								},500);
							});
						} else {
							element.html(sm_format_twitter(tweets));
						}
					});
				});
			}
		},

		flickrFeed: function(){

			if( !$().jflickrfeed ) {
				console.log('flickrFeed: jflickrfeed not Defined.');
				return true;
			}

			var $flickrFeedEl = $('.flickr-feed');
			if( $flickrFeedEl.length > 0 ){
				$flickrFeedEl.each(function() {
					var element = $(this),
						flickrFeedID = element.attr('data-id'),
						flickrFeedCount = element.attr('data-count'),
						flickrFeedType = element.attr('data-type'),
						flickrFeedTypeGet = 'photos_public.gne';

					if( flickrFeedType == 'group' ) { flickrFeedTypeGet = 'groups_pool.gne'; }
					if( !flickrFeedCount ) { flickrFeedCount = 9; }

					element.jflickrfeed({
						feedapi: flickrFeedTypeGet,
						limit: Number(flickrFeedCount),
						qstrings: {
							id: flickrFeedID
						},
						itemTemplate: '<a href="{{image_b}}" title="{{title}}" data-lightbox="gallery-item">' +
											'<img src="{{image_s}}" alt="{{title}}" />' +
									  '</a>'
					}, function(data) {
						SEMICOLON.initialize.lightbox();
					});
				});
			}
		},

		instagramPhotos: function( c_accessToken, c_clientID ){

			if( typeof Instafeed === 'undefined' ) {
				console.log('Instafeed not Defined.');
				return true;
			}

			var $instagramPhotosEl = $('.instagram-photos');
			if( $instagramPhotosEl.length > 0 ){

				$instagramPhotosEl.each(function() {
					var element = $(this),
						instaGramTarget = element.attr('id'),
						instaGramUserId = element.attr('data-user'),
						instaGramTag = element.attr('data-tag'),
						instaGramLocation = element.attr('data-location'),
						instaGramCount = element.attr('data-count'),
						instaGramType = element.attr('data-type'),
						instaGramSortBy = element.attr('data-sortBy'),
						instaGramRes = element.attr('data-resolution');

					if( !instaGramCount ) { instaGramCount = 9; }
					if( !instaGramSortBy ) { instaGramSortBy = 'none'; }
					if( !instaGramRes ) { instaGramRes = 'thumbnail'; }

					if( instaGramType == 'user' ) {

						var feed = new Instafeed({
							target: instaGramTarget,
							get: instaGramType,
							userId: Number(instaGramUserId),
							limit: Number(instaGramCount),
							sortBy: instaGramSortBy,
							resolution: instaGramRes,
							accessToken: c_accessToken,
							clientId: c_clientID
						});

					} else if( instaGramType == 'tagged' ) {

						var feed = new Instafeed({
							target: instaGramTarget,
							get: instaGramType,
							tagName: instaGramTag,
							limit: Number(instaGramCount),
							sortBy: instaGramSortBy,
							resolution: instaGramRes,
							clientId: c_clientID
						});

					} else if( instaGramType == 'location' ) {

						var feed = new Instafeed({
							target: instaGramTarget,
							get: instaGramType,
							locationId: Number(instaGramUserId),
							limit: Number(instaGramCount),
							sortBy: instaGramSortBy,
							resolution: instaGramRes,
							clientId: c_clientID
						});

					} else {

						var feed = new Instafeed({
							target: instaGramTarget,
							get: 'popular',
							limit: Number(instaGramCount),
							sortBy: instaGramSortBy,
							resolution: instaGramRes,
							clientId: c_clientID
						});

					}

					feed.run();
				});
			}
		},

		dribbbleShots: function( c_accessToken ){

			if( !$.jribbble ) {
				console.log('dribbbleShots: Jribbble not Defined.');
				return true;
			}

			if( !$().imagesLoaded ) {
				console.log('dribbbleShots: imagesLoaded not Defined.');
				return true;
			}

			var $dribbbleShotsEl = $('.dribbble-shots');
			if( $dribbbleShotsEl.length > 0 ){

				$.jribbble.setToken( c_accessToken );

				$dribbbleShotsEl.each(function() {
					var element = $(this),
						dribbbleUsername = element.attr('data-user'),
						dribbbleCount = element.attr('data-count'),
						dribbbleList = element.attr('data-list'),
						dribbbleType = element.attr('data-type');

					element.addClass('customjs');

					if( !dribbbleCount ) { dribbbleCount = 9; }

					if( dribbbleType == 'user' ) {

						$.jribbble.users( dribbbleUsername ).shots({
							'sort': 'recent',
							'page': 1,
							'per_page': Number(dribbbleCount)
						}).then( function(res) {
							var html = [];
							res.forEach( function(shot) {
								html.push('<a href="' + shot.html_url + '" target="_blank">');
								html.push('<img src="' + shot.images.teaser + '" ');
								html.push('alt="' + shot.title + '"></a>');
							});
							element.html(html.join(''));

							element.imagesLoaded().done( function() {
								element.removeClass('customjs');
								SEMICOLON.widget.masonryThumbs();
							});
						});

					} else if( dribbbleType == 'list' ) {

						$.jribbble.shots( dribbbleList, {
							'sort': 'recent',
							'page': 1,
							'per_page': Number(dribbbleCount)
						}).then( function(res) {
							var html = [];
							res.forEach( function(shot) {
								html.push('<a href="' + shot.html_url + '" target="_blank">');
								html.push('<img src="' + shot.images.teaser + '" ');
								html.push('alt="' + shot.title + '"></a>');
							});
							element.html(html.join(''));

							element.imagesLoaded().done( function() {
								element.removeClass('customjs');
								SEMICOLON.widget.masonryThumbs();
							});
						});
					}

				});
			}
		},

		navTree: function(){
			var $navTreeEl = $('.nav-tree');
			if( $navTreeEl.length > 0 ){
				$navTreeEl.each( function(){
					var element = $(this),
						elementSpeed = element.attr('data-speed'),
						elementEasing = element.attr('data-easing');

					if( !elementSpeed ) { elementSpeed = 250; }
					if( !elementEasing ) { elementEasing = 'swing'; }

					element.find( 'ul li:has(ul)' ).addClass('sub-menu');
					element.find( 'ul li:has(ul) > a' ).append( ' <i class="icon-angle-down"></i>' );

					if( element.hasClass('on-hover') ){
						element.find( 'ul li:has(ul):not(.active)' ).hover( function(e){
							$(this).children('ul').stop(true, true).slideDown( Number(elementSpeed), elementEasing);
						}, function(){
							$(this).children('ul').delay(250).slideUp( Number(elementSpeed), elementEasing);
						});
					} else {
						element.find( 'ul li:has(ul) > a' ).click( function(e){
							var childElement = $(this);
							element.find( 'ul li' ).not(childElement.parents()).removeClass('active');
							childElement.parent().children('ul').slideToggle( Number(elementSpeed), elementEasing, function(){
								$(this).find('ul').hide();
								$(this).find('li.active').removeClass('active');
							});
							element.find( 'ul li > ul' ).not(childElement.parent().children('ul')).not(childElement.parents('ul')).slideUp( Number(elementSpeed), elementEasing );
							childElement.parent('li:has(ul)').toggleClass('active');
							e.preventDefault();
						});
					}
				});
			}
		},

		carousel: function(){

			if( !$().owlCarousel ) {
				console.log('carousel: Owl Carousel not Defined.');
				return true;
			}

			var $carousel = $('.carousel-widget:not(.customjs)');
			if( $carousel.length < 1 ){ return true; }

			$carousel.each( function(){
				var element = $(this),
					elementItems = element.attr('data-items'),
					elementItemsLg = element.attr('data-items-lg'),
					elementItemsMd = element.attr('data-items-md'),
					elementItemsSm = element.attr('data-items-sm'),
					elementItemsXs = element.attr('data-items-xs'),
					elementItemsXxs = element.attr('data-items-xxs'),
					elementLoop = element.attr('data-loop'),
					elementAutoPlay = element.attr('data-autoplay'),
					elementSpeed = element.attr('data-speed'),
					elementAnimateIn = element.attr('data-animate-in'),
					elementAnimateOut = element.attr('data-animate-out'),
					elementNav = element.attr('data-nav'),
					elementPagi = element.attr('data-pagi'),
					elementMargin = element.attr('data-margin'),
					elementStage = element.attr('data-stage-padding'),
					elementMerge = element.attr('data-merge'),
					elementStart = element.attr('data-start'),
					elementRewind = element.attr('data-rewind'),
					elementSlideBy = element.attr('data-slideby'),
					elementCenter = element.attr('data-center'),
					elementLazy = element.attr('data-lazyload'),
					elementVideo = element.attr('data-video'),
					elementRTL = element.attr('data-rtl');

				if( !elementItems ) { elementItems = 4; }
				if( !elementItemsLg ) { elementItemsLg = Number(elementItems); }
				if( !elementItemsMd ) { elementItemsMd = Number(elementItemsLg); }
				if( !elementItemsSm ) { elementItemsSm = Number(elementItemsMd); }
				if( !elementItemsXs ) { elementItemsXs = Number(elementItemsSm); }
				if( !elementItemsXxs ) { elementItemsXxs = Number(elementItemsXs); }
				if( !elementSpeed ) { elementSpeed = 250; }
				if( !elementMargin ) { elementMargin = 20; }
				if( !elementStage ) { elementStage = 0; }
				if( !elementStart ) { elementStart = 0; }

				if( !elementSlideBy ) { elementSlideBy = 1; }
				if( elementSlideBy == 'page' ) {
					elementSlideBy = 'page';
				} else {
					elementSlideBy = Number(elementSlideBy);
				}

				if( elementLoop == 'true' ){ elementLoop = true; } else { elementLoop = false; }
				if( !elementAutoPlay ){
					elementAutoPlay = false;
					var elementAutoPlayTime = 0;
				} else {
					var elementAutoPlayTime = Number(elementAutoPlay);
					elementAutoPlay = true;
				}
				if( !elementAnimateIn ) { elementAnimateIn = false; }
				if( !elementAnimateOut ) { elementAnimateOut = false; }
				if( elementNav == 'false' ){ elementNav = false; } else { elementNav = true; }
				if( elementPagi == 'false' ){ elementPagi = false; } else { elementPagi = true; }
				if( elementRewind == 'true' ){ elementRewind = true; } else { elementRewind = false; }
				if( elementMerge == 'true' ){ elementMerge = true; } else { elementMerge = false; }
				if( elementCenter == 'true' ){ elementCenter = true; } else { elementCenter = false; }
				if( elementLazy == 'true' ){ elementLazy = true; } else { elementLazy = false; }
				if( elementVideo == 'true' ){ elementVideo = true; } else { elementVideo = false; }
				if( elementRTL == 'true' || $body.hasClass('rtl') ){ elementRTL = true; } else { elementRTL = false; }

				element.owlCarousel({
					margin: Number(elementMargin),
					loop: elementLoop,
					stagePadding: Number(elementStage),
					merge: elementMerge,
					startPosition: Number(elementStart),
					rewind: elementRewind,
					slideBy: elementSlideBy,
					center: elementCenter,
					lazyLoad: elementLazy,
					nav: elementNav,
					navText: ['<i class="icon-angle-left"></i>','<i class="icon-angle-right"></i>'],
					autoplay: elementAutoPlay,
					autoplayTimeout: elementAutoPlayTime,
					autoplayHoverPause: true,
					dots: elementPagi,
					smartSpeed: Number(elementSpeed),
					fluidSpeed: Number(elementSpeed),
					video: elementVideo,
					animateIn: elementAnimateIn,
					animateOut: elementAnimateOut,
					rtl: elementRTL,
					responsive:{
						0:{ items:Number(elementItemsXxs) },
						480:{ items:Number(elementItemsXs) },
						768:{ items:Number(elementItemsSm) },
						992:{ items:Number(elementItemsMd) },
						1200:{ items:Number(elementItemsLg) }
					},
					onInitialized: function(){
						SEMICOLON.slider.owlCaptionInit();
						SEMICOLON.slider.sliderParallaxDimensions();
						SEMICOLON.initialize.lightbox();
					}
				});
			});
		},

		masonryThumbs: function(){
			var $masonryThumbsEl = $('.masonry-thumbs:not(.customjs)');
			if( $masonryThumbsEl.length > 0 ){
				$masonryThumbsEl.each( function(){
					var masonryItemContainer = $(this);
					SEMICOLON.widget.masonryThumbsArrange( masonryItemContainer );
				});
			}
		},

		masonryThumbsArrange: function( element ){

			if( !$().isotope ) {
				console.log('masonryThumbsArrange: Isotope not Defined.');
				return true;
			}

			SEMICOLON.initialize.setFullColumnWidth( element );
			element.isotope('layout');
		},

		notifications: function( element ){

			if( typeof toastr === 'undefined' ) {
				console.log('notifications: Toastr not Defined.');
				return true;
			}

			toastr.remove();
			var notifyElement = $(element),
				notifyPosition = notifyElement.attr('data-notify-position'),
				notifyType = notifyElement.attr('data-notify-type'),
				notifyMsg = notifyElement.attr('data-notify-msg'),
				notifyTimeout = notifyElement.attr('data-notify-timeout'),
				notifyCloseButton = notifyElement.attr('data-notify-close');

			if( !notifyPosition ) { notifyPosition = 'toast-top-right'; } else { notifyPosition = 'toast-' + notifyElement.attr('data-notify-position'); }
			if( !notifyMsg ) { notifyMsg = 'Please set a message!'; }
			if( !notifyTimeout ) { notifyTimeout = 5000; }
			if( notifyCloseButton == 'true' ) { notifyCloseButton = true; } else { notifyCloseButton = false; }

			toastr.options.positionClass = notifyPosition;
			toastr.options.timeOut = Number( notifyTimeout );
			toastr.options.closeButton = notifyCloseButton;
			toastr.options.closeHtml = '<button><i class="icon-remove"></i></button>';

			if( notifyType == 'warning' ) {
				toastr.warning(notifyMsg);
			} else if( notifyType == 'error' ) {
				toastr.error(notifyMsg);
			} else if( notifyType == 'success' ) {
				toastr.success(notifyMsg);
			} else {
				toastr.info(notifyMsg);
			}

			return false;
		},

		textRotater: function(){

			if( !$().Morphext ) {
				console.log('textRotater: Morphext not Defined.');
				return true;
			}

			if( $textRotaterEl.length > 0 ){
				$textRotaterEl.each(function(){
					var element = $(this),
						trRotate = $(this).attr('data-rotate'),
						trSpeed = $(this).attr('data-speed'),
						trSeparator = $(this).attr('data-separator');

					if( !trRotate ) { trRotate = "fade"; }
					if( !trSpeed ) { trSpeed = 1200; }
					if( !trSeparator ) { trSeparator = ","; }

					var tRotater = $(this).find('.t-rotate');

					tRotater.Morphext({
						animation: trRotate,
						separator: trSeparator,
						speed: Number(trSpeed)
					});
				});
			}
		},

		linkScroll: function(){
			$("a[data-scrollto]").click(function(){
				var element = $(this),
					divScrollToAnchor = element.attr('data-scrollto'),
					divScrollSpeed = element.attr('data-speed'),
					divScrollOffset = element.attr('data-offset'),
					divScrollEasing = element.attr('data-easing'),
					divScrollHighlight = element.attr('data-highlight');

					if( !divScrollSpeed ) { divScrollSpeed = 750; }
					if( !divScrollOffset ) { divScrollOffset = SEMICOLON.initialize.topScrollOffset(); }
					if( !divScrollEasing ) { divScrollEasing = 'easeOutQuad'; }

				$('html,body').stop(true).animate({
					'scrollTop': $( divScrollToAnchor ).offset().top - Number(divScrollOffset)
				}, Number(divScrollSpeed), divScrollEasing, function(){
					if( divScrollHighlight ) {
						if( $(divScrollToAnchor).find('.highlight-me').length > 0 ) {
							$(divScrollToAnchor).find('.highlight-me').animate({'backgroundColor': divScrollHighlight}, 300);
							var t = setTimeout( function(){ $(divScrollToAnchor).find('.highlight-me').animate({'backgroundColor': 'transparent'}, 300); }, 500 );
						} else {
							$(divScrollToAnchor).animate({'backgroundColor': divScrollHighlight}, 300);
							var t = setTimeout( function(){ $(divScrollToAnchor).animate({'backgroundColor': 'transparent'}, 300); }, 500 );
						}
					}
				});

				return false;
			});
		},

		contactForm: function(){

			if( !$().validate ) {
				console.log('contactForm: Form Validate not Defined.');
				return true;
			}

			if( !$().ajaxSubmit ) {
				console.log('contactForm: jQuery Form not Defined.');
				return true;
			}

			var $contactForm = $('.contact-widget:not(.customjs)');
			if( $contactForm.length < 1 ){ return true; }

			$contactForm.each( function(){
				var element = $(this),
					elementAlert = element.attr('data-alert-type'),
					elementLoader = element.attr('data-loader'),
					elementResult = element.find('.contact-form-result'),
					elementRedirect = element.attr('data-redirect');

				element.find('form').validate({
					submitHandler: function(form) {

						elementResult.hide();

						if( elementLoader == 'button' ) {
							var defButton = $(form).find('button'),
								defButtonText = defButton.html();

							defButton.html('<i class="icon-line-loader icon-spin nomargin"></i>');
						} else {
							$(form).find('.form-process').fadeIn();
						}

						$(form).ajaxSubmit({
							target: elementResult,
							dataType: 'json',
							success: function( data ) {
								if( elementLoader == 'button' ) {
									defButton.html( defButtonText );
								} else {
									$(form).find('.form-process').fadeOut();
								}
								if( data.alert != 'error' && elementRedirect ){
									window.location.replace( elementRedirect );
									return true;
								}
								if( elementAlert == 'inline' ) {
									if( data.alert == 'error' ) {
										var alertType = 'alert-danger';
									} else {
										var alertType = 'alert-success';
									}

									elementResult.removeClass( 'alert-danger alert-success' ).addClass( 'alert ' + alertType ).html( data.message ).slideDown( 400 );
								} else {
									elementResult.attr( 'data-notify-type', data.alert ).attr( 'data-notify-msg', data.message ).html('');
									SEMICOLON.widget.notifications( elementResult );
								}
								if( $(form).find('.g-recaptcha').children('div').length > 0 ) { grecaptcha.reset(); }
								if( data.alert != 'error' ) { $(form).clearForm(); }
							}
						});
					}
				});

			});
		},

		subscription: function(){

			if( !$().validate ) {
				console.log('subscription: Form Validate not Defined.');
				return true;
			}

			if( !$().ajaxSubmit ) {
				console.log('subscription: jQuery Form not Defined.');
				return true;
			}

			var $subscribeForm = $('.subscribe-widget:not(.customjs)');
			if( $subscribeForm.length < 1 ){ return true; }

			$subscribeForm.each( function(){
				var element = $(this),
					elementAlert = element.attr('data-alert-type'),
					elementLoader = element.attr('data-loader'),
					elementResult = element.find('.widget-subscribe-form-result'),
					elementRedirect = element.attr('data-redirect');

				element.find('form').validate({
					submitHandler: function(form) {

						elementResult.hide();

						if( elementLoader == 'button' ) {
							var defButton = $(form).find('button'),
								defButtonText = defButton.html();

							defButton.html('<i class="icon-line-loader icon-spin nomargin"></i>');
						} else {
							$(form).find('.input-group-addon').find('.icon-email2').removeClass('icon-email2').addClass('icon-line-loader icon-spin');
						}

						$(form).ajaxSubmit({
							target: elementResult,
							dataType: 'json',
							resetForm: true,
							success: function( data ) {
								if( elementLoader == 'button' ) {
									defButton.html( defButtonText );
								} else {
									$(form).find('.input-group-addon').find('.icon-line-loader').removeClass('icon-line-loader icon-spin').addClass('icon-email2');
								}
								if( data.alert != 'error' && elementRedirect ){
									window.location.replace( elementRedirect );
									return true;
								}
								if( elementAlert == 'inline' ) {
									if( data.alert == 'error' ) {
										var alertType = 'alert-danger';
									} else {
										var alertType = 'alert-success';
									}

									elementResult.addClass( 'alert ' + alertType ).html( data.message ).slideDown( 400 );
								} else {
									elementResult.attr( 'data-notify-type', data.alert ).attr( 'data-notify-msg', data.message ).html('');
									SEMICOLON.widget.notifications( elementResult );
								}
							}
						});
					}
				});

			});
		},

		quickContact: function(){

			if( !$().validate ) {
				console.log('quickContact: Form Validate not Defined.');
				return true;
			}

			if( !$().ajaxSubmit ) {
				console.log('quickContact: jQuery Form not Defined.');
				return true;
			}

			var $quickContact = $('.quick-contact-widget:not(.customjs)');
			if( $quickContact.length < 1 ){ return true; }

			$quickContact.each( function(){
				var element = $(this),
					elementAlert = element.attr('data-alert-type'),
					elementLoader = element.attr('data-loader'),
					elementResult = element.find('.quick-contact-form-result'),
					elementRedirect = element.attr('data-redirect');

				element.find('form').validate({
					submitHandler: function(form) {

						elementResult.hide();
						$(form).animate({ opacity: 0.4 });

						if( elementLoader == 'button' ) {
							var defButton = $(form).find('button'),
								defButtonText = defButton.html();

							defButton.html('<i class="icon-line-loader icon-spin nomargin"></i>');
						} else {
							$(form).find('.form-process').fadeIn();
						}

						$(form).ajaxSubmit({
							target: elementResult,
							dataType: 'json',
							resetForm: true,
							success: function( data ) {
								$(form).animate({ opacity: 1 });
								if( elementLoader == 'button' ) {
									defButton.html( defButtonText );
								} else {
									$(form).find('.form-process').fadeOut();
								}
								if( data.alert != 'error' && elementRedirect ){
									window.location.replace( elementRedirect );
									return true;
								}
								if( elementAlert == 'inline' ) {
									if( data.alert == 'error' ) {
										var alertType = 'alert-danger';
									} else {
										var alertType = 'alert-success';
									}

									elementResult.addClass( 'alert ' + alertType ).html( data.message ).slideDown( 400 );
								} else {
									elementResult.attr( 'data-notify-type', data.alert ).attr( 'data-notify-msg', data.message ).html('');
									SEMICOLON.widget.notifications( elementResult );
								}
								if( $(form).find('.g-recaptcha').children('div').length > 0 ) { grecaptcha.reset(); }
							}
						});
					}
				});

			});
		},

		stickySidebar: function(){

			if( !$().scwStickySidebar ) {
				console.log('stickySidebar: Sticky Sidebar is not Defined.');
				return true;
			}

			var $stickySidebar = jQuery('.sticky-sidebar-wrap');
			if( $stickySidebar.length < 1 ){ return true; }

			$stickySidebar.each( function(){
				var element = $(this),
					elementTop = element.attr('data-offset-top'),
					elementBottom = element.attr('data-offset-bottom');

				if( !elementTop ) { elementTop = 110; }
				if( !elementBottom ) { elementBottom = 50; }

				element.scwStickySidebar({
					additionalMarginTop: Number( elementTop ),
					additionalMarginBottom: Number( elementBottom )
				});
			});
		},

		cookieNotify: function(){

			if( !$.cookie ) {
				console.log('cookieNotify: Cookie Function not defined.');
				return true;
			}

			if( $cookieNotification.length > 0 ) {
				var cookieNotificationHeight = $cookieNotification.outerHeight();

				$cookieNotification.css({ bottom: -cookieNotificationHeight });

				if( $.cookie('websiteUsesCookies') != 'yesConfirmed' ) {
					$cookieNotification.css({ bottom: 0 });
				}

				$('.cookie-accept').click( function(){
					$cookieNotification.css({ bottom: -cookieNotificationHeight });
					$.cookie('websiteUsesCookies', 'yesConfirmed', { expires: 30 });
					return false;
				});
			}
		},

		extras: function(){

			if( $().tooltip ) {
				$('[data-toggle="tooltip"]').tooltip({container: 'body'});
			} else {
				console.log('extras: Bootstrap Tooltip not defined.');
			}

			if( $().popover ) {
				$('[data-toggle=popover]').popover();
			} else {
				console.log('extras: Bootstrap Popover not defined.');
			}

			$('.style-msg').on( 'click', '.close', function(e){
				$( this ).parents( '.style-msg' ).slideUp();
				e.preventDefault();
			});

			$('#primary-menu-trigger,#overlay-menu-close').click(function() {
				if( $('#primary-menu').find('ul.mobile-primary-menu').length > 0 ) {
					$( '#primary-menu > ul.mobile-primary-menu, #primary-menu > div > ul.mobile-primary-menu' ).toggleClass("show");
				} else {
					$( '#primary-menu > ul, #primary-menu > div > ul' ).toggleClass("show");
				}
				$body.toggleClass("primary-menu-open");
				return false;
			});
			$('#page-submenu-trigger').click(function() {
				$body.toggleClass('top-search-open', false);
				$pagemenu.toggleClass("pagemenu-active");
				return false;
			});
			$pagemenu.find('nav').click(function(e){
				$body.toggleClass('top-search-open', false);
				$topCart.toggleClass('top-cart-open', false);
			});
			if( SEMICOLON.isMobile.any() ){
				$body.addClass('device-touch');
			}
			// var el = {
			//     darkLogo : $("<img>", {src: defaultDarkLogo}),
			//     darkRetinaLogo : $("<img>", {src: retinaDarkLogo})
			// };
			// el.darkLogo.prependTo("body");
			// el.darkRetinaLogo.prependTo("body");
			// el.darkLogo.css({'position':'absolute','z-index':'-100'});
			// el.darkRetinaLogo.css({'position':'absolute','z-index':'-100'});
		}

	};

	SEMICOLON.isMobile = {
		Android: function() {
			return navigator.userAgent.match(/Android/i);
		},
		BlackBerry: function() {
			return navigator.userAgent.match(/BlackBerry/i);
		},
		iOS: function() {
			return navigator.userAgent.match(/iPhone|iPad|iPod/i);
		},
		Opera: function() {
			return navigator.userAgent.match(/Opera Mini/i);
		},
		Windows: function() {
			return navigator.userAgent.match(/IEMobile/i);
		},
		any: function() {
			return (SEMICOLON.isMobile.Android() || SEMICOLON.isMobile.BlackBerry() || SEMICOLON.isMobile.iOS() || SEMICOLON.isMobile.Opera() || SEMICOLON.isMobile.Windows());
		}
	};

	SEMICOLON.documentOnResize = {

		init: function(){

			var t = setTimeout( function(){
				SEMICOLON.header.topsocial();
				SEMICOLON.header.fullWidthMenu();
				SEMICOLON.header.overlayMenu();
				SEMICOLON.initialize.fullScreen();
				SEMICOLON.initialize.verticalMiddle();
				SEMICOLON.initialize.maxHeight();
				SEMICOLON.initialize.testimonialsGrid();
				SEMICOLON.initialize.stickyFooter();
				SEMICOLON.slider.sliderParallaxDimensions();
				SEMICOLON.slider.captionPosition();
				SEMICOLON.portfolio.arrange();
				SEMICOLON.portfolio.portfolioDescMargin();
				SEMICOLON.widget.tabsResponsiveResize();
				SEMICOLON.widget.tabsJustify();
				SEMICOLON.widget.html5Video();
				SEMICOLON.widget.masonryThumbs();
				SEMICOLON.initialize.dataResponsiveClasses();
				SEMICOLON.initialize.dataResponsiveHeights();
				if( $gridContainer.length > 0 ) {
					if( !$gridContainer.hasClass('.customjs') ) {
						if( $().isotope ) {
							$gridContainer.isotope('layout');
						} else {
							console.log('documentOnResize > init: Isotope not defined.');
						}
					}
				}
				if( $body.hasClass('device-lg') || $body.hasClass('device-md') ) {
					$('#primary-menu').find('ul.mobile-primary-menu').removeClass('show');
				}
			}, 500 );

			windowWidth = $window.width();

		}

	};

	SEMICOLON.documentOnReady = {

		init: function(){
			SEMICOLON.initialize.init();
			SEMICOLON.header.init();
			if( $slider.length > 0 ) { SEMICOLON.slider.init(); }
			if( $portfolio.length > 0 ) { SEMICOLON.portfolio.init(); }
			SEMICOLON.widget.init();
			SEMICOLON.documentOnReady.windowscroll();
		},

		windowscroll: function(){

			var headerOffset = 0,
				headerWrapOffset = 0,
				pageMenuOffset = 0;

			if( $header.length > 0 ) { headerOffset = $header.offset().top; }
			if( $header.length > 0 ) { headerWrapOffset = $headerWrap.offset().top; }
			if( $pagemenu.length > 0 ) {
				if( $header.length > 0 && !$header.hasClass('no-sticky') ) {
					if( $header.hasClass('sticky-style-2') || $header.hasClass('sticky-style-3') ) {
						pageMenuOffset = $pagemenu.offset().top - $headerWrap.outerHeight();
					} else {
						pageMenuOffset = $pagemenu.offset().top - $header.outerHeight();
					}
				} else {
					pageMenuOffset = $pagemenu.offset().top;
				}
			}

			var headerDefinedOffset = $header.attr('data-sticky-offset');
			if( typeof headerDefinedOffset !== 'undefined' ) {
				if( headerDefinedOffset == 'full' ) {
					headerWrapOffset = $window.height();
					var headerOffsetNegative = $header.attr('data-sticky-offset-negative');
					if( typeof headerOffsetNegative !== 'undefined' ) { headerWrapOffset = headerWrapOffset - headerOffsetNegative - 1; }
				} else {
					headerWrapOffset = Number(headerDefinedOffset);
				}
			}

			SEMICOLON.header.stickyMenu( headerWrapOffset );
			SEMICOLON.header.stickyPageMenu( pageMenuOffset );

			$window.on( 'scroll', function(){

				SEMICOLON.initialize.goToTopScroll();
				$('body.open-header.close-header-on-scroll').removeClass("side-header-open");
				SEMICOLON.header.stickyMenu( headerWrapOffset );
				SEMICOLON.header.stickyPageMenu( pageMenuOffset );
				SEMICOLON.header.logo();

			});

			window.addEventListener('scroll', onScrollSliderParallax, false);

			if( $onePageMenuEl.length > 0 ){
				if( $().scrolled ) {
					$window.scrolled(function() {
						SEMICOLON.header.onepageScroller();
					});
				} else {
					console.log('windowscroll: Scrolled Function not defined.');
				}
			}
		}

	};

	SEMICOLON.documentOnLoad = {

		init: function(){
			SEMICOLON.slider.captionPosition();
			SEMICOLON.slider.swiperSliderMenu(true);
			SEMICOLON.slider.revolutionSliderMenu(true);
			SEMICOLON.initialize.maxHeight();
			SEMICOLON.initialize.testimonialsGrid();
			SEMICOLON.initialize.verticalMiddle();
			SEMICOLON.initialize.stickFooterOnSmall();
			SEMICOLON.initialize.stickyFooter();
			SEMICOLON.portfolio.gridInit( $gridContainer );
			SEMICOLON.portfolio.filterInit();
			SEMICOLON.portfolio.shuffleInit();
			SEMICOLON.portfolio.arrange();
			SEMICOLON.portfolio.portfolioDescMargin();
			SEMICOLON.widget.parallax();
			SEMICOLON.widget.loadFlexSlider();
			SEMICOLON.widget.html5Video();
			SEMICOLON.widget.masonryThumbs();
			SEMICOLON.header.topsocial();
			SEMICOLON.header.responsiveMenuClass();
			SEMICOLON.initialize.modal();
		}

	};

	var $window = $(window),
		$body = $('body'),
		$wrapper = $('#wrapper'),
		$header = $('#header'),
		$headerWrap = $('#header-wrap'),
		$content = $('#content'),
		$footer = $('#footer'),
		windowWidth = $window.width(),
		oldHeaderClasses = $header.attr('class'),
		oldHeaderWrapClasses = $headerWrap.attr('class'),
		stickyMenuClasses = $header.attr('data-sticky-class'),
		responsiveMenuClasses = $header.attr('data-responsive-class'),
		defaultLogo = $('#logo').find('.standard-logo'),
		defaultLogoWidth = defaultLogo.find('img').outerWidth(),
		retinaLogo = $('#logo').find('.retina-logo'),
		defaultLogoImg = defaultLogo.find('img').attr('src'),
		retinaLogoImg = retinaLogo.find('img').attr('src'),
		defaultDarkLogo = defaultLogo.attr('data-dark-logo'),
		retinaDarkLogo = retinaLogo.attr('data-dark-logo'),
		defaultStickyLogo = defaultLogo.attr('data-sticky-logo'),
		retinaStickyLogo = retinaLogo.attr('data-sticky-logo'),
		defaultMobileLogo = defaultLogo.attr('data-mobile-logo'),
		retinaMobileLogo = retinaLogo.attr('data-mobile-logo'),
		$pagemenu = $('#page-menu'),
		$onePageMenuEl = $('.one-page-menu'),
		onePageGlobalOffset = 0,
		$portfolio = $('.portfolio'),
		$shop = $('.shop'),
		$gridContainer = $('.grid-container'),
		$slider = $('#slider'),
		$sliderParallaxEl = $('.slider-parallax'),
		swiperSlider = '',
		$pageTitle = $('#page-title'),
		$portfolioItems = $('.portfolio-ajax').find('.portfolio-item'),
		$portfolioDetails = $('#portfolio-ajax-wrap'),
		$portfolioDetailsContainer = $('#portfolio-ajax-container'),
		$portfolioAjaxLoader = $('#portfolio-ajax-loader'),
		$portfolioFilter = $('.portfolio-filter,.custom-filter'),
		prevPostPortId = '',
		$topSearch = $('#top-search'),
		$topCart = $('#top-cart'),
		$verticalMiddleEl = $('.vertical-middle'),
		$topSocialEl = $('#top-social').find('li'),
		$siStickyEl = $('.si-sticky'),
		$dotsMenuEl = $('.dots-menu'),
		$goToTopEl = $('#gotoTop'),
		$fullScreenEl = $('.full-screen'),
		$commonHeightEl = $('.common-height'),
		$testimonialsGridEl = $('.testimonials-grid'),
		$pageSectionEl = $('.page-section'),
		$owlCarouselEl = $('.owl-carousel'),
		$parallaxEl = $('.parallax'),
		$parallaxPageTitleEl = $('.page-title-parallax'),
		$parallaxPortfolioEl = $('.portfolio-parallax').find('.portfolio-image'),
		$textRotaterEl = $('.text-rotater'),
		$cookieNotification = $('#cookie-notification');

	$(document).ready( SEMICOLON.documentOnReady.init );
	$window.load( SEMICOLON.documentOnLoad.init );
	$window.on( 'resize', SEMICOLON.documentOnResize.init );

})(jQuery);
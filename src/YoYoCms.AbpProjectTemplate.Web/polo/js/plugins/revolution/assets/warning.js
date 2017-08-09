jQuery(document).ready(function() {
	
    try{
        if (jQuery("svg").length > 0 || jQuery('.tp-caption[data-type="svg"]').length || jQuery(".rs-background-video-layer, .tp-videolayer").length > 0 || jQuery("iframe").length > 0) {
			
            if ("file://" === location.origin) {
                jQuery('body').append('<div class="localwarning"><div class="localwarningimage"></div><a target="_blank" href="https://codecanyon.net/item/slider-revolution-jquery-visual-editor-addon/13934907" class="localwarningadvert"></a><div class="localwarningclose"><i class="fa-icon-close"></i></div></div>');
                jQuery('head').append('<link rel="stylesheet" type="text/css" href="../../revolution/fonts/font-awesome/css/font-awesome.css">');
                var a = new punchgs.TimelineLite({
                        paused: !0
                    }),
                    b = jQuery(".localwarning"),
                    c = jQuery(".localwarningimage"),
                    d = jQuery(".localwarningclose"),
                    e = jQuery(".localwarningadvert");
                a.add(punchgs.TweenLite.fromTo(b, .5, {
                    y: 50,
                    autoAlpha: 0
                }, {
                    y: 0,
                    autoAlpha: 1,
                    ease: punchgs.Power3.easeInOut
                }), 1), a.add(punchgs.TweenLite.fromTo(c, .5, {
                    y: 50,
                    autoAlpha: 0
                }, {
                    y: 0,
                    autoAlpha: 1,
                    ease: punchgs.Power3.easeInOut
                }), 1.3), a.add(punchgs.TweenLite.fromTo(e, .5, {
                    y: 50,
                    autoAlpha: 0
                }, {
                    y: 0,
                    autoAlpha: 1,
                    ease: punchgs.Power3.easeInOut
                }), 1.4), a.add(punchgs.TweenLite.fromTo(d, .5, {
                    x: 50,
                    autoAlpha: 0
                }, {
                    x: 0,
                    autoAlpha: 1,
                    ease: punchgs.Power3.easeInOut
                }), 1.9), a.play(0), d.on("mouseover", function() {
                    punchgs.TweenLite.to(d, .3, {
                        rotation: 180,
                        ease: punchgs.Power3.easeOut
                    })
                }), d.on("mouseleave", function() {
                    punchgs.TweenLite.to(d, .3, {
                        rotation: 0,
                        ease: punchgs.Power3.easeOut
                    })
                }), jQuery("body").on("click", ".localwarning, .localwarningclose", function() {
                    a.reverse()
                })
            }
        } 
    } catch(e) {

    }
})

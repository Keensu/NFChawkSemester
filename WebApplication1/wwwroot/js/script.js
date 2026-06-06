(function ($) {

	"use strict";

	// button animation
	$(function () {
		$('.btn-1')
			.on('mouseenter', function (e) {
				var parentOffset = $(this).offset(),
					relX = e.pageX - parentOffset.left,
					relY = e.pageY - parentOffset.top;
				$(this).find('span').css({ top: relY, left: relX })
			})
			.on('mouseout', function (e) {
				var parentOffset = $(this).offset(),
					relX = e.pageX - parentOffset.left,
					relY = e.pageY - parentOffset.top;
				$(this).find('span').css({ top: relY, left: relX })
			});
	});
	// button animation

	// swiper slider
	function thmSwiperInit() {
		if ($(".thm-swiper__slider").length) {
			$(".thm-swiper__slider").each(function () {
				let elm = $(this);
				let options = elm.data('swiper-options');
				let thmSwiperSlider = new Swiper(elm, options);
			});
		}
	}

	function thmOwlInit() {
		// owl slider

		if ($(".thm-owl__carousel").length) {
			$(".thm-owl__carousel").each(function () {
				let elm = $(this);
				let options = elm.data('owl-options');
				let thmOwlCarousel = elm.owlCarousel(options);
			});
		}

		if ($(".thm-owl__carousel--custom-nav").length) {
			$(".thm-owl__carousel--custom-nav").each(function () {
				let elm = $(this);
				let owlNavPrev = elm.data('owl-nav-prev');
				let owlNavNext = elm.data('owl-nav-next');
				$(owlNavPrev).on("click", function (e) {
					elm.trigger('prev.owl.carousel');
					e.preventDefault();
				})

				$(owlNavNext).on("click", function (e) {
					elm.trigger('next.owl.carousel');
					e.preventDefault();
				})
			});
		}

	}


	// Nice Select
	$(document).ready(function () {
		$('select:not(.ignore)').niceSelect();

	});



	// curved-circle
	$(window).scroll(function () {
		var theta = $(window).scrollTop() / 15;
		$(".about-one__left-content .round-box-content .curved-circle").css({ transform: "rotate(" + theta + "deg)" });
	});
	// curved-circle


	// Industries Served Tab
	if ($('.quote-tab').length) {
		$('.quote-tab .tabs-button-box .tab-btn-Item').on('click', function (e) {
			e.preventDefault();
			var target = $($(this).attr('data-tab'));

			if ($(target).hasClass('actve-tab')) {
				return false;
			} else {
				$('.quote-tab .tabs-button-box .tab-btn-Item').removeClass('active-btn-Item');
				$(this).addClass('active-btn-Item');
				$('.quote-tab .tabs-content-box .tab-content-box-Item').removeClass('tab-content-box-Item-active');
				$(target).addClass('tab-content-box-Item-active');
			}
		});
	}


	// ISOTOP STARTS 
	var $grid = $('.filter-row').isotope({
		ItemSelector: '.filter-Item',
		percentPosition: true,
		masonry: {
			// use outer width of grid-sizer for columnWidth
			columnWidth: 1
		}
	})

	// filter Items on button click
	$('.filter-button-group').on('click', 'button', function () {
		var filterValue = $(this).attr('data-filter');
		$grid.isotope({ filter: filterValue });
	});

	// menu active class (it can use any menu active with class active and css color !!!!!)
	$('.filter-button-group button').on('click', function (event) {
		$(this).siblings('.active').removeClass('active');
		$(this).addClass('active');
		event.preventDefault();
	});
	// ISOTOP ENDS 


	// LOAD MORE STARTS
	$('.Item-list').slice(0, 6).show();

	$('.load-more').click(function () {
		$('.Item-list:hidden').slice(0, 3).slideDown(300);

		// hide btn after fully loaded
		if ($('.Item-list:hidden').length == 0) {
			$(this).fadeOut(300);
		}
	});
	// LOAD MORE ENDS

	// LOAD MORE 2 STARTS
	$('.author-list').slice(0, 8).show();

	$('.load-more').click(function () {
		$('.author-list:hidden').slice(0, 4).slideDown(300);

		// hide btn after fully loaded
		if ($('.author-list:hidden').length == 0) {
			$(this).fadeOut(300);
		}
	});
	// LOAD MORE 2 ENDS

	// LOAD MORE 3 STARTS
	$('.activity-list').slice(0, 8).show();

	$('.load-more').click(function () {
		$('.activity-list:hidden').slice(0, 8).slideDown(300);

		// hide btn after fully loaded
		if ($('.activity-list:hidden').length == 0) {
			$(this).fadeOut(300);
		}
	});
	// LOAD MORE 3 ENDS



	// Scroll top button
	$('.scroll-top-inner').on("click", function () {
		$('html, body').animate({ scrollTop: 0 }, 500);
		return false;
	});

	function handleScrollbar() {
		const bHeight = $('body').height();
		const scrolled = $(window).innerHeight() + $(window).scrollTop();

		let percentage = ((scrolled / bHeight) * 100);

		$('.scroll-top-inner .bar-inner').css('width', percentage + '%');
	}





	//  Progress Bar
	if ($(".count-bar").length) {
		$(".count-bar").appear(
			function () {
				var el = $(this);
				var percent = el.data("percent");
				$(el).css("width", percent).addClass("counted");
			}, {
			accY: 150
		}
		);
	}

	//Progress Bar / Levels
	if ($(".progress-levels .progress-box .bar-fill").length) {
		$(".progress-box .bar-fill").each(
			function () {
				$(".progress-box .bar-fill").appear(function () {
					var progressWidth = $(this).attr("data-percent");
					$(this).css("width", progressWidth + "%");
				});
			}, {
			accY: 0
		}
		);
	}


	//Fact Counter + Text Count
	if ($(".count-box").length) {
		$(".count-box").appear(
			function () {
				var $t = $(this),
					n = $t.find(".count-text").attr("data-stop"),
					r = parseInt($t.find(".count-text").attr("data-speed"), 10);

				if (!$t.hasClass("counted")) {
					$t.addClass("counted");
					$({
						countNum: $t.find(".count-text").text()
					}).animate({
						countNum: n
					}, {
						duration: r,
						easing: "linear",
						step: function () {
							$t.find(".count-text").text(Math.floor(this.countNum));
						},
						complete: function () {
							$t.find(".count-text").text(this.countNum);
						}
					});
				}
			}, {
			accY: 0
		}
		);
	}




	// ===Portfolio===
	function projectMasonaryLayout() {
		if ($(".masonary-layout").length) {
			$(".masonary-layout").isotope({
				layoutMode: "masonry"
			});
		}
		if ($(".post-filter").length) {
			$(".post-filter li")
				.children(".filter-text")
				.on("click", function () {
					var Self = $(this);
					var selector = Self.parent().attr("data-filter");
					$(".post-filter li").removeClass("active");
					Self.parent().addClass("active");
					$(".filter-layout").isotope({
						filter: selector,
						animationOptions: {
							duration: 500,
							easing: "linear",
							queue: false
						}
					});
					return false;
				});
		}

		if ($(".post-filter.has-dynamic-filters-counter").length) {
			// var allItem = $('.single-filter-Item').length;
			var activeFilterItem = $(".post-filter.has-dynamic-filters-counter").find(
				"li"
			);
			activeFilterItem.each(function () {
				var filterElement = $(this).data("filter");
				var count = $(".filter-layout").find(filterElement).length;
				$(this)
					.children(".filter-text")
					.append('<span class="count">' + count + "</span>");
			});
		}
	}




	if ($(".img-popup").length) {
		var groups = {};
		$(".img-popup").each(function () {
			var id = parseInt($(this).attr("data-group"), 10);

			if (!groups[id]) {
				groups[id] = [];
			}

			groups[id].push(this);
		});

		$.each(groups, function () {
			$(this).magnificPopup({
				type: "image",
				closeOnContentClick: true,
				closeBtnInside: false,
				gallery: {
					enabled: true
				}
			});
		});
	}


	if ($(".project-three__list li").length) {
		$(".project-three__list li").each(function () {
			let self = $(this);

			self.on("mouseenter", function () {
				console.log($(this));
				$(".project-three__list li").removeClass("active");
				$(this).addClass("active");
			});
		});
	}




	// ===Project One Swiper Carousel===
	if ($(".project-one__swiper-carousel").length > 0) {
		var totalSlides2 = $(".project-one__swiper-carousel .swiper-slide").length;
		var gridCarusel = new Swiper(".project-one__swiper-carousel", {
			preloadImages: false,
			loop: true,
			freeMode: false,
			slidesPerView: 1,
			spaceBetween: 30,
			grabCursor: true,
			mousewheel: false,
			speed: 500,
			effect: "slide",
			autoplay: {
				delay: 3000,
				disableOnInteraction: false
			},
			pagination: {
				el: '.swiper-pagination',
				type: 'progressbar',
			},
			navigation: {
				nextEl: '.project-one__nav-next',
				prevEl: '.project-one__nav-prev',
			},
			breakpoints: {
				1600: {
					slidesPerView: 4,
				},
				1200: {
					slidesPerView: 3,
				},
				992: {
					slidesPerView: 3,
				},
				768: {
					slidesPerView: 2,
				},

			}
		});

		gridCarusel.on('slideChange', function () {
			var csli = gridCarusel.realIndex + 1,
				curnum = $('#current2');
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: -10,
				opacity: 0,
				ease: Power2.easeOut,
				onComplete: function () {
					TweenMax.to(curnum, 0.1, {
						force3D: true,
						y: 10
					});
					curnum.html('0' + csli);
				}
			});
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: 0,
				delay: 0.3,
				opacity: 1,
				ease: Power2.easeOut
			});
		});

		var totalSlides2 = gridCarusel.slides.length - 6;
		$('#total2').html(totalSlides2);
	}



	// Banner Slider Two //Home Two
	if ($(".slider-two-style2").length > 0) {
		var bannerSlider = new Swiper('.slider-two-style2', {
			spaceBetween: 0,
			slidesPerView: 1,
			mousewheel: false,
			height: 500,
			grabCursor: true,
			loop: true,
			speed: 1400,
			autoplay: {
				delay: 10000,
			},
			pagination: {
				el: '.swiper-pagination',
				type: 'progressbar',
			},
			navigation: {
				prevEl: '.slider-two-button-prev',
				nextEl: '.slider-two-button-next',
			},
		});
		bannerSlider.on('slideChange', function () {
			var csli = bannerSlider.realIndex + 1,
				curnum = $('#slider-two-current');
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: -10,
				opacity: 0,
				ease: Power2.easeOut,
				onComplete: function () {
					TweenMax.to(curnum, 0.1, {
						force3D: true,
						y: 10
					});
					curnum.html('0' + csli);
				}
			});
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: 0,
				delay: 0.3,
				opacity: 1,
				ease: Power2.easeOut
			});
		});

		var totalSlides = bannerSlider.slides.length - 2;
		$('#slider-two-total').html('0' + totalSlides);
	}



	// Banner Slider Three //Home Three
	if ($(".slider-three-style3").length > 0) {
		var bannerSlider = new Swiper('.slider-three-style3', {
			spaceBetween: 0,
			slidesPerView: 1,
			mousewheel: false,
			height: 500,
			grabCursor: true,
			loop: true,
			speed: 1400,
			autoplay: {
				delay: 10000,
			},
			pagination: {
				el: '.swiper-pagination',
				type: 'progressbar',
			},
			navigation: {
				prevEl: '.slider-three-button-prev',
				nextEl: '.slider-three-button-next',
			},
		});
		bannerSlider.on('slideChange', function () {
			var csli = bannerSlider.realIndex + 1,
				curnum = $('#slider-three-current');
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: -10,
				opacity: 0,
				ease: Power2.easeOut,
				onComplete: function () {
					TweenMax.to(curnum, 0.1, {
						force3D: true,
						y: 10
					});
					curnum.html('0' + csli);
				}
			});
			TweenMax.to(curnum, 0.2, {
				force3D: true,
				y: 0,
				delay: 0.3,
				opacity: 1,
				ease: Power2.easeOut
			});
		});

		var totalSlides = bannerSlider.slides.length - 2;
		$('#slider-three-total').html('0' + totalSlides);
	}




	function SmoothMenuScroll() {
		var anchor = $(".scrollToLink");
		if (anchor.length) {
			anchor.children("a").bind("click", function (event) {
				if ($(window).scrollTop() > 10) {
					var headerH = "90";
				} else {
					var headerH = "90";
				}
				var target = $(this);
				$("html, body")
					.stop()
					.animate({
						scrollTop: $(target.attr("href")).offset().top - headerH + "px"
					},
						1200,
						"easeInOutExpo"
					);
				anchor.removeClass("current");
				anchor.removeClass("current-menu-ancestor");
				anchor.removeClass("current_page_Item");
				anchor.removeClass("current-menu-parent");
				target.parent().addClass("current");
				event.preventDefault();
			});
		}
	}
	SmoothMenuScroll();

	function OnePageMenuScroll() {
		var windscroll = $(window).scrollTop();
		if (windscroll >= 117) {
			var menuAnchor = $(".one-page-scroll-menu .scrollToLink").children("a");
			menuAnchor.each(function () {
				var sections = $(this).attr("href");
				$(sections).each(function () {
					if ($(this).offset().top <= windscroll + 120) {
						var Sectionid = $(sections).attr("id");
						$(".one-page-scroll-menu").find("li").removeClass("current");
						$(".one-page-scroll-menu").find("li").removeClass("current-menu-ancestor");
						$(".one-page-scroll-menu").find("li").removeClass("current_page_Item");
						$(".one-page-scroll-menu").find("li").removeClass("current-menu-parent");
						$(".one-page-scroll-menu")
							.find("a[href*=\\#" + Sectionid + "]")
							.parent()
							.addClass("current");
					}
				});
			});
		} else {
			$(".one-page-scroll-menu li.current").removeClass("current");
			$(".one-page-scroll-menu li:first").addClass("current");
		}
	}







	const serviceImgItem = document.querySelectorAll(".service-block_one-inner");

	function followImageCursor(event, serviceImgItem) {
		const contentBox = serviceImgItem.getBoundingClientRect();
		const dx = event.clientX - contentBox.x;
		const dy = event.clientY - contentBox.y;
		serviceImgItem.children[4].style.transform = `translate(${dx}px, ${dy}px)`;
	}

	serviceImgItem.forEach((Item, i) => {
		Item.addEventListener("mousemove", (event) => {
			setInterval(followImageCursor(event, Item), 1000);
		});
	});




	const serviceImgItemTwo = document.querySelectorAll(".service-block_four-inner");

	function followImageCursorTwo(event, serviceImgItemTwo) {
		const contentBox = serviceImgItemTwo.getBoundingClientRect();
		const dx = event.clientX - contentBox.x;
		const dy = event.clientY - contentBox.y;
		serviceImgItemTwo.children[2].style.transform = `translate(${dx}px, ${dy}px)`;
	}

	serviceImgItemTwo.forEach((Item, i) => {
		Item.addEventListener("mousemove", (event) => {
			setInterval(followImageCursorTwo(event, Item), 1000);
		});
	});




	const partnersImgItem = document.querySelectorAll(".partners-one li");

	function followPartnersCursor(event, partnersImgItem) {
		const contentBox = partnersImgItem.getBoundingClientRect();
		const dx = event.clientX - contentBox.x;
		const dy = event.clientY - contentBox.y;
		//partnersImgItem.children[0].style.mix-blend-mode = 'difference';
		partnersImgItem.children[1].style.transform = `translate(${dx}px, ${dy}px)`;

	}

	partnersImgItem.forEach((Item, i) => {
		Item.addEventListener("mousemove", (event) => {
			setInterval(followPartnersCursor(event, Item), 1000);
		});
	});




	const portfolio_listss = gsap.utils.toArray(".project-detail_image img")
	if (portfolio_listss) {
		portfolio_listss.forEach((Item, i) => {
			gsap.from(Item, {
				scrollTrigger: {
					trigger: Item,
					start: "top center",
					scrub: 1.2,
				},
				scale: 2.0,
				duration: 1,
			})
		})
	}



	//Hide Loading Box (Preloader)
	function handlePreloader() {
		if ($('.preloader').length) {
			$('.preloader').delay(200).fadeOut(500);
		}
	}



	//  Animation Fade Left End

	/////////////////////////////////////////////////////
	// CURSOR
	var cursor = $(".cursor"),
		follower = $(".cursor-follower");

	var posX = 0,
		posY = 0;

	var mouseX = 0,
		mouseY = 0;

	TweenMax.to({}, 0.016, {
		repeat: -1,
		onRepeat: function () {
			posX += (mouseX - posX) / 9;
			posY += (mouseY - posY) / 9;

			TweenMax.set(follower, {
				css: {
					left: posX - 12,
					top: posY - 12
				}
			});

			TweenMax.set(cursor, {
				css: {
					left: mouseX,
					top: mouseY
				}
			});
		}
	});

	$(document).on("mousemove", function (e) {
		mouseX = e.clientX;
		mouseY = e.clientY;
	});
	//circle
	$(".theme-btn, a").on("mouseenter", function () {
		cursor.addClass("active");
		follower.addClass("active");
	});
	$(".theme-btn, a").on("mouseleave", function () {
		cursor.removeClass("active");
		follower.removeClass("active");
	});
	// CURSOR End



	// Style 4 Zoom out

	let splitTitleLines = gsap.utils.toArray(".title-anim");

	splitTitleLines.forEach(splitTextLine => {
		const tl = gsap.timeline({
			scrollTrigger: {
				trigger: splitTextLine,
				start: 'top 90%',
				end: 'bottom 60%',
				scrub: false,
				markers: false,
				toggleActions: 'play none none none'
			}
		});

		const ItemSplitted = new SplitText(splitTextLine, {
			type: "words, lines"
		});
		gsap.set(splitTextLine, {
			perspective: 400
		});
		ItemSplitted.split({
			type: "lines"
		})
		tl.from(ItemSplitted.lines, {
			duration: 1,
			delay: 0.3,
			opacity: 0,
			rotationX: -80,
			force3D: true,
			transformOrigin: "top center -50",
			stagger: 0.1
		});
	});



	//Update Header Style and Scroll to Top
	function headerStyle() {
		if ($('.main-header').length) {
			var windowpos = $(window).scrollTop();
			var siteHeader = $('.main-header');
			var scrollLink = $('.scroll-top');
			if (windowpos >= 110) {
				siteHeader.addClass('fixed-header');
				scrollLink.addClass('open');
			} else {
				siteHeader.removeClass('fixed-header');
				scrollLink.removeClass('open');
			}
		}
	}

	headerStyle();




	//Submenu Dropdown Toggle
	if ($('.main-header li.dropdown ul').length) {
		$('.main-header li.dropdown').append('<div class="dropdown-btn"><span class="far fa-angle-down fa-fw"></span></div>');

		//Dropdown Button
		$('.main-header li.dropdown .dropdown-btn').on('click', function () {
			$(this).prev('ul').slideToggle(500);
		});

		//Disable dropdown parent link
		$('.navigation li.dropdown > a').on('click', function (e) {
			e.preventDefault();
		});


		$('.hamburger').on('click', function (e) {
			$('.about-sidebar').addClass('active');
		});

		$('.about-sidebar .close-button').on('click', function (e) {
			$('.about-sidebar').removeClass('active');
		});

		$('.about-sidebar .gradient-layer').on('click', function (e) {
			$('.about-sidebar').removeClass('active');
		});

		$('.xs-sidebar-group .close-button').on('click', function (e) {
			$('.xs-sidebar-group.info-group').removeClass('isActive');
		});

	}


	// Add Current Class Auto
	function dynamicCurrentMenuClass(selector) {
		let FileName = window.location.href.split("/").reverse()[0];

		selector.find("li").each(function () {
			let anchor = $(this).find("a");
			if ($(anchor).attr("href") == FileName) {
				$(this).addClass("current");
			}
		});
		// if any li has .current elmnt add class
		selector.children("li").each(function () {
			if ($(this).find(".current").length) {
				$(this).addClass("current");
			}
		});
		// if no file name return
		if ("" == FileName) {
			selector.find("li").eq(0).addClass("current");
		}
	}

	if ($('.main-header .header-lower .main-menu .navigation').length) {
		dynamicCurrentMenuClass($('.main-header .header-lower .main-menu .navigation'));
	}



	//Header Search
	if ($('.header-upper__search').length) {
		$('.header-upper__search').on('click', function () {
			$('body').addClass('search-active');
		});
		$('.close-search').on('click', function () {
			$('body').removeClass('search-active');
		});

		$('.search-popup .color-layer').on('click', function () {
			$('body').removeClass('search-active');
		});
	}



	// Mobile Nav Hide Show
	if ($('.mobile-menu').length) {

		//$('.mobile-menu .menu-box').mCustomScrollbar();

		var mobileMenuContent = $('.main-header .nav-outer .main-menu').html();
		$('.mobile-menu .menu-box .menu-outer').append(mobileMenuContent);
		$('.sticky-header .main-menu').append(mobileMenuContent);

		//Hide / Show Submenu
		$('.mobile-menu .navigation > li.dropdown > .dropdown-btn').on('click', function (e) {
			e.preventDefault();
			var target = $(this).parent('li').children('ul');

			if ($(target).is(':visible')) {
				$(this).parent('li').removeClass('open');
				$(target).slideUp(500);
				$(this).parents('.navigation').children('li.dropdown').removeClass('open');
				$(this).parents('.navigation').children('li.dropdown > ul').slideUp(500);
				return false;
			} else {
				$(this).parents('.navigation').children('li.dropdown').removeClass('open');
				$(this).parents('.navigation').children('li.dropdown').children('ul').slideUp(500);
				$(this).parent('li').toggleClass('open');
				$(this).parent('li').children('ul').slideToggle(500);
			}
		});

		//3rd Level Nav
		$('.mobile-menu .navigation > li.dropdown > ul  > li.dropdown > .dropdown-btn').on('click', function (e) {
			e.preventDefault();
			var targetInner = $(this).parent('li').children('ul');

			if ($(targetInner).is(':visible')) {
				$(this).parent('li').removeClass('open');
				$(targetInner).slideUp(500);
				$(this).parents('.navigation > ul').find('li.dropdown').removeClass('open');
				$(this).parents('.navigation > ul').find('li.dropdown > ul').slideUp(500);
				return false;
			} else {
				$(this).parents('.navigation > ul').find('li.dropdown').removeClass('open');
				$(this).parents('.navigation > ul').find('li.dropdown > ul').slideUp(500);
				$(this).parent('li').toggleClass('open');
				$(this).parent('li').children('ul').slideToggle(500);
			}
		});

		//Menu Toggle Btn
		$('.mobile-nav-toggler').on('click', function () {
			$('body').addClass('mobile-menu-visible');

		});

		//Menu Toggle Btn
		$('.mobile-menu .menu-backdrop,.mobile-menu .close-btn').on('click', function () {
			$('body').removeClass('mobile-menu-visible');
			$('.mobile-menu .navigation > li').removeClass('open');
			$('.mobile-menu .navigation li ul').slideUp(0);
		});

		$(document).keydown(function (e) {
			if (e.keyCode == 27) {
				$('body').removeClass('mobile-menu-visible');
				$('.mobile-menu .navigation > li').removeClass('open');
				$('.mobile-menu .navigation li ul').slideUp(0);
			}
		});

	}


	// Animation gsap 
	function title_animation() {
		var tg_var = jQuery('.sec-title-animation');
		if (!tg_var.length) {
			return;
		}
		const quotes = document.querySelectorAll(".sec-title-animation .title-animation");

		quotes.forEach(quote => {

			//Reset if needed
			if (quote.animation) {
				quote.animation.progress(1).kill();
				quote.split.revert();
			}

			var getclass = quote.closest('.sec-title-animation').className;
			var animation = getclass.split('animation-');
			if (animation[1] == "style4") return

			quote.split = new SplitText(quote, {
				type: "lines,words,chars",
				linesClass: "split-line"
			});
			gsap.set(quote, { perspective: 400 });

			if (animation[1] == "style1") {
				gsap.set(quote.split.chars, {
					opacity: 0,
					y: "90%",
					rotateX: "-40deg"
				});
			}
			if (animation[1] == "style2") {
				gsap.set(quote.split.chars, {
					opacity: 0,
					x: "50"
				});
			}
			if (animation[1] == "style3") {
				gsap.set(quote.split.chars, {
					opacity: 0,
				});
			}
			quote.animation = gsap.to(quote.split.chars, {
				scrollTrigger: {
					trigger: quote,
					start: "top 90%",
				},
				x: "0",
				y: "0",
				rotateX: "0",
				opacity: 1,
				duration: 1,
				ease: Back.easeOut,
				stagger: .02
			});
		});
	}
	ScrollTrigger.addEventListener("refresh", title_animation);


	//Gallery Filters
	if ($('.filter-list').length) {
		$('.filter-list').mixItUp({});
	}



	//Parallax Scene for Icons
	if ($('.parallax-scene-1').length) {
		var scene = $('.parallax-scene-1').get(0);
		var parallaxInstance = new Parallax(scene);
	}
	if ($('.parallax-scene-2').length) {
		var scene = $('.parallax-scene-2').get(0);
		var parallaxInstance = new Parallax(scene);
	}
	if ($('.parallax-scene-3').length) {
		var scene = $('.parallax-scene-3').get(0);
		var parallaxInstance = new Parallax(scene);
	}
	if ($('.parallax-scene-4').length) {
		var scene = $('.parallax-scene-4').get(0);
		var parallaxInstance = new Parallax(scene);
	}






	// parollar
	$('[data-paroller-factor]').paroller();
	$('.paroller').paroller({
		factor: 0.1,
		factorXs: 0.3,
		// direction: 'horizontal',
		transition: 'transform 2s ease-out',
		type: 'foreground',
	});
	//  parollar






	// Button Hover Animation
	$('.get-in_touch').on('mouseenter', function (e) {
		var x = e.pageX - $(this).offset().left;
		var y = e.pageY - $(this).offset().top;

		$(this).find('span').css({
			top: y,
			left: x
		});
	});

	$('.get-in_touch').on('mouseout', function (e) {
		var x = e.pageX - $(this).offset().left;
		var y = e.pageY - $(this).offset().top;

		$(this).find('span').css({
			top: y,
			left: x
		});
	});




	// Button Hover Animation
	$('.projects_more').on('mouseenter', function (e) {
		var x = e.pageX - $(this).offset().left;
		var y = e.pageY - $(this).offset().top;

		$(this).find('span').css({
			top: y,
			left: x
		});
	});

	$('.projects_more').on('mouseout', function (e) {
		var x = e.pageX - $(this).offset().left;
		var y = e.pageY - $(this).offset().top;

		$(this).find('span').css({
			top: y,
			left: x
		});
	});



	// Trending Tabs
	if ($('.project-tab').length) {
		$('.project-tab .product-tab-btns .p-tab-btn').on('click', function (e) {
			e.preventDefault();
			var target = $($(this).attr('data-tab'));

			if ($(target).hasClass('actve-tab')) {
				return false;
			} else {
				$('.project-tab .product-tab-btns .p-tab-btn').removeClass('active-btn');
				$(this).addClass('active-btn');
				$('.project-tab .p-tabs-content .p-tab').removeClass('active-tab');
				$(target).addClass('active-tab');
			}
		});
	}



	//Accordion Box
	if ($('.accordion-box').length) {
		$(".accordion-box").on('click', '.acc-btn', function () {

			var outerBox = $(this).parents('.accordion-box');
			var target = $(this).parents('.accordion');

			if ($(this).hasClass('active') !== true) {
				$(outerBox).find('.accordion .acc-btn').removeClass('active');
			}

			if ($(this).next('.acc-content').is(':visible')) {
				return false;
			} else {
				$(this).addClass('active');
				$(outerBox).children('.accordion').removeClass('active-block');
				$(outerBox).find('.accordion').children('.acc-content').slideUp(300);
				target.addClass('active-block');
				$(this).next('.acc-content').slideDown(300);
			}
		});
	}




	// Masonary
	function enableMasonry() {
		if ($('.masonry-Items-container').length) {

			var winDow = $(window);
			// Needed variables
			var $container = $('.masonry-Items-container');

			$container.isotope({
				ItemSelector: '.masonry-Item',
				masonry: {
					columnWidth: '.masonry-Item.col-lg-6'
				},
				animationOptions: {
					duration: 500,
					easing: 'linear'
				}
			});

			winDow.bind('resize', function () {

				$container.isotope({
					ItemSelector: '.masonry-Item',
					animationOptions: {
						duration: 500,
						easing: 'linear',
						queue: false
					}
				});
			});
		}
	}

	enableMasonry();





	if ($(".odometer").length) {
		var odo = $(".odometer");
		odo.each(function () {
			$(this).appear(function () {
				var countNumber = $(this).attr("data-count");
				$(this).html(countNumber);
			});

		});
	}



	// LightBox Image
	if ($('.lightbox-image').length) {
		$('.lightbox-image').magnificPopup({
			type: 'image',
			gallery: {
				enabled: true
			}
		});
	}


	// magnifipopup videi
	$(document).ready(function () {
		$('.hv-popup-link').magnificPopup({
			disableOn: 700,
			type: 'iframe',
			mainClass: 'mfp-fade',
			removalDelay: 160,
			preloader: false,

			fixedContentPos: false
		});
	});


	//LightBox Video
	if ($('.lightbox-video').length) {
		$('.lightbox-video').magnificPopup({
			// disableOn: 700,
			type: 'iframe',
			mainClass: 'mfp-fade',
			removalDelay: 160,
			preloader: false,
			iframe: {
				patterns: {
					youtube: {
						index: 'youtube.com',
						id: 'v=',
						src: 'https://www.youtube.com/embed/%id%'
					},
				},
				srcAction: 'iframe_src',
			},
			fixedContentPos: false
		});
	}



	//Contact Form Validation
	if ($('#contact-form').length) {
		$('#contact-form').validate({
			rules: {
				username: {
					required: true
				},
				email: {
					required: true
				},
				organization: {
					required: true
				},
				job: {
					required: true
				},
				message: {
					required: true
				}
			}
		});
	}



	// Scroll to a Specific Div
	if ($('.scroll-to-target').length) {
		$(".scroll-to-target").on('click', function () {
			var target = $(this).attr('data-target');
			// animate
			$('html, body').animate({
				scrollTop: $(target).offset().top
			}, 300);

		});
	}


	/* =====================================
   When document is Scrollig, do
   ========================================= */
	$(window).on('scroll', function () {


		headerStyle();
		handleScrollbar();
		if ($(window).scrollTop() > 200) {
			$('.scroll-top-inner').addClass('visible');
		} else {
			$('.scroll-top-inner').removeClass('visible');
		};
	});




	// Elements Animation
	if ($('.wow').length) {
		var wow = new WOW({
			boxClass: 'wow', // animated element css class (default is wow)
			animateClass: 'animated', // animation css class (default is animated)
			offset: 0, // distance to the element when triggering the animation (default is 0)
			mobile: true, // trigger animations on mobile devices (default is true)
			live: true // act on asynchronously loaded content (default is true)
		});
		wow.init();
	}




	/* ==========================================================================
	   When document is Scrollig, do
	   ========================================================================== */

	$(window).on('scroll', function () {
		headerStyle();
	});

	/* ==========================================================================
	   When document is loading, do
	   ========================================================================== */

	$(window).on('load', function () {
		handlePreloader();
		enableMasonry();
		thmOwlInit();
		projectMasonaryLayout();
		OnePageMenuScroll();
		thmSwiperInit();

	});

	// === ВАЛИДАЦИЯ И ФОРМАТИРОВАНИЕ ПОЛЕЙ КАРТЫ ===
	// Только цифры + форматирование номера карты
	const cardNumber = document.getElementById('cardNumber');

	if (cardNumber) {
		cardNumber.addEventListener('input', (e) => {
			let value = e.target.value.replace(/\D/g, '');
			value = value.substring(0, 16);

			value = value.replace(/(.{4})/g, '$1 ').trim();

			e.target.value = value;
		});
	}

	// Только цифры для CVC
	const cvc = document.getElementById('cvc');

	if (cvc) {
		cvc.addEventListener('input', (e) => {
			e.target.value = e.target.value
				.replace(/\D/g, '')
				.substring(0, 3);
		});
	}

	// Плавающий слэш MM/YY
	const expiry = document.getElementById('expiry');

	if (expiry) {
		expiry.addEventListener('input', (e) => {
			let value = e.target.value.replace(/\D/g, '');
			value = value.substring(0, 4);

			if (value.length >= 3) {
				value = value.slice(0, 2) + '/' + value.slice(2);
			}

			e.target.value = value;
		});
	}
	// === ВАЛИДАЦИЯ И ФОРМАТИРОВАНИЕ ПОЛЕЙ КАРТЫ ===

	// Фильтры по NFT в личном кабинете
	const tabs = document.querySelectorAll(".tab-link");
	const contents = document.querySelectorAll(".tab-content");

	tabs.forEach(tab => {

		tab.addEventListener("click", function (e) {

			e.preventDefault();

			// убрать active у кнопок
			tabs.forEach(t => t.classList.remove("active"));

			// убрать active у контента
			contents.forEach(c => c.classList.remove("active"));

			// активная кнопка
			this.classList.add("active");

			// показать нужный блок
			const tabId = this.getAttribute("data-tab");

			document.getElementById(tabId)
				.classList.add("active");
		});
	});
	// Фильтры по NFT в личном кабинете


	// Функция для обновления превью при изменении полей
	window.updatePreview = function () {
		const nameVal = document.getElementById('Name')?.value || "NFT Title";
		const priceVal = document.getElementById('Price')?.value || "0.00";

		const titleP = document.getElementById('titlePreview');
		const priceP = document.getElementById('pricePreview');

		if (titleP) titleP.innerText = nameVal;
		if (priceP) priceP.innerText = priceVal + " ETH";
	};
	// Функция для обновления превью при изменении полей

	// Инициализация превью при загрузке страницы
	window.previewFile = function () {
		const preview = document.getElementById('imgPreview');
		const fileInput = document.getElementById('ImageFile');

		if (fileInput && fileInput.files && fileInput.files[0]) {
			const reader = new FileReader();
			reader.onload = function (e) {
				if (preview) preview.src = e.target.result;
			};
			reader.readAsDataURL(fileInput.files[0]);
		}
		window.updatePreview(); // Синхронизируем текст

	};
	// Инициализация превью при загрузке страницы

	// Функция для обновления превью коллекции
	window.updateCollectionPreview = function () {
		const nameVal = document.getElementById('ColName')?.value || "Collection Title";
		const descVal = document.getElementById('ColDesc')?.value || "Description will appear here...";

		const titleP = document.getElementById('collectionNamePreview');
		const descP = document.getElementById('collectionDescPreview');

		if (titleP) titleP.innerText = nameVal;
		if (descP) descP.innerText = descVal;
	};

	// Функция для обработки загрузки изображения баннера
	window.previewCollectionFile = function () {
		const preview = document.getElementById('collectionImgPreview');
		const fileInput = document.getElementById('BannerImage');

		if (fileInput && fileInput.files && fileInput.files[0]) {
			const reader = new FileReader();
			reader.onload = function (e) {
				if (preview) preview.src = e.target.result;
			};
			reader.readAsDataURL(fileInput.files[0]);
		}

		window.updateCollectionPreview();
	};

	// Форма обновления 
	document.addEventListener('DOMContentLoaded', function () {


		const stopPropagationElements = document.querySelectorAll('.nft-card-content form, .nft-card-content select, .nft-card-content button');

		stopPropagationElements.forEach(el => {
			el.addEventListener('click', function (e) {
				e.stopPropagation();
			});
		});


		const editForm = document.querySelector('.Items-details__right-title form');
		if (editForm) {
			editForm.style.opacity = '0';
			editForm.style.transform = 'translateY(10px)';
			setTimeout(() => {
				editForm.style.transition = 'all 0.5s ease';
				editForm.style.opacity = '1';
				editForm.style.transform = 'translateY(0)';
			}, 100);
		}
	});
	// Форма обновления
	// Валидация поля цены (неотрицательное число)
	document.addEventListener('DOMContentLoaded', function () {
		const priceInput = document.querySelector('input[name="Price"]');

		if (priceInput) {
			priceInput.addEventListener('input', function () {
				if (this.value < 0) {
					this.value = 0;
				}
			});
		}
	});
	// Валидация поля цены (неотрицательное число)

	// Обновление превью при изменении полей в форме редактирования
	window.updateEditAssetPreview = function () {

		const nameInput = document.getElementById('Name');
		const descInput = document.getElementById('Description');
		const priceInput = document.getElementById('Price');


		const titlePreview = document.getElementById('titlePreview');
		const descPreview = document.getElementById('descPreview');
		const pricePreview = document.getElementById('pricePreview');


		if (titlePreview && nameInput) {
			titlePreview.innerText = nameInput.value.trim() !== ""
				? nameInput.value
				: "NFT Title";
		}

		if (descPreview && descInput) {
			descPreview.innerText = descInput.value.trim() !== ""
				? descInput.value
				: "No description provided.";
		}


		if (pricePreview && priceInput) {
			let price = parseFloat(priceInput.value);


			if (isNaN(price) || price < 0) {
				price = 0;
			}

			pricePreview.innerText = price.toFixed(3) + " ETH";
		}
	};

	// Обновление превью при изменении полей в форме редактирования

    //Timer + SignalR для обновления данных аукциона в реальном времени
	document.addEventListener("DOMContentLoaded", () => {
		const auctionData = document.getElementById("auctionData");
		const timerElement = document.getElementById("auctionTimer");

		if (!auctionData || !timerElement) return;


		let endDate = new Date(timerElement.dataset.end);
		let timerInterval;


		function updateAuctionTimer() {
			const now = new Date();
			const distance = endDate - now;

			if (distance <= 0) {
				timerElement.innerHTML = "Auction ended";
				clearInterval(timerInterval);
				return;
			}

			const hours = Math.floor(distance / (1000 * 60 * 60));
			const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
			const seconds = Math.floor((distance % (1000 * 60)) / 1000);

			timerElement.innerHTML = `${hours}h ${minutes}m ${seconds}s`;
		}

		updateAuctionTimer();
		timerInterval = setInterval(updateAuctionTimer, 1000);

		const auctionId = auctionData.dataset.auctionId;
		const connection = new signalR.HubConnectionBuilder()
			.withUrl("/auctionHub")
			.withAutomaticReconnect()
			.build();

		connection.on("ReceiveBid", function (data) {

			const currentPrice = document.getElementById("currentPrice");
			if (currentPrice) currentPrice.innerText = `${data.currentPrice} $`;

			const topBidder = document.getElementById("topBidder");
			if (topBidder) topBidder.innerText = data.userName;


			if (data.endTime) {
				endDate = new Date(data.endTime);

				if (timerInterval) clearInterval(timerInterval);
				timerInterval = setInterval(updateAuctionTimer, 1000);
			}


			const bidsList = document.getElementById("bidsList");
			if (bidsList) {
				const safeUserName = document.createTextNode(data.userName).wholeText;
				const html = `
                <div class="auction-bid-item">
                    <div>
                        <h6>${safeUserName}</h6>
                        <p>${new Date().toLocaleTimeString()}</p>
                    </div>
                    <h5>${data.amount} $</h5>
                </div>`;
				bidsList.insertAdjacentHTML("afterbegin", html);
			}
		});

		connection.start()
			.then(() => connection.invoke("JoinAuctionGroup", parseInt(auctionId)))
			.catch(err => console.error("SignalR Error:", err));
	});
	//Timer + SignalR для обновления данных аукциона в реальном времени

    // Автоматическая отправка формы при изменении радиокнопки
	document.querySelectorAll('#filterForm input[type=radio]').forEach(radio => {
		radio.addEventListener('change', function () {
			this.form.submit();
		});
	});
	// Автоматическая отправка формы при изменении радиокнопки



}) (window.jQuery);


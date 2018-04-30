/**
 * Resize function without multiple trigger
 *
 * Usage:
 * $(window).smartresize(function(){  
 *     // code here
 * });
 */
(function ($, sr) {
	// debouncing function from John Hann
	// http://unscriptable.com/index.php/2009/03/20/debouncing-javascript-methods/
	var debounce = function (func, threshold, execAsap) {
		var timeout;

		return function debounced() {
			var obj = this, args = arguments;

			function delayed() {
				if (!execAsap)
					func.apply(obj, args);
				timeout = null;
			}

			if (timeout)
				clearTimeout(timeout);
			else if (execAsap)
				func.apply(obj, args);

			timeout = setTimeout(delayed, threshold || 100);
		};
	};

	// smartresize 
	jQuery.fn[sr] = function (fn) {
		return fn ? this.bind('resize', debounce(fn)) : this.trigger(sr);
	};

})(jQuery, 'smartresize');

var CURRENT_URL = window.location.href.split('#')[0].split('?')[0],
	$BODY = $('body'),
	$MENU_TOGGLE = $('#menu_toggle'),
	$SIDEBAR_FOOTER = $('.sidebar-footer'),
	$LEFT_COL = $('.left_col'),
	$RIGHT_COL = $('.right_col'),
	$NAV_MENU = $('.nav_menu'),
	$FOOTER = $('footer'),
	primary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-color').split(' ').join(''),
	primary_darker = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-darker').split(' ').join(''),
	primary_darkest = window.getComputedStyle(document.documentElement).getPropertyValue('--primary-darkest').split(' ').join(''),
	secondary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--secondary-color').split(' ').join('');
	tertiary_color = window.getComputedStyle(document.documentElement).getPropertyValue('--tertiary-color').split(' ').join('');
	

var randNum = function () {
	return (Math.floor(Math.random() * (1 + 40 - 20))) + 20;
};


// Panel toolbox
$(document).ready(function () {
	$('.collapse-link').on('click', function () {
		var $BOX_PANEL = $(this).closest('.x_panel'),
			$ICON = $(this).find('i'),
			$BOX_CONTENT = $BOX_PANEL.find('.x_content');

		// fix for some div with hardcoded fix class
		if ($BOX_PANEL.attr('style')) {
			$BOX_CONTENT.slideToggle(200, function () {
				$BOX_PANEL.removeAttr('style');
			});
		} else {
			$BOX_CONTENT.slideToggle(200);
			$BOX_PANEL.css('height', 'auto');
		}

		$ICON.toggleClass('fa-chevron-up fa-chevron-down');
	});

	$('.close-link').click(function () {
		var $BOX_PANEL = $(this).closest('.x_panel');

		$BOX_PANEL.remove();
	});
});
// /Panel toolbox

// Tooltip
$(document).ready(function () {
	$('[data-toggle="tooltip"]').tooltip({
		container: 'body'
	});
});
// Switchery
$(document).ready(function () {
	if ($(".js-switch")[0]) {
		var elems = Array.prototype.slice.call(document.querySelectorAll('.js-switch'));
		elems.forEach(function (html) {
			var switchery = new Switchery(html, {
				color: primary_color
			});
		});
	}
});

// Accordion
$(document).ready(function () {
	$(".expand").on("click", function () {
		$(this).next().slideToggle(200);
		$expand = $(this).find(">:first-child");

		if ($expand.text() == "+") {
			$expand.text("-");
		} else {
			$expand.text("+");
		}
	});
});


//hover and retain popover when on popover content
var originalLeave = $.fn.popover.Constructor.prototype.leave;
$.fn.popover.Constructor.prototype.leave = function (obj) {
	var self = obj instanceof this.constructor ?
		obj : $(obj.currentTarget)[this.type](this.getDelegateOptions()).data('bs.' + this.type);
	var container, timeout;

	originalLeave.call(this, obj);

	if (obj.currentTarget) {
		container = $(obj.currentTarget).siblings('.popover');
		timeout = self.timeout;
		container.one('mouseenter', function () {
			//We entered the actual popover – call off the dogs
			clearTimeout(timeout);
			//Let's monitor popover content instead
			container.one('mouseleave', function () {
				$.fn.popover.Constructor.prototype.leave.call(self, self);
			});
		});
	}
};

$('body').popover({
	selector: '[data-popover]',
	trigger: 'click hover',
	delay: {
		show: 50,
		hide: 400
	}
});


function gd(year, month, day) {
	return new Date(year, month - 1, day).getTime();
}

function init_JQVmap() {

	//console.log('check init_JQVmap [' + typeof (VectorCanvas) + '][' + typeof (jQuery.fn.vectorMap) + ']' );	

	if (typeof (jQuery.fn.vectorMap) === 'undefined') {
		return;
	}

	console.log('init_JQVmap');

	if ($('#world-map-gdp').length) {
		var colors = ["#E6F2F0", primary_color];
		$('#world-map-gdp').vectorMap({
			map: 'be_mill',
			backgroundColor: null,
			enableZoom: false,
			series: {
				regions: [{
					values: sample_data,
					scale: colors,
				}]
			},
			onRegionTipShow: function (e, el, code) {
				el.html(el.html() + ' (' + sample_data[code] + ' stemmen)');
			}
		});

	}

};


/* AUTOSIZE */

function init_autosize() {

	if (typeof $.fn.autosize !== 'undefined') {

		autosize($('.resizable_textarea'));

	}

};

//tags input
function init_TagsInput() {

	if (typeof $.fn.tagsInput !== 'undefined') {

		$('#tags_1').tagsInput({
			width: 'auto'
		});

	}

};

/* SELECT2 */

function init_select2() {

	if (typeof (select2) === 'undefined') {
		return;
	}
	console.log('init_toolbox');

	$(".select2_single").select2({
		placeholder: "Select a state",
		allowClear: true
	});
	$(".select2_group").select2({});
	$(".select2_multiple").select2({
		maximumSelectionLength: 4,
		placeholder: "With Max Selection limit 4",
		allowClear: true
	});

};

/* WYSIWYG EDITOR */

function init_wysiwyg() {

	if (typeof ($.fn.wysiwyg) === 'undefined') {
		return;
	}
	console.log('init_wysiwyg');

	function init_ToolbarBootstrapBindings() {
		var fonts = ['Serif', 'Sans', 'Arial', 'Arial Black', 'Courier',
				'Courier New', 'Comic Sans MS', 'Helvetica', 'Impact', 'Lucida Grande', 'Lucida Sans', 'Tahoma', 'Times',
				'Times New Roman', 'Verdana'
			],
			fontTarget = $('[title=Font]').siblings('.dropdown-menu');
		$.each(fonts, function (idx, fontName) {
			fontTarget.append($('<li><a data-edit="fontName ' + fontName + '" style="font-family:\'' + fontName + '\'">' + fontName + '</a></li>'));
		});
		$('a[title]').tooltip({
			container: 'body'
		});
		$('.dropdown-menu input').click(function () {
			return false;
		})
			.change(function () {
				$(this).parent('.dropdown-menu').siblings('.dropdown-toggle').dropdown('toggle');
			})
			.keydown('esc', function () {
				this.value = '';
				$(this).change();
			});

		$('[data-role=magic-overlay]').each(function () {
			var overlay = $(this),
				target = $(overlay.data('target'));
			overlay.css('opacity', 0).css('position', 'absolute').offset(target.offset()).width(target.outerWidth()).height(target.outerHeight());
		});

		if ("onwebkitspeechchange" in document.createElement("input")) {
			var editorOffset = $('#editor').offset();

			$('.voiceBtn').css('position', 'absolute').offset({
				top: editorOffset.top,
				left: editorOffset.left + $('#editor').innerWidth() - 35
			});
		} else {
			$('.voiceBtn').hide();
		}
	}

	function showErrorAlert(reason, detail) {
		var msg = '';
		if (reason === 'unsupported-file-type') {
			msg = "Unsupported format " + detail;
		} else {
			console.log("error uploading file", reason, detail);
		}
		$('<div class="alert"> <button type="button" class="close" data-dismiss="alert">&times;</button>' +
			'<strong>File upload error</strong> ' + msg + ' </div>').prependTo('#alerts');
	}

	$('.editor-wrapper').each(function () {
		var id = $(this).attr('id');	//editor-one

		$(this).wysiwyg({
			toolbarSelector: '[data-target="#' + id + '"]',
			fileUploadError: showErrorAlert
		});
	});


	window.prettyPrint;
	prettyPrint();

};

/* CROPPER */

function init_cropper() {


	if (typeof ($.fn.cropper) === 'undefined') {
		return;
	}
	console.log('init_cropper');

	var $image = $('#image');
	var $download = $('#download');
	var $dataX = $('#dataX');
	var $dataY = $('#dataY');
	var $dataHeight = $('#dataHeight');
	var $dataWidth = $('#dataWidth');
	var $dataRotate = $('#dataRotate');
	var $dataScaleX = $('#dataScaleX');
	var $dataScaleY = $('#dataScaleY');
	var options = {
		aspectRatio: 16 / 9,
		preview: '.img-preview',
		crop: function (e) {
			$dataX.val(Math.round(e.x));
			$dataY.val(Math.round(e.y));
			$dataHeight.val(Math.round(e.height));
			$dataWidth.val(Math.round(e.width));
			$dataRotate.val(e.rotate);
			$dataScaleX.val(e.scaleX);
			$dataScaleY.val(e.scaleY);
		}
	};


	// Tooltip
	$('[data-toggle="tooltip"]').tooltip();


	// Cropper
	$image.on({
		'build.cropper': function (e) {
			console.log(e.type);
		},
		'built.cropper': function (e) {
			console.log(e.type);
		},
		'cropstart.cropper': function (e) {
			console.log(e.type, e.action);
		},
		'cropmove.cropper': function (e) {
			console.log(e.type, e.action);
		},
		'cropend.cropper': function (e) {
			console.log(e.type, e.action);
		},
		'crop.cropper': function (e) {
			console.log(e.type, e.x, e.y, e.width, e.height, e.rotate, e.scaleX, e.scaleY);
		},
		'zoom.cropper': function (e) {
			console.log(e.type, e.ratio);
		}
	}).cropper(options);


	// Buttons
	if (!$.isFunction(document.createElement('canvas').getContext)) {
		$('button[data-method="getCroppedCanvas"]').prop('disabled', true);
	}

	if (typeof document.createElement('cropper').style.transition === 'undefined') {
		$('button[data-method="rotate"]').prop('disabled', true);
		$('button[data-method="scale"]').prop('disabled', true);
	}


	// Download
	if (typeof $download[0].download === 'undefined') {
		$download.addClass('disabled');
	}


	// Options
	$('.docs-toggles').on('change', 'input', function () {
		var $this = $(this);
		var name = $this.attr('name');
		var type = $this.prop('type');
		var cropBoxData;
		var canvasData;

		if (!$image.data('cropper')) {
			return;
		}

		if (type === 'checkbox') {
			options[name] = $this.prop('checked');
			cropBoxData = $image.cropper('getCropBoxData');
			canvasData = $image.cropper('getCanvasData');

			options.built = function () {
				$image.cropper('setCropBoxData', cropBoxData);
				$image.cropper('setCanvasData', canvasData);
			};
		} else if (type === 'radio') {
			options[name] = $this.val();
		}

		$image.cropper('destroy').cropper(options);
	});


	// Methods
	$('.docs-buttons').on('click', '[data-method]', function () {
		var $this = $(this);
		var data = $this.data();
		var $target;
		var result;

		if ($this.prop('disabled') || $this.hasClass('disabled')) {
			return;
		}

		if ($image.data('cropper') && data.method) {
			data = $.extend({}, data); // Clone a new one

			if (typeof data.target !== 'undefined') {
				$target = $(data.target);

				if (typeof data.option === 'undefined') {
					try {
						data.option = JSON.parse($target.val());
					} catch (e) {
						console.log(e.message);
					}
				}
			}

			result = $image.cropper(data.method, data.option, data.secondOption);

			switch (data.method) {
				case 'scaleX':
				case 'scaleY':
					$(this).data('option', -data.option);
					break;

				case 'getCroppedCanvas':
					if (result) {

						// Bootstrap's Modal
						$('#getCroppedCanvasModal').modal().find('.modal-body').html(result);

						if (!$download.hasClass('disabled')) {
							$download.attr('href', result.toDataURL());
						}
					}

					break;
			}

			if ($.isPlainObject(result) && $target) {
				try {
					$target.val(JSON.stringify(result));
				} catch (e) {
					console.log(e.message);
				}
			}

		}
	});

	// Keyboard
	$(document.body).on('keydown', function (e) {
		if (!$image.data('cropper') || this.scrollTop > 300) {
			return;
		}

		switch (e.which) {
			case 37:
				e.preventDefault();
				$image.cropper('move', -1, 0);
				break;

			case 38:
				e.preventDefault();
				$image.cropper('move', 0, -1);
				break;

			case 39:
				e.preventDefault();
				$image.cropper('move', 1, 0);
				break;

			case 40:
				e.preventDefault();
				$image.cropper('move', 0, 1);
				break;
		}
	});

	// Import image
	var $inputImage = $('#inputImage');
	var URL = window.URL || window.webkitURL;
	var blobURL;

	if (URL) {
		$inputImage.change(function () {
			var files = this.files;
			var file;

			if (!$image.data('cropper')) {
				return;
			}

			if (files && files.length) {
				file = files[0];

				if (/^image\/\w+$/.test(file.type)) {
					blobURL = URL.createObjectURL(file);
					$image.one('built.cropper', function () {

						// Revoke when load complete
						URL.revokeObjectURL(blobURL);
					}).cropper('reset').cropper('replace', blobURL);
					$inputImage.val('');
				} else {
					window.alert('Please choose an image file.');
				}
			}
		});
	} else {
		$inputImage.prop('disabled', true).parent().addClass('disabled');
	}


};

/* VALIDATOR */

function init_validator() {

	if (typeof (validator) === 'undefined') {
		return;
	}
	console.log('init_validator');

	// initialize the validator function
	validator.message.date = 'not a real date';

	// validate a field on "blur" event, a 'select' on 'change' event & a '.reuired' classed multifield on 'keyup':
	$('form')
		.on('blur', 'input[required], input.optional, select.required', validator.checkField)
		.on('change', 'select.required', validator.checkField)
		.on('keypress', 'input[required][pattern]', validator.keypress);

	$('.multi.required').on('keyup blur', 'input', function () {
		validator.checkField.apply($(this).siblings().last()[0]);
	});

	$('form').submit(function (e) {
		e.preventDefault();
		var submit = true;

		// evaluate the form using generic validaing
		if (!validator.checkAll($(this))) {
			submit = false;
		}

		if (submit)
			this.submit();

		return false;
	});

};

/* PNotify */

function init_PNotify() {

	if (typeof (PNotify) === 'undefined') {
		return;
	}
	console.log('init_PNotify');
	/*
		new PNotify({
			title: "PNotify",
			type: "info",
			text: "Welcome. Try hovering over me. You can click things behind me, because I'm non-blocking.",
			nonblock: {
				nonblock: true
			},
			addclass: 'dark',
			styling: 'bootstrap3',
			hide: false,
			before_close: function (PNotify) {
				PNotify.update({
					title: PNotify.options.title + " - Enjoy your Stay",
					before_close: null
				});
	
				PNotify.queueRemove();
	
				return false;
			}
		});
	*/
};

/* COMPOSE */

function init_compose() {

	if (typeof ($.fn.slideToggle) === 'undefined') {
		return;
	}
	console.log('init_compose');

	$('#compose, .compose-close').click(function () {
		$('.compose').slideToggle();
	});

};

function init_DataTables() {
	if (console.log("run_datatables"), "undefined" != typeof $.fn.DataTable) {
		console.log("init_DataTables");
		var a = function () {
			$("#datatable-buttons").length && $("#datatable-buttons").DataTable({
				dom: "Blfrtip",
				buttons: [{extend: "copy", className: "btn-sm"}, {extend: "csv", className: "btn-sm"}, {
					extend: "excel",
					className: "btn-sm"
				}, {extend: "pdfHtml5", className: "btn-sm"}, {extend: "print", className: "btn-sm"}],
				responsive: !0
			})
		};
		TableManageButtons = function () {
			"use strict";
			return {
				init: function () {
					a()
				}
			}
		}(), $("#datatable").dataTable(), $("#datatable-keytable").DataTable({keys: !0}), $("#datatable-responsive").DataTable(), $("#datatable-scroller").DataTable({
			ajax: "js/datatables/json/scroller-demo.json",
			deferRender: !0,
			scrollY: 380,
			scrollCollapse: !0,
			scroller: !0
		}), $("#datatable-fixed-header").DataTable({fixedHeader: !0});
	}
}

//Generic Ajax call
function submitForm($this, event, message=null) {
    $.ajax({
        type: $this.attr('method'),
        url: $this.attr('action'),
        data: $this.serialize(),
        succes: message
            .addClass('green')
            .html("Saved.")
            .fadeOut(1000,
                function() {
                    $(this)
                        .removeClass()
                        .html("")
                        .css("display", "inline");
                })
    }).fail(() => message
        .addClass('red')
        .html("Mislukt.")
        .fadeOut(1000,
            function() {
                $(this)
                    .removeClass()
                    .html("")
                    .css("display", "inline");
            }));
    event.preventDefault();
}

//Generic ajax toggle button
var wto;
function ajaxToggleSubscribe($this) {
	clearTimeout(wto);
	var id = $this.data('item-id');
	var text = $this.html();
	$this.html("<i class='fa fa-circle-o-notch fa-spin'></i>");
	wto = setTimeout(function() {
			$.ajax({
				type: 'POST',
				url: '/api/ToggleSubscribe/' + id
			}).fail(() => { /* ok */ })
				.done(function() {
					if (text === Resources.Subscribe) $this.html(Resources.Unsubscribe);
					else $this.html(Resources.Subscribe);
					$this.toggleClass("btn-danger btn-success");
				});
		},
		500);
}

$.fn.toggleText = function(t1, t2){
    if (this.text() === t1) this.text(t2);
    else                   this.text(t1);
    return this;
};
	
function check() {
	if ($('.showchbox').length) {
		$('.checkbox').removeClass('showchbox');
	} else {
		$('.checkbox').addClass('showchbox');
	}
}

function callMethods() {
	init_wysiwyg();
	init_cropper();
	init_TagsInput();
	init_select2();
	init_DataTables();
	init_validator();
	init_PNotify();
	init_compose();
	init_autosize();
}

$(document).ready(function () {

    $('.search-field').on('focus', function (e) {
        $('form.navbar-search .input-group').addClass('search-focus');
    });
    $('.search-field').on('focusout', function (e) {
        $('form.navbar-search .input-group').removeClass('search-focus');
    });
	callMethods();
	init_JQVmap();
});	

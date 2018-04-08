/* http://github.com/mindmup/bootstrap-wysiwyg */
/*global jQuery, $, FileReader*/
/*jslint browser:true*/
(function ($) {
	'use strict';
	var readFileIntoDataUrl = function (fileInfo) {
		var loader = $.Deferred(),
			fReader = new FileReader();
		fReader.onload = function (e) {
			loader.resolve(e.target.result);
		};
		fReader.onerror = loader.reject;
		fReader.onprogress = loader.notify;
		fReader.readAsDataURL(fileInfo);
		return loader.promise();
	};
	$.fn.cleanHtml = function () {
		var html = $(this).html();
		return html && html.replace(/(<br>|\s|<div><br><\/div>|&nbsp;)*$/, '');
	};
	$.fn.wysiwyg = function (userOptions) {
		var editor = this,
			selectedRange,
			selectionNode,
			caretPos,
			options,
			toolbarBtnSelector,
			commandCache = {},
			updateCommandCache = function () {
				var key;
				for (key in commandCache) {
					var commandValue = document.queryCommandValue(key);
					var commandState = document.queryCommandState(key);

					if (commandState) {
						commandCache[key] = commandState;
					} else if (commandValue.length > 0 && commandValue !== 'false') {
						commandCache[key] = commandValue;
					} else {
						commandCache[key] = false;
					}
				}
			},
			restoreCommandCache = function() {
				var key;
				for (key in commandCache) {
					var val = commandCache[key];
					if (typeof(val) === 'boolean') {
						if (val !== document.queryCommandState(key)) {
							document.execCommand(key, 0, null);
						}
					} else if (val !== document.queryCommandValue(key)) {
						document.execCommand(key, 0, val);
					}
				}
			},
			namespaceEvents = function(events) {
				if (events.toString() != '[object Array]') {
					events = events.split(' ');
				}

				events.push(''); // Add empty element for easy join() support.

				if (options.eventNamespace != null && options.eventNamespace.length > 0)
				{
					return events.join('.' + options.eventNamespace + ' ');
				} else {
					return events.join(' ');
				}

			},
			updateToolbar = function () {
				if (options.activeToolbarClass) {
					$(options.toolbarSelector).find(toolbarBtnSelector).each(function () {
						var command = $(this).data(options.commandRole);
						var commandNoArgs; // Temporarily store index, replace with command.

						if ((commandNoArgs = command.indexOf(' ')) >= 0) {
							commandNoArgs = command.slice(0, commandNoArgs);
						} else {
							commandNoArgs = command;
						}

            if (commandNoArgs === "fontSize" && options.setRealFontSize != null) {
                options.setRealFontSize(selectedRange);
            } else if (document.queryCommandState(command)) {
							$(this).addClass(options.activeToolbarClass);
						} else if (commandNoArgs + ' ' + document.queryCommandValue(commandNoArgs) === command) {
							$(this).addClass(options.activeToolbarClass);
						} else {
							$(this).removeClass(options.activeToolbarClass);
						}
					});
				}
			},
			execCommand = function (commandWithArgs, valueArg) {
				var commandArr = commandWithArgs.split(' '),
					command = commandArr.shift(),
					args = commandArr.join(' ') + (valueArg || '');
				document.execCommand(command, 0, args);
				if (args.length > 0) {
					commandCache[command] = document.queryCommandValue(command);
				} else {
					commandCache[command] = document.queryCommandState(command);
				}
				updateToolbar();
			},
			mapHotKeyToCommand = function (event, hotKeys) {
				var hotKey = '';

				if (event.ctrlKey) {
					hotKey += 'ctrl+';
				} else if (event.metaKey) {
					hotKey += 'meta+';
				}

				if (event.shiftKey) {
					hotKey += 'shift+';
				}

				var keyCode = event.keyCode;
				if (keyCode >= 20) { // Real characters
					// Reduce UTF chars.
					if (96 <= event.keyCode && event.keyCode <= 105) {
						keyCode -= 48;
					}
					hotKey += String.fromCharCode(keyCode).toLowerCase();

				} else if (event.keyCode === 9) { // TAB character
					hotKey += 'tab';
				}

				return hotKeys[hotKey];

			},
			editorActive = function () {
				return (editor.attr('contenteditable') && editor.is(':visible'));
			},
			bindHotkeys = function (hotKeys) {
				/*
				 * Break up the hotkeys into multiple values. For example
				 * {'ctrl+b meta+b': 'bold'} -> {'ctrl+b': 'bold'},{'meta+b': 'bold'}.
				 * Also ensures everything is stored in lower case.
				 */
				for (var hotKey in hotKeys) {
					var split;
					if ((split = hotKey.split(' ')).length > 1) {
						for (var i = 0; i < split.length; i++) {
							hotKeys[split[i].toLowerCase()] = hotKeys[hotKey].toLowerCase();
						}
						delete hotKeys[hotKey];
					} else {
						hotKeys[hotKey.toLowerCase()] = hotKeys[hotKey].toLowerCase();
					}
				}

				var eventNamespace = options.eventNamespace + '-' + 'bindKeys';

				editor.on(namespaceEvents('keydown'), hotKeys, function (e) {
					var command = '';

					// Ensure we have a command to execute for this hotkey, before blocking anything.
					if (editorActive() && (command = mapHotKeyToCommand(e, hotKeys)) != null) {
						e.preventDefault();
						e.stopPropagation();

						// Execute hotkey if the callback tells us it's enabled.
						if (
							typeof options.hotKeyEnabledCallback === 'function' &&
								options.hotKeyEnabledCallback(command)
							) {
							execCommand(command);
						}
					}
				});

				// Install a single keyup handler to block the keyup events matching a hotKey.
				editor.on(namespaceEvents('keyup'), hotKeys, function (e) {
					var command = '';
					if (editorActive() && (command = mapHotKeyToCommand(e, hotKeys)) != null) {
						e.preventDefault();
						e.stopPropagation();
					}
				});
			},
			getCurrentRange = function () {
				var sel = window.getSelection();
				if (sel.getRangeAt && sel.rangeCount) {
					return sel.getRangeAt(0);
				}
			},
			saveSelection = function () {
				updateCommandCache();
				selectedRange = getCurrentRange();
				selectionNode = window.getSelection().anchorNode;
				caretPos = window.getSelection().baseOffset;
			},
			restoreSelection = function (restoreCaret) {
				var selection = window.getSelection();
				if (selectedRange) {
					try {
						selection.removeAllRanges();
					} catch (ex) {
						document.body.createTextRange().select();
						document.selection.empty();
					}

					if (restoreCaret && selectionNode) {
						selectedRange.setStart(selectionNode, caretPos);
						selectedRange.collapse(true);
					}
					selection.addRange(selectedRange);
				}
			},
			insertFiles = function (files) {
				editor.focus();
				$.each(files, function (idx, fileInfo) {
					if (/^image\//.test(fileInfo.type)) {
						$.when(readFileIntoDataUrl(fileInfo)).done(function (dataUrl) {
							execCommand('insertimage', dataUrl);
						}).fail(function (e) {
								options.fileUploadError("file-reader", e);
							});
					} else {
						options.fileUploadError("unsupported-file-type", fileInfo.type);
					}
				});
			},
			markSelection = function (input, color) {
				restoreSelection();
				if (document.queryCommandSupported('hiliteColor')) {
					document.execCommand('hiliteColor', 0, color || 'transparent');
				}
				saveSelection();
				input.data(options.selectionMarker, color);
			},
			bindToolbar = function (toolbar, options) {
				toolbar.find(toolbarBtnSelector).click(function () {
					// restore caret position after certain commands
					var restoreCaret = ['indent', 'outdent'].indexOf($(this).data(options.commandRole)) > -1
					restoreSelection(restoreCaret);
					editor.focus();
					restoreCommandCache();
					execCommand($(this).data(options.commandRole));
					saveSelection();
				});
				toolbar.find('[data-toggle=dropdown]').click(restoreSelection);

				toolbar.find('input[type=text][data-' + options.commandRole + ']').on(namespaceEvents('webkitspeechchange change'), function () {
					var newValue = this.value; /* ugly but prevents fake double-calls due to selection restoration */
					this.value = '';
					restoreSelection();
					if (newValue) {
						editor.focus();
						execCommand($(this).data(options.commandRole), newValue);
					}
					saveSelection();
				}).on(namespaceEvents('focus'), function () {
						var input = $(this);
						if (!input.data(options.selectionMarker)) {
							markSelection(input, options.selectionColor);
							input.focus();
						}
					}).on(namespaceEvents('blur'), function () {
						var input = $(this);
						if (input.data(options.selectionMarker)) {
							markSelection(input, false);
						}
					});
				toolbar.find('input[type=file][data-' + options.commandRole + ']').on(namespaceEvents('change'), (function () {
					restoreSelection();
					if (this.type === 'file' && this.files && this.files.length > 0) {
						insertFiles(this.files);
					}
					saveSelection();
					this.value = '';
				}));
			},
			initFileDrops = function () {
				editor.on(namespaceEvents('dragenter dragover'), false)
					.on(namespaceEvents('drop'), function (e) {
						var dataTransfer = e.originalEvent.dataTransfer;
						e.stopPropagation();
						e.preventDefault();
						if (dataTransfer && dataTransfer.files && dataTransfer.files.length > 0) {
							insertFiles(dataTransfer.files);
						}
					});
			};
		options = $.extend({}, $.fn.wysiwyg.defaults, userOptions);
		toolbarBtnSelector = 'a[data-' + options.commandRole + '],button[data-' + options.commandRole + '],input[type=button][data-' + options.commandRole + '],div[data-' + options.commandRole + ']';
		bindHotkeys(options.hotKeys);
		if (options.dragAndDropImages) {
			initFileDrops();
		}
		bindToolbar($(options.toolbarSelector), options);
		editor.attr('contenteditable', true)
			.on(namespaceEvents('mouseup keyup mouseout'), function () {
				saveSelection();
				updateToolbar();
			});

		$(toolbarBtnSelector).each(function () {
			var btnAttr = this.getAttribute('data-' + options.commandRole);
			commandCache[btnAttr.split(' ')[0]] = false;
		});

		$(window).bind('touchend', function (e) {
			var isInside = (editor.is(e.target) || editor.has(e.target).length > 0),
				currentRange = getCurrentRange(),
				clear = currentRange && (currentRange.startContainer === currentRange.endContainer && currentRange.startOffset === currentRange.endOffset);
			if (!clear || isInside) {
				saveSelection();
				updateToolbar();
			}
		});
        return this;
	};
	$.fn.wysiwyg.defaults = {
		hotKeys: {
			'ctrl+b meta+b': 'bold',
			'ctrl+i meta+i': 'italic',
			'ctrl+u meta+u': 'underline',
			'ctrl+z meta+z': 'undo',
			'ctrl+y meta+y meta+shift+z': 'redo',
			'ctrl+l meta+l': 'justifyleft',
			'ctrl+r meta+r': 'justifyright',
			'ctrl+e meta+e': 'justifycenter',
			'ctrl+j meta+j': 'justifyfull',
			'shift+tab': 'outdent',
			'tab': 'indent'
		},
		toolbarSelector: '[data-role=editor-toolbar]',
		commandRole: 'edit',
		activeToolbarClass: 'btn-info',
		selectionMarker: 'edit-focus-marker',
		selectionColor: 'darkgrey',
		dragAndDropImages: true,
		eventNamespace: 'bootstrap-wysiwyg',
		hotKeyEnabledCallback: function(command) { return true; },
		fileUploadError: function (reason, detail) { console.log("File upload error", reason, detail); }
	};
}(window.jQuery));
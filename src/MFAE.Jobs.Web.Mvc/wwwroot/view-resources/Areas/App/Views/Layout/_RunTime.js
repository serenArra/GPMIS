
"use strict";


var Jobs = {
    init: function () {
        Jobs.initAutocomplete();
        Jobs.initDatetimepicker();
        Jobs.initTimepicker();
        Jobs.intitCopyto();
        Jobs.initTextAreas();
        Jobs.initSelect();
        Jobs.initRepeater();
    },
    initRepeater: function (selector) {

        $('.repeater', $(selector ? selector : 'body')).each(function () {
            var select = $(this);
            var selectId = select.attr("id");

            $('#' + selectId).repeater({
                initEmpty: false,

                show: function () {
                    $(this).slideDown();
                    DisplayFromBuilder.ApplyWidgetStyle();
                },

                hide: function (deleteElement) {

                    var current = $(this);
                    abp.message.confirm(
                        "",
                        app.localize("AreYouSure"),
                        function (isConfirmed) {
                            if (isConfirmed) {
                                current.slideUp(deleteElement);
                            }
                        }
                    );
                },
                isFirstItemUndeletable: true
            });
        });
    },
    initTimepicker: function (selector) {

        $('.time-picker', $(selector ? selector : 'body')).each(function () {
            $(this).datetimepicker({
                format: 'LT'
            });
        });
    },

    initTextAreas: function (selector) {
        ///   $('.textArea', $(selector ? selector : 'body'))
        $('textarea', $(selector ? selector : 'body')).each(function () {

            var input = $(this);
            var maxLength = input.attr("maxlength");

            if (maxLength) {
                input.closest('textarea').after(" <span class='form-text text-muted'><span id='span" + input.attr("id") + "'>"
                    + maxLength + "</span> remaining</span>");

                input.bind("keyup change", function () {
                    Jobs.checkMaxLength(this.id, maxLength);
                })
                Jobs.checkMaxLength(this.id, maxLength);
            }
        });
    },

    checkMaxLength: function (textareaID, maxLength) {

        var currentLengthInTextarea = $("#" + textareaID).val().length;
        $('#span' + textareaID).text(parseInt(maxLength) - parseInt(currentLengthInTextarea));

        if (currentLengthInTextarea > (maxLength)) {

            // Trim the field current length over the maxlength.
            $("#" + textareaID).val($("#" + textareaID).val().slice(0, maxLength));
            $('#span' + textareaID).text(0);

        }
    },
    initSelect: function (selector) {

        $('.select2', $(selector ? selector : 'body')).each(function () {
            var select = $(this);
            var selectId = select.attr("id");
            $('#' + selectId).next().remove("span");

            console.log("select: ", select);
            $('#' + selectId).select2({
                theme: 'bootstrap5',
                width: "100%",
                selectionCssClass: 'form-select form-select-solid',
                dropdownParent: select.closest('.modal').length ? select.closest('.modal') : select.parent()
            });
        });
    },



    intitCopyto: function (selector) {

        $('.copyto', $(selector ? selector : 'body')).each(function () {
            var select = $(this);
            var selectId = select.attr("id");
            $('#' + selectId).change(function () {
                var copyto = $('#' + selectId).attr("copyto");
                if ($('#' + copyto).val() == '')
                    $('#' + copyto).val($('#' + selectId).val());
            });
        });
    },

    initDatetimepicker: function (selector) {

        $('.date-picker', $(selector ? selector : 'body')).each(function () {
            $(this).datetimepicker({
                viewMode: 'days',
                format: 'DD/MM/Y'
            });

        });

    },
    initAutocomplete: function (selector) {


        $('.select2-remote', $(selector ? selector : 'body')).each(function () {

            var select = $(this);
            var selectId = select.attr("id");
            $('#' + selectId).attr('data-allow-clear', true);
            var idname = select.attr("optionkey");
            if (!idname) idname = "id";
            var textname = select.attr("optionvalue");
            var isMultiple = "true";///select.attr("ismultiple");
            if (!textname) textname = "name";
            var values = select.attr("values");
            var preventSpaces = "false";
            // select.attr("preventspaces");
            var delay = 100;
            if (preventSpaces == "true") {
                delay = 1000;
            }

            var controller = select.attr("controller");
            var action = select.attr("action");


            var beforeSendFunction = function (params, success, failure) {
                if (preventSpaces == "false") {
                    var $request = $.ajax(params);
                    $request.then(success);
                    $request.fail(failure);
                    return $request;
                } else {
                    if (isMultiple == "true") {
                        _container = $("#" + selectId).data('select2').$container;
                        _searchInput = _container.find("input");
                    } else {
                        _container = $("#" + selectId).data('select2').$dropdown;
                        _searchInput = _container.find("input");
                    }
                    _str = _searchInput.val() || '';
                    _spaces = _str.length - _str.replace(/ /g, '').length;
                    if (_str != "" && (_spaces >= 2 || !isNaN(_str))) {
                        _searchInput.prop("disabled", true);
                        var $request = $.ajax(params);
                        $request.then(success);
                        $request.fail(failure);
                        return $request;
                    } else {
                        $('#select2-' + selectId + '-results').find('li[aria-selected="false"]').remove();
                        $('#select2-' + selectId + '-results').find('.select2-results__option').html("${message(code:'select2.spaceError.label')}");
                    }
                }
            };



            var formatResultFunction = function (item) {
                return item.text;
            };


            var findValue = function (obj, prop) {
                prop = prop.split('.');
                for (var i = 0; i < prop.length; i++) {
                    if (typeof obj[prop[i]] == 'undefined')
                        return defval;
                    obj = obj[prop[i]];
                }
                return obj;
            }


            if (select.attr("formatResultFunction")) {
                formatResultFunction = window[select.attr("formatResultFunction")]
            }

            if (select.attr("beforeSendFunction")) {
                beforeSendFunction = window[select.attr("beforeSendFunction")]
            }

            var paramsGenerateFunction;
            if (select.attr("paramsGenerateFunction")) {
                paramsGenerateFunction = select.attr("paramsGenerateFunction")
            }


            var minimumInputLength = select.attr("minimumInputLength");
            var maximumInputLength = select.attr("maximumInputLength");

            if (!maximumInputLength) maximumInputLength = 250;
            if (!minimumInputLength) minimumInputLength = 1;



            $('#' + selectId).select2({
                theme: 'bootstrap5',
                selectionCssClass: 'form-select form-select-solid',
                dropdownParent: select.closest('.modal').length ? select.closest('.modal') : select.parent(),
                allowClear: true,
                templateResult: formatResultFunction,
                templateSelection: formatResultFunction,
                dir: app.isRTL() ? 'rtl' : 'ltr',
                escapeMarkup: function (markup) {
                    return markup;
                },
                placeholder: app.localize("SelectOption"),
                ajax: {
                    url: abp.appPath + "api/services/app/" + controller + "/" + action,
                    dataType: 'json',
                    delay: 250,

                    data: function (params) {
                        var searchParams = {};
                        if (paramsGenerateFunction) {
                            searchParams = eval(paramsGenerateFunction)
                        }
                        searchParams.page = params.page;
                        searchParams.filter = params.term;

                        return searchParams
                    },
                    transport: beforeSendFunction,

                    processResults: function (data, params) {


                        if (isMultiple == "true") {
                            $("#" + selectId).data('select2').$container.find("input").prop("disabled", false);
                        } else {
                            $("#" + selectId).data('select2').$dropdown.find("input").prop("disabled", false);
                        }

                        params.page = params.page || 1;


                        return {
                            results: $.map(data.result.items, function (item) {
                                return {

                                    text: findValue(item, textname),
                                    id: findValue(item, idname)
                                }
                            }),
                            pagination: {
                                more: (params.page * 30) < data.result.length
                            }
                        };
                    },
                    cache: true
                },
                minimumInputLength: 0,
                maximumInputLength: maximumInputLength,
                language: abp.localization.currentCulture.name,
            });


            if (values) {
                values = JSON.parse(values);
                jQuery.each(values, function (idx, item) {


                    if (item) {
                        var arrayValues = item.toString().split(",");
                        if (arrayValues.length) {


                            if (item.text != '' && item.id != '') {
                                var newOption = new Option(item.text, item.id, true, true);
                                $(select).append(newOption)
                            }


                        }
                    }
                });
                $(select).trigger('change');
            }


        });


        $(document).on('focus', '.select2-selection.select2-selection--single', function (e) {
            $(this).closest(".select2-container").siblings('select:enabled').select2('open');
        });
    },

    initDualListbox: function (dualListId) {

        var $this = $("#" + dualListId);

        var dualListBox = new DualListbox($this.get(0), {
            availableTitle: app.localize('AvailableOptions'),
            selectedTitle: app.localize('SelectedOptions'),
            addButtonText: app.localize('Add'),
            removeButtonText: app.localize('Remove'),
            addAllButtonText: app.localize('AddAll'),
            removeAllButtonText: app.localize('RemoveAll'),
        });

    }
}

$(document).ready(function () {
    Jobs.init();
});

// Class definition
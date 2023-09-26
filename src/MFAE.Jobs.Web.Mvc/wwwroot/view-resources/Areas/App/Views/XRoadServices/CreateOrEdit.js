(function ($) {
    //    app.modals.CreateOrEditXRoadServiceModal = function () {

    var _xRoadServicesService = abp.services.app.xRoadServices;

    var _modalManager;
    var _$xRoadServiceInformationForm = null;
    _$xRoadServiceInformationForm = $('form[name=XRoadServiceInformationsForm]');// _modalManager.getModal().find('form[name=XRoadServiceInformationsForm]');
    _$xRoadServiceInformationForm.validate();

    //this.init = function (modalManager) {
    //    _modalManager = modalManager;

    //    var modal = _modalManager.getModal();
    //    modal.find('.date-picker').datetimepicker({
    //        locale: abp.localization.currentLanguage.name,
    //        format: 'L'
    //    });

    //    _$xRoadServiceInformationForm = _modalManager.getModal().find('form[name=XRoadServiceInformationsForm]');
    //    _$xRoadServiceInformationForm.validate();
    //};
    $(".select2").select2({
        width: "100%"
    });

    var loadXRoadServiceAttributesRequestView = function (serviceId) {
        var _args = {};
        //return;
        var options = {
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/IndexRequest?serviceId=' + serviceId,
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributes/IndexRequest.js',
        };

        abp.ui.setBusy($("body"));
        $('.XRoadServiceAttributesIndexRequest')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    abp.ui.clearBusy($("body"));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceAttributesRequestIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                abp.ui.clearBusy($("body"));
            });
    };


    var loadXRoadServiceAttributesResponseView = function (serviceId) {
        var _args = {};
        //return;
        var options = {
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/IndexResponse?serviceId=' + serviceId,
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributes/IndexResponse.js',
        };

        abp.ui.setBusy($("body"));
        $('.XRoadServiceAttributesIndexResponse')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    abp.ui.clearBusy($("body"));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceAttributesResponseIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                abp.ui.clearBusy($("body"));
            });
    };
    var loadXRoadServiceErrors = function (serviceId) {
        var _args = {};
        //return;
        var options = {
            viewUrl: abp.appPath + 'App/XRoadServiceErrors/GetPartial?serviceId=' + serviceId,
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceErrors/_index.js',
        };

        abp.ui.setBusy($("body"));
        $('.XRoadServiceErrorSection')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    abp.ui.clearBusy($("body"));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceErrorIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                abp.ui.clearBusy($("body"));
            });
    };

    var loadReport = function (attributeId) {
        var _args = {};
        var options = {
            viewUrl: "http://172.20.5.75:8088/ReportServer2019/Pages/ReportViewer.aspx?%2fReports%2fFamilyReport&rs:Command=Render",
            //scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributeMappings/_IndexRequest.js',
        };

        //abp.ui.setBusy($("body"));
        $('.Report')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceAttributesMappingRequestIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                //abp.ui.clearBusy($("body"));
            });
    };

    var loadXRoadServiceAttributesMappingRequestView = function (attributeId) {
        var _args = {};
        var options = {
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/IndexRequest?attributeId=' + attributeId,
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributeMappings/_IndexRequest.js',
        };

        abp.ui.setBusy($("body"));
        $('.XRoadServiceAttributeMappingsIndexRequest')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    abp.ui.clearBusy($("body"));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceAttributesMappingRequestIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                abp.ui.clearBusy($("body"));
            });
    };
    var loadXRoadServiceAttributesMappingResponseView = function (attributeId) {
        var _args = {};
        var options = {
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/IndexResponse?attributeId=' + attributeId,
            scriptUrl: abp.appPath + 'view-resources/Areas/eGov/Views/XRoadServiceAttributeMappings/_IndexResponse.js',
        };

        abp.ui.setBusy($("body"));
        $('.XRoadServiceAttributeMappingsIndexResponse')
            .load(options.viewUrl, _args, function (response, status, xhr) {
                if (status == "error") {
                    abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                    abp.ui.clearBusy($("body"));
                    return;
                };

                if (options.scriptUrl) {
                    app.ResourceLoader.loadScript(options.scriptUrl, function () {
                        //EligibilityDimensionsPartialIndex
                        var modalClass = app.modals["XRoadServiceAttributesMappingResponseIndex"];
                        if (modalClass) {
                            var _modalObject = new modalClass();
                            //if (_modalObject.init) {
                            //    _modalObject.init(_publicApi, _args);
                            //}
                        }
                    });
                } else {
                    //_initAndShowModal();
                }

                abp.ui.clearBusy($("body"));
            });
    };

    _$xRoadServiceInformationForm.submit(function (event) {
        event.preventDefault();
        if (!_$xRoadServiceInformationForm.valid()) {
            return;
        }

        var xRoadService = _$xRoadServiceInformationForm.serializeFormToObject();

        abp.ui.setBusy();
        _xRoadServicesService.createOrEdit(
            xRoadService
        ).done(function (data) {
            abp.notify.info(app.localize('SavedSuccessfully'));
            //_modalManager.close();
            //abp.event.trigger('app.createOrEditXRoadServiceModalSaved');
            _$xRoadServiceInformationForm.find('input[name="id"]').val(data.id);
            console.log("_xRoadServicesService.createOrEdit");
            loadXRoadServiceAttributesRequestView(data.id);
            loadXRoadServiceAttributesResponseView(data.id);
            loadXRoadServiceErrors(data.id);
            $('.XRoadServiceAttributeMappingsIndexRequest').empty();
            $('.XRoadServiceAttributeMappingsIndexResponse').empty();
            
            

        }).always(function () {
            abp.ui.clearBusy();
        });

    });
    
    abp.event.on('app.XRoadServiceAttributeViewRequest', function (obj) {
        //alert(JSON.stringify(obj))
        //loadPovertyDimensionRanges(obj.id);
        //loadEligibilityIndicators(obj.id);
        //$('.eligibilityIndicatorCriteriasSection').empty();
        loadXRoadServiceAttributesMappingRequestView(obj.id);

    });
    abp.event.on('app.XRoadServiceAttributeViewResponse', function (obj) {
        //alert(JSON.stringify(obj))
        //loadPovertyDimensionRanges(obj.id);
        //loadEligibilityIndicators(obj.id);
        //$('.eligibilityIndicatorCriteriasSection').empty();
        loadXRoadServiceAttributesMappingResponseView(obj.id);

    });
    abp.event.on('app.createOrEditXRoadServiceAttributeModalSavedRequest', function () {
        $('.XRoadServiceAttributeMappingsIndexRequest').empty();
    });
    abp.event.on('app.createOrEditXRoadServiceAttributeModalSavedResponse', function () {
        $('.XRoadServiceAttributeMappingsIndexResponse').empty();
    });
    
    if (_$xRoadServiceInformationForm.find('input[name="id"]').val()) {
        loadXRoadServiceAttributesRequestView(_$xRoadServiceInformationForm.find('input[name="id"]').val());
        loadXRoadServiceAttributesResponseView(_$xRoadServiceInformationForm.find('input[name="id"]').val());
        loadXRoadServiceErrors(_$xRoadServiceInformationForm.find('input[name="id"]').val());
    }
    //loadReport();
})(jQuery);

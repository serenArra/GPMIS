(function () {
    $(function () {
        
        var _xRoadServicesService = abp.services.app.xRoadServices;

        var _$InformationBankForm = null;
        _$InformationBankForm = $('form[name=InformationBankForm]');// _modalManager.getModal().find('form[name=XRoadServiceInformationsForm]');
        _$InformationBankForm.validate(
            {
                rules: {
                    identificationDocumentNo: {
                        required: true,
                        maxLength: '#identificationTypeId',
                        minLength: '#identificationTypeId',
                        validateIdentificationDocumentNo: true, validateIdentificationDocumentNo: '#identificationTypeId',
                    }
                },
                messages: {
                    identificationDocumentNo: {
                        required: app.localize('Required'),
                        maxLength: app.localize('maxlength', 9),
                        minLength: app.localize('minlength', 9),
                        validateIdentificationDocumentNo: app.localize('NotValidIdentificationDocumentNo'),
                    }
                }
            }
        );        

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServices.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServices.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServices.Delete')
        };
        $(".select2").select2({
            width: "100%"
        }).on('select2:select', function (e) {

            $(this).valid();

        });

        $("#identificationTypeId").on('change', function (e) {
            if ($("#identificationTypeId").val() == IdType.PS) {
                $('#btnFetchPersonInformationBank').show();
            }
            else {
                $('#btnFetchPersonInformationBank').hide();
            }
        });



        var loadInformationBank = function (identificationTypeId, identificationDocumentNo) {

            var _args = {};
            //return;
            var options = {
                viewUrl: abp.appPath + 'App/XRoadServices/ViewInformationBank?identificationTypeId='
                    + identificationTypeId + "&identificationDocumentNo=" + identificationDocumentNo,
                scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServices/_ViewInformationBank.js',
            };

            abp.ui.setBusy($("body"));
            $('.informationBankView')
                .load(options.viewUrl, _args, function (response, status, xhr) {
                    if (status == "error") {
                        abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                        abp.ui.clearBusy($("body"));
                        return;
                    };

                    if (options.scriptUrl) {
                        app.ResourceLoader.loadScript(options.scriptUrl, function () {
                            //EligibilityDimensionsPartialIndex
                            var modalClass = app.modals["ViewInformationBankModal"];
                            if (modalClass) {
                                console.log("inside app.ResourceLoader.loadScript");
                                var _modalObject = new modalClass();
                                _modalObject.initData();
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






        $('#CreateNewXRoadServiceButton').click(function () {
            //_createOrEditModal.open();
            location.href = abp.appPath + 'App/XRoadServices/createoredit';
        });

      
      

        $('#GetXRoadServicesButton').click(function (e) {
            e.preventDefault();
            getXRoadServices();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                //getXRoadServices();
            }
        });


        $("#btnFetchPerson").click(function (event) {
            event.preventDefault();
            var idNo = $("#Person_IdentificationDocumentNo").val();
            // var idType = $("#identificationTypeId").val();
            if ($("#identificationTypeId").valid()) {
                if (($("#identificationTypeId").val() == IdType.PS || $("#identificationTypeId").val() == IdType.IL) && !$("#Person_IdentificationDocumentNo").valid()) {
                    return;
                }
                if (idNo == $("#Person_IdentificationDocumentNo2").val()) {
                    abp.notify.error(app.localize('ErrorFetchPerson'));
                    return;
                }
                abp.ui.setBusy();

                _applicantsService.fetchPerson(
                    {
                        identificationDocumentNoId: $("#Person_IdentificationDocumentNo").val(),
                        identificationDocumentNoTypeId: $("#identificationTypeId").val()
                    }
                ).done(function (data) {
                }).fail(function (error) {
                    abp.notify.error(app.localize('ErrorFetchPerson'));
                }).always(function () {
                    abp.ui.clearBusy();
                });
            }

            //  }
        });

        _$InformationBankForm.submit(function (event) {
            event.preventDefault();
            if (!_$InformationBankForm.valid()) {
                return;
            }

            var idNo = $("#Person_IdentificationDocumentNo").val();
            var idType = $("#identificationTypeId").val();
            loadInformationBank(idType, idNo);

        });
    });
})();

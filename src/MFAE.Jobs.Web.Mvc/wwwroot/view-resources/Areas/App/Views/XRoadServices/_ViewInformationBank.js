(function ($) {
    app.modals.ViewInformationBankModal = function () {

        var _xRoadServicesService = abp.services.app.xRoadServices;

        var _modalManager;

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
           
        };
        this.initData = function () {
            console.log("in initData")
            var idNo = $("#InformationBank_IdentificationDocumentNo").val();
            var idType = $("#InformationBank_identificationTypeId").val();
            loadCitizensListWithCodes(idType, idNo);
            loadPassportInfo(idType, idNo);
            loadCitizenPhoto(idType, idNo);
        };


        var loadCitizensListWithCodes = function (identificationTypeId, identificationDocumentNo) {
            if ($(".CitizensListWithCodesSection").length) {
                var _args = {};
                var options = {
                    viewUrl: abp.appPath + 'App/XRoadServices/ViewInformationBankCitizensListWithCodes?identificationTypeId='
                        + identificationTypeId + "&identificationDocumentNo=" + identificationDocumentNo,                    
                };

                abp.ui.setBusy($(".CitizensListWithCodesSection"));
                $('.CitizensListWithCodesSection')
                    .load(options.viewUrl, _args, function (response, status, xhr) {
                        if (status == "error") {
                            abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                            abp.ui.clearBusy($(".CitizensListWithCodesSection"));
                            return;
                        };                        
                        abp.ui.clearBusy($(".CitizensListWithCodesSection"));
                    });
            }
        };

        var loadPassportInfo = function (identificationTypeId, identificationDocumentNo) {
            if ($(".PassportInfoSection").length) {
                var _args = {};
                var options = {
                    viewUrl: abp.appPath + 'App/XRoadServices/ViewInformationBankPassportInfo?identificationTypeId='
                        + identificationTypeId + "&identificationDocumentNo=" + identificationDocumentNo,
                    
                };

                abp.ui.setBusy($(".PassportInfoSection"));
                $('.PassportInfoSection')
                    .load(options.viewUrl, _args, function (response, status, xhr) {
                        if (status == "error") {
                            abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                            abp.ui.clearBusy($(".PassportInfoSection"));
                            return;
                        };
                        
                        abp.ui.clearBusy($(".PassportInfoSection"));
                    });
            }
        };


        var loadCitizenPhoto = function (identificationTypeId, identificationDocumentNo) {
            if ($(".CitizenPictureSection").length) {
                var _args = {};
                var options = {
                    viewUrl: abp.appPath + 'App/XRoadServices/ViewInformationCitizenPicture?identificationTypeId='
                        + identificationTypeId + "&identificationDocumentNo=" + identificationDocumentNo,
                    
                };

                abp.ui.setBusy($(".CitizenPictureSection"));
                $('.CitizenPictureSection')
                    .load(options.viewUrl, _args, function (response, status, xhr) {
                        if (status == "error") {
                            abp.message.warn(abp.localization.abpWeb('InternalServerError'));
                            abp.ui.clearBusy($(".CitizenPictureSection"));
                            return;
                        };
                        
                        abp.ui.clearBusy($(".CitizenPictureSection"));
                    });
            }
        };
    };
})(jQuery);
(function ($) {
    app.modals.CreateOrEditXRoadServiceErrorModal = function () {

        var _xRoadServiceErrorsService = abp.services.app.xRoadServiceErrors;

        var _modalManager;
        var _$xRoadServiceErrorInformationForm = null;

		        var _XRoadServiceErrorxRoadServiceLookupTableModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadServiceErrors/XRoadServiceLookupTableModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceErrors/_XRoadServiceErrorXRoadServiceLookupTableModal.js',
            modalClass: 'XRoadServiceLookupTableModal'
        });

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            

            _$xRoadServiceErrorInformationForm = _modalManager.getModal().find('form[name=XRoadServiceErrorInformationsForm]');
            _$xRoadServiceErrorInformationForm.validate();
        };

		          $('#OpenXRoadServiceLookupTableButton').click(function () {

            var xRoadServiceError = _$xRoadServiceErrorInformationForm.serializeFormToObject();

            _XRoadServiceErrorxRoadServiceLookupTableModal.open({ id: xRoadServiceError.xRoadServiceId, displayName: xRoadServiceError.xRoadServiceName }, function (data) {
                _$xRoadServiceErrorInformationForm.find('input[name=xRoadServiceName]').val(data.displayName); 
                _$xRoadServiceErrorInformationForm.find('input[name=xRoadServiceId]').val(data.id); 
            });
        });
		
		$('#ClearXRoadServiceNameButton').click(function () {
                _$xRoadServiceErrorInformationForm.find('input[name=xRoadServiceName]').val(''); 
                _$xRoadServiceErrorInformationForm.find('input[name=xRoadServiceId]').val(''); 
        });
		


        this.save = function () {
            if (!_$xRoadServiceErrorInformationForm.valid()) {
                return;
            }
            if ($('#XRoadServiceError_XRoadServiceId').prop('required') && $('#XRoadServiceError_XRoadServiceId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('XRoadService')));
                return;
            }

            var xRoadServiceError = _$xRoadServiceErrorInformationForm.serializeFormToObject();
			
			 _modalManager.setBusy(true);
			 _xRoadServiceErrorsService.createOrEdit(
				xRoadServiceError
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditXRoadServiceErrorModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
    };
})(jQuery);
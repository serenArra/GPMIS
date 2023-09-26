(function ($) {
    app.modals.CreateOrEditXRoadServiceAttributeModal = function () {

        var _xRoadServiceAttributesService = abp.services.app.xRoadServiceAttributes;

        var _modalManager;
        var _$xRoadServiceAttributeInformationForm = null;

		        var _XRoadServiceAttributexRoadServiceLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/XRoadServiceLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributes/_XRoadServiceAttributeXRoadServiceLookupTableModal.js',
            modalClass: 'XRoadServiceLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$xRoadServiceAttributeInformationForm = _modalManager.getModal().find('form[name=XRoadServiceAttributeInformationsForm]');
            _$xRoadServiceAttributeInformationForm.validate();
        };

		          $('#OpenXRoadServiceLookupTableButton').click(function () {

            var xRoadServiceAttribute = _$xRoadServiceAttributeInformationForm.serializeFormToObject();

            _XRoadServiceAttributexRoadServiceLookupTableModal.open({ id: xRoadServiceAttribute.xRoadServiceID, displayName: xRoadServiceAttribute.xRoadServiceName }, function (data) {
                _$xRoadServiceAttributeInformationForm.find('input[name=xRoadServiceName]').val(data.displayName); 
                _$xRoadServiceAttributeInformationForm.find('input[name=xRoadServiceID]').val(data.id); 
            });
        });
		
		$('#ClearXRoadServiceNameButton').click(function () {
                _$xRoadServiceAttributeInformationForm.find('input[name=xRoadServiceName]').val(''); 
                _$xRoadServiceAttributeInformationForm.find('input[name=xRoadServiceID]').val(''); 
        });
		


        this.save = function () {
            if (!_$xRoadServiceAttributeInformationForm.valid()) {
                return;
            }
            if ($('#XRoadServiceAttribute_XRoadServiceID').prop('required') && $('#XRoadServiceAttribute_XRoadServiceID').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('XRoadService')));
                return;
            }

            

            var xRoadServiceAttribute = _$xRoadServiceAttributeInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _xRoadServiceAttributesService.createOrEdit(
				xRoadServiceAttribute
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditXRoadServiceAttributeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
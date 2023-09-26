(function ($) {
    app.modals.CreateOrEditXRoadServiceAttributeMappingModal = function () {

        var _xRoadServiceAttributeMappingsService = abp.services.app.xRoadServiceAttributeMappings;

        var _modalManager;
        var _$xRoadServiceAttributeMappingInformationForm = null;

		        var _XRoadServiceAttributeMappingxRoadServiceAttributeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/XRoadServiceAttributeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributeMappings/_XRoadServiceAttributeMappingXRoadServiceAttributeLookupTableModal.js',
            modalClass: 'XRoadServiceAttributeLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$xRoadServiceAttributeMappingInformationForm = _modalManager.getModal().find('form[name=XRoadServiceAttributeMappingInformationsForm]');
            _$xRoadServiceAttributeMappingInformationForm.validate();
        };

		          $('#OpenXRoadServiceAttributeLookupTableButton').click(function () {

            var xRoadServiceAttributeMapping = _$xRoadServiceAttributeMappingInformationForm.serializeFormToObject();

            _XRoadServiceAttributeMappingxRoadServiceAttributeLookupTableModal.open({ id: xRoadServiceAttributeMapping.attributeID, displayName: xRoadServiceAttributeMapping.xRoadServiceAttributeName }, function (data) {
                _$xRoadServiceAttributeMappingInformationForm.find('input[name=xRoadServiceAttributeName]').val(data.displayName); 
                _$xRoadServiceAttributeMappingInformationForm.find('input[name=attributeID]').val(data.id); 
            });
        });
		
		$('#ClearXRoadServiceAttributeNameButton').click(function () {
                _$xRoadServiceAttributeMappingInformationForm.find('input[name=xRoadServiceAttributeName]').val(''); 
                _$xRoadServiceAttributeMappingInformationForm.find('input[name=attributeID]').val(''); 
        });
		


        this.save = function () {
            if (!_$xRoadServiceAttributeMappingInformationForm.valid()) {
                return;
            }
            if ($('#XRoadServiceAttributeMapping_AttributeID').prop('required') && $('#XRoadServiceAttributeMapping_AttributeID').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('XRoadServiceAttribute')));
                return;
            }

            

            var xRoadServiceAttributeMapping = _$xRoadServiceAttributeMappingInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _xRoadServiceAttributeMappingsService.createOrEdit(
				xRoadServiceAttributeMapping
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditXRoadServiceAttributeMappingModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
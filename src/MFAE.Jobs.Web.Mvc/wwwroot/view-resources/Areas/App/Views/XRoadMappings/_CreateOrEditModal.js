(function ($) {
    app.modals.CreateOrEditXRoadMappingModal = function () {

        var _xRoadMappingsService = abp.services.app.xRoadMappings;

        var _modalManager;
        var _$xRoadMappingInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$xRoadMappingInformationForm = _modalManager.getModal().find('form[name=XRoadMappingInformationsForm]');
            _$xRoadMappingInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$xRoadMappingInformationForm.valid()) {
                return;
            }

            

            var xRoadMapping = _$xRoadMappingInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _xRoadMappingsService.createOrEdit(
				xRoadMapping
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditXRoadMappingModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
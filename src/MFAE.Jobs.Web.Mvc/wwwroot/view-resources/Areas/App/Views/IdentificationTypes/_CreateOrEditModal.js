(function ($) {
    app.modals.CreateOrEditIdentificationTypeModal = function () {

        var _identificationTypesService = abp.services.app.identificationTypes;

        var _modalManager;
        var _$identificationTypeInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$identificationTypeInformationForm = _modalManager.getModal().find('form[name=IdentificationTypeInformationsForm]');
            _$identificationTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$identificationTypeInformationForm.valid()) {
                return;
            }

            

            var identificationType = _$identificationTypeInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _identificationTypesService.createOrEdit(
				identificationType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditIdentificationTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
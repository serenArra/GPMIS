(function ($) {
    app.modals.CreateOrEditSpecialtiesModal = function () {

        var _specialtiesesService = abp.services.app.specialtieses;

        var _modalManager;
        var _$specialtiesInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$specialtiesInformationForm = _modalManager.getModal().find('form[name=SpecialtiesInformationsForm]');
            _$specialtiesInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$specialtiesInformationForm.valid()) {
                return;
            }

            

            var specialties = _$specialtiesInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _specialtiesesService.createOrEdit(
				specialties
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditSpecialtiesModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
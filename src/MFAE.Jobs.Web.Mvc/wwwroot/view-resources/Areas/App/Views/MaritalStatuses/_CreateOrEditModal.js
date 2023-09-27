(function ($) {
    app.modals.CreateOrEditMaritalStatusModal = function () {

        var _maritalStatusesService = abp.services.app.maritalStatuses;

        var _modalManager;
        var _$maritalStatusInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$maritalStatusInformationForm = _modalManager.getModal().find('form[name=MaritalStatusInformationsForm]');
            _$maritalStatusInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$maritalStatusInformationForm.valid()) {
                return;
            }

            

            var maritalStatus = _$maritalStatusInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _maritalStatusesService.createOrEdit(
				maritalStatus
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditMaritalStatusModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
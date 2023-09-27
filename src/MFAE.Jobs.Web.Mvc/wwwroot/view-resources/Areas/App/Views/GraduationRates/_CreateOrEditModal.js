(function ($) {
    app.modals.CreateOrEditGraduationRateModal = function () {

        var _graduationRatesService = abp.services.app.graduationRates;

        var _modalManager;
        var _$graduationRateInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$graduationRateInformationForm = _modalManager.getModal().find('form[name=GraduationRateInformationsForm]');
            _$graduationRateInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$graduationRateInformationForm.valid()) {
                return;
            }

            

            var graduationRate = _$graduationRateInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _graduationRatesService.createOrEdit(
				graduationRate
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditGraduationRateModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
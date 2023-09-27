(function ($) {
    app.modals.CreateOrEditJobAdvertisementModal = function () {

        var _jobAdvertisementsService = abp.services.app.jobAdvertisements;

        var _modalManager;
        var _$jobAdvertisementInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').datetimepicker({
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$jobAdvertisementInformationForm = _modalManager.getModal().find('form[name=JobAdvertisementInformationsForm]');
            _$jobAdvertisementInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$jobAdvertisementInformationForm.valid()) {
                return;
            }

            

            var jobAdvertisement = _$jobAdvertisementInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _jobAdvertisementsService.createOrEdit(
				jobAdvertisement
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditJobAdvertisementModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
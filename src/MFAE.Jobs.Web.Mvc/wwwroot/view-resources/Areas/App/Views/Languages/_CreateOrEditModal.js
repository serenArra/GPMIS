(function ($) {
    app.modals.CreateOrEditLanguageModal = function () {

        var _languagesService = abp.services.app.languages;

        var _modalManager;
        var _$languageInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$languageInformationForm = _modalManager.getModal().find('form[name=LanguageInformationsForm]');
            _$languageInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$languageInformationForm.valid()) {
                return;
            }

            

            var language = _$languageInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _languagesService.createOrEdit(
				language
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLanguageModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
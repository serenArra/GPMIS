(function ($) {
    app.modals.CreateOrEditConversationRateModal = function () {

        var _conversationRatesService = abp.services.app.conversationRates;

        var _modalManager;
        var _$conversationRateInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$conversationRateInformationForm = _modalManager.getModal().find('form[name=ConversationRateInformationsForm]');
            _$conversationRateInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$conversationRateInformationForm.valid()) {
                return;
            }

            

            var conversationRate = _$conversationRateInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _conversationRatesService.createOrEdit(
				conversationRate
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditConversationRateModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
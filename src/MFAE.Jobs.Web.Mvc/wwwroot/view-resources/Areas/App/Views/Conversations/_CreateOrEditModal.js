(function ($) {
    app.modals.CreateOrEditConversationModal = function () {

        var _conversationsService = abp.services.app.conversations;

        var _modalManager;
        var _$conversationInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$conversationInformationForm = _modalManager.getModal().find('form[name=ConversationInformationsForm]');
            _$conversationInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$conversationInformationForm.valid()) {
                return;
            }

            

            var conversation = _$conversationInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _conversationsService.createOrEdit(
				conversation
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditConversationModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
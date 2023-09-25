(function ($) {
    app.modals.CreateOrEditAttachmentEntityTypeModal = function () {

        var _attachmentEntityTypesService = abp.services.app.attachmentEntityTypes;

        var _modalManager;
        var _$attachmentEntityTypeInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentEntityTypeInformationForm = _modalManager.getModal().find('form[name=AttachmentEntityTypeInformationsForm]');
            _$attachmentEntityTypeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$attachmentEntityTypeInformationForm.valid()) {
                return;
            }

            

            var attachmentEntityType = _$attachmentEntityTypeInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _attachmentEntityTypesService.createOrEdit(
				attachmentEntityType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentEntityTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
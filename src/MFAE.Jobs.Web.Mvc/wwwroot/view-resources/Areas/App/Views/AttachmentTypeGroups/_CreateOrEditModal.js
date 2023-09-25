(function ($) {
    app.modals.CreateOrEditAttachmentTypeGroupModal = function () {

        var _attachmentTypeGroupsService = abp.services.app.attachmentTypeGroups;

        var _modalManager;
        var _$attachmentTypeGroupInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentTypeGroupInformationForm = _modalManager.getModal().find('form[name=AttachmentTypeGroupInformationsForm]');
            _$attachmentTypeGroupInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$attachmentTypeGroupInformationForm.valid()) {
                return;
            }

            

            var attachmentTypeGroup = _$attachmentTypeGroupInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _attachmentTypeGroupsService.createOrEdit(
				attachmentTypeGroup
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentTypeGroupModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
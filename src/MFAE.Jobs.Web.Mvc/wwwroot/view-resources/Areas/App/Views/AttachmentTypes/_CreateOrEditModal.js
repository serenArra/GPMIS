(function ($) {
    app.modals.CreateOrEditAttachmentTypeModal = function () {

        var _attachmentTypesService = abp.services.app.attachmentTypes;

        var _modalManager;
        var _$attachmentTypeInformationForm = null;

		        var _AttachmentTypeattachmentEntityTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentTypes/AttachmentEntityTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentTypes/_AttachmentTypeAttachmentEntityTypeLookupTableModal.js',
            modalClass: 'AttachmentEntityTypeLookupTableModal'
        });        var _AttachmentTypeattachmentTypeGroupLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentTypes/AttachmentTypeGroupLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentTypes/_AttachmentTypeAttachmentTypeGroupLookupTableModal.js',
            modalClass: 'AttachmentTypeGroupLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$attachmentTypeInformationForm = _modalManager.getModal().find('form[name=AttachmentTypeInformationsForm]');
            _$attachmentTypeInformationForm.validate();
        };

		          $('#OpenAttachmentEntityTypeLookupTableButton').click(function () {

            var attachmentType = _$attachmentTypeInformationForm.serializeFormToObject();

            _AttachmentTypeattachmentEntityTypeLookupTableModal.open({ id: attachmentType.entityTypeId, displayName: attachmentType.attachmentEntityTypeName }, function (data) {
                _$attachmentTypeInformationForm.find('input[name=attachmentEntityTypeName]').val(data.displayName); 
                _$attachmentTypeInformationForm.find('input[name=entityTypeId]').val(data.id); 
            });
        });
		
		$('#ClearAttachmentEntityTypeNameButton').click(function () {
                _$attachmentTypeInformationForm.find('input[name=attachmentEntityTypeName]').val(''); 
                _$attachmentTypeInformationForm.find('input[name=entityTypeId]').val(''); 
        });
		
        $('#OpenAttachmentTypeGroupLookupTableButton').click(function () {

            var attachmentType = _$attachmentTypeInformationForm.serializeFormToObject();

            _AttachmentTypeattachmentTypeGroupLookupTableModal.open({ id: attachmentType.groupId, displayName: attachmentType.attachmentTypeGroupName }, function (data) {
                _$attachmentTypeInformationForm.find('input[name=attachmentTypeGroupName]').val(data.displayName); 
                _$attachmentTypeInformationForm.find('input[name=groupId]').val(data.id); 
            });
        });
		
		$('#ClearAttachmentTypeGroupNameButton').click(function () {
                _$attachmentTypeInformationForm.find('input[name=attachmentTypeGroupName]').val(''); 
                _$attachmentTypeInformationForm.find('input[name=groupId]').val(''); 
        });
		


        this.save = function () {
            if (!_$attachmentTypeInformationForm.valid()) {
                return;
            }
            if ($('#AttachmentType_EntityTypeId').prop('required') && $('#AttachmentType_EntityTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AttachmentEntityType')));
                return;
            }
            if ($('#AttachmentType_GroupId').prop('required') && $('#AttachmentType_GroupId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AttachmentTypeGroup')));
                return;
            }

            

            var attachmentType = _$attachmentTypeInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _attachmentTypesService.createOrEdit(
				attachmentType
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAttachmentTypeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
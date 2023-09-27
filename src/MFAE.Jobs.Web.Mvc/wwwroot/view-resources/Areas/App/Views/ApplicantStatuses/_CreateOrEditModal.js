(function ($) {
    app.modals.CreateOrEditApplicantStatusModal = function () {

        var _applicantStatusesService = abp.services.app.applicantStatuses;

        var _modalManager;
        var _$applicantStatusInformationForm = null;

		        var _ApplicantStatusapplicantLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStatuses/ApplicantLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStatuses/_ApplicantStatusApplicantLookupTableModal.js',
            modalClass: 'ApplicantLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$applicantStatusInformationForm = _modalManager.getModal().find('form[name=ApplicantStatusInformationsForm]');
            _$applicantStatusInformationForm.validate();
        };

		          $('#OpenApplicantLookupTableButton').click(function () {

            var applicantStatus = _$applicantStatusInformationForm.serializeFormToObject();

            _ApplicantStatusapplicantLookupTableModal.open({ id: applicantStatus.applicantId, displayName: applicantStatus.applicantFullName }, function (data) {
                _$applicantStatusInformationForm.find('input[name=applicantFullName]').val(data.displayName); 
                _$applicantStatusInformationForm.find('input[name=applicantId]').val(data.id); 
            });
        });
		
		$('#ClearApplicantFullNameButton').click(function () {
                _$applicantStatusInformationForm.find('input[name=applicantFullName]').val(''); 
                _$applicantStatusInformationForm.find('input[name=applicantId]').val(''); 
        });
		


        this.save = function () {
            if (!_$applicantStatusInformationForm.valid()) {
                return;
            }
            if ($('#ApplicantStatus_ApplicantId').prop('required') && $('#ApplicantStatus_ApplicantId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Applicant')));
                return;
            }

            

            var applicantStatus = _$applicantStatusInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _applicantStatusesService.createOrEdit(
				applicantStatus
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditApplicantStatusModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
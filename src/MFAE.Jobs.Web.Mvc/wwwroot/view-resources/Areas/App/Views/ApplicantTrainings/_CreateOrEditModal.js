(function ($) {
    app.modals.CreateOrEditApplicantTrainingModal = function () {

        var _applicantTrainingsService = abp.services.app.applicantTrainings;

        var _modalManager;
        var _$applicantTrainingInformationForm = null;

		        var _ApplicantTrainingapplicantLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantTrainings/ApplicantLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantTrainings/_ApplicantTrainingApplicantLookupTableModal.js',
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

            _$applicantTrainingInformationForm = _modalManager.getModal().find('form[name=ApplicantTrainingInformationsForm]');
            _$applicantTrainingInformationForm.validate();
        };

		          $('#OpenApplicantLookupTableButton').click(function () {

            var applicantTraining = _$applicantTrainingInformationForm.serializeFormToObject();

            _ApplicantTrainingapplicantLookupTableModal.open({ id: applicantTraining.applicantId, displayName: applicantTraining.applicantFirstName }, function (data) {
                _$applicantTrainingInformationForm.find('input[name=applicantFirstName]').val(data.displayName); 
                _$applicantTrainingInformationForm.find('input[name=applicantId]').val(data.id); 
            });
        });
		
		$('#ClearApplicantFirstNameButton').click(function () {
                _$applicantTrainingInformationForm.find('input[name=applicantFirstName]').val(''); 
                _$applicantTrainingInformationForm.find('input[name=applicantId]').val(''); 
        });
		


        this.save = function () {
            if (!_$applicantTrainingInformationForm.valid()) {
                return;
            }
            if ($('#ApplicantTraining_ApplicantId').prop('required') && $('#ApplicantTraining_ApplicantId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Applicant')));
                return;
            }

            

            var applicantTraining = _$applicantTrainingInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _applicantTrainingsService.createOrEdit(
				applicantTraining
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditApplicantTrainingModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
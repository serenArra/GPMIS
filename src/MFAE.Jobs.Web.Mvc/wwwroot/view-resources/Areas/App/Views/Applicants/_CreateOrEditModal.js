﻿(function ($) {
    app.modals.CreateOrEditApplicantModal = function () {

        var _applicantsService = abp.services.app.applicants;

        var _modalManager;
        var _$applicantInformationForm = null;

		        var _ApplicantidentificationTypeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/IdentificationTypeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantIdentificationTypeLookupTableModal.js',
            modalClass: 'IdentificationTypeLookupTableModal'
        });        var _ApplicantmaritalStatusLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/MaritalStatusLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantMaritalStatusLookupTableModal.js',
            modalClass: 'MaritalStatusLookupTableModal'
        });        var _ApplicantuserLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/UserLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantUserLookupTableModal.js',
            modalClass: 'UserLookupTableModal'
        });        var _ApplicantapplicantStatusLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/ApplicantStatusLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantApplicantStatusLookupTableModal.js',
            modalClass: 'ApplicantStatusLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$applicantInformationForm = _modalManager.getModal().find('form[name=ApplicantInformationsForm]');
            _$applicantInformationForm.validate();
        };

		          $('#OpenIdentificationTypeLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantidentificationTypeLookupTableModal.open({ id: applicant.identificationTypeId, displayName: applicant.identificationTypeName }, function (data) {
                _$applicantInformationForm.find('input[name=identificationTypeName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=identificationTypeId]').val(data.id); 
            });
        });
		
		$('#ClearIdentificationTypeNameButton').click(function () {
                _$applicantInformationForm.find('input[name=identificationTypeName]').val(''); 
                _$applicantInformationForm.find('input[name=identificationTypeId]').val(''); 
        });
		
        $('#OpenMaritalStatusLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantmaritalStatusLookupTableModal.open({ id: applicant.maritalStatusId, displayName: applicant.maritalStatusName }, function (data) {
                _$applicantInformationForm.find('input[name=maritalStatusName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=maritalStatusId]').val(data.id); 
            });
        });
		
		$('#ClearMaritalStatusNameButton').click(function () {
                _$applicantInformationForm.find('input[name=maritalStatusName]').val(''); 
                _$applicantInformationForm.find('input[name=maritalStatusId]').val(''); 
        });
		
        $('#OpenUserLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantuserLookupTableModal.open({ id: applicant.lockedBy, displayName: applicant.userName }, function (data) {
                _$applicantInformationForm.find('input[name=userName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=lockedBy]').val(data.id); 
            });
        });
		
		$('#ClearUserNameButton').click(function () {
                _$applicantInformationForm.find('input[name=userName]').val(''); 
                _$applicantInformationForm.find('input[name=lockedBy]').val(''); 
        });
		
        $('#OpenApplicantStatusLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantapplicantStatusLookupTableModal.open({ id: applicant.currentStatusId, displayName: applicant.applicantStatusDescription }, function (data) {
                _$applicantInformationForm.find('input[name=applicantStatusDescription]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=currentStatusId]').val(data.id); 
            });
        });
		
		$('#ClearApplicantStatusDescriptionButton').click(function () {
                _$applicantInformationForm.find('input[name=applicantStatusDescription]').val(''); 
                _$applicantInformationForm.find('input[name=currentStatusId]').val(''); 
        });
		


        this.save = function () {
            if (!_$applicantInformationForm.valid()) {
                return;
            }
            if ($('#Applicant_IdentificationTypeId').prop('required') && $('#Applicant_IdentificationTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('IdentificationType')));
                return;
            }
            if ($('#Applicant_MaritalStatusId').prop('required') && $('#Applicant_MaritalStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MaritalStatus')));
                return;
            }
            if ($('#Applicant_LockedBy').prop('required') && $('#Applicant_LockedBy').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Applicant_CurrentStatusId').prop('required') && $('#Applicant_CurrentStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ApplicantStatus')));
                return;
            }

            

            var applicant = _$applicantInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _applicantsService.createOrEdit(
				applicant
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditApplicantModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
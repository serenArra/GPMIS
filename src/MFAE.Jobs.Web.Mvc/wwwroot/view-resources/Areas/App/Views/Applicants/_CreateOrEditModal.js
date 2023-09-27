(function ($) {
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
        });        var _ApplicantcountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });        var _ApplicantgovernorateLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/GovernorateLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantGovernorateLookupTableModal.js',
            modalClass: 'GovernorateLookupTableModal'
        });        var _ApplicantlocalityLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/LocalityLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_ApplicantLocalityLookupTableModal.js',
            modalClass: 'LocalityLookupTableModal'
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
		
        $('#OpenCountryLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantcountryLookupTableModal.open({ id: applicant.countryId, displayName: applicant.countryName }, function (data) {
                _$applicantInformationForm.find('input[name=countryName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryNameButton').click(function () {
                _$applicantInformationForm.find('input[name=countryName]').val(''); 
                _$applicantInformationForm.find('input[name=countryId]').val(''); 
        });
		
        $('#OpenGovernorateLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantgovernorateLookupTableModal.open({ id: applicant.governorateId, displayName: applicant.governorateName }, function (data) {
                _$applicantInformationForm.find('input[name=governorateName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=governorateId]').val(data.id); 
            });
        });
		
		$('#ClearGovernorateNameButton').click(function () {
                _$applicantInformationForm.find('input[name=governorateName]').val(''); 
                _$applicantInformationForm.find('input[name=governorateId]').val(''); 
        });
		
        $('#OpenLocalityLookupTableButton').click(function () {

            var applicant = _$applicantInformationForm.serializeFormToObject();

            _ApplicantlocalityLookupTableModal.open({ id: applicant.localityId, displayName: applicant.localityName }, function (data) {
                _$applicantInformationForm.find('input[name=localityName]').val(data.displayName); 
                _$applicantInformationForm.find('input[name=localityId]').val(data.id); 
            });
        });
		
		$('#ClearLocalityNameButton').click(function () {
                _$applicantInformationForm.find('input[name=localityName]').val(''); 
                _$applicantInformationForm.find('input[name=localityId]').val(''); 
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
            if ($('#Applicant_CountryId').prop('required') && $('#Applicant_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }
            if ($('#Applicant_GovernorateId').prop('required') && $('#Applicant_GovernorateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Governorate')));
                return;
            }
            if ($('#Applicant_LocalityId').prop('required') && $('#Applicant_LocalityId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locality')));
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
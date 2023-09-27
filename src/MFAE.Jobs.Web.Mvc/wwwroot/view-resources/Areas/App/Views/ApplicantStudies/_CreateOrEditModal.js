(function ($) {
    app.modals.CreateOrEditApplicantStudyModal = function () {

        var _applicantStudiesService = abp.services.app.applicantStudies;

        var _modalManager;
        var _$applicantStudyInformationForm = null;

		        var _ApplicantStudygraduationRateLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStudies/GraduationRateLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStudies/_ApplicantStudyGraduationRateLookupTableModal.js',
            modalClass: 'GraduationRateLookupTableModal'
        });        var _ApplicantStudyacademicDegreeLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStudies/AcademicDegreeLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStudies/_ApplicantStudyAcademicDegreeLookupTableModal.js',
            modalClass: 'AcademicDegreeLookupTableModal'
        });        var _ApplicantStudyspecialtiesLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStudies/SpecialtiesLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStudies/_ApplicantStudySpecialtiesLookupTableModal.js',
            modalClass: 'SpecialtiesLookupTableModal'
        });        var _ApplicantStudyapplicantLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStudies/ApplicantLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStudies/_ApplicantStudyApplicantLookupTableModal.js',
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

            _$applicantStudyInformationForm = _modalManager.getModal().find('form[name=ApplicantStudyInformationsForm]');
            _$applicantStudyInformationForm.validate();
        };

		          $('#OpenGraduationRateLookupTableButton').click(function () {

            var applicantStudy = _$applicantStudyInformationForm.serializeFormToObject();

            _ApplicantStudygraduationRateLookupTableModal.open({ id: applicantStudy.graduationRateId, displayName: applicantStudy.graduationRateName }, function (data) {
                _$applicantStudyInformationForm.find('input[name=graduationRateName]').val(data.displayName); 
                _$applicantStudyInformationForm.find('input[name=graduationRateId]').val(data.id); 
            });
        });
		
		$('#ClearGraduationRateNameButton').click(function () {
                _$applicantStudyInformationForm.find('input[name=graduationRateName]').val(''); 
                _$applicantStudyInformationForm.find('input[name=graduationRateId]').val(''); 
        });
		
        $('#OpenAcademicDegreeLookupTableButton').click(function () {

            var applicantStudy = _$applicantStudyInformationForm.serializeFormToObject();

            _ApplicantStudyacademicDegreeLookupTableModal.open({ id: applicantStudy.academicDegreeId, displayName: applicantStudy.academicDegreeName }, function (data) {
                _$applicantStudyInformationForm.find('input[name=academicDegreeName]').val(data.displayName); 
                _$applicantStudyInformationForm.find('input[name=academicDegreeId]').val(data.id); 
            });
        });
		
		$('#ClearAcademicDegreeNameButton').click(function () {
                _$applicantStudyInformationForm.find('input[name=academicDegreeName]').val(''); 
                _$applicantStudyInformationForm.find('input[name=academicDegreeId]').val(''); 
        });
		
        $('#OpenSpecialtiesLookupTableButton').click(function () {

            var applicantStudy = _$applicantStudyInformationForm.serializeFormToObject();

            _ApplicantStudyspecialtiesLookupTableModal.open({ id: applicantStudy.specialtiesId, displayName: applicantStudy.specialtiesName }, function (data) {
                _$applicantStudyInformationForm.find('input[name=specialtiesName]').val(data.displayName); 
                _$applicantStudyInformationForm.find('input[name=specialtiesId]').val(data.id); 
            });
        });
		
		$('#ClearSpecialtiesNameButton').click(function () {
                _$applicantStudyInformationForm.find('input[name=specialtiesName]').val(''); 
                _$applicantStudyInformationForm.find('input[name=specialtiesId]').val(''); 
        });
		
        $('#OpenApplicantLookupTableButton').click(function () {

            var applicantStudy = _$applicantStudyInformationForm.serializeFormToObject();

            _ApplicantStudyapplicantLookupTableModal.open({ id: applicantStudy.applicantId, displayName: applicantStudy.applicantFirstName }, function (data) {
                _$applicantStudyInformationForm.find('input[name=applicantFirstName]').val(data.displayName); 
                _$applicantStudyInformationForm.find('input[name=applicantId]').val(data.id); 
            });
        });
		
		$('#ClearApplicantFirstNameButton').click(function () {
                _$applicantStudyInformationForm.find('input[name=applicantFirstName]').val(''); 
                _$applicantStudyInformationForm.find('input[name=applicantId]').val(''); 
        });
		


        this.save = function () {
            if (!_$applicantStudyInformationForm.valid()) {
                return;
            }
            if ($('#ApplicantStudy_GraduationRateId').prop('required') && $('#ApplicantStudy_GraduationRateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('GraduationRate')));
                return;
            }
            if ($('#ApplicantStudy_AcademicDegreeId').prop('required') && $('#ApplicantStudy_AcademicDegreeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('AcademicDegree')));
                return;
            }
            if ($('#ApplicantStudy_SpecialtiesId').prop('required') && $('#ApplicantStudy_SpecialtiesId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Specialties')));
                return;
            }
            if ($('#ApplicantStudy_ApplicantId').prop('required') && $('#ApplicantStudy_ApplicantId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Applicant')));
                return;
            }

            

            var applicantStudy = _$applicantStudyInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _applicantStudiesService.createOrEdit(
				applicantStudy
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditApplicantStudyModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
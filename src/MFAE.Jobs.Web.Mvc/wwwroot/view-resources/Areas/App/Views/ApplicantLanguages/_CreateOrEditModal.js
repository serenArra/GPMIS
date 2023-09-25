(function ($) {
    app.modals.CreateOrEditApplicantLanguageModal = function () {

        var _applicantLanguagesService = abp.services.app.applicantLanguages;

        var _modalManager;
        var _$applicantLanguageInformationForm = null;

		        var _ApplicantLanguageapplicantLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantLanguages/ApplicantLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantLanguages/_ApplicantLanguageApplicantLookupTableModal.js',
            modalClass: 'ApplicantLookupTableModal'
        });        var _ApplicantLanguagelanguageLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantLanguages/LanguageLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantLanguages/_ApplicantLanguageLanguageLookupTableModal.js',
            modalClass: 'LanguageLookupTableModal'
        });        var _ApplicantLanguageconversationLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantLanguages/ConversationLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantLanguages/_ApplicantLanguageConversationLookupTableModal.js',
            modalClass: 'ConversationLookupTableModal'
        });        var _ApplicantLanguageconversationRateLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantLanguages/ConversationRateLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantLanguages/_ApplicantLanguageConversationRateLookupTableModal.js',
            modalClass: 'ConversationRateLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$applicantLanguageInformationForm = _modalManager.getModal().find('form[name=ApplicantLanguageInformationsForm]');
            _$applicantLanguageInformationForm.validate();
        };

		          $('#OpenApplicantLookupTableButton').click(function () {

            var applicantLanguage = _$applicantLanguageInformationForm.serializeFormToObject();

            _ApplicantLanguageapplicantLookupTableModal.open({ id: applicantLanguage.applicantId, displayName: applicantLanguage.applicantFirstName }, function (data) {
                _$applicantLanguageInformationForm.find('input[name=applicantFirstName]').val(data.displayName); 
                _$applicantLanguageInformationForm.find('input[name=applicantId]').val(data.id); 
            });
        });
		
		$('#ClearApplicantFirstNameButton').click(function () {
                _$applicantLanguageInformationForm.find('input[name=applicantFirstName]').val(''); 
                _$applicantLanguageInformationForm.find('input[name=applicantId]').val(''); 
        });
		
        $('#OpenLanguageLookupTableButton').click(function () {

            var applicantLanguage = _$applicantLanguageInformationForm.serializeFormToObject();

            _ApplicantLanguagelanguageLookupTableModal.open({ id: applicantLanguage.languageId, displayName: applicantLanguage.languageName }, function (data) {
                _$applicantLanguageInformationForm.find('input[name=languageName]').val(data.displayName); 
                _$applicantLanguageInformationForm.find('input[name=languageId]').val(data.id); 
            });
        });
		
		$('#ClearLanguageNameButton').click(function () {
                _$applicantLanguageInformationForm.find('input[name=languageName]').val(''); 
                _$applicantLanguageInformationForm.find('input[name=languageId]').val(''); 
        });
		
        $('#OpenConversationLookupTableButton').click(function () {

            var applicantLanguage = _$applicantLanguageInformationForm.serializeFormToObject();

            _ApplicantLanguageconversationLookupTableModal.open({ id: applicantLanguage.conversationId, displayName: applicantLanguage.conversationName }, function (data) {
                _$applicantLanguageInformationForm.find('input[name=conversationName]').val(data.displayName); 
                _$applicantLanguageInformationForm.find('input[name=conversationId]').val(data.id); 
            });
        });
		
		$('#ClearConversationNameButton').click(function () {
                _$applicantLanguageInformationForm.find('input[name=conversationName]').val(''); 
                _$applicantLanguageInformationForm.find('input[name=conversationId]').val(''); 
        });
		
        $('#OpenConversationRateLookupTableButton').click(function () {

            var applicantLanguage = _$applicantLanguageInformationForm.serializeFormToObject();

            _ApplicantLanguageconversationRateLookupTableModal.open({ id: applicantLanguage.conversationRateId, displayName: applicantLanguage.conversationRateName }, function (data) {
                _$applicantLanguageInformationForm.find('input[name=conversationRateName]').val(data.displayName); 
                _$applicantLanguageInformationForm.find('input[name=conversationRateId]').val(data.id); 
            });
        });
		
		$('#ClearConversationRateNameButton').click(function () {
                _$applicantLanguageInformationForm.find('input[name=conversationRateName]').val(''); 
                _$applicantLanguageInformationForm.find('input[name=conversationRateId]').val(''); 
        });
		


        this.save = function () {
            if (!_$applicantLanguageInformationForm.valid()) {
                return;
            }
            if ($('#ApplicantLanguage_ApplicantId').prop('required') && $('#ApplicantLanguage_ApplicantId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Applicant')));
                return;
            }
            if ($('#ApplicantLanguage_LanguageId').prop('required') && $('#ApplicantLanguage_LanguageId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Language')));
                return;
            }
            if ($('#ApplicantLanguage_ConversationId').prop('required') && $('#ApplicantLanguage_ConversationId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Conversation')));
                return;
            }
            if ($('#ApplicantLanguage_ConversationRateId').prop('required') && $('#ApplicantLanguage_ConversationRateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ConversationRate')));
                return;
            }

            

            var applicantLanguage = _$applicantLanguageInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _applicantLanguagesService.createOrEdit(
				applicantLanguage
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditApplicantLanguageModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
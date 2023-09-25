(function ($) {
    app.modals.CreateOrEditAcademicDegreeModal = function () {

        var _academicDegreesService = abp.services.app.academicDegrees;

        var _modalManager;
        var _$academicDegreeInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$academicDegreeInformationForm = _modalManager.getModal().find('form[name=AcademicDegreeInformationsForm]');
            _$academicDegreeInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$academicDegreeInformationForm.valid()) {
                return;
            }

            

            var academicDegree = _$academicDegreeInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _academicDegreesService.createOrEdit(
				academicDegree
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditAcademicDegreeModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
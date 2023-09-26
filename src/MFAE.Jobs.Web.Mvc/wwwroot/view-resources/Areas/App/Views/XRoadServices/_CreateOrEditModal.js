(function ($) {
    app.modals.CreateOrEditXRoadServiceModal = function () {

        var _xRoadServicesService = abp.services.app.xRoadServices;

        var _modalManager;
        var _$xRoadServiceInformationForm = null;

		
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$xRoadServiceInformationForm = _modalManager.getModal().find('form[name=XRoadServiceInformationsForm]');
            _$xRoadServiceInformationForm.validate();
        };

		  

        this.save = function () {
            if (!_$xRoadServiceInformationForm.valid()) {
                return;
            }

            

            var xRoadService = _$xRoadServiceInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _xRoadServicesService.createOrEdit(
				xRoadService
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditXRoadServiceModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
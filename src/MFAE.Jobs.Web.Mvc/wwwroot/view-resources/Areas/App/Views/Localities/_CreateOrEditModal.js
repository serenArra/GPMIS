(function ($) {
    app.modals.CreateOrEditLocalityModal = function () {

        var _localitiesService = abp.services.app.localities;

        var _modalManager;
        var _$localityInformationForm = null;

		        var _LocalitygovernorateLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Localities/GovernorateLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Localities/_LocalityGovernorateLookupTableModal.js',
            modalClass: 'GovernorateLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$localityInformationForm = _modalManager.getModal().find('form[name=LocalityInformationsForm]');
            _$localityInformationForm.validate();
        };

		          $('#OpenGovernorateLookupTableButton').click(function () {

            var locality = _$localityInformationForm.serializeFormToObject();

            _LocalitygovernorateLookupTableModal.open({ id: locality.governorateId, displayName: locality.governorateName }, function (data) {
                _$localityInformationForm.find('input[name=governorateName]').val(data.displayName); 
                _$localityInformationForm.find('input[name=governorateId]').val(data.id); 
            });
        });
		
		$('#ClearGovernorateNameButton').click(function () {
                _$localityInformationForm.find('input[name=governorateName]').val(''); 
                _$localityInformationForm.find('input[name=governorateId]').val(''); 
        });
		


        this.save = function () {
            if (!_$localityInformationForm.valid()) {
                return;
            }
            if ($('#Locality_GovernorateId').prop('required') && $('#Locality_GovernorateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Governorate')));
                return;
            }

            

            var locality = _$localityInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _localitiesService.createOrEdit(
				locality
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditLocalityModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
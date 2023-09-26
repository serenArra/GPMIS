(function ($) {
    app.modals.CreateOrEditGovernorateModal = function () {

        var _governoratesService = abp.services.app.governorates;

        var _modalManager;
        var _$governorateInformationForm = null;

		        var _GovernoratecountryLookupTableModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Governorates/CountryLookupTableModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Governorates/_GovernorateCountryLookupTableModal.js',
            modalClass: 'CountryLookupTableModal'
        });
		
		

        this.init = function (modalManager) {
            _modalManager = modalManager;

			var modal = _modalManager.getModal();
            modal.find('.date-picker').daterangepicker({
                singleDatePicker: true,
                locale: abp.localization.currentLanguage.name,
                format: 'L'
            });

            _$governorateInformationForm = _modalManager.getModal().find('form[name=GovernorateInformationsForm]');
            _$governorateInformationForm.validate();
        };

		          $('#OpenCountryLookupTableButton').click(function () {

            var governorate = _$governorateInformationForm.serializeFormToObject();

            _GovernoratecountryLookupTableModal.open({ id: governorate.countryId, displayName: governorate.countryName }, function (data) {
                _$governorateInformationForm.find('input[name=countryName]').val(data.displayName); 
                _$governorateInformationForm.find('input[name=countryId]').val(data.id); 
            });
        });
		
		$('#ClearCountryNameButton').click(function () {
                _$governorateInformationForm.find('input[name=countryName]').val(''); 
                _$governorateInformationForm.find('input[name=countryId]').val(''); 
        });
		


        this.save = function () {
            if (!_$governorateInformationForm.valid()) {
                return;
            }
            if ($('#Governorate_CountryId').prop('required') && $('#Governorate_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }

            

            var governorate = _$governorateInformationForm.serializeFormToObject();
            
            
            
			
			 _modalManager.setBusy(true);
			 _governoratesService.createOrEdit(
				governorate
			 ).done(function () {
               abp.notify.info(app.localize('SavedSuccessfully'));
               _modalManager.close();
               abp.event.trigger('app.createOrEditGovernorateModalSaved');
			 }).always(function () {
               _modalManager.setBusy(false);
			});
        };
        
        
    };
})(jQuery);
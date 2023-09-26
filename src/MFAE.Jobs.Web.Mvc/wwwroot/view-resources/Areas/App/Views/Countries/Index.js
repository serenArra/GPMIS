(function () {
    $(function () {

        var _$countriesTable = $('#CountriesTable');
        var _countriesService = abp.services.app.countries;
		var _entityTypeFullName = 'MFAE.Jobs.Location.Country';
        
       var $selectedDate = {
            startDate: null,
            endDate: null,
        }

        $('.date-picker').on('apply.daterangepicker', function (ev, picker) {
            $(this).val(picker.startDate.format('MM/DD/YYYY'));
        });

        $('.startDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.startDate = picker.startDate;
            getCountries();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getCountries();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getCountries();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getCountries();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Countries.Create'),
            edit: abp.auth.hasPermission('Pages.Countries.Edit'),
            'delete': abp.auth.hasPermission('Pages.Countries.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Countries/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Countries/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditCountryModal'
                });
                   

		 var _viewCountryModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Countries/ViewcountryModal',
            modalClass: 'ViewCountryModal'
        });

		        var _entityTypeHistoryModal = app.modals.EntityTypeHistoryModal.create();
		        function entityHistoryIsEnabled() {
            return abp.auth.hasPermission('Pages.Administration.AuditLogs') &&
                abp.custom.EntityHistory &&
                abp.custom.EntityHistory.IsEnabled &&
                _.filter(abp.custom.EntityHistory.EnabledEntities, entityType => entityType === _entityTypeFullName).length === 1;
        }

        var getDateFilter = function (element) {
            if ($selectedDate.startDate == null) {
                return null;
            }
            return $selectedDate.startDate.format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if ($selectedDate.endDate == null) {
                return null;
            }
            return $selectedDate.endDate.format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$countriesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _countriesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#CountriesTableFilter').val(),
					isoNumericFilter: $('#IsoNumericFilterId').val(),
					isoAlphaFilter: $('#IsoAlphaFilterId').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    render: function () {
                        return '';
                    },
                    targets: 0
                },
                {
                    width: 120,
                    targets: 1,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: '',
                    rowAction: {
                        cssClass: 'btn btn-brand dropdown-toggle',
                        text: '<i class="fa fa-cog"></i> ' + app.localize('Actions') + ' <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                action: function (data) {
                                    _viewCountryModal.open({ id: data.record.country.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.country.id });                                
                            }
                        },
                        {
                            text: app.localize('History'),
                            iconStyle: 'fas fa-history mr-2',
                            visible: function () {
                                return entityHistoryIsEnabled();
                            },
                            action: function (data) {
                                _entityTypeHistoryModal.open({
                                    entityTypeFullName: _entityTypeFullName,
                                    entityId: data.record.country.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteCountry(data.record.country);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "country.isoNumeric",
						 name: "isoNumeric"   
					},
					{
						targets: 3,
						 data: "country.isoAlpha",
						 name: "isoAlpha"   
					},
					{
						targets: 4,
						 data: "country.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 5,
						 data: "country.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 6,
						 data: "country.universalCode",
						 name: "universalCode"   
					}
            ]
        });

        function getCountries() {
            dataTable.ajax.reload();
        }

        function deleteCountry(country) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _countriesService.delete({
                            id: country.id
                        }).done(function () {
                            getCountries(true);
                            abp.notify.success(app.localize('SuccessfullyDeleted'));
                        });
                    }
                }
            );
        }

		$('#ShowAdvancedFiltersSpan').click(function () {
            $('#ShowAdvancedFiltersSpan').hide();
            $('#HideAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideDown();
        });

        $('#HideAdvancedFiltersSpan').click(function () {
            $('#HideAdvancedFiltersSpan').hide();
            $('#ShowAdvancedFiltersSpan').show();
            $('#AdvacedAuditFiltersArea').slideUp();
        });

        $('#CreateNewCountryButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _countriesService
                .getCountriesToExcel({
				filter : $('#CountriesTableFilter').val(),
					isoNumericFilter: $('#IsoNumericFilterId').val(),
					isoAlphaFilter: $('#IsoAlphaFilterId').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditCountryModalSaved', function () {
            getCountries();
        });

		$('#GetCountriesButton').click(function (e) {
            e.preventDefault();
            getCountries();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getCountries();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getCountries();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getCountries();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getCountries();
        });
		
		
		

    });
})();

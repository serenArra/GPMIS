(function () {
    $(function () {

        var _$governoratesTable = $('#GovernoratesTable');
        var _governoratesService = abp.services.app.governorates;
		var _entityTypeFullName = 'MFAE.Jobs.Location.Governorate';
        
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
            getGovernorates();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getGovernorates();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getGovernorates();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getGovernorates();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Governorates.Create'),
            edit: abp.auth.hasPermission('Pages.Governorates.Edit'),
            'delete': abp.auth.hasPermission('Pages.Governorates.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Governorates/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Governorates/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditGovernorateModal'
                });
                   

		 var _viewGovernorateModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Governorates/ViewgovernorateModal',
            modalClass: 'ViewGovernorateModal'
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

        var dataTable = _$governoratesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _governoratesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#GovernoratesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val(),
					countryNameFilter: $('#CountryNameFilterId').val()
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
                                    _viewGovernorateModal.open({ id: data.record.governorate.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.governorate.id });                                
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
                                    entityId: data.record.governorate.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteGovernorate(data.record.governorate);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "governorate.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "governorate.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "governorate.universalCode",
						 name: "universalCode"   
					},
					{
						targets: 5,
						 data: "countryName" ,
						 name: "countryFk.name" 
					}
            ]
        });

        function getGovernorates() {
            dataTable.ajax.reload();
        }

        function deleteGovernorate(governorate) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _governoratesService.delete({
                            id: governorate.id
                        }).done(function () {
                            getGovernorates(true);
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

        $('#CreateNewGovernorateButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _governoratesService
                .getGovernoratesToExcel({
				filter : $('#GovernoratesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val(),
					countryNameFilter: $('#CountryNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditGovernorateModalSaved', function () {
            getGovernorates();
        });

		$('#GetGovernoratesButton').click(function (e) {
            e.preventDefault();
            getGovernorates();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getGovernorates();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getGovernorates();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getGovernorates();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getGovernorates();
        });
		
		
		

    });
})();

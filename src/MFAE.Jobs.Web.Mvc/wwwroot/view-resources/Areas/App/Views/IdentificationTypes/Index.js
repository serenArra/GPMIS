(function () {
    $(function () {

        var _$identificationTypesTable = $('#IdentificationTypesTable');
        var _identificationTypesService = abp.services.app.identificationTypes;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.IdentificationType';
        
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
            getIdentificationTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getIdentificationTypes();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getIdentificationTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getIdentificationTypes();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.IdentificationTypes.Create'),
            edit: abp.auth.hasPermission('Pages.IdentificationTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.IdentificationTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/IdentificationTypes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/IdentificationTypes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditIdentificationTypeModal'
                });
                   

		 var _viewIdentificationTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/IdentificationTypes/ViewidentificationTypeModal',
            modalClass: 'ViewIdentificationTypeModal'
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

        var dataTable = _$identificationTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _identificationTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#IdentificationTypesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					isDefaultFilter: $('#IsDefaultFilterId').val()
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
                                    _viewIdentificationTypeModal.open({ id: data.record.identificationType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.identificationType.id });                                
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
                                    entityId: data.record.identificationType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteIdentificationType(data.record.identificationType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "identificationType.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "identificationType.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "identificationType.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 5,
						 data: "identificationType.isDefault",
						 name: "isDefault"  ,
						render: function (isDefault) {
							if (isDefault) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getIdentificationTypes() {
            dataTable.ajax.reload();
        }

        function deleteIdentificationType(identificationType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _identificationTypesService.delete({
                            id: identificationType.id
                        }).done(function () {
                            getIdentificationTypes(true);
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

        $('#CreateNewIdentificationTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _identificationTypesService
                .getIdentificationTypesToExcel({
				filter : $('#IdentificationTypesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val(),
					isDefaultFilter: $('#IsDefaultFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditIdentificationTypeModalSaved', function () {
            getIdentificationTypes();
        });

		$('#GetIdentificationTypesButton').click(function (e) {
            e.preventDefault();
            getIdentificationTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getIdentificationTypes();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getIdentificationTypes();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getIdentificationTypes();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getIdentificationTypes();
        });
		
		
		

    });
})();

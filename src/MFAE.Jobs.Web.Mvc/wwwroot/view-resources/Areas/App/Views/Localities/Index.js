(function () {
    $(function () {

        var _$localitiesTable = $('#LocalitiesTable');
        var _localitiesService = abp.services.app.localities;
		var _entityTypeFullName = 'MFAE.Jobs.Location.Locality';
        
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
            getLocalities();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getLocalities();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getLocalities();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getLocalities();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Localities.Create'),
            edit: abp.auth.hasPermission('Pages.Localities.Edit'),
            'delete': abp.auth.hasPermission('Pages.Localities.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Localities/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Localities/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditLocalityModal'
                });
                   

		 var _viewLocalityModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Localities/ViewlocalityModal',
            modalClass: 'ViewLocalityModal'
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

        var dataTable = _$localitiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _localitiesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#LocalitiesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val(),
					governorateNameFilter: $('#GovernorateNameFilterId').val()
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
                                    _viewLocalityModal.open({ id: data.record.locality.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.locality.id });                                
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
                                    entityId: data.record.locality.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteLocality(data.record.locality);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "locality.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "locality.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "locality.universalCode",
						 name: "universalCode"   
					},
					{
						targets: 5,
						 data: "governorateName" ,
						 name: "governorateFk.name" 
					}
            ]
        });

        function getLocalities() {
            dataTable.ajax.reload();
        }

        function deleteLocality(locality) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _localitiesService.delete({
                            id: locality.id
                        }).done(function () {
                            getLocalities(true);
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

        $('#CreateNewLocalityButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _localitiesService
                .getLocalitiesToExcel({
				filter : $('#LocalitiesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					universalCodeFilter: $('#UniversalCodeFilterId').val(),
					governorateNameFilter: $('#GovernorateNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditLocalityModalSaved', function () {
            getLocalities();
        });

		$('#GetLocalitiesButton').click(function (e) {
            e.preventDefault();
            getLocalities();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getLocalities();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getLocalities();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getLocalities();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getLocalities();
        });
		
		
		

    });
})();

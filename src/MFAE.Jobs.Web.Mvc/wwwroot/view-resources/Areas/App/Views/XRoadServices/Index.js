(function () {
    $(function () {

        var _$xRoadServicesTable = $('#XRoadServicesTable');
        var _xRoadServicesService = abp.services.app.xRoadServices;
		var _entityTypeFullName = 'MFAE.Jobs.XRoad.XRoadService';
        
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
            getXRoadServices();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getXRoadServices();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getXRoadServices();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getXRoadServices();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServices.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServices.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServices.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadServices/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServices/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadServiceModal'
                });
                   

		 var _viewXRoadServiceModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServices/ViewxRoadServiceModal',
            modalClass: 'ViewXRoadServiceModal'
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

        var dataTable = _$xRoadServicesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServicesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadServicesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					providerCodeFilter: $('#ProviderCodeFilterId').val(),
					resultCodePathFilter: $('#ResultCodePathFilterId').val(),
					actionNameFilter: $('#ActionNameFilterId').val(),
					soapActionNameFilter: $('#SoapActionNameFilterId').val(),
					versionNoFilter: $('#VersionNoFilterId').val(),
					producerCodeFilter: $('#ProducerCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					codeFilter: $('#CodeFilterId').val()
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
                                    _viewXRoadServiceModal.open({ id: data.record.xRoadService.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.xRoadService.id });                                
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
                                    entityId: data.record.xRoadService.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadService(data.record.xRoadService);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadService.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "xRoadService.providerCode",
						 name: "providerCode"   
					},
					{
						targets: 4,
						 data: "xRoadService.resultCodePath",
						 name: "resultCodePath"   
					},
					{
						targets: 5,
						 data: "xRoadService.actionName",
						 name: "actionName"   
					},
					{
						targets: 6,
						 data: "xRoadService.soapActionName",
						 name: "soapActionName"   
					},
					{
						targets: 7,
						 data: "xRoadService.versionNo",
						 name: "versionNo"   
					},
					{
						targets: 8,
						 data: "xRoadService.producerCode",
						 name: "producerCode"   
					},
					{
						targets: 9,
						 data: "xRoadService.description",
						 name: "description"   
					},
					{
						targets: 10,
						 data: "xRoadService.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_XRoadServiceStatusEnum_' + status);
						}
			
					},
					{
						targets: 11,
						 data: "xRoadService.code",
						 name: "code"   
					}
            ]
        });

        function getXRoadServices() {
            dataTable.ajax.reload();
        }

        function deleteXRoadService(xRoadService) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServicesService.delete({
                            id: xRoadService.id
                        }).done(function () {
                            getXRoadServices(true);
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

        $('#CreateNewXRoadServiceButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadServicesService
                .getXRoadServicesToExcel({
				filter : $('#XRoadServicesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					providerCodeFilter: $('#ProviderCodeFilterId').val(),
					resultCodePathFilter: $('#ResultCodePathFilterId').val(),
					actionNameFilter: $('#ActionNameFilterId').val(),
					soapActionNameFilter: $('#SoapActionNameFilterId').val(),
					versionNoFilter: $('#VersionNoFilterId').val(),
					producerCodeFilter: $('#ProducerCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					statusFilter: $('#StatusFilterId').val(),
					codeFilter: $('#CodeFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceModalSaved', function () {
            getXRoadServices();
        });

		$('#GetXRoadServicesButton').click(function (e) {
            e.preventDefault();
            getXRoadServices();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadServices();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getXRoadServices();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getXRoadServices();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getXRoadServices();
        });
		
		
		

    });
})();

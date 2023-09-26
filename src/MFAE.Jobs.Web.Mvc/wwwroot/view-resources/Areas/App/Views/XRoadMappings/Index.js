(function () {
    $(function () {

        var _$xRoadMappingsTable = $('#XRoadMappingsTable');
        var _xRoadMappingsService = abp.services.app.xRoadMappings;
		var _entityTypeFullName = 'MFAE.Jobs.XRoad.XRoadMapping';
        
       var $selectedDate = {
            startDate: null,
            endDate: null,
        }

        

        $('.startDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.startDate = picker.startDate;
            getXRoadMappings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getXRoadMappings();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getXRoadMappings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getXRoadMappings();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadMappings.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadMappings.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadMappings.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadMappings/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadMappings/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadMappingModal'
                });
                   

		 var _viewXRoadMappingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadMappings/ViewxRoadMappingModal',
            modalClass: 'ViewXRoadMappingModal'
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

        var dataTable = _$xRoadMappingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadMappingsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadMappingsTableFilter').val(),
					lookupFilter: $('#LookupFilterId').val(),
					serviceNameFilter: $('#ServiceNameFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					minSystemIdFilter: $('#MinSystemIdFilterId').val(),
					maxSystemIdFilter: $('#MaxSystemIdFilterId').val()
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
                                    _viewXRoadMappingModal.open({ id: data.record.xRoadMapping.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.xRoadMapping.id });                                
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
                                    entityId: data.record.xRoadMapping.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadMapping(data.record.xRoadMapping);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadMapping.lookup",
						 name: "lookup"   ,
						render: function (lookup) {
							return app.localize('Enum_XRoadLookupEnum_' + lookup);
						}
			
					},
					{
						targets: 3,
						 data: "xRoadMapping.serviceName",
						 name: "serviceName"   ,
						render: function (serviceName) {
							return app.localize('Enum_XRoadServicesEnum_' + serviceName);
						}
			
					},
					{
						targets: 4,
						 data: "xRoadMapping.code",
						 name: "code"   
					},
					{
						targets: 5,
						 data: "xRoadMapping.systemId",
						 name: "systemId"   
					}
            ]
        });

        function getXRoadMappings() {
            dataTable.ajax.reload();
        }

        function deleteXRoadMapping(xRoadMapping) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadMappingsService.delete({
                            id: xRoadMapping.id
                        }).done(function () {
                            getXRoadMappings(true);
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

        $('#CreateNewXRoadMappingButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadMappingsService
                .getXRoadMappingsToExcel({
				filter : $('#XRoadMappingsTableFilter').val(),
					lookupFilter: $('#LookupFilterId').val(),
					serviceNameFilter: $('#ServiceNameFilterId').val(),
					codeFilter: $('#CodeFilterId').val(),
					minSystemIdFilter: $('#MinSystemIdFilterId').val(),
					maxSystemIdFilter: $('#MaxSystemIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadMappingModalSaved', function () {
            getXRoadMappings();
        });

		$('#GetXRoadMappingsButton').click(function (e) {
            e.preventDefault();
            getXRoadMappings();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadMappings();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getXRoadMappings();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getXRoadMappings();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getXRoadMappings();
        });
		
		
		

    });
})();

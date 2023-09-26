﻿(function () {
    $(function () {

        var _$xRoadServiceAttributeMappingsTable = $('#XRoadServiceAttributeMappingsTable');
        var _xRoadServiceAttributeMappingsService = abp.services.app.xRoadServiceAttributeMappings;
		var _entityTypeFullName = 'MFAE.Jobs.XRoad.XRoadServiceAttributeMapping';
        
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
            getXRoadServiceAttributeMappings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getXRoadServiceAttributeMappings();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getXRoadServiceAttributeMappings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getXRoadServiceAttributeMappings();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributeMappings/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadServiceAttributeMappingModal'
                });
                   

		 var _viewXRoadServiceAttributeMappingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/ViewxRoadServiceAttributeMappingModal',
            modalClass: 'ViewXRoadServiceAttributeMappingModal'
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

        var dataTable = _$xRoadServiceAttributeMappingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceAttributeMappingsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadServiceAttributeMappingsTableFilter').val(),
					serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
					sourceValueFilter: $('#SourceValueFilterId').val(),
					destinationValueFilter: $('#DestinationValueFilterId').val(),
					xRoadServiceAttributeNameFilter: $('#XRoadServiceAttributeNameFilterId').val()
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
                                    _viewXRoadServiceAttributeMappingModal.open({ id: data.record.xRoadServiceAttributeMapping.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.xRoadServiceAttributeMapping.id });                                
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
                                    entityId: data.record.xRoadServiceAttributeMapping.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadServiceAttributeMapping(data.record.xRoadServiceAttributeMapping);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadServiceAttributeMapping.serviceAttributeType",
						 name: "serviceAttributeType"   ,
						render: function (serviceAttributeType) {
							return app.localize('Enum_XRoadAttributeTypeEnum_' + serviceAttributeType);
						}
			
					},
					{
						targets: 3,
						 data: "xRoadServiceAttributeMapping.sourceValue",
						 name: "sourceValue"   
					},
					{
						targets: 4,
						 data: "xRoadServiceAttributeMapping.destinationValue",
						 name: "destinationValue"   
					},
					{
						targets: 5,
						 data: "xRoadServiceAttributeName" ,
						 name: "attributeFk.name" 
					}
            ]
        });

        function getXRoadServiceAttributeMappings() {
            dataTable.ajax.reload();
        }

        function deleteXRoadServiceAttributeMapping(xRoadServiceAttributeMapping) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServiceAttributeMappingsService.delete({
                            id: xRoadServiceAttributeMapping.id
                        }).done(function () {
                            getXRoadServiceAttributeMappings(true);
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

        $('#CreateNewXRoadServiceAttributeMappingButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadServiceAttributeMappingsService
                .getXRoadServiceAttributeMappingsToExcel({
				filter : $('#XRoadServiceAttributeMappingsTableFilter').val(),
					serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
					sourceValueFilter: $('#SourceValueFilterId').val(),
					destinationValueFilter: $('#DestinationValueFilterId').val(),
					xRoadServiceAttributeNameFilter: $('#XRoadServiceAttributeNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceAttributeMappingModalSaved', function () {
            getXRoadServiceAttributeMappings();
        });

		$('#GetXRoadServiceAttributeMappingsButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceAttributeMappings();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadServiceAttributeMappings();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getXRoadServiceAttributeMappings();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getXRoadServiceAttributeMappings();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getXRoadServiceAttributeMappings();
        });
		
		
		

    });
})();
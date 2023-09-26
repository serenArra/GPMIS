(function () {
    $(function () {

        var _$xRoadServiceAttributesTable = $('#XRoadServiceAttributesTable');
        var _xRoadServiceAttributesService = abp.services.app.xRoadServiceAttributes;
		var _entityTypeFullName = 'MFAE.Jobs.XRoad.XRoadServiceAttribute';
        
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
            getXRoadServiceAttributes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getXRoadServiceAttributes();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getXRoadServiceAttributes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getXRoadServiceAttributes();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServiceAttributes.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServiceAttributes.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServiceAttributes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadServiceAttributes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadServiceAttributeModal'
                });
                   

		 var _viewXRoadServiceAttributeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/ViewxRoadServiceAttributeModal',
            modalClass: 'ViewXRoadServiceAttributeModal'
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

        var dataTable = _$xRoadServiceAttributesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceAttributesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadServiceAttributesTableFilter').val(),
					serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
					attributeCodeFilter: $('#AttributeCodeFilterId').val(),
					xMLPathFilter: $('#XMLPathFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formatTransitionFilter: $('#FormatTransitionFilterId').val(),
					xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val()
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
                                    _viewXRoadServiceAttributeModal.open({ id: data.record.xRoadServiceAttribute.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.xRoadServiceAttribute.id });                                
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
                                    entityId: data.record.xRoadServiceAttribute.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadServiceAttribute(data.record.xRoadServiceAttribute);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadServiceAttribute.serviceAttributeType",
						 name: "serviceAttributeType"   ,
						render: function (serviceAttributeType) {
							return app.localize('Enum_XRoadServiceAttributeTypeEnum_' + serviceAttributeType);
						}
			
					},
					{
						targets: 3,
						 data: "xRoadServiceAttribute.attributeCode",
						 name: "attributeCode"   
					},
					{
						targets: 4,
						 data: "xRoadServiceAttribute.xmlPath",
						 name: "xmlPath"   
					},
					{
						targets: 5,
						 data: "xRoadServiceAttribute.name",
						 name: "name"   
					},
					{
						targets: 6,
						 data: "xRoadServiceAttribute.description",
						 name: "description"   
					},
					{
						targets: 7,
						 data: "xRoadServiceAttribute.formatTransition",
						 name: "formatTransition"   
					},
					{
						targets: 8,
						 data: "xRoadServiceName" ,
						 name: "xRoadServiceFk.name" 
					}
            ]
        });

        function getXRoadServiceAttributes() {
            dataTable.ajax.reload();
        }

        function deleteXRoadServiceAttribute(xRoadServiceAttribute) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServiceAttributesService.delete({
                            id: xRoadServiceAttribute.id
                        }).done(function () {
                            getXRoadServiceAttributes(true);
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

        $('#CreateNewXRoadServiceAttributeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadServiceAttributesService
                .getXRoadServiceAttributesToExcel({
				filter : $('#XRoadServiceAttributesTableFilter').val(),
					serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
					attributeCodeFilter: $('#AttributeCodeFilterId').val(),
					xMLPathFilter: $('#XMLPathFilterId').val(),
					nameFilter: $('#NameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					formatTransitionFilter: $('#FormatTransitionFilterId').val(),
					xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceAttributeModalSaved', function () {
            getXRoadServiceAttributes();
        });

		$('#GetXRoadServiceAttributesButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceAttributes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadServiceAttributes();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getXRoadServiceAttributes();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getXRoadServiceAttributes();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getXRoadServiceAttributes();
        });
		
		
		

    });
})();

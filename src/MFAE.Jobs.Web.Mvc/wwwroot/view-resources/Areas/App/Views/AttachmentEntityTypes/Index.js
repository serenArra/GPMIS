(function () {
    $(function () {

        var _$attachmentEntityTypesTable = $('#AttachmentEntityTypesTable');
        var _attachmentEntityTypesService = abp.services.app.attachmentEntityTypes;
		var _entityTypeFullName = 'MFAE.Jobs.Attachments.AttachmentEntityType';
        
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
            getAttachmentEntityTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getAttachmentEntityTypes();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getAttachmentEntityTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getAttachmentEntityTypes();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentEntityTypes.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentEntityTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentEntityTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AttachmentEntityTypes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentEntityTypes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentEntityTypeModal'
                });
                   

		 var _viewAttachmentEntityTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentEntityTypes/ViewattachmentEntityTypeModal',
            modalClass: 'ViewAttachmentEntityTypeModal'
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

        var dataTable = _$attachmentEntityTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentEntityTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentEntityTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					folderFilter: $('#FolderFilterId').val(),
					minParentTypeIdFilter: $('#MinParentTypeIdFilterId').val(),
					maxParentTypeIdFilter: $('#MaxParentTypeIdFilterId').val()
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
                                    _viewAttachmentEntityTypeModal.open({ id: data.record.attachmentEntityType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentEntityType.id });                                
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
                                    entityId: data.record.attachmentEntityType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentEntityType(data.record.attachmentEntityType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentEntityType.arName",
						 name: "arName"   
					},
					{
						targets: 3,
						 data: "attachmentEntityType.enName",
						 name: "enName"   
					},
					{
						targets: 4,
						 data: "attachmentEntityType.folder",
						 name: "folder"   
					},
					{
						targets: 5,
						 data: "attachmentEntityType.parentTypeId",
						 name: "parentTypeId"   
					}
            ]
        });

        function getAttachmentEntityTypes() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentEntityType(attachmentEntityType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentEntityTypesService.delete({
                            id: attachmentEntityType.id
                        }).done(function () {
                            getAttachmentEntityTypes(true);
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

        $('#CreateNewAttachmentEntityTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentEntityTypesService
                .getAttachmentEntityTypesToExcel({
				filter : $('#AttachmentEntityTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					folderFilter: $('#FolderFilterId').val(),
					minParentTypeIdFilter: $('#MinParentTypeIdFilterId').val(),
					maxParentTypeIdFilter: $('#MaxParentTypeIdFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentEntityTypeModalSaved', function () {
            getAttachmentEntityTypes();
        });

		$('#GetAttachmentEntityTypesButton').click(function (e) {
            e.preventDefault();
            getAttachmentEntityTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentEntityTypes();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getAttachmentEntityTypes();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getAttachmentEntityTypes();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getAttachmentEntityTypes();
        });
		
		
		

    });
})();

(function () {
    $(function () {

        var _$attachmentFilesTable = $('#AttachmentFilesTable');
        var _attachmentFilesService = abp.services.app.attachmentFiles;
		var _entityTypeFullName = 'MFAE.Jobs.Attachments.AttachmentFile';
        
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
            getAttachmentFiles();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getAttachmentFiles();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getAttachmentFiles();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getAttachmentFiles();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentFiles.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentFiles.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentFiles.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AttachmentFiles/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentFiles/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentFileModal'
                });
                   

		 var _viewAttachmentFileModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentFiles/ViewattachmentFileModal',
            modalClass: 'ViewAttachmentFileModal'
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

        var dataTable = _$attachmentFilesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentFilesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentFilesTableFilter').val(),
					physicalNameFilter: $('#PhysicalNameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					originalNameFilter: $('#OriginalNameFilterId').val(),
					minSizeFilter: $('#MinSizeFilterId').val(),
					maxSizeFilter: $('#MaxSizeFilterId').val(),
					objectIdFilter: $('#ObjectIdFilterId').val(),
					pathFilter: $('#PathFilterId').val(),
					minVersionFilter: $('#MinVersionFilterId').val(),
					maxVersionFilter: $('#MaxVersionFilterId').val(),
					fileTokenFilter: $('#FileTokenFilterId').val(),
					tagFilter: $('#TagFilterId').val(),
					filterKeyFilter: $('#FilterKeyFilterId').val(),
					attachmentTypeArNameFilter: $('#AttachmentTypeArNameFilterId').val()
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
                                    _viewAttachmentFileModal.open({ id: data.record.attachmentFile.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentFile.id });                                
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
                                    entityId: data.record.attachmentFile.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentFile(data.record.attachmentFile);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentFile.physicalName",
						 name: "physicalName"   
					},
					{
						targets: 3,
						 data: "attachmentFile.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "attachmentFile.originalName",
						 name: "originalName"   
					},
					{
						targets: 5,
						 data: "attachmentFile.size",
						 name: "size"   
					},
					{
						targets: 6,
						 data: "attachmentFile.objectId",
						 name: "objectId"   
					},
					{
						targets: 7,
						 data: "attachmentFile.path",
						 name: "path"   
					},
					{
						targets: 8,
						 data: "attachmentFile.version",
						 name: "version"   
					},
					{
						targets: 9,
						 data: "attachmentFile.fileToken",
						 name: "fileToken"   
					},
					{
						targets: 10,
						 data: "attachmentFile.tag",
						 name: "tag"   
					},
					{
						targets: 11,
						 data: "attachmentFile.filterKey",
						 name: "filterKey"   
					},
					{
						targets: 12,
						 data: "attachmentTypeArName" ,
						 name: "attachmentTypeFk.arName" 
					}
            ]
        });

        function getAttachmentFiles() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentFile(attachmentFile) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentFilesService.delete({
                            id: attachmentFile.id
                        }).done(function () {
                            getAttachmentFiles(true);
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

        $('#CreateNewAttachmentFileButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentFilesService
                .getAttachmentFilesToExcel({
				filter : $('#AttachmentFilesTableFilter').val(),
					physicalNameFilter: $('#PhysicalNameFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					originalNameFilter: $('#OriginalNameFilterId').val(),
					minSizeFilter: $('#MinSizeFilterId').val(),
					maxSizeFilter: $('#MaxSizeFilterId').val(),
					objectIdFilter: $('#ObjectIdFilterId').val(),
					pathFilter: $('#PathFilterId').val(),
					minVersionFilter: $('#MinVersionFilterId').val(),
					maxVersionFilter: $('#MaxVersionFilterId').val(),
					fileTokenFilter: $('#FileTokenFilterId').val(),
					tagFilter: $('#TagFilterId').val(),
					filterKeyFilter: $('#FilterKeyFilterId').val(),
					attachmentTypeArNameFilter: $('#AttachmentTypeArNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentFileModalSaved', function () {
            getAttachmentFiles();
        });

		$('#GetAttachmentFilesButton').click(function (e) {
            e.preventDefault();
            getAttachmentFiles();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentFiles();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getAttachmentFiles();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getAttachmentFiles();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getAttachmentFiles();
        });
		
		
		

    });
})();

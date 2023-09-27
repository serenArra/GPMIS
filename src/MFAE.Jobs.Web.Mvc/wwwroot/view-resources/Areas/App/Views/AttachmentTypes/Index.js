(function () {
    $(function () {

        var _$attachmentTypesTable = $('#AttachmentTypesTable');
        var _attachmentTypesService = abp.services.app.attachmentTypes;
		var _entityTypeFullName = 'MFAE.Jobs.Attachments.AttachmentType';
        
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
            getAttachmentTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getAttachmentTypes();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getAttachmentTypes();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getAttachmentTypes();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentTypes.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentTypes.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentTypes.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AttachmentTypes/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentTypes/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentTypeModal'
                });
                   

		 var _viewAttachmentTypeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentTypes/ViewattachmentTypeModal',
            modalClass: 'ViewAttachmentTypeModal'
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

        var dataTable = _$attachmentTypesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentTypesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					minMaxSizeMBFilter: $('#MinMaxSizeMBFilterId').val(),
					maxMaxSizeMBFilter: $('#MaxMaxSizeMBFilterId').val(),
					allowedExtensionsFilter: $('#AllowedExtensionsFilterId').val(),
					minMaxAttachmentsFilter: $('#MinMaxAttachmentsFilterId').val(),
					maxMaxAttachmentsFilter: $('#MaxMaxAttachmentsFilterId').val(),
					minMinRequiredAttachmentsFilter: $('#MinMinRequiredAttachmentsFilterId').val(),
					maxMinRequiredAttachmentsFilter: $('#MaxMinRequiredAttachmentsFilterId').val(),
					categoryFilter: $('#CategoryFilterId').val(),
					privacyFlagFilter: $('#PrivacyFlagFilterId').val(),
					attachmentEntityTypeNameFilter: $('#AttachmentEntityTypeNameFilterId').val(),
					attachmentTypeGroupNameFilter: $('#AttachmentTypeGroupNameFilterId').val()
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
                                    _viewAttachmentTypeModal.open({ id: data.record.attachmentType.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentType.id });                                
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
                                    entityId: data.record.attachmentType.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentType(data.record.attachmentType);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentType.arName",
						 name: "arName"   
					},
					{
						targets: 3,
						 data: "attachmentType.enName",
						 name: "enName"   
					},
					{
						targets: 4,
						 data: "attachmentType.maxSizeMB",
						 name: "maxSizeMB"   
					},
					{
						targets: 5,
						 data: "attachmentType.allowedExtensions",
						 name: "allowedExtensions"   
					},
					{
						targets: 6,
						 data: "attachmentType.maxAttachments",
						 name: "maxAttachments"   
					},
					{
						targets: 7,
						 data: "attachmentType.minRequiredAttachments",
						 name: "minRequiredAttachments"   
					},
					{
						targets: 8,
						 data: "attachmentType.category",
						 name: "category"   ,
						render: function (category) {
							return app.localize('Enum_AttachmentTypeCategories_' + category);
						}
			
					},
					{
						targets: 9,
						 data: "attachmentType.privacyFlag",
						 name: "privacyFlag"   ,
						render: function (privacyFlag) {
							return app.localize('Enum_PrivacyFlag_' + privacyFlag);
						}
			
					},
					{
						targets: 10,
						 data: "attachmentEntityTypeName" ,
						 name: "entityTypeFk.name" 
					},
					{
						targets: 11,
						 data: "attachmentTypeGroupName" ,
						 name: "groupFk.name" 
					}
            ]
        });

        function getAttachmentTypes() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentType(attachmentType) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentTypesService.delete({
                            id: attachmentType.id
                        }).done(function () {
                            getAttachmentTypes(true);
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

        $('#CreateNewAttachmentTypeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentTypesService
                .getAttachmentTypesToExcel({
				filter : $('#AttachmentTypesTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val(),
					minMaxSizeMBFilter: $('#MinMaxSizeMBFilterId').val(),
					maxMaxSizeMBFilter: $('#MaxMaxSizeMBFilterId').val(),
					allowedExtensionsFilter: $('#AllowedExtensionsFilterId').val(),
					minMaxAttachmentsFilter: $('#MinMaxAttachmentsFilterId').val(),
					maxMaxAttachmentsFilter: $('#MaxMaxAttachmentsFilterId').val(),
					minMinRequiredAttachmentsFilter: $('#MinMinRequiredAttachmentsFilterId').val(),
					maxMinRequiredAttachmentsFilter: $('#MaxMinRequiredAttachmentsFilterId').val(),
					categoryFilter: $('#CategoryFilterId').val(),
					privacyFlagFilter: $('#PrivacyFlagFilterId').val(),
					attachmentEntityTypeNameFilter: $('#AttachmentEntityTypeNameFilterId').val(),
					attachmentTypeGroupNameFilter: $('#AttachmentTypeGroupNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentTypeModalSaved', function () {
            getAttachmentTypes();
        });

		$('#GetAttachmentTypesButton').click(function (e) {
            e.preventDefault();
            getAttachmentTypes();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentTypes();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getAttachmentTypes();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getAttachmentTypes();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getAttachmentTypes();
        });
		
		
		

    });
})();

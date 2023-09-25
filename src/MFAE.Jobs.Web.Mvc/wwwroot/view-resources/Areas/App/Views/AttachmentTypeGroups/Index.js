(function () {
    $(function () {

        var _$attachmentTypeGroupsTable = $('#AttachmentTypeGroupsTable');
        var _attachmentTypeGroupsService = abp.services.app.attachmentTypeGroups;
		var _entityTypeFullName = 'MFAE.Jobs.Attachments.AttachmentTypeGroup';
        
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
            getAttachmentTypeGroups();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getAttachmentTypeGroups();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getAttachmentTypeGroups();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getAttachmentTypeGroups();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AttachmentTypeGroups.Create'),
            edit: abp.auth.hasPermission('Pages.AttachmentTypeGroups.Edit'),
            'delete': abp.auth.hasPermission('Pages.AttachmentTypeGroups.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AttachmentTypeGroups/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AttachmentTypeGroups/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAttachmentTypeGroupModal'
                });
                   

		 var _viewAttachmentTypeGroupModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AttachmentTypeGroups/ViewattachmentTypeGroupModal',
            modalClass: 'ViewAttachmentTypeGroupModal'
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

        var dataTable = _$attachmentTypeGroupsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _attachmentTypeGroupsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AttachmentTypeGroupsTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val()
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
                                    _viewAttachmentTypeGroupModal.open({ id: data.record.attachmentTypeGroup.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.attachmentTypeGroup.id });                                
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
                                    entityId: data.record.attachmentTypeGroup.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAttachmentTypeGroup(data.record.attachmentTypeGroup);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "attachmentTypeGroup.arName",
						 name: "arName"   
					},
					{
						targets: 3,
						 data: "attachmentTypeGroup.enName",
						 name: "enName"   
					}
            ]
        });

        function getAttachmentTypeGroups() {
            dataTable.ajax.reload();
        }

        function deleteAttachmentTypeGroup(attachmentTypeGroup) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _attachmentTypeGroupsService.delete({
                            id: attachmentTypeGroup.id
                        }).done(function () {
                            getAttachmentTypeGroups(true);
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

        $('#CreateNewAttachmentTypeGroupButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _attachmentTypeGroupsService
                .getAttachmentTypeGroupsToExcel({
				filter : $('#AttachmentTypeGroupsTableFilter').val(),
					arNameFilter: $('#ArNameFilterId').val(),
					enNameFilter: $('#EnNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAttachmentTypeGroupModalSaved', function () {
            getAttachmentTypeGroups();
        });

		$('#GetAttachmentTypeGroupsButton').click(function (e) {
            e.preventDefault();
            getAttachmentTypeGroups();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAttachmentTypeGroups();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getAttachmentTypeGroups();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getAttachmentTypeGroups();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getAttachmentTypeGroups();
        });
		
		
		

    });
})();

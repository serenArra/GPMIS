(function () {
    $(function () {

        var _$applicantStatusesTable = $('#ApplicantStatusesTable');
        var _applicantStatusesService = abp.services.app.applicantStatuses;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.ApplicantStatus';
        
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
            getApplicantStatuses();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getApplicantStatuses();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getApplicantStatuses();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getApplicantStatuses();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ApplicantStatuses.Create'),
            edit: abp.auth.hasPermission('Pages.ApplicantStatuses.Edit'),
            'delete': abp.auth.hasPermission('Pages.ApplicantStatuses.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ApplicantStatuses/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStatuses/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditApplicantStatusModal'
                });
                   

		 var _viewApplicantStatusModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStatuses/ViewapplicantStatusModal',
            modalClass: 'ViewApplicantStatusModal'
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

        var dataTable = _$applicantStatusesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _applicantStatusesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ApplicantStatusesTableFilter').val(),
					statusFilter: $('#StatusFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					applicantFullNameFilter: $('#ApplicantFullNameFilterId').val()
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
                                    _viewApplicantStatusModal.open({ id: data.record.applicantStatus.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.applicantStatus.id });                                
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
                                    entityId: data.record.applicantStatus.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteApplicantStatus(data.record.applicantStatus);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "applicantStatus.status",
						 name: "status"   ,
						render: function (status) {
							return app.localize('Enum_ApplicantStatusEnum_' + status);
						}
			
					},
					{
						targets: 3,
						 data: "applicantStatus.description",
						 name: "description"   
					},
					{
						targets: 4,
						 data: "applicantFullName" ,
						 name: "applicantFk.fullName" 
					}
            ]
        });

        function getApplicantStatuses() {
            dataTable.ajax.reload();
        }

        function deleteApplicantStatus(applicantStatus) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _applicantStatusesService.delete({
                            id: applicantStatus.id
                        }).done(function () {
                            getApplicantStatuses(true);
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

        $('#CreateNewApplicantStatusButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _applicantStatusesService
                .getApplicantStatusesToExcel({
				filter : $('#ApplicantStatusesTableFilter').val(),
					statusFilter: $('#StatusFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val(),
					applicantFullNameFilter: $('#ApplicantFullNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditApplicantStatusModalSaved', function () {
            getApplicantStatuses();
        });

		$('#GetApplicantStatusesButton').click(function (e) {
            e.preventDefault();
            getApplicantStatuses();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getApplicantStatuses();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getApplicantStatuses();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getApplicantStatuses();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getApplicantStatuses();
        });
		
		
		

    });
})();

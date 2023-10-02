(function () {
    $(function () {

        var _$applicantTrainingsTable = $('#ApplicantTrainingsTable');
        var _applicantTrainingsService = abp.services.app.applicantTrainings;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.ApplicantTraining';
        
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
            getApplicantTrainings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getApplicantTrainings();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getApplicantTrainings();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getApplicantTrainings();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ApplicantTrainings.Create'),
            edit: abp.auth.hasPermission('Pages.ApplicantTrainings.Edit'),
            'delete': abp.auth.hasPermission('Pages.ApplicantTrainings.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ApplicantTrainings/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantTrainings/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditApplicantTrainingModal'
         });
                   

		 var _viewApplicantTrainingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantTrainings/ViewapplicantTrainingModal',
            modalClass: 'ViewApplicantTrainingModal'
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

        var dataTable = _$applicantTrainingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            makeAjax: $('#ApplicantId').val() > 0,
            deferLoading: $('#ApplicantId').val() > 0 ? null : 0,
            listAction: {
                ajaxFunction: _applicantTrainingsService.getAll,
                inputFilter: function () {
                    return {
                    filter: $('#ApplicantTrainingsTableFilter').val(),
                    applicantIdFilter: $('#ApplicantId').val(),
					subjectFilter: $('#SubjectFilterId').val(),
					locationFilter: $('#LocationFilterId').val(),
					minTrainingDateFilter:  getDateFilter($('#MinTrainingDateFilterId')),
					maxTrainingDateFilter:  getMaxDateFilter($('#MaxTrainingDateFilterId')),
					minDurationFilter: $('#MinDurationFilterId').val(),
					maxDurationFilter: $('#MaxDurationFilterId').val(),
					durationTypeFilter: $('#DurationTypeFilterId').val(),
					applicantFirstNameFilter: $('#ApplicantFirstNameFilterId').val()
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
                                    _viewApplicantTrainingModal.open({ id: data.record.applicantTraining.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.applicantTraining.id });                                
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
                                    entityId: data.record.applicantTraining.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteApplicantTraining(data.record.applicantTraining);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "applicantTraining.subject",
						 name: "subject"   
					},
					{
						targets: 3,
						 data: "applicantTraining.location",
						 name: "location"   
					},
					{
						targets: 4,
						 data: "applicantTraining.trainingDate",
						 name: "trainingDate" ,
					render: function (trainingDate) {
						if (trainingDate) {
							return moment(trainingDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 5,
						 data: "applicantTraining.duration",
						 name: "duration"   
					},
					{
						targets: 6,
						 data: "applicantTraining.durationType",
						 name: "durationType"   ,
						render: function (durationType) {
							return app.localize('Enum_DurationType_' + durationType);
						}
			
					},
					{
						targets: 7,
						 data: "applicantFirstName" ,
						 name: "applicantFk.firstName" 
					}
            ]
        });

        function getApplicantTrainings() {
            dataTable.ajax.reload();
        }

        function deleteApplicantTraining(applicantTraining) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _applicantTrainingsService.delete({
                            id: applicantTraining.id
                        }).done(function () {
                            getApplicantTrainings(true);
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

        $('#CreateNewApplicantTrainingButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _applicantTrainingsService
                .getApplicantTrainingsToExcel({
				filter : $('#ApplicantTrainingsTableFilter').val(),
					subjectFilter: $('#SubjectFilterId').val(),
					locationFilter: $('#LocationFilterId').val(),
					minTrainingDateFilter:  getDateFilter($('#MinTrainingDateFilterId')),
					maxTrainingDateFilter:  getMaxDateFilter($('#MaxTrainingDateFilterId')),
					minDurationFilter: $('#MinDurationFilterId').val(),
					maxDurationFilter: $('#MaxDurationFilterId').val(),
					durationTypeFilter: $('#DurationTypeFilterId').val(),
					applicantFirstNameFilter: $('#ApplicantFirstNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditApplicantTrainingModalSaved', function () {
            getApplicantTrainings();
        });

		$('#GetApplicantTrainingsButton').click(function (e) {
            e.preventDefault();
            getApplicantTrainings();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getApplicantTrainings();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getApplicantTrainings();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getApplicantTrainings();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getApplicantTrainings();
        });
		
		
		

    });
})();

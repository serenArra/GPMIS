(function () {
    $(function () {

        var _$applicantStudiesTable = $('#ApplicantStudiesTable');
        var _applicantStudiesService = abp.services.app.applicantStudies;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.ApplicantStudy';
        
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
            getApplicantStudies();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getApplicantStudies();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getApplicantStudies();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getApplicantStudies();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ApplicantStudies.Create'),
            edit: abp.auth.hasPermission('Pages.ApplicantStudies.Edit'),
            'delete': abp.auth.hasPermission('Pages.ApplicantStudies.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ApplicantStudies/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantStudies/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditApplicantStudyModal'
                });
                   

		 var _viewApplicantStudyModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantStudies/ViewapplicantStudyModal',
            modalClass: 'ViewApplicantStudyModal'
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

        var dataTable = _$applicantStudiesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            makeAjax: $('#ApplicantId').val() > 0,
            deferLoading: $('#ApplicantId').val() > 0 ? null : 0,
            listAction: {
                ajaxFunction: _applicantStudiesService.getAll,
                inputFilter: function () {
                    return {
                    filter: $('#ApplicantStudiesTableFilter').val(),
                    applicantIdFilter: $('#ApplicantId').val(),
					otherSpecialtyFilter: $('#OtherSpecialtyFilterId').val(),
					secondSpecialtyFilter: $('#SecondSpecialtyFilterId').val(),
					universityFilter: $('#UniversityFilterId').val(),
					minGraduationYearFilter: $('#MinGraduationYearFilterId').val(),
					maxGraduationYearFilter: $('#MaxGraduationYearFilterId').val(),
					graduationCountryFilter: $('#GraduationCountryFilterId').val(),
					graduationRateNameFilter: $('#GraduationRateNameFilterId').val(),
					academicDegreeNameFilter: $('#AcademicDegreeNameFilterId').val(),
					specialtiesNameFilter: $('#SpecialtiesNameFilterId').val(),
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
                                    _viewApplicantStudyModal.open({ id: data.record.applicantStudy.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.applicantStudy.id, applicantId: data.record.applicantStudy.applicantId });                                
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
                                    entityId: data.record.applicantStudy.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteApplicantStudy(data.record.applicantStudy);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "applicantStudy.otherSpecialty",
						 name: "otherSpecialty"   
					},
					{
						targets: 3,
						 data: "applicantStudy.secondSpecialty",
						 name: "secondSpecialty"   
					},
					{
						targets: 4,
						 data: "applicantStudy.university",
						 name: "university"   
					},
					{
						targets: 5,
						 data: "applicantStudy.graduationYear",
						 name: "graduationYear"   
					},
					{
						targets: 6,
						 data: "applicantStudy.graduationCountry",
						 name: "graduationCountry"   
					},
					{
						targets: 7,
						 data: "graduationRateName" ,
						 name: "graduationRateFk.name" 
					},
					{
						targets: 8,
						 data: "academicDegreeName" ,
						 name: "academicDegreeFk.name" 
					},
					{
						targets: 9,
						 data: "specialtiesName" ,
						 name: "specialtiesFk.name" 
					},
					{
						targets: 10,
						 data: "applicantFirstName" ,
						 name: "applicantFk.firstName" 
					}
            ]
        });

        function getApplicantStudies() {
            dataTable.ajax.reload();
        }

        function deleteApplicantStudy(applicantStudy) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _applicantStudiesService.delete({
                            id: applicantStudy.id
                        }).done(function () {
                            getApplicantStudies(true);
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

        $('#CreateNewApplicantStudyButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _applicantStudiesService
                .getApplicantStudiesToExcel({
				filter : $('#ApplicantStudiesTableFilter').val(),
					otherSpecialtyFilter: $('#OtherSpecialtyFilterId').val(),
					secondSpecialtyFilter: $('#SecondSpecialtyFilterId').val(),
					universityFilter: $('#UniversityFilterId').val(),
					minGraduationYearFilter: $('#MinGraduationYearFilterId').val(),
					maxGraduationYearFilter: $('#MaxGraduationYearFilterId').val(),
					graduationCountryFilter: $('#GraduationCountryFilterId').val(),
					graduationRateNameFilter: $('#GraduationRateNameFilterId').val(),
					academicDegreeNameFilter: $('#AcademicDegreeNameFilterId').val(),
					specialtiesNameFilter: $('#SpecialtiesNameFilterId').val(),
					applicantFirstNameFilter: $('#ApplicantFirstNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditApplicantStudyModalSaved', function () {
            getApplicantStudies();
        });

		$('#GetApplicantStudiesButton').click(function (e) {
            e.preventDefault();
            getApplicantStudies();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getApplicantStudies();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getApplicantStudies();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getApplicantStudies();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getApplicantStudies();
        });
		
		
		

    });
})();

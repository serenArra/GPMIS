(function () {
    $(function () {

        var _$applicantsTable = $('#ApplicantsTable');
        var _applicantsService = abp.services.app.applicants;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.Applicant';
        
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
            getApplicants();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getApplicants();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getApplicants();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getApplicants();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Applicants.Create'),
            edit: abp.auth.hasPermission('Pages.Applicants.Edit'),
            'delete': abp.auth.hasPermission('Pages.Applicants.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Applicants/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditApplicantModal'
                });
                   

		 var _viewApplicantModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/ViewapplicantModal',
            modalClass: 'ViewApplicantModal'
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

        var dataTable = _$applicantsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _applicantsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ApplicantsTableFilter').val(),
					documentNoFilter: $('#DocumentNoFilterId').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					fatherNameFilter: $('#FatherNameFilterId').val(),
					grandFatherNameFilter: $('#GrandFatherNameFilterId').val(),
					familyNameFilter: $('#FamilyNameFilterId').val(),
					firstNameEnFilter: $('#FirstNameEnFilterId').val(),
					fatherNameEnFilter: $('#FatherNameEnFilterId').val(),
					grandFatherNameEnFilter: $('#GrandFatherNameEnFilterId').val(),
					familyNameEnFilter: $('#FamilyNameEnFilterId').val(),
					minBirthDateFilter:  getDateFilter($('#MinBirthDateFilterId')),
					maxBirthDateFilter:  getMaxDateFilter($('#MaxBirthDateFilterId')),
					birthPlaceFilter: $('#BirthPlaceFilterId').val(),
					minNumberOfChildrenFilter: $('#MinNumberOfChildrenFilterId').val(),
					maxNumberOfChildrenFilter: $('#MaxNumberOfChildrenFilterId').val(),
					addressFilter: $('#AddressFilterId').val(),
					mobileFilter: $('#MobileFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					isLockedFilter: $('#IsLockedFilterId').val(),
					genderFilter: $('#GenderFilterId').val(),
					identificationTypeNameFilter: $('#IdentificationTypeNameFilterId').val(),
					maritalStatusNameFilter: $('#MaritalStatusNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					applicantStatusDescriptionFilter: $('#ApplicantStatusDescriptionFilterId').val(),
					countryNameFilter: $('#CountryNameFilterId').val(),
					governorateNameFilter: $('#GovernorateNameFilterId').val(),
					localityNameFilter: $('#LocalityNameFilterId').val()
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
                                    _viewApplicantModal.open({ id: data.record.applicant.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.applicant.id });                                
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
                                    entityId: data.record.applicant.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteApplicant(data.record.applicant);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "applicant.documentNo",
						 name: "documentNo"   
					},
					{
						targets: 3,
						 data: "applicant.firstName",
						 name: "firstName"   
					},
					{
						targets: 4,
						 data: "applicant.fatherName",
						 name: "fatherName"   
					},
					{
						targets: 5,
						 data: "applicant.grandFatherName",
						 name: "grandFatherName"   
					},
					{
						targets: 6,
						 data: "applicant.familyName",
						 name: "familyName"   
					},
					{
						targets: 7,
						 data: "applicant.firstNameEn",
						 name: "firstNameEn"   
					},
					{
						targets: 8,
						 data: "applicant.fatherNameEn",
						 name: "fatherNameEn"   
					},
					{
						targets: 9,
						 data: "applicant.grandFatherNameEn",
						 name: "grandFatherNameEn"   
					},
					{
						targets: 10,
						 data: "applicant.familyNameEn",
						 name: "familyNameEn"   
					},
					{
						targets: 11,
						 data: "applicant.birthDate",
						 name: "birthDate" ,
					render: function (birthDate) {
						if (birthDate) {
							return moment(birthDate).format('L');
						}
						return "";
					}
			  
					},
					{
						targets: 12,
						 data: "applicant.birthPlace",
						 name: "birthPlace"   
					},
					{
						targets: 13,
						 data: "applicant.numberOfChildren",
						 name: "numberOfChildren"   
					},
					{
						targets: 14,
						 data: "applicant.address",
						 name: "address"   
					},
					{
						targets: 15,
						 data: "applicant.mobile",
						 name: "mobile"   
					},
					{
						targets: 16,
						 data: "applicant.email",
						 name: "email"   
					},
					{
						targets: 17,
						 data: "applicant.isLocked",
						 name: "isLocked"  ,
						render: function (isLocked) {
							if (isLocked) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					},
					{
						targets: 18,
						 data: "applicant.gender",
						 name: "gender"   ,
						render: function (gender) {
							return app.localize('Enum_Gender_' + gender);
						}
			
					},
					{
						targets: 19,
						 data: "identificationTypeName" ,
						 name: "identificationTypeFk.name" 
					},
					{
						targets: 20,
						 data: "maritalStatusName" ,
						 name: "maritalStatusFk.name" 
					},
					{
						targets: 21,
						 data: "userName" ,
						 name: "lockedByFk.name" 
					},
					{
						targets: 22,
						 data: "applicantStatusDescription" ,
						 name: "currentStatusFk.description" 
					},
					{
						targets: 23,
						 data: "countryName" ,
						 name: "countryFk.name" 
					},
					{
						targets: 24,
						 data: "governorateName" ,
						 name: "governorateFk.name" 
					},
					{
						targets: 25,
						 data: "localityName" ,
						 name: "localityFk.name" 
					}
            ]
        });

        function getApplicants() {
            dataTable.ajax.reload();
        }

        function deleteApplicant(applicant) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _applicantsService.delete({
                            id: applicant.id
                        }).done(function () {
                            getApplicants(true);
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

        $('#CreateNewApplicantButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _applicantsService
                .getApplicantsToExcel({
				filter : $('#ApplicantsTableFilter').val(),
					documentNoFilter: $('#DocumentNoFilterId').val(),
					firstNameFilter: $('#FirstNameFilterId').val(),
					fatherNameFilter: $('#FatherNameFilterId').val(),
					grandFatherNameFilter: $('#GrandFatherNameFilterId').val(),
					familyNameFilter: $('#FamilyNameFilterId').val(),
					firstNameEnFilter: $('#FirstNameEnFilterId').val(),
					fatherNameEnFilter: $('#FatherNameEnFilterId').val(),
					grandFatherNameEnFilter: $('#GrandFatherNameEnFilterId').val(),
					familyNameEnFilter: $('#FamilyNameEnFilterId').val(),
					minBirthDateFilter:  getDateFilter($('#MinBirthDateFilterId')),
					maxBirthDateFilter:  getMaxDateFilter($('#MaxBirthDateFilterId')),
					birthPlaceFilter: $('#BirthPlaceFilterId').val(),
					minNumberOfChildrenFilter: $('#MinNumberOfChildrenFilterId').val(),
					maxNumberOfChildrenFilter: $('#MaxNumberOfChildrenFilterId').val(),
					addressFilter: $('#AddressFilterId').val(),
					mobileFilter: $('#MobileFilterId').val(),
					emailFilter: $('#EmailFilterId').val(),
					isLockedFilter: $('#IsLockedFilterId').val(),
					genderFilter: $('#GenderFilterId').val(),
					identificationTypeNameFilter: $('#IdentificationTypeNameFilterId').val(),
					maritalStatusNameFilter: $('#MaritalStatusNameFilterId').val(),
					userNameFilter: $('#UserNameFilterId').val(),
					applicantStatusDescriptionFilter: $('#ApplicantStatusDescriptionFilterId').val(),
					countryNameFilter: $('#CountryNameFilterId').val(),
					governorateNameFilter: $('#GovernorateNameFilterId').val(),
					localityNameFilter: $('#LocalityNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditApplicantModalSaved', function () {
            getApplicants();
        });

		$('#GetApplicantsButton').click(function (e) {
            e.preventDefault();
            getApplicants();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getApplicants();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getApplicants();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getApplicants();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getApplicants();
        });
    });
})();

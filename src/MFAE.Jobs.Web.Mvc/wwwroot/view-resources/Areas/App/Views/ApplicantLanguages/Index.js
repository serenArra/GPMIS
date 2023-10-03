(function () {
    $(function () {

        var _$applicantLanguagesTable = $('#ApplicantLanguagesTable');
        var _applicantLanguagesService = abp.services.app.applicantLanguages;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.ApplicantLanguage';
        
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
            getApplicantLanguages();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getApplicantLanguages();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getApplicantLanguages();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getApplicantLanguages();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ApplicantLanguages.Create'),
            edit: abp.auth.hasPermission('Pages.ApplicantLanguages.Edit'),
            'delete': abp.auth.hasPermission('Pages.ApplicantLanguages.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ApplicantLanguages/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ApplicantLanguages/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditApplicantLanguageModal'
                });
                   

		 var _viewApplicantLanguageModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ApplicantLanguages/ViewapplicantLanguageModal',
            modalClass: 'ViewApplicantLanguageModal'
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

        var dataTable = _$applicantLanguagesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            makeAjax: $('#ApplicantId').val() > 0,
            deferLoading: $('#ApplicantId').val() > 0 ? null : 0,
            listAction: {
                ajaxFunction: _applicantLanguagesService.getAll,
                inputFilter: function () {
                    return {
                    filter: $('#ApplicantLanguagesTableFilter').val(),
                    applicantIdFilter: $('#ApplicantId').val(),
					narrativeFilter: $('#NarrativeFilterId').val(),
					applicantFirstNameFilter: $('#ApplicantFirstNameFilterId').val(),
					languageNameFilter: $('#LanguageNameFilterId').val(),
					conversationNameFilter: $('#ConversationNameFilterId').val(),
					conversationRateNameFilter: $('#ConversationRateNameFilterId').val()
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
                                    _viewApplicantLanguageModal.open({ id: data.record.applicantLanguage.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                _createOrEditModal.open({ id: data.record.applicantLanguage.id, applicantId: data.record.applicantLanguage.applicantId });   
                              
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
                                    entityId: data.record.applicantLanguage.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteApplicantLanguage(data.record.applicantLanguage);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "applicantLanguage.narrative",
						 name: "narrative"   
					},
					{
						targets: 3,
						 data: "applicantFirstName" ,
						 name: "applicantFk.firstName" 
					},
					{
						targets: 4,
						 data: "languageName" ,
						 name: "languageFk.name" 
					},
					{
						targets: 5,
						 data: "conversationName" ,
						 name: "conversationFk.name" 
					},
					{
						targets: 6,
						 data: "conversationRateName" ,
						 name: "conversationRateFk.name" 
					}
            ]
        });

        function getApplicantLanguages() {
            dataTable.ajax.reload();
        }

        function deleteApplicantLanguage(applicantLanguage) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _applicantLanguagesService.delete({
                            id: applicantLanguage.id
                        }).done(function () {
                            getApplicantLanguages(true);
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

        $('#CreateNewApplicantLanguageButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _applicantLanguagesService
                .getApplicantLanguagesToExcel({
				filter : $('#ApplicantLanguagesTableFilter').val(),
					narrativeFilter: $('#NarrativeFilterId').val(),
					applicantFirstNameFilter: $('#ApplicantFirstNameFilterId').val(),
					languageNameFilter: $('#LanguageNameFilterId').val(),
					conversationNameFilter: $('#ConversationNameFilterId').val(),
					conversationRateNameFilter: $('#ConversationRateNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditApplicantLanguageModalSaved', function () {
            getApplicantLanguages();
        });

		$('#GetApplicantLanguagesButton').click(function (e) {
            e.preventDefault();
            getApplicantLanguages();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getApplicantLanguages();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getApplicantLanguages();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getApplicantLanguages();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getApplicantLanguages();
        });
		
		
		

    });
})();

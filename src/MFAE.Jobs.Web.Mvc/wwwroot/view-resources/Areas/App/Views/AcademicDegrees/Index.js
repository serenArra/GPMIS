(function () {
    $(function () {

        var _$academicDegreesTable = $('#AcademicDegreesTable');
        var _academicDegreesService = abp.services.app.academicDegrees;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.AcademicDegree';
        
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
            getAcademicDegrees();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getAcademicDegrees();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getAcademicDegrees();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getAcademicDegrees();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.AcademicDegrees.Create'),
            edit: abp.auth.hasPermission('Pages.AcademicDegrees.Edit'),
            'delete': abp.auth.hasPermission('Pages.AcademicDegrees.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/AcademicDegrees/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/AcademicDegrees/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditAcademicDegreeModal'
                });
                   

		 var _viewAcademicDegreeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/AcademicDegrees/ViewacademicDegreeModal',
            modalClass: 'ViewAcademicDegreeModal'
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

        var dataTable = _$academicDegreesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _academicDegreesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#AcademicDegreesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
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
                                    _viewAcademicDegreeModal.open({ id: data.record.academicDegree.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.academicDegree.id });                                
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
                                    entityId: data.record.academicDegree.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteAcademicDegree(data.record.academicDegree);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "academicDegree.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "academicDegree.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "academicDegree.isActive",
						 name: "isActive"  ,
						render: function (isActive) {
							if (isActive) {
								return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
							}
							return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
					}
			 
					}
            ]
        });

        function getAcademicDegrees() {
            dataTable.ajax.reload();
        }

        function deleteAcademicDegree(academicDegree) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _academicDegreesService.delete({
                            id: academicDegree.id
                        }).done(function () {
                            getAcademicDegrees(true);
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

        $('#CreateNewAcademicDegreeButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _academicDegreesService
                .getAcademicDegreesToExcel({
				filter : $('#AcademicDegreesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditAcademicDegreeModalSaved', function () {
            getAcademicDegrees();
        });

		$('#GetAcademicDegreesButton').click(function (e) {
            e.preventDefault();
            getAcademicDegrees();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getAcademicDegrees();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getAcademicDegrees();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getAcademicDegrees();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getAcademicDegrees();
        });
		
		
		

    });
})();

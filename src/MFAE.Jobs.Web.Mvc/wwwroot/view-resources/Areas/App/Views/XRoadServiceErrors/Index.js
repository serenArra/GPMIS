﻿(function () {
    $(function () {

        var _$xRoadServiceErrorsTable = $('#XRoadServiceErrorsTable');
        var _xRoadServiceErrorsService = abp.services.app.xRoadServiceErrors;
		var _entityTypeFullName = 'MFAE.Jobs.XRoad.XRoadServiceError';
        
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
            getXRoadServiceErrors();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getXRoadServiceErrors();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getXRoadServiceErrors();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getXRoadServiceErrors();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServiceErrors.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServiceErrors.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServiceErrors.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/XRoadServiceErrors/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceErrors/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadServiceErrorModal'
                });
                   

		 var _viewXRoadServiceErrorModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceErrors/ViewxRoadServiceErrorModal',
            modalClass: 'ViewXRoadServiceErrorModal'
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

        var dataTable = _$xRoadServiceErrorsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceErrorsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadServiceErrorsTableFilter').val(),
					errorCodeFilter: $('#ErrorCodeFilterId').val(),
					errorMessageArFilter: $('#ErrorMessageArFilterId').val(),
					errorMessageEnFilter: $('#ErrorMessageEnFilterId').val(),
					xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val()
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
                                    _viewXRoadServiceErrorModal.open({ id: data.record.xRoadServiceError.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.xRoadServiceError.id });                                
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
                                    entityId: data.record.xRoadServiceError.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadServiceError(data.record.xRoadServiceError);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadServiceError.errorCode",
						 name: "errorCode"   
					},
					{
						targets: 3,
						 data: "xRoadServiceError.errorMessageAr",
						 name: "errorMessageAr"   
					},
					{
						targets: 4,
						 data: "xRoadServiceError.errorMessageEn",
						 name: "errorMessageEn"   
					},
					{
						targets: 5,
						 data: "xRoadServiceName" ,
						 name: "xRoadServiceFk.name" 
					}
            ]
        });

        function getXRoadServiceErrors() {
            dataTable.ajax.reload();
        }

        function deleteXRoadServiceError(xRoadServiceError) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServiceErrorsService.delete({
                            id: xRoadServiceError.id
                        }).done(function () {
                            getXRoadServiceErrors(true);
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

        $('#CreateNewXRoadServiceErrorButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadServiceErrorsService
                .getXRoadServiceErrorsToExcel({
				filter : $('#XRoadServiceErrorsTableFilter').val(),
					errorCodeFilter: $('#ErrorCodeFilterId').val(),
					errorMessageArFilter: $('#ErrorMessageArFilterId').val(),
					errorMessageEnFilter: $('#ErrorMessageEnFilterId').val(),
					xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceErrorModalSaved', function () {
            getXRoadServiceErrors();
        });

		$('#GetXRoadServiceErrorsButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceErrors();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadServiceErrors();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getXRoadServiceErrors();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getXRoadServiceErrors();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getXRoadServiceErrors();
        });
		
		
		

    });
})();

(function () {
    $(function () {

        var _$conversationRatesTable = $('#ConversationRatesTable');
        var _conversationRatesService = abp.services.app.conversationRates;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.ConversationRate';
        
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
            getConversationRates();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getConversationRates();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getConversationRates();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getConversationRates();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.ConversationRates.Create'),
            edit: abp.auth.hasPermission('Pages.ConversationRates.Edit'),
            'delete': abp.auth.hasPermission('Pages.ConversationRates.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/ConversationRates/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/ConversationRates/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditConversationRateModal'
                });
                   

		 var _viewConversationRateModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/ConversationRates/ViewconversationRateModal',
            modalClass: 'ViewConversationRateModal'
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

        var dataTable = _$conversationRatesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _conversationRatesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ConversationRatesTableFilter').val(),
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
                                    _viewConversationRateModal.open({ id: data.record.conversationRate.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.conversationRate.id });                                
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
                                    entityId: data.record.conversationRate.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteConversationRate(data.record.conversationRate);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "conversationRate.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "conversationRate.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "conversationRate.isActive",
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

        function getConversationRates() {
            dataTable.ajax.reload();
        }

        function deleteConversationRate(conversationRate) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _conversationRatesService.delete({
                            id: conversationRate.id
                        }).done(function () {
                            getConversationRates(true);
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

        $('#CreateNewConversationRateButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _conversationRatesService
                .getConversationRatesToExcel({
				filter : $('#ConversationRatesTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditConversationRateModalSaved', function () {
            getConversationRates();
        });

		$('#GetConversationRatesButton').click(function (e) {
            e.preventDefault();
            getConversationRates();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getConversationRates();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getConversationRates();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getConversationRates();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getConversationRates();
        });
		
		
		

    });
})();

(function () {
    $(function () {

        var _$conversationsTable = $('#ConversationsTable');
        var _conversationsService = abp.services.app.conversations;
		var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.Conversation';
        
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
            getConversations();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.startDate = null;
            getConversations();
        });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
        .on("apply.daterangepicker", (ev, picker) => {
            $selectedDate.endDate = picker.startDate;
            getConversations();
        })
        .on('cancel.daterangepicker', function (ev, picker) {
            $(this).val("");
            $selectedDate.endDate = null;
            getConversations();
        });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Conversations.Create'),
            edit: abp.auth.hasPermission('Pages.Conversations.Edit'),
            'delete': abp.auth.hasPermission('Pages.Conversations.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
                    viewUrl: abp.appPath + 'App/Conversations/CreateOrEditModal',
                    scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Conversations/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditConversationModal'
                });
                   

		 var _viewConversationModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Conversations/ViewconversationModal',
            modalClass: 'ViewConversationModal'
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

        var dataTable = _$conversationsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _conversationsService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#ConversationsTableFilter').val(),
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
                                    _viewConversationModal.open({ id: data.record.conversation.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                            _createOrEditModal.open({ id: data.record.conversation.id });                                
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
                                    entityId: data.record.conversation.id
                                });
                            }
						}, 
						{
                            text: app.localize('Delete'),
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteConversation(data.record.conversation);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "conversation.nameAr",
						 name: "nameAr"   
					},
					{
						targets: 3,
						 data: "conversation.nameEn",
						 name: "nameEn"   
					},
					{
						targets: 4,
						 data: "conversation.isActive",
						 name: "isActive"   
					}
            ]
        });

        function getConversations() {
            dataTable.ajax.reload();
        }

        function deleteConversation(conversation) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _conversationsService.delete({
                            id: conversation.id
                        }).done(function () {
                            getConversations(true);
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

        $('#CreateNewConversationButton').click(function () {
            _createOrEditModal.open();
        });        

		$('#ExportToExcelButton').click(function () {
            _conversationsService
                .getConversationsToExcel({
				filter : $('#ConversationsTableFilter').val(),
					nameArFilter: $('#NameArFilterId').val(),
					nameEnFilter: $('#NameEnFilterId').val(),
					isActiveFilter: $('#IsActiveFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditConversationModalSaved', function () {
            getConversations();
        });

		$('#GetConversationsButton').click(function (e) {
            e.preventDefault();
            getConversations();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getConversations();
		  }
		});

        $('.reload-on-change').change(function(e) {
			getConversations();
		});

        $('.reload-on-keyup').keyup(function(e) {
			getConversations();
		});

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getConversations();
        });
		
		
		

    });
})();

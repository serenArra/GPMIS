(function () {
    $(function () {

        var _$xRoadServicesTable = $('#XRoadServicesTable');
        var _xRoadServicesService = abp.services.app.xRoadServices;
		
        //$('.date-picker').datetimepicker({
        //    locale: abp.localization.currentLanguage.name,
        //    format: 'L'
        //});

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServices.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServices.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServices.Delete')
        };

         var _createOrEditModal = new app.ModalManager({
             viewUrl: abp.appPath + 'App/XRoadServices/CreateOrEditModal',
             scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServices/_CreateOrEditModal.js',
                    modalClass: 'CreateOrEditXRoadServiceModal'
                });
                   

		 var _viewXRoadServiceModal = new app.ModalManager({
             viewUrl: abp.appPath + 'App/XRoadServices/ViewxRoadServiceModal',
            modalClass: 'ViewXRoadServiceModal'
        });

		
		

        var getDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT00:00:00Z"); 
        }
        
        var getMaxDateFilter = function (element) {
            if (element.data("DateTimePicker").date() == null) {
                return null;
            }
            return element.data("DateTimePicker").date().format("YYYY-MM-DDT23:59:59Z"); 
        }

        var dataTable = _$xRoadServicesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServicesService.getAll,
                inputFilter: function () {
                    return {
					filter: $('#XRoadServicesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					providerCodeFilter: $('#ProviderCodeFilterId').val(),
					actionNameFilter: $('#ActionNameFilterId').val(),
					soapActionNameFilter: $('#SoapActionNameFilterId').val(),
					versionNoFilter: $('#VersionNoFilterId').val(),
					producerCodeFilter: $('#ProducerCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
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
                        cssClass: '',
                        text: '<i class="fa fa-cog"></i> <span class="d-none d-md-inline-block d-lg-inline-block d-xl-inline-block">' + app.localize('Actions') + '</span> <span class="caret"></span>',
                        items: [
						{
                                text: app.localize('View'),
                                iconStyle: 'far fa-eye mr-2',
                                action: function (data) {
                                    _viewXRoadServiceModal.open({ id: data.record.xRoadService.id });
                                }
                        },
						{
                            text: app.localize('Edit'),
                            iconStyle: 'far fa-edit mr-2',
                            visible: function () {
                                return _permissions.edit;
                            },
                            action: function (data) {
                                location.href = abp.appPath + 'App/XRoadServices/createoredit/' + data.record.xRoadService.id;
                            //_createOrEditModal.open({ id: data.record.xRoadService.id });                                
                            }
                        }, 
						{
                            text: app.localize('Delete'),
                            iconStyle: 'far fa-trash-alt mr-2',
                            visible: function () {
                                return _permissions.delete;
                            },
                            action: function (data) {
                                deleteXRoadService(data.record.xRoadService);
                            }
                        }]
                    }
                },
					{
						targets: 2,
						 data: "xRoadService.name",
						 name: "name"   
					},
					{
						targets: 3,
						 data: "xRoadService.providerCode",
						 name: "providerCode"   
					},
					{
						targets: 4,
						 data: "xRoadService.actionName",
						 name: "actionName"   
					},
					{
						targets: 5,
						 data: "xRoadService.soapActionName",
						 name: "soapActionName"   
					},
					{
						targets: 6,
						 data: "xRoadService.versionNo",
						 name: "versionNo"   
					},
					{
						targets: 7,
						 data: "xRoadService.producerCode",
						 name: "producerCode"   
                },
                {
                    targets: 8,
                    data: "xRoadService.status",
                    name: "status",
                    render: function (status) {
                        /*New = 1,Active = 2,InActive = 3*/
                        var displayName = app.localize('Enum_XRoadServiceStatusEnum_' + status);
                        switch (status) {
                            case 1:
                                return '<span class="badge badge-success">' + displayName + '</span>';
                            case 2:
                                return '<span class="badge badge-warning">' + displayName + '</span>';
                            default:
                                return '<span class="badge badge-secondary">' + displayName + '</span>';;
                        }
                    }

                }
					//{
					//	targets: 8,
					//	 data: "xRoadService.description",
					//	 name: "description"   
					//}
            ]
        });

        function getXRoadServices() {
            dataTable.ajax.reload();
        }

        function deleteXRoadService(xRoadService) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServicesService.delete({
                            id: xRoadService.id
                        }).done(function () {
                            getXRoadServices(true);
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

        $('#CreateNewXRoadServiceButton').click(function () {
            //_createOrEditModal.open();
            location.href = abp.appPath + 'App/XRoadServices/CreateOrEdit';
        });        

		$('#ExportToExcelButton').click(function () {
            _xRoadServicesService
                .getXRoadServicesToExcel({
				filter : $('#XRoadServicesTableFilter').val(),
					nameFilter: $('#NameFilterId').val(),
					providerCodeFilter: $('#ProviderCodeFilterId').val(),
					actionNameFilter: $('#ActionNameFilterId').val(),
					soapActionNameFilter: $('#SoapActionNameFilterId').val(),
					versionNoFilter: $('#VersionNoFilterId').val(),
					producerCodeFilter: $('#ProducerCodeFilterId').val(),
					descriptionFilter: $('#DescriptionFilterId').val()
				})
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceModalSaved', function () {
            getXRoadServices();
        });

		$('#GetXRoadServicesButton').click(function (e) {
            e.preventDefault();
            getXRoadServices();
        });

		$(document).keypress(function(e) {
		  if(e.which === 13) {
			getXRoadServices();
		  }
		});
		
		
		
    });
})();

(function () {
    app.modals.XRoadServiceAttributesMappingRequestIndex = function () {
        var tableId = 'XRoadServiceAttributeMappingsTableRequest' + new Date().getTime();
        $('table[name="XRoadServiceAttributeMappingsTableRequest"]').attr('id', tableId);

        var _$xRoadServiceAttributeMappingsTable = $('#' + tableId);//$('#XRoadServiceAttributeMappingsTableRequest');
        
        var _xRoadServiceAttributeMappingsService = abp.services.app.xRoadServiceAttributeMappings;

        

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServiceAttributeMappings.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/CreateOrEditModalRequest',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributeMappings/_CreateOrEditModalRequest.js',
            modalClass: 'CreateOrEditXRoadServiceAttributeMappingModalRequest'
        });


        var _viewXRoadServiceAttributeMappingModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributeMappings/ViewxRoadServiceAttributeMappingModal',
            modalClass: 'ViewXRoadServiceAttributeMappingModal'
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
        console.log(11111111111)
        var dataTable = _$xRoadServiceAttributeMappingsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceAttributeMappingsService.getAll,
                inputFilter: function () {
                    return {
                        //filter: $('#XRoadServiceAttributeMappingsTableFilter').val(),
                        //serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
                        //sourceValueFilter: $('#SourceValueFilterId').val(),
                        //destinationValueFilter: $('#DestinationValueFilterId').val(),
                        //                   xRoadServiceAttributeNameFilter: $('#XRoadServiceAttributeNameFilterId').val(),
                        attributeId: $('#AttributesMapping_AttributeIdRequest').val()
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
                                    _viewXRoadServiceAttributeMappingModal.open({ id: data.record.xRoadServiceAttributeMapping.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.xRoadServiceAttributeMapping.id });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteXRoadServiceAttributeMapping(data.record.xRoadServiceAttributeMapping);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "xRoadServiceAttributeMapping.sourceValue",
                    name: "sourceValue"
                },
                {
                    targets: 3,
                    data: "xRoadServiceAttributeMapping.destinationValue",
                    name: "destinationValue"
                },
                //{
                //    targets: 2,
                //    data: "xRoadServiceAttributeMapping.serviceAttributeType",
                //    name: "serviceAttributeType",
                //    render: function (serviceAttributeType) {
                //        return app.localize('Enum_XRoadAttributeTypeEnum_' + serviceAttributeType);
                //    }
                //},
                //{
                //    targets: 5,
                //    data: "xRoadServiceAttributeName",
                //    name: "attributeFk.name"
                //}
            ]
        });

        function getXRoadServiceAttributeMappings() {
            dataTable.ajax.reload();
        }

        function deleteXRoadServiceAttributeMapping(xRoadServiceAttributeMapping) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServiceAttributeMappingsService.delete({
                            id: xRoadServiceAttributeMapping.id
                        }).done(function () {
                            getXRoadServiceAttributeMappings(true);
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

        $('#CreateNewXRoadServiceAttributeMappingButtonRequest').click(function () {
            _createOrEditModal.open({ attributeId: $("#AttributesMapping_AttributeIdRequest").val() });
        });

        $('#ExportToExcelButton').click(function () {
            _xRoadServiceAttributeMappingsService
                .getXRoadServiceAttributeMappingsToExcel({
                    filter: $('#XRoadServiceAttributeMappingsTableFilter').val(),
                    serviceAttributeTypeFilter: $('#ServiceAttributeTypeFilterId').val(),
                    sourceValueFilter: $('#SourceValueFilterId').val(),
                    destinationValueFilter: $('#DestinationValueFilterId').val(),
                    xRoadServiceAttributeNameFilter: $('#XRoadServiceAttributeNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceAttributeMappingModalSavedRequest', function () {
            getXRoadServiceAttributeMappings();
        });

        $('#GetXRoadServiceAttributeMappingsButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceAttributeMappings();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getXRoadServiceAttributeMappings();
            }
        });



    };
})();

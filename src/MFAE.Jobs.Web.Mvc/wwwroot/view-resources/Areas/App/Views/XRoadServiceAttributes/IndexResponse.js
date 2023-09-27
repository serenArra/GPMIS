﻿(function () {
    app.modals.XRoadServiceAttributesResponseIndex = function () {

        var tableId = 'XRoadServiceAttributesTableResponse' + new Date().getTime();
        $('table[name="XRoadServiceAttributesTableResponse"]').attr('id', tableId);

        var _$xRoadServiceAttributesTable = $('#' + tableId);//$('#_$xRoadServiceAttributesTable');
        var _xRoadServiceAttributesService = abp.services.app.xRoadServiceAttributes;

        

        var _permissions = {
            create: abp.auth.hasPermission('Pages.XRoadServiceAttributes.Create'),
            edit: abp.auth.hasPermission('Pages.XRoadServiceAttributes.Edit'),
            'delete': abp.auth.hasPermission('Pages.XRoadServiceAttributes.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/CreateOrEditModalResponse',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/XRoadServiceAttributes/_CreateOrEditModalResponse.js',
            modalClass: 'CreateOrEditXRoadServiceAttributeModalResponse'
        });


        var _viewXRoadServiceAttributeModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/XRoadServiceAttributes/ViewxRoadServiceAttributeModal',
            modalClass: 'ViewXRoadServiceAttributeModal'
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

        var dataTable = _$xRoadServiceAttributesTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceAttributesService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#XRoadServiceAttributesTableFilter').val(),
                        serviceAttributeTypeFilter:2,// $('#ServiceAttributeTypeFilterId').val(),
                        attributeCodeFilter: $('#AttributeCodeFilterId').val(),
                        xMLPathFilter: $('#XMLPathFilterId').val(),
                        nameFilter: $('#NameFilterId').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        formatTransitionFilter: $('#FormatTransitionFilterId').val(),
                        xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val(),
                        xRoadServiceId: $("#XRoadServiceAttributes_XRoadServiceIdResponse").val()
                    };
                }
            },
            columnDefs: [
                {
                    className: 'control responsive',
                    orderable: false,
                    data: "xRoadServiceAttribute.id",
                    render: function (id) {
                        return '<span id="' + id + '"></span>';
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
                                    //_viewXRoadServiceAttributeModal.open({ id: data.record.xRoadServiceAttribute.id });
                                    console.log(99999999999);
                                    abp.event.trigger('app.XRoadServiceAttributeViewResponse', { id: data.record.xRoadServiceAttribute.id });
                                    dataTable.$('span#' + data.record.xRoadServiceAttribute.id).closest('table').find('tr').each(function (index, tr) {
                                        $(tr).removeClass('selected');
                                    });
                                    dataTable.$('span#' + data.record.xRoadServiceAttribute.id).closest('tr').addClass('selected');
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                iconStyle: 'far fa-edit mr-2',
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.xRoadServiceAttribute.id });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                iconStyle: 'far fa-trash-alt mr-2',
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteXRoadServiceAttribute(data.record.xRoadServiceAttribute);
                                }
                            }]
                    }
                },
                {
                    targets: 2,
                    data: "xRoadServiceAttribute.name",
                    name: "name"
                },
                {
                    targets: 3,
                    data: "xRoadServiceAttribute.xmlPath",
                    name: "xmlPath"
                },
                //{
                //    targets: 2,
                //    data: "xRoadServiceAttribute.serviceAttributeType",
                //    name: "serviceAttributeType",
                //    render: function (serviceAttributeType) {
                //        return app.localize('Enum_XRoadServiceAttributeTypeEnum_' + serviceAttributeType);
                //    }

                //},
                //{
                //    targets: 3,
                //    data: "xRoadServiceAttribute.attributeCode",
                //    name: "attributeCode"
                //},
                //{
                //    targets: 6,
                //    data: "xRoadServiceAttribute.description",
                //    name: "description"
                //},
                //{
                //    targets: 7,
                //    data: "xRoadServiceAttribute.formatTransition",
                //    name: "formatTransition"
                //},
                //{
                //    targets: 8,
                //    data: "xRoadServiceName",
                //    name: "xRoadServiceFk.name"
                //}
            ]
        });

        function getXRoadServiceAttributes() {
            dataTable.ajax.reload();
        }

        function deleteXRoadServiceAttribute(xRoadServiceAttribute) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _xRoadServiceAttributesService.delete({
                            id: xRoadServiceAttribute.id
                        }).done(function () {
                            getXRoadServiceAttributes(true);
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

        $('#CreateNewXRoadServiceAttributeButtonResponse').click(function () {
            _createOrEditModal.open({ xRoadServiceId: $("#XRoadServiceAttributes_XRoadServiceIdResponse").val() });
        });

        $('#ExportToExcelButton').click(function () {
            _xRoadServiceAttributesService
                .getXRoadServiceAttributesToExcel({
                    filter: $('#XRoadServiceAttributesTableFilter').val(),
                    serviceAttributeTypeFilter: 2,//$('#ServiceAttributeTypeFilterId').val(),
                    attributeCodeFilter: $('#AttributeCodeFilterId').val(),
                    xMLPathFilter: $('#XMLPathFilterId').val(),
                    nameFilter: $('#NameFilterId').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    formatTransitionFilter: $('#FormatTransitionFilterId').val(),
                    xRoadServiceNameFilter: $('#XRoadServiceNameFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditXRoadServiceAttributeModalSavedResponse', function () {
            getXRoadServiceAttributes();
        });

        $('#GetXRoadServiceAttributesButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceAttributes();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getXRoadServiceAttributes();
            }
        });



    };
})();
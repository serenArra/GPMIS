(function () {
    $(function () {

        var _$jobAdvertisementsTable = $('#JobAdvertisementsTable');
        var _jobAdvertisementsService = abp.services.app.jobAdvertisements;
        var _entityTypeFullName = 'MFAE.Jobs.ApplicationForm.JobAdvertisement';

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
        })
            .on("apply.daterangepicker", (ev, picker) => {
                $selectedDate.startDate = picker.startDate;
                getJobAdvertisements();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val("");
                $selectedDate.startDate = null;
                getJobAdvertisements();
            });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
        })
            .on("apply.daterangepicker", (ev, picker) => {
                $selectedDate.endDate = picker.startDate;
                getJobAdvertisements();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val("");
                $selectedDate.endDate = null;
                getJobAdvertisements();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.JobAdvertisements.Create'),
            edit: abp.auth.hasPermission('Pages.JobAdvertisements.Edit'),
            'delete': abp.auth.hasPermission('Pages.JobAdvertisements.Delete')
        };

        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/JobAdvertisements/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/JobAdvertisements/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditJobAdvertisementModal',
            modalSize: 'modal-xl'
        });


        var _viewJobAdvertisementModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/JobAdvertisements/ViewjobAdvertisementModal',
            modalClass: 'ViewJobAdvertisementModal'
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

        var dataTable = _$jobAdvertisementsTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _jobAdvertisementsService.getAll,
                inputFilter: function () {
                    return {
                        filter: $('#JobAdvertisementsTableFilter').val(),
                        descriptionFilter: $('#DescriptionFilterId').val(),
                        advertisementIdFilter: $('#AdvertisementIdFilterId').val(),
                        minAdvertisementDateFilter: getDateFilter($('#MinAdvertisementDateFilterId')),
                        maxAdvertisementDateFilter: getMaxDateFilter($('#MaxAdvertisementDateFilterId')),
                        minFromDateFilter: getDateFilter($('#MinFromDateFilterId')),
                        maxFromDateFilter: getMaxDateFilter($('#MaxFromDateFilterId')),
                        minToDateFilter: getDateFilter($('#MinToDateFilterId')),
                        maxToDateFilter: getMaxDateFilter($('#MaxToDateFilterId')),
                        minAllowedAgeFilter: $('#MinAllowedAgeFilterId').val(),
                        maxAllowedAgeFilter: $('#MaxAllowedAgeFilterId').val(),
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
                                    _viewJobAdvertisementModal.open({ id: data.record.jobAdvertisement.id });
                                }
                            },
                            {
                                text: app.localize('Edit'),
                                visible: function () {
                                    return _permissions.edit;
                                },
                                action: function (data) {
                                    _createOrEditModal.open({ id: data.record.jobAdvertisement.id });
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
                                        entityId: data.record.jobAdvertisement.id
                                    });
                                }
                            },
                            {
                                text: app.localize('Delete'),
                                visible: function () {
                                    return _permissions.delete;
                                },
                                action: function (data) {
                                    deleteJobAdvertisement(data.record.jobAdvertisement);
                                }
                            }]
                    }
                },                
                {
                    targets: 2,
                    data: "jobAdvertisement.advertisementId",
                    name: "advertisementId"
                },
                {
                    targets: 3,
                    data: "jobAdvertisement.advertisementDate",
                    name: "advertisementDate",
                    render: function (advertisementDate) {
                        if (advertisementDate) {
                            return moment(advertisementDate).format('D/M/Y');
                        }
                        return "";
                    }

                },
                {
                    targets: 4,
                    data: "jobAdvertisement.fromDate",
                    name: "fromDate",
                    render: function (fromDate) {
                        if (fromDate) {
                            return moment(fromDate).format('D/M/Y');
                        }
                        return "";
                    }

                },
                {
                    targets: 5,
                    data: "jobAdvertisement.toDate",
                    name: "toDate",
                    render: function (toDate) {
                        if (toDate) {
                            return moment(toDate).format('D/M/Y');
                        }
                        return "";
                    }

                },
                {
                    targets: 6,
                    data: "jobAdvertisement.allowedAge",
                    name: "allowedAge"
                },
                {
                    targets: 7,
                    data: "jobAdvertisement.isActive",
                    name: "isActive",
                    render: function (isActive) {
                        if (isActive) {
                            return '<div class="text-center"><i class="fa fa-check text-success" title="True"></i></div>';
                        }
                        return '<div class="text-center"><i class="fa fa-times-circle" title="False"></i></div>';
                    }

                }
            ]
        });

        function getJobAdvertisements() {
            dataTable.ajax.reload();
        }

        function deleteJobAdvertisement(jobAdvertisement) {
            abp.message.confirm(
                '',
                app.localize('AreYouSure'),
                function (isConfirmed) {
                    if (isConfirmed) {
                        _jobAdvertisementsService.delete({
                            id: jobAdvertisement.id
                        }).done(function () {
                            getJobAdvertisements(true);
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

        $('#CreateNewJobAdvertisementButton').click(function () {
            _createOrEditModal.open();
        });

        $('#ExportToExcelButton').click(function () {
            _jobAdvertisementsService
                .getJobAdvertisementsToExcel({
                    filter: $('#JobAdvertisementsTableFilter').val(),
                    descriptionFilter: $('#DescriptionFilterId').val(),
                    advertisementIdFilter: $('#AdvertisementIdFilterId').val(),
                    minAdvertisementDateFilter: getDateFilter($('#MinAdvertisementDateFilterId')),
                    maxAdvertisementDateFilter: getMaxDateFilter($('#MaxAdvertisementDateFilterId')),
                    minFromDateFilter: getDateFilter($('#MinFromDateFilterId')),
                    maxFromDateFilter: getMaxDateFilter($('#MaxFromDateFilterId')),
                    minToDateFilter: getDateFilter($('#MinToDateFilterId')),
                    maxToDateFilter: getMaxDateFilter($('#MaxToDateFilterId')),
                    minAllowedAgeFilter: $('#MinAllowedAgeFilterId').val(),
                    maxAllowedAgeFilter: $('#MaxAllowedAgeFilterId').val(),
                    isActiveFilter: $('#IsActiveFilterId').val()
                })
                .done(function (result) {
                    app.downloadTempFile(result);
                });
        });

        abp.event.on('app.createOrEditJobAdvertisementModalSaved', function () {
            getJobAdvertisements();
        });

        $('#GetJobAdvertisementsButton').click(function (e) {
            e.preventDefault();
            getJobAdvertisements();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getJobAdvertisements();
            }
        });

        $('.reload-on-change').change(function (e) {
            getJobAdvertisements();
        });

        $('.reload-on-keyup').keyup(function (e) {
            getJobAdvertisements();
        });

        $('#btn-reset-filters').click(function (e) {
            $('.reload-on-change,.reload-on-keyup,#MyEntsTableFilter').val('');
            getJobAdvertisements();
        });




    });
})();

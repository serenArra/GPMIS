(function ($) {
    app.modals.XRoadServiceLookupTableModal = function () {

        var _modalManager;

        var _xRoadServiceErrorsService = abp.services.app.xRoadServiceErrors;
        var _$xRoadServiceTable = $('#XRoadServiceTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$xRoadServiceTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceErrorsService.getAllXRoadServiceForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#XRoadServiceTableFilter').val()
                    };
                }
            },
            columnDefs: [
                {
                    targets: 0,
                    data: null,
                    orderable: false,
                    autoWidth: false,
                    defaultContent: "<div class=\"text-center\"><input id='selectbtn' class='btn btn-success' type='button' width='25px' value='" + app.localize('Select') + "' /></div>"
                },
                {
                    autoWidth: false,
                    orderable: false,
                    targets: 1,
                    data: "displayName"
                }
            ]
        });

        $('#XRoadServiceTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getXRoadService() {
            dataTable.ajax.reload();
        }

        $('#GetXRoadServiceButton').click(function (e) {
            e.preventDefault();
            getXRoadService();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getXRoadService();
            }
        });

    };
})(jQuery);


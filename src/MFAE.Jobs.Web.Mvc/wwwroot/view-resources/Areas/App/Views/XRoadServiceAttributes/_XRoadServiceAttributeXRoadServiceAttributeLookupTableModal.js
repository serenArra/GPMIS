(function ($) {
    app.modals.XRoadServiceAttributeLookupTableModal = function () {

        var _modalManager;

        var _xRoadServiceAttributesService = abp.services.app.xRoadServiceAttributes;
        var _$xRoadServiceAttributeTable = $('#XRoadServiceAttributeTable');

        this.init = function (modalManager) {
            _modalManager = modalManager;
        };


        var dataTable = _$xRoadServiceAttributeTable.DataTable({
            paging: true,
            serverSide: true,
            processing: true,
            listAction: {
                ajaxFunction: _xRoadServiceAttributesService.getAllXRoadServiceAttributeForLookupTable,
                inputFilter: function () {
                    return {
                        filter: $('#XRoadServiceAttributeTableFilter').val()
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

        $('#XRoadServiceAttributeTable tbody').on('click', '[id*=selectbtn]', function () {
            var data = dataTable.row($(this).parents('tr')).data();
            _modalManager.setResult(data);
            _modalManager.close();
        });

        function getXRoadServiceAttribute() {
            dataTable.ajax.reload();
        }

        $('#GetXRoadServiceAttributeButton').click(function (e) {
            e.preventDefault();
            getXRoadServiceAttribute();
        });

        $('#SelectButton').click(function (e) {
            e.preventDefault();
        });

        $(document).keypress(function (e) {
            if (e.which === 13) {
                getXRoadServiceAttribute();
            }
        });

    };
})(jQuery);


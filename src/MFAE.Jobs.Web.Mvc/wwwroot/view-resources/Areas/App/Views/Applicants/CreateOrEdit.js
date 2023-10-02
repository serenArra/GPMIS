(function () {
    $(function ()
    {
        var _applicantsService = abp.services.app.applicants;
        var _$applicantInformationForm = $('form[name=ApplicantInformationsForm]');

        _$applicantInformationForm.validate(
            {
                rules: {
                    identificationNo: {
                        required: true,
                        maxLength: '#identificationTypeId',
                        minLength: '#identificationTypeId',
                        validateIdentificationNo: true, validateIdentificationNo: '#identificationTypeId'

                    },
                    firstName: {
                        arabicName: true
                    },
                    fatherName: {
                        arabicName: true
                    },
                    grandFatherName: {
                        arabicName: true
                    },
                    familyName: {
                        arabicName: true
                    }
                },
                messages: {
                    identificationNo: {
                        required: false,
                        maxLength: app.localize('maxlength', 9),
                        minLength: app.localize('minlength', 9),
                        validateIdentificationNo: app.localize('NotValidIdentificationDocumentNo'),

                    }
                }
            }
        );

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
           getApplicants();
       })
       .on('cancel.daterangepicker', function (ev, picker) {
           $(this).val("");
           $selectedDate.startDate = null;
           getApplicants();
       });

        $('.endDate').daterangepicker({
            autoUpdateInput: false,
            singleDatePicker: true,
            locale: abp.localization.currentLanguage.name,
            format: 'L',
        })
            .on("apply.daterangepicker", (ev, picker) => {
                $selectedDate.endDate = picker.startDate;
                getApplicants();
            })
            .on('cancel.daterangepicker', function (ev, picker) {
                $(this).val("");
                $selectedDate.endDate = null;
                getApplicants();
            });

        var _permissions = {
            create: abp.auth.hasPermission('Pages.Applicants.Create'),
            edit: abp.auth.hasPermission('Pages.Applicants.Edit'),
            'delete': abp.auth.hasPermission('Pages.Applicants.Delete')
        };

        _$applicantInformationForm.validate();
        var _createOrEditModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/CreateOrEditModal',
            scriptUrl: abp.appPath + 'view-resources/Areas/App/Views/Applicants/_CreateOrEditModal.js',
            modalClass: 'CreateOrEditApplicantModal'
        });


        var _viewApplicantModal = new app.ModalManager({
            viewUrl: abp.appPath + 'App/Applicants/ViewapplicantModal',
            modalClass: 'ViewApplicantModal'
        });
        
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

        function save(successCallback) {

            if (!_$applicantInformationForm.valid()) {
                return;
            }
           
            if ($('#Applicant_IdentificationTypeId').prop('required') && $('#Applicant_IdentificationTypeId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('IdentificationType')));
                return;
            }
            if ($('#Applicant_MaritalStatusId').prop('required') && $('#Applicant_MaritalStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('MaritalStatus')));
                return;
            }
            if ($('#Applicant_LockedBy').prop('required') && $('#Applicant_LockedBy').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('User')));
                return;
            }
            if ($('#Applicant_CurrentStatusId').prop('required') && $('#Applicant_CurrentStatusId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('ApplicantStatus')));
                return;
            }
            if ($('#Applicant_CountryId').prop('required') && $('#Applicant_CountryId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Country')));
                return;
            }
            if ($('#Applicant_GovernorateId').prop('required') && $('#Applicant_GovernorateId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Governorate')));
                return;
            }
            if ($('#Applicant_LocalityId').prop('required') && $('#Applicant_LocalityId').val() == '') {
                abp.message.error(app.localize('{0}IsRequired', app.localize('Locality')));
                return;
            }

            var applicant = _$applicantInformationForm.serializeFormToObject();


            abp.ui.setBusy();
            _applicantsService.createOrEdit(
                applicant
            ).done(function () {
                abp.notify.info(app.localize('SavedSuccessfully'));
                abp.event.trigger('app.createOrEditApplicationFormModalSaved');

                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }).always(function () {
                abp.ui.clearBusy();
            });
        };

        function clearForm() {
            _$applicantInformationForm[0].reset();
        }

        $('#saveBtn').click(function () {
            save(function () {
                window.location = "/App/Welcome";
            });
        });

        $('#btnSave').click(function () {

            if (_$applicantInformationForm.valid()) {
                window.location = "/App/Welcome";
            }

        });

        $('#saveAndNewBtn').click(function () {
            save(function () {
                if (!$('input[name=id]').val()) {//if it is create page
                    clearForm();
                }
            });
        });


        var applicantId = function () {
            return $("#ApplicantId").val();
        };
       
        if (!applicantId()) {

            // Stepper lement
            var element = document.querySelector("#kt_wizard");

            // Initialize Stepper
            var stepper = new KTStepper(element);

            // Handle next step
            stepper.on("kt.stepper.next", function (stepper) {

                var isValid = true;
                $('#step' + stepper.getCurrentStepIndex()).find(':input').each(function () {
                    if ($(this).valid() == false) {
                        isValid = false;
                    }
                });

                if (isValid) {
                    if (stepper.getCurrentStepIndex() == "1") {

                        if (!_$applicantInformationForm.valid()) {
                            return false;
                        }

                        var applicantForm = _$applicantInformationForm.serializeFormToObject();
                       
                        abp.ui.setBusy();
                        _applicantsService.createOrEdit(
                            applicantForm
                        ).done(function (data) {
                            $("#ApplicantId").val(data);
                            abp.notify.info(app.localize('SavedSuccessfully'));
                            abp.event.trigger('app.createOrEditApplicationFormModalSaved');

                            if (typeof (successCallback) === 'function') {
                                successCallback();
                            }
                        }).always(function () {
                            abp.ui.clearBusy();
                        });
                    }
                    if (stepper.getCurrentStepIndex() == "2") {
                        if (!applicantId()) {
                            return false;
                        }

                        abp.ui.setBusy();
                        _applicantsService.checkApplicantStudiesInfo(
                            $("#ApplicantId").val(data)
                        ).done(function (data) {

                            if (data == false) {
                                abp.notify.info(app.localize('AtLeastOneApplicantStudiesInformationMustBeEntered'));
                                abp.event.trigger('app.checkApplicantStudiesInfo');
                                return false;
                            }

                        }).always(function () {
                            abp.ui.clearBusy();
                        });
                    }
                    if (stepper.getCurrentStepIndex() == "3") {

                        if (!applicantId()) {
                            return false;
                        }

                        abp.ui.setBusy();
                        _applicantsService.checkApplicantLanguagesInfo(
                            $("#ApplicantId").val(data)
                        ).done(function (data) {

                            if (data == false) {
                                abp.notify.info(app.localize('AtLeastOneApplicantLanguagesInformationMustBeEntered'));
                                abp.event.trigger('app.checkApplicantLanguagesInfo');
                                return false;
                            }

                        }).always(function () {
                            abp.ui.clearBusy();
                        });

                    }
                    if (stepper.getCurrentStepIndex() == "4") {
                        if (!_$applicantInformationForm.valid()) {
                            return false;
                        }

                        if (!applicantId()) {
                            return false;
                        }
                    }
                    if (stepper.getCurrentStepIndex() == "5") {

                        if (!_$applicantInformationForm.valid()) {
                            return false;
                        }

                        if (!applicantId()) {
                            return false;
                        }
                    }
                    stepper.goNext(); // go next step
                }
            });

            // Handle previous step
            stepper.on("kt.stepper.previous", function (stepper) {
                stepper.goPrevious(); // go previous step
            });
        }

    });
})();
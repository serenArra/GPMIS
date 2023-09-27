(function () {
    var _xRoadServicesService = abp.services.app.xRoadServices;
    var _applicantsService = abp.services.app.applicants;
    var _$InformationBankForm = null;
    _$InformationBankForm = $('form[name=InformationBankForm]');// _modalManager.getModal().find('form[name=XRoadServiceInformationsForm]');
    _$InformationBankForm.validate(
        {
            rules: {
                identificationDocumentNo: {
                    required: true,
                    maxLength: '#identificationTypeId',
                    minLength: '#identificationTypeId',
                    validateIdentificationDocumentNo: true, validateIdentificationDocumentNo: '#identificationTypeId',
                }
            },
            messages: {
                identificationDocumentNo: {
                    required: app.localize('Required'),
                    maxLength: app.localize('maxlength', 9),
                    minLength: app.localize('minlength', 9),
                    validateIdentificationDocumentNo: app.localize('NotValidIdentificationDocumentNo'),
                }
            }
        }
    );
    $("#btnFetchPersonInformationBank").click(function (event) {
        console.log($("#identificationTypeId").val());
        event.preventDefault();
        var sex = "";
        var idNo = $("#Person_IdentificationDocumentNo").val();
        // var idType = $("#identificationTypeId").val();
        $("#exsist_div").addClass('d-none');
        $('#PersonId').val('');
        if ($("#identificationTypeId").valid()) {
            if (($("#identificationTypeId").val() == IdType.PS || $("#identificationTypeId").val() == IdType.IL) && !$("#Person_IdentificationDocumentNo").valid()) {
                return;
            }

            abp.ui.setBusy();

            _applicantsService.fetchPerson(
                {
                    identificationDocumentNoId: $("#Person_IdentificationDocumentNo").val(),
                    identificationDocumentNoTypeId: $("#identificationTypeId").val()
                }
            ).done(function (data) {
                if (data != null) {
                    $("#Male").addClass('d-none');
                    $("#MaleEN").addClass('d-none');
                    $("#FeMale").addClass('d-none');
                    $("#FeMaleEN").addClass('d-none');

                    $(".informationBankView").removeClass('d-none');
                    console.log(data);
                    $("#FullnameAr").text(data.person.firstNameAr + " " + data.person.secondNameAr + " " + data.person.thirdNameAr + " " + data.person.fourthNameAr);
                    $("#FullnameEn").text(data.person.firstNameEn + " " + data.person.secondNameEn + " " + data.person.thirdNameEn + " " + data.person.fourthNameEn);

                    $("#MothernameEn").text(data.person.motherNameAr);
                    $("#MothernameAr").text(data.person.motherNameEn);
                    $("#PassportNum").text(data.person.passportNo);
                    $("#GenderAr").text(data.person.genderNameAr);
                    $("#GenderEn").text(data.person.genderNameEn);
                    $("#IDnumAr").text(data.person.documentNo);
                    $("#IDnumEn").text(data.person.documentNo);
                    var dateOfBirth = data.person.dateOfBirth.split("T");
                    $("#DateofDirthAr").text(dateOfBirth[0]);
                    $("#DateofDirthEn").text(dateOfBirth[0]);             
                    $("#ProfessionsAr").text(data.person.profession);
                    $("#ProfessionsEn").text(data.person.profession);
                    if (data.person.gender == Gender.Male)
                    {
                        $("#Male").removeClass('d-none');
                        $("#MaleEN").removeClass('d-none');
                    }
                    else if (data.person.gender == Gender.Female) {
                        $("#FeMale").removeClass('d-none');
                        $("#FeMaleEN").removeClass('d-none');
                    }
               
                    if (data.citizenPicture != null) {
                        $("#citizenPicture").attr('src', 'data:image/png;base64,' + data.citizenPicture);
                   
                    }
                    else {
                        $("#citizenPicture").attr('src', '/Common/Images/default-profile-picture.png');
                    }


                    //$("#birthPlace_countryId").attr('values', '[{"id": "' + data.person.birthPlace.countryId + '", "text": "' + data.person.birthPlace.countryName + '"}]');
                    //$("#birthPlace_governorateId").attr('values', '[{"id": "' + data.person.birthPlace.governorateId + '", "text": "' + data.person.birthPlace.governorateName + '"}]');
                    //  //$("#birthPlace_countryId").attr('values', '[{"id": "' + data.person.birthPlace.countryId + '", "text": "' + data.person.birthPlace.countryName + '"}]');
                    //$("#birthPlace_governorateId").attr('values', '[{"id": "' + data.person.birthPlace.governorateId + '", "text": "' + data.person.birthPlace.governorateName + '"}]');
                    //$("#birthPlace_localityId").attr('values', '[{"id": "' + data.person.birthPlace.localityId + '", "text": "' + data.person.birthPlace.localityName + '"}]');

                    EGov.init();

                    abp.ui.clearBusy();
                    abp.notify.info(app.localize('FetchedSuccessfully'));
                 
                } else {
                    abp.notify.error(app.localize('IdentificationDocumentNoNotExist'));
                }
            }).fail(function (error) {
                abp.notify.error(app.localize('ErrorFetchPerson'));
            }).always(function () {
                abp.ui.clearBusy();
            });
            _applicantsService.getPassportInfo(
                {
                    identificationDocumentNoId: $("#Person_IdentificationDocumentNo").val(),
                    identificationDocumentNoTypeId: $("#identificationTypeId").val()
                }
            ).done(function (data) {
                if (data != null) {
                    console.log(data);
                    var expiredDate = data.expireDate.split("T");
                    $("#ExpiryAr").text(expiredDate[0]);
                    $("#ExpiryEn").text(expiredDate[0]);
                }
            }).fail(function (error) {
                abp.notify.error(app.localize('ErrorFetchPerson'));
            }).always(function () {
                abp.ui.clearBusy();
            });
        }
    });

})();
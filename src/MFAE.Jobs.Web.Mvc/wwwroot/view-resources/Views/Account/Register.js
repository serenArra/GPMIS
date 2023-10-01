var CurrentPage = (function () {
    jQuery.validator.addMethod(
        'customUsername',
        function (value, element) {
            if (value === $('input[name="EmailAddress"]').val()) {
                return true;
            }

            return !$.validator.methods.email.apply(this, arguments);
        },
        abp.localization.localize('RegisterFormUserNameInvalidMessage')
    );

    jQuery.validator.addMethod("IdNotContinsSpace", function (value, element) {
        var id = String($('input[name="DocumentNo"]').val());

        if (id.indexOf(' ') == -1)
            return true;
        return false;

    }, app.localize('InvalidDocumentId'));

    jQuery.validator.addMethod("IdMoreThanFourCharacter", function (value, element) {
        var id = String($('input[name="DocumentNo"]').val());

        if (id.length > 4)
            return true;
        return false;

    }, app.localize('IdMoreThanFourCharacter'));

    jQuery.validator.addMethod("isValidID", function (value, element) {

        var documentId = $('#identificationTypeId').val();
        if (documentId == "1" || documentId == "3") {

            var id = String($('input[name="DocumentNo"]').val()).trim();

            if (id.length != 9 || isNaN(id)) return false;

            if (documentId == "1") {
                if (id.charAt(0) != '4' && id.charAt(0) != '8' && id.charAt(0) != '9')
                    return false;
            }
            // Pad string with zeros up to 9 digits
            id = id.length < 9 ? ("00000000" + id).slice(-9) : id;

            var result = Array
                .from(id, Number)
                .reduce((counter, digit, i) => {
                    const step = digit * ((i % 2) + 1);
                    return counter + (step > 9 ? step - 9 : step);
                }) % 10 === 0;


            return result;
        }
        else {
            return true;
        }

    }, app.localize('InvalidDocumentId'));

    var _passwordComplexityHelper = new app.PasswordComplexityHelper();

    var handleRegister = function () {
        $('.register-form').validate({
            rules: {
                PasswordRepeat: {
                    equalTo: '#RegisterPassword',
                },
                UserName: {
                    required: true,
                    customUsername: true,
                },
                DocumentNo: {
                    required: true,
                    isValidID: true,
                    IdNotContinsSpace: true,
                    IdMoreThanFourCharacter: true
                },
            },

            submitHandler: function (form) {
                function setCaptchaToken(callback) {
                    callback = callback || function () { };
                    if (!abp.setting.getBoolean('App.UserManagement.UseCaptchaOnRegistration')) {
                        callback();
                    } else {
                        grecaptcha.reExecute(function (token) {
                            $('#recaptchaResponse').val(token);
                            callback();
                        });
                    }
                }

                setCaptchaToken(function () {
                    form.submit();
                });
            },
        });

        $('.register-form input').keypress(function (e) {
            if (e.which === 13) {
                if ($('.register-form').valid()) {
                    $('.register-form').submit();
                }
                return false;
            }
        });

        $('input[name=Password]').pwstrength({
            i18n: {
                t: function (key) {
                    return app.localize(key);
                },
            },
        });

        _passwordComplexityHelper.setPasswordComplexityRules(
            $('input[name=Password], input[name=PasswordRepeat]'),
            window.passwordComplexitySetting
        );
    };

    return {
        init: function () {
            handleRegister();
        },
    };
})();

function verfiyCitizenInfo(successCallback) {
    var _verfiyCitizenService = abp.services.app.account;
    var DocType = $('#identificationTypeId').val();
    if (DocType == 1) {
        abp.ui.setBusy();
        _verfiyCitizenService.verifyCitizenInfo({
            identificationTypeId: DocType,
            identityNo: $('#DocumentNo').val()           
        }).done(function (output) {

            if (output.code != "") {
                abp.message.error(output.message, app.localize('Error'))
            }
            else {

                $('#fakeId').val(output.identityNo);
                $('#FirstName').val(output.name).prop("readonly", true).addClass('readonly-input');
                $('#SecondName').val(output.secondName).prop("readonly", true).addClass('readonly-input');
                $('#ThirdName').val(output.thirdName).prop("readonly", true).addClass('readonly-input');
                $('#Surname').val(output.surname).prop("readonly", true).addClass('readonly-input');
                if (output.firstNameEn.trim() != 'null') {
                    $('#FirstNameEn').val(output.firstNameEn).prop("readonly", true).addClass('readonly-input');
                }
                else {
                    $('#FirstNameEn').val(output.name).prop("readonly", true).addClass('readonly-input');
                }
                if (output.secondNameEn.trim() != 'null') {
                    $('#SecondNameEn').val(output.secondNameEn).prop("readonly", true).addClass('readonly-input');
                }
                else {
                    $('#SecondNameEn').val(output.secondName).prop("readonly", true).addClass('readonly-input');
                }
                if (output.thirdNameEn.trim() != 'null') {
                    $('#ThirdNameEn').val(output.thirdNameEn).prop("readonly", true).addClass('readonly-input');
                }
                else {
                    $('#ThirdNameEn').val(output.thirdName).prop("readonly", true).addClass('readonly-input');
                }
                if (output.fourthNameEn.trim() != 'null') {
                    $('#FourthNameEn').val(output.fourthNameEn).prop("readonly", true).addClass('readonly-input');
                }
                else {
                    $('#FourthNameEn').val(output.surname).prop("readonly", true).addClass('readonly-input');
                }
                if (output.docPhoto.trim() != 'null') {
                    $('#imageUrl').removeClass('d-none');
                    $('#defaultImage').addClass('d-none');
                    $('#imageUrl').attr('src', 'data:image/png;base64,' + output.docPhoto);
                }
                else {
                    $('#imageUrl').addClass('d-none');
                    $('#defaultImage').removeClass('d-none');
                }

                if (typeof (successCallback) === 'function') {
                    successCallback();
                }
            }
        }).always(function () {
            abp.ui.clearBusy();
        });
    }
    else {
        $('.readonly-input').val('').prop("readonly", false).removeClass('readonly-input');
        $('#imageUrl').addClass('d-none');
        $('#defaultImage').removeClass('d-none');

        if (typeof (successCallback) === 'function') {
            successCallback();
        }
    }
}

$("#register-back-btn").on("click", function () {
    location.href = '/Account/Login';
});

$(document).ready(function () {
    

    var _$applicationFormInformationForm = null;
    _$applicationFormInformationForm = $('form[name=ApplicationForm]');
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

                if (!_$applicationFormInformationForm.valid()) {                    
                    return false;
                }

                verfiyCitizenInfo(function () {
                    
                });

                
            }
            if (stepper.getCurrentStepIndex() == "2") {
                if (!_$applicationFormInformationForm.valid()) {
                    return false;
                }



            }
            if (stepper.getCurrentStepIndex() == "3") {

                if (!_$applicationFormInformationForm.valid()) {
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
});

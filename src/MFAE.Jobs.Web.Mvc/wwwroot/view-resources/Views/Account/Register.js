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

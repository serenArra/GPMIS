(function ($) {
  $.validator.setDefaults({
    errorElement: 'div',
    errorClass: 'invalid-feedback',
    focusInvalid: false,
    submitOnKeyPress: true,
    ignore: ':hidden',
    highlight: function (element) {
      $(element).closest('.form-group').find('input:eq(0)').addClass('is-invalid');
    },

    unhighlight: function (element) {
      $(element).closest('.form-group').find('input:eq(0)').removeClass('is-invalid');
    },

    errorPlacement: function (error, element) {
      if (element.closest('.input-icon').length === 1) {
        error.insertAfter(element.closest('.input-icon'));
      } else {
        error.insertAfter(element);
      }
    },

    success: function (label) {
      label.closest('.form-group').removeClass('has-danger');
      label.remove();
    },

    submitHandler: function (form) {
      $(form).find('.alert-danger').hide();
    },
  });

  $.validator.addMethod(
    'email',
    function (value, element) {
      return /^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(value);
    },
    'Please enter a valid Email.'
  );

  $.validator.addMethod(
    'text',
    function (value, element) {
      return new RegExp($(element).attr('pattern')).test(value);
    },
    'Please enter a well formatted value.'
  );

    $.validator.addMethod('minLength', function (value, element, param) {
        if (element.name == "identificationDocumentNo" || element.name == "identificationDocumentNo2") {
            if ($(param).val() == 1 || $(param).val() == 5) {
                if (value.length < 9) {
                    return false
                }
                else {
                    return true
                }
            }
            return true
        }
        else {
            return true
        }

    }, 'Invalid value');

    $.validator.addMethod('maxLength', function (value, element, param) {

        if (element.name == "identificationDocumentNo") {
            if ($(param).val() == 1 || $(param).val() == 5) {
                if (value.length > 9) {
                    return false
                }
                else {
                    return true
                }
            }
            return true
        }
        else {
            return true
        }

    }, 'Invalid value');

    $.validator.addMethod('validateIdentificationDocumentNo', function (value, element, param) {

        if (element.name == "identificationDocumentNo") {
            if ($(param).val() == IdType.PS || $(param).val() == IdType.IL) {
                var splitted_id = value.split('');

                if ($(param).val() == IdType.PS && !(splitted_id[0] == 4 || splitted_id[0] == 8 || splitted_id[0] == 9))
                    return false;

                if ($(param).val() == IdType.IL && !(splitted_id[0] == 0 || splitted_id[0] == 2 || splitted_id[0] == 3))
                    return false;

                var result1 = new Array();
                var sum_total = 0;
                var div = 10;
                var div_total = 0;
                var last_result = 0;

                var validator = new Array();
                validator[0] = "1"; validator[1] = "2";
                validator[2] = "1"; validator[3] = "2";
                validator[4] = "1"; validator[5] = "2";
                validator[6] = "1"; validator[7] = "2";
                for (var i = 0; i < splitted_id.length - 1; i++) {

                    result1[i] = splitted_id[i] * validator[i];

                    if (result1[i] >= 10) {

                        result1[i] = (result1[i]).toString();
                        var splitted_result = result1[i].split('');
                        var sum = parseInt(splitted_result[0]) + parseInt(splitted_result[1]);
                        result1[i] = sum;

                    }

                    sum_total = parseInt(sum_total) + parseInt(result1[i]);

                }
                div_total = parseFloat(sum_total) % div;

                if (div_total != 0) {
                    last_result = div - parseInt(div_total);
                } else {
                    last_result = 0;
                }

                if (last_result == parseInt(splitted_id[8])) {
                    return true;
                } else {
                    return false;
                }
            }
            return true;

        }
        else {
            return true
        }



    }, app.localize("NotValidIdentificationDocumentNo"));

    $.validator.addMethod('validateDocumentNo', function (value, element, param) {

        if (element.name == "documentNo") {
            if ($(param).val() == IdType.PS || $(param).val() == IdType.IL) {
                var splitted_id = value.split('');

                if ($(param).val() == IdType.PS && !(splitted_id[0] == 4 || splitted_id[0] == 8 || splitted_id[0] == 9))
                    return false;

                if ($(param).val() == IdType.IL && !(splitted_id[0] == 0 || splitted_id[0] == 2 || splitted_id[0] == 3))
                    return false;

                var result1 = new Array();
                var sum_total = 0;
                var div = 10;
                var div_total = 0;
                var last_result = 0;

                var validator = new Array();
                validator[0] = "1"; validator[1] = "2";
                validator[2] = "1"; validator[3] = "2";
                validator[4] = "1"; validator[5] = "2";
                validator[6] = "1"; validator[7] = "2";
                for (var i = 0; i < splitted_id.length - 1; i++) {

                    result1[i] = splitted_id[i] * validator[i];

                    if (result1[i] >= 10) {

                        result1[i] = (result1[i]).toString();
                        var splitted_result = result1[i].split('');
                        var sum = parseInt(splitted_result[0]) + parseInt(splitted_result[1]);
                        result1[i] = sum;

                    }

                    sum_total = parseInt(sum_total) + parseInt(result1[i]);

                }
                div_total = parseFloat(sum_total) % div;

                if (div_total != 0) {
                    last_result = div - parseInt(div_total);
                } else {
                    last_result = 0;
                }

                if (last_result == parseInt(splitted_id[8])) {
                    return true;
                } else {
                    return false;
                }
            }
            return true;

        }
        else {
            return true
        }



    }, app.localize("NotValidIdentificationDocumentNo"));

    $.validator.addMethod("age18", function (value, element) {
        if (this.optional(element))
            return true;
        var today = new Date();
        var parts = value.split('/');
        var birthDate = new Date(parts[2], parts[1] - 1, parts[0]);

        var age = today.getFullYear() - birthDate.getFullYear();

        if (age > 18) {
            return true;
        }

        var m = today.getMonth() - birthDate.getMonth();

        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;


        }

        return age >= 18;
    }, app.localize("AgeMustBe+18"));

})(jQuery);

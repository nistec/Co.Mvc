
(function($) {

    /*
     * Validate phone
     */
    $.formUtils.addValidator({
        name : 'phone',
        validatorFunction: function (phone) {
            if (phone.match(/^(|\()(0|972)(\d{1}|\d{2})(|[\)\/\.-])([0-9]{7})$/) === null) {
                return false;
            } 
            return true;
        },
        errorMessage : '',
        errorMessageKey: 'badPhone'
    });
    /*
    * Validate mobile
    */
    $.formUtils.addValidator({
        name: 'mobile',
        validatorFunction: function (mobile) {
            if (mobile.match(/^(05(0|[2-9])(|-)[0-9]{7}|(|972)5(0|[2-9])[0-9]{7})$/) === null) {
                return false;
            }
            return true;
        },
        errorMessage: '',
        errorMessageKey: 'badMobile'
    });
    /*
    * Validate time hh:mm
    */
    $.formUtils.addValidator({
        name: 'time',
        validatorFunction: function (time) {
            if (time.match(/^(\d{2}):(\d{2})$/) === null) {
                return false;
            } else {
                var hours = parseInt(time.split(':')[0], 10);
                var minutes = parseInt(time.split(':')[1], 10);
                if (hours > 23 || minutes > 59) {
                    return false;
                }
            }
            return true;
        },
        errorMessage: '',
        errorMessageKey: 'badTime'
    });

    /*
     * Is this a valid birth date
     */
    $.formUtils.addValidator({
        name: 'birthdate',
        validatorFunction: function (val, $el, conf) {
            var dateFormat = 'dd/mm/yyyy';
            if ($el.valAttr('format')) {
                dateFormat = $el.valAttr('format');
            }
            else if (typeof conf.dateFormat != 'undefined') {
                dateFormat = conf.dateFormat;
            }

            var inputDate = $.formUtils.parseDate(val, dateFormat);
            if (!inputDate) {
                return false;
            }

            var d = new Date();
            var currentYear = d.getFullYear();
            var year = inputDate[0];
            var month = inputDate[1];
            var day = inputDate[2];

            if (year === currentYear) {
                var currentMonth = d.getMonth() + 1;
                if (month === currentMonth) {
                    var currentDay = d.getDate();
                    return day <= currentDay;
                }
                else {
                    return month < currentMonth;
                }
            }
            else {
                return year < currentYear && year > (currentYear - 124); // we can not live for ever yet...
            }
        },
        errorMessage: '',
        errorMessageKey: 'badDate'
    });
})(jQuery);
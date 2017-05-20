
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

})(jQuery);
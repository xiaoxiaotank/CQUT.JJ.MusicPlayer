$(function () {
    $('input').iCheck({
        checkboxClass: 'icheckbox_square-blue',
        radioClass: 'iradio_square-blue',
        increaseArea: '20%' /* optional */
    });
});


function afterLogin(data) {
    if (data.isSuccessed) {
        location.href = data.jsonObject.value;
    }
    else if (!data.isSuccessed) {
        fail_prompt(data.message);
    }
}
function postCnt() {
    var _id = parseInt($(this).data('id')),
        _cnt = parseInt($(this).val());
    if (_cnt >= 10) {
        _cnt = 10;
        ($(this).val(10));
    }
    if (_cnt <= 0) {
        _cnt = 1;
        ($(this).val(1));
    }
    $.post(
        '/editbasket',
        { PartID: _id, Quantity: _cnt },
        function (ans) {
            console.log(ans);
        },
        'json'
    ); 
}

$('.cnt').change(postCnt).keyup(postCnt);
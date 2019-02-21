function postCnt() {
    var _id = parseInt($(this).data('id')),
        _cnt = parseInt($(this).val());
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
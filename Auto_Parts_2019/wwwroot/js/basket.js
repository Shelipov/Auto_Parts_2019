
//function postCnt() {
//    var _id = parseInt($(this).data('id')),
//        _cnt = parseInt($(this).val());
//    $.post(
//        '/editbasket',
//        { PartID: _id, Quantity: _cnt },
//        function (ans) {
//            console.log(ans);
//        },
//        'json'
//    );
//}

//$('.cnt').change(postCnt).keyup(postCnt);

var elem = document.getElementById('count');
elem.onchange = function (e) {
    var value = parseInt(e.target.value);
    if (!value || value >= 10) {
        e.target.value = '10';
    }
    if (!value || value <= 0) {
        e.target.value = '1';
    }
}
function postCnt() {
    var _id = parseInt($(this).data('id')),
        _cnt = elem.value;//parseInt($(this).val());
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
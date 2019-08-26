function postEmail() {
    var _id = parseInt($(this).data('id'));
    $.post(
        '/editemail',
        { _MutualSettlementID: _id},
        function (ans) {
            console.log(ans);
        },
        'json'
    );
}

$('#TT').click(postEmail); //.change(postEmail).keyup(postEmail);
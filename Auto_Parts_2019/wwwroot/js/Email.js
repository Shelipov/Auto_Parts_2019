function postEmail() {
    var _id = parseInt($(this).data('id'));
    $.post(
        '/editemail',
        { _MutualSettlementID: _id},
        'json'
    );
}

$('.glyphicon').click(postEmail); //.change(postEmail).keyup(postEmail);
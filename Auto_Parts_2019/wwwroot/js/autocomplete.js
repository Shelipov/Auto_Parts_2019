$(function () {
    $("[data-autocomplete-sourse]").each(function () {
        var target = $(this);
        target.autocomplete({
            source: function (req, rsp) {
                $.get(
                    target.attr("data-autocomplete-sourse"),
                    { number: req.term },
                    rsp);
            }       
        });
    });
});
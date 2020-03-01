function expand() {
    $(".search").toggleClass("close");
    $(".input").toggleClass("square");
    if ($('.search').hasClass('close')) {
        $('input').focus();
    } else {
        $('input').blur();
    }
}
$('button').on('click', expand);
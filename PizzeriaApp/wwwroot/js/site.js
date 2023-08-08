if (document.readyState == 'loading') {
    document.addEventListener('DOMContentLoaded', ready)
} else {
    ready()
}

function ready() {
    // Page loadup
    $(window).on("load", function () {
        $(".loader-wrapper").fadeOut("slow");
    });

    // Enable Tooltips Bootstrap
    $(document).ready(function () {
        $("body").tooltip({ selector: '[data-toggle=tooltip]' });
    });

    // Carousel reset to begining
    $('#carousel-example').on('slide.bs.carousel', function (e) {
        var $e = $(e.relatedTarget);
        var idx = $e.index();
        var itemsPerSlide = 3;
        var totalItems = $('.carousel-item').length;

        if (idx >= totalItems - (itemsPerSlide - 1)) {
            var it = itemsPerSlide - (totalItems - idx);
            for (var i = 0; i < it; i++) {
                // append slides to end
                if (e.direction == "left") {
                    $('.carousel-item').eq(i).appendTo('.carousel-inner');
                }
                else {
                    $('.carousel-item').eq(0).appendTo('.carousel-inner');
                }
            }
        }
    });
    // Scroll down
    $(document).ready(function () {
        $('a#animate').click(function (e) {
            e.preventDefault();
            $('html, body').animate({ scrollTop: $($(this).attr('href')).offset().top }, 330, 'linear');
        })
    });

}
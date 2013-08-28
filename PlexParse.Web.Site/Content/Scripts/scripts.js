/// <reference path="//ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js" />
(function () {

    var plexParse = {

        init: function () {
            
            // bind events
            plexParse.events.init();
        },

        events: {

            init: function () {

                // init form
                $('form#form').on('submit', function (e) {

                    var form = $(this);
                    var data = form.serialize();
                    //var url = $('input#url', form).val();
                    var links = $('div#links');

                    // while processing
                    links.html('<em>One sec...</em>');

                    $.ajax({
                        url: form.attr('action'),
                        data: data,
                        type: 'post',
                        success: function (data) {
                            links.html(data);
                        },
                        error: function () {
                            links.html('<span class="error">An internal error has occurred. Please try again later.</span>');
                        }
                    });

                    e.preventDefault();
                });

                // init row delete
                $('div#links').on('click', 'div > a.delete', function () {
                    if (confirm('You sure?')) {
                        $(this).parent().remove();
                    }
                });
            }
        }
    };

    window.pp = plexParse;

    $(window.pp.init);

})();
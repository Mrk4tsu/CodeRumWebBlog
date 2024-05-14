$(document).ready(function () {
    $(document).on('click', function (event) {
        var $clickedOn = $(event.target),
            $collapsableItems = $('.collapse'),
            isToggleButton = ($clickedOn.closest('.collapse').length == 1),
            isLink = ($clickedOn.closest('a').length == 1),
            isOutsideNavbar = ($clickedOn.parents('.navbar').length == 0);

        if (!isToggleButton && isLink && isOutsideNavbar) {
            $collapsableItems.each(function () {
                $(this).collapse('hide');
            });
        }
    });



    $('#comment-button').click(function () {
        var commentButton = $(this);
        var comment = $('#comment').val();
        if (!comment) {
            $('#messageModalBody').text('Vui lòng nhập nội dung, không được để trống!');
            $('#messageModal').modal('show');
            return;
        }
        var contentId = commentButton.data('content-id'); // get the content id from data-content-id attribute

        $.ajax({
            url: '/Blog/AddComment',
            type: 'POST',
            data: { comment: comment, contentId: contentId },
            success: function (response) {
                if (response.success) {
                    // append the new comment to the comment list
                    var newComment = '<div class="row-comment mt-1">' +
                        '<div class="row mx-2">' +
                        '<img class="mr-2" src="' + response.avatar + '" alt="">' +
                        '<div class="comment-list-top ">' +
                        '<span class="h6 font-weight-bold">' +
                        '<a href="/Account/Detail/' + response.userId + '" class="text-info">' + response.userName + '</a>' +
                        '</span>' +
                        '<br>' +
                        (response.isAuthor != "author" ? '<p class="badge badge-success mr-2"> Author </p>' : '<p class="badge badge-secondary mr-2"> Visitor </p>') +
                        '<span class="small text-secondary text-date" data-date=' + response.createAt + '></span>' +
                        '</div>' +
                        '</div>' +
                        '<div class="comment-list-body mx-4">' +
                        '<span class="text-light ">' + comment + '</span>' +
                        '</div>' +
                        '<div class="comment-list-footer p-2">' +
                        '<div class="d-flex justify-content-between w-100">' +
                        '<a href="#" class="ml-3" data-toggle="collapse" data-target="#reply_' + response.commentId + '"> <i class="fa fa-reply" aria-hidden="true"></i> <span>Reply</span></a>' +
                        '</div>' +
                        '<div>' +
                        '<a href="#" class="mx-2" data-toggle="tooltip" title="Báo xấu!">' +
                        '<i class="fa fa-flag" aria-hidden="true"></i>' +
                        '</a>' +
                        '</div>' +
                        '</div>' +
                        '</div>';
                    $('.list-comment-recent').prepend(newComment);
                    $('#comment').val(''); // clear the comment input
                } else {
                    $('#messageModalBody').text(response.message);
                    $('#messageModal').modal('show');
                }
            }
        });
    });
});


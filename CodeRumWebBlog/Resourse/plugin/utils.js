window.onload = function () {
    var allScripts = document.getElementsByTagName('script');
    for (var i = allScripts.length; i >= 0; i--) {
        if (allScripts[i] && allScripts[i].getAttribute('src') != null && allScripts[i].getAttribute('src').indexOf('https://ads.mgmt.somee.com/serveimages/ad2/WholeInsert5.js') != -1)
            allScripts[i].parentNode.removeChild(allScripts[i]);
    }
};

$(document).ready(function () {
    $('.delete-link').html('<i class="fas fa-trash p-1"></i>');
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });

    $(".text-date").each(function () {
        var date = new Date($(this).data("date"));
        $(this).text(utils.timeAgo(date));
    });
});

var utils = {
    init: function () {
        utils.registerEvents('.btn-change-user-status', "/Admin/User/ChangeStatus");
        utils.registerEvents('.btn-change-content-status', "/Admin/Content/ChangeStatus");
        utils.registerEvents('.btn-change-category-status', "/Admin/Category/ChangeStatus");
    },
    registerEvents: function (buttonClass, url) {
        $(buttonClass).off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: url,
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã kích hoạt');
                        btn.removeClass('badge-danger').addClass('badge-success');
                    }
                    else {
                        btn.text('Chưa kích hoạt');
                        btn.removeClass('badge-success').addClass('badge-danger');
                    }
                }
            });
        });
    },
    timeAgo: function (date) {
    var seconds = Math.floor((new Date() - date) / 1000);
    var interval = Math.floor(seconds / 31536000);
    if (interval >= 1) {
        return interval + " năm trước";
    }
    interval = Math.floor(seconds / 2592000);
    if (interval >= 1) {
        return interval + " tháng trước";
    }
    interval = Math.floor(seconds / 86400);
    if (interval >= 1) {
        return interval + " ngày trước";
    }
    interval = Math.floor(seconds / 3600);
    if (interval >= 1) {
        return interval + " giờ trước";
    }
    interval = Math.floor(seconds / 60);
    if (interval >= 1) {
        return interval + " phút trước";
    }
    return "Vừa xong";
},
    updateTimeAgo: function () {
        $(".text-date").each(function () {
            var date = new Date($(this).data("date"));
            $(this).text(utils.timeAgo(date));
        });
    }

}
utils.init();

function deleteComment(id) {
    var confirmDelete = confirm("Bạn có chắc chắc xóa bình luận này?");
    if (confirmDelete) {
        $.post('/Blog/DeleteComment', { id: id }, function (data, e) {
            if (data.success) {
                $('.comment_' + id).remove();
                // Refresh the page or remove the comment from the DOM
            } else {
                // Show an error message
            }
        });
    }
}

$(document).ready(function () {
    $('.delete-link').html('<i class="fas fa-trash p-1"></i>');
});
$(document).ready(function () {
    $("#myInput").on("keyup", function () {
        var value = $(this).val().toLowerCase();
        $("#myTable tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
        });
    });
});
$(document).ready(function () {
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
        if (interval > 1) {
            return interval + " năm trước";
        }
        interval = Math.floor(seconds / 2592000);
        if (interval >= 1) {
            return interval + " tháng trước";
        }
        interval = Math.floor(seconds / 86400);
        if (interval > 1) {
            return interval + " ngày trước";
        }
        interval = Math.floor(seconds / 3600);
        if (interval >= 1) {
            var minutes = Math.floor((seconds % 3600) / 60);
            return interval + " giờ " + minutes + " phút trước";
        }
        interval = Math.floor(seconds / 60);
        if (interval >= 1) {
            return interval + " phút trước";
        }
        return Math.floor(seconds) + " giây trước";
    },
    updateTimeAgo: function () {
        $(".text-date").each(function () {
            var date = new Date($(this).data("date"));
            $(this).text(utils.timeAgo(date));
        });
    }
}
utils.init();
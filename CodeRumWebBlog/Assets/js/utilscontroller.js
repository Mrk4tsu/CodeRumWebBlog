var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-change-user-status').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",
                data: { id: id },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Đã kích hoạt');
                    }
                    else {
                        btn.text('Chưa kích hoạt');
                    }
                }
            });
        });
    }
}
user.init();
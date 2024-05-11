//var user = {
//    init: function () {
//        user.registerEvents();
//    },
//    registerEvents: function () {
//        $('.btn-change-user-status').off('click').on('click', function (e) {
//            e.preventDefault();
//            var btn = $(this);
//            var id = btn.data('id');
//            $.ajax({
//                url: "/Admin/User/ChangeStatus",
//                data: { id: id },
//                dataType: "json",
//                type: "POST",
//                success: function (response) {
//                    console.log(response);
//                    if (response.status == true) {
//                        btn.text('Đã kích hoạt');
//                    }
//                    else {
//                        btn.text('Chưa kích hoạt');
//                    }
//                }
//            });
//        });
//    },
//}
//user.init();

//var content = {
//    init: function () {
//        content.registerEvents();
//    },
//    registerEvents: function () {
//        $('.btn-change-content-status').off('click').on('click', function (e) {
//            e.preventDefault();
//            var btn = $(this);
//            var id = btn.data('id');
//            $.ajax({
//                url: "/Admin/Content/ChangeStatus",
//                data: { id: id },
//                dataType: "json",
//                type: "POST",
//                success: function (response) {
//                    console.log(response);
//                    if (response.status == true) {
//                        btn.text('Đã kích hoạt');
//                    }
//                    else {
//                        btn.text('Chưa kích hoạt');
//                    }
//                }
//            });
//        });
//    },
//}
//content.init();

//var category = {
//    init: function () {
//        category.registerEvents();
//    },
//    registerEvents: function () {
//        $('.btn-change-category-status').off('click').on('click', function (e) {
//            e.preventDefault();
//            var btn = $(this);
//            var id = btn.data('id');
//            $.ajax({
//                url: "/Admin/Category/ChangeStatus",
//                data: { id: id },
//                dataType: "json",
//                type: "POST",
//                success: function (response) {
//                    console.log(response);
//                    if (response.status == true) {
//                        btn.text('Đã kích hoạt');
//                    }
//                    else {
//                        btn.text('Chưa kích hoạt');
//                    }
//                }
//            });
//        });
//    },
//}
//category.init();

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
                    }
                    else {
                        btn.text('Chưa kích hoạt');
                    }
                }
            });
        });
    },
}
utils.init();
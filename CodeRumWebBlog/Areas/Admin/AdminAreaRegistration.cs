using System.Web.Mvc;

namespace CodeRumWebBlog.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            // Route trang chủ cho admin
            context.MapRoute(
                name: "AdminPage",
                url: "trang-chu-quan-tri-vien",
                defaults: new
                {
                    controller = "Home",
                    action = "Index"
                });
            // Route cho danh sách danh mục
            context.MapRoute(
                name: "AdminListCategory",
                url: "danh-sach-danh-muc",
                defaults: new
                {
                    controller = "Category",
                    action = "Index"
                });
            // Route cho danh sách danh mục
            context.MapRoute(
                name: "AdminCreateCategory",
                url: "tao-moi-danh-muc",
                defaults: new
                {
                    controller = "Category",
                    action = "Create"
                });

            // Route cho danh sách người dùng
            context.MapRoute(
                name: "AdminListUser",
                url: "danh-sach-nguoi-dung",
                defaults: new
                {
                    controller = "User",
                    action = "Index"
                });

            // Route cho tạo mới người dùng
            context.MapRoute(
                name: "AdminCreateUser",
                url: "tao-moi-nguoi-dung",
                defaults: new
                {
                    controller = "User",
                    action = "Create"
                });

            // Route cho chỉnh sửa người dùng
            context.MapRoute(
                name: "AdminEditUser",
                url: "chinh-sua-nguoi-dung-{id}",
                defaults: new
                {
                    controller = "User",
                    action = "Edit"
                });

            context.MapRoute(
                name: "Admin_default",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}
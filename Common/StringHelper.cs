using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{
    public class StringHelper
    {
        public static string ToUnsignString(string input)
        {
            input = input.Trim();
            for (int i = 0x20; i < 0x30; i++)
            {
                input = input.Replace(((char)i).ToString(), " ");
            }
            input = input.Replace(".", "-");
            input = input.Replace(" ", "-");
            input = input.Replace(",", "-");
            input = input.Replace(";", "-");
            input = input.Replace(":", "-");
            input = input.Replace("  ", "-");
            Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
            string str = input.Normalize(NormalizationForm.FormD);
            string str2 = regex.Replace(str, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
            while (str2.IndexOf("?") >= 0)
            {
                str2 = str2.Remove(str2.IndexOf("?"), 1);
            }
            while (str2.Contains("--"))
            {
                str2 = str2.Replace("--", "-").ToLower();
            }
            return str2.ToLower();
        }
        public static string EmailBody(string url, string code)
        {
            string logoUrl = "https://gatapchoi.id.vn/Resourse/client/img/logo.png"; // Replace with your absolute logo URL
            string html = $@"<div style=""display:flex; justify-content:center"">
    <div style=""width: fit-content; border-radius:15px; font-family: 'Montserrat Medium', sans-serif; background: rgba(39, 61, 82); text-align: center; padding: 10px 40px; "">
        <div style="" margin: 20px 0px;"">
            <img style=""width: 250px;"" src='{logoUrl}' />
            <p style=""font-size: 30px; color: #fff;"">Chào mừng bạn đến với <span style=""color: #28a745; font-weight: bold;"">CodeRumBlog</span></p>
            <p style=""font-size: 16px; color: #fff;"">Đây là thư dùng 1 lần để thực hiện xác minh tài khoản.</p>
            <a style=""background: #28a745; color: #fff; padding: 10px 20px; border-radius: 5px; text-decoration: none; "" href='{url}'>Xác minh tài khoản</a>
            <p style=""font-size: 30px; color: #fff;"">Mã xác thực người dùng: </p>
            <a style=""background: rgba(39, 101, 112) ;font-size:28px; color: #fff; padding: 15px 20px; margin-bottom:15px; border-radius: 15px; text-decoration: none; "">{code}</a>
            <p style=""color:orangered; font-style:italic; margin-top: 15px;"">Vui lòng không chia sẻ mã này với bất kì ai</p>
        </div>
    </div>
</div>";
            return html;
        }
        public static string[] GetFilesInDirectory(string path)
        {
            return Directory.GetFiles(path);
        }

    }

}

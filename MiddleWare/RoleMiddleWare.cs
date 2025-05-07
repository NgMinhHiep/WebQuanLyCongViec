using System.IO;

namespace WebNC_BTL_QLCV.MiddleWare
{
    public class RoleMiddleWare
    {
        private readonly RequestDelegate _next;

        public RoleMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // lấy thông tin vai trò từ session
            var role = context.Session.GetInt32("userRole");
            var path = context.Request.Path.ToString().ToLower();
            /*
                        // kiểm tra quyền truy cập
                        if(context.Request.Path.StartsWithSegments("/Account/AdminDashboard") && role != 1)
                        {
                            // nếu ko đủ quyền, chuyển hướng tới trang NoAccess
                            context.Response.Redirect("/Errors/NoAccess");
                            return;
                        }

                        //tiếp tục xử lý request nếu đủ quyền
                        await _next(context);
            */
            // Các đường dẫn cho phép người dùng truy cập
            string[] userAllowedPaths = new string[]
            {
            "/account/login",
            "/account/register",
            "/account/logout",
            "/home",
            "/personal_information",
            "/personaltask",
            "/group",
            "/feedbacktask",
            "/groupmember",
            "/groupnote",
            "/grouptask",
            "/grouptaskfile",
            "/notification",
            "/parentgrouptask",
            "/personalnote",
            "/reporttaskfile",
            "/user/index",
            "/user/changepassword",
            "/errors/noaccess",
            "/css", "/js", "/lib", "/images"
            };

            // Các đường dẫn cho phép admin truy cập
            string[] adminAllowedPaths = new string[]
            {
            "/account/login",
            "/account/register",
            "/account/logout",
            "/account/admindashboard",
            "/personal_information",
            "/user/index",
            "/user/changepassword",
            "/errors/noaccess",
            "/css", "/js", "/lib", "/images"
            };

            // Nếu chưa đăng nhập, cho qua middleware (để đến controller xử lý redirect về login)
            if (!role.HasValue)
            {
                await _next(context);
                return;
            }

            Console.WriteLine("Yêu cầu path: " + path);

            if (role == 1) // admin
            {
                if (!adminAllowedPaths.Any(p => path.StartsWith(p)))
                {
                    context.Response.Redirect("/Errors/NoAccess");
                    return;
                }
            }
            else // người dùng
            {
                if (!userAllowedPaths.Any(p => path.StartsWith(p)))
                {
                    context.Response.Redirect("/Errors/NoAccess");
                    return;
                }
            }


            await _next(context);
        }
    }
}

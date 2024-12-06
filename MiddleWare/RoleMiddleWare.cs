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

            // kiểm tra quyền truy cập
            if(context.Request.Path.StartsWithSegments("/Account/AdminDashboard") && role != 1)
            {
                // nếu ko đủ quyền, chuyển hướng tới trang NoAccess
                context.Response.Redirect("/Errors/NoAccess");
                return;
            }

            //tiếp tục xử lý request nếu đủ quyền
            await _next(context);
        }
    }
}

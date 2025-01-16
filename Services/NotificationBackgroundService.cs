namespace WebNC_BTL_QLCV.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public NotificationBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var personalTaskService = scope.ServiceProvider.GetRequiredService<PersonalTaskService>();
                    personalTaskService.NotifyTasksEndingSoon(3);

                    var groupTaskService = scope.ServiceProvider.GetRequiredService<GroupTaskService>();
                    groupTaskService.NotifyTasksEndingSoon(2);
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // Chạy mỗi 24 giờ
            }

            

        }
    }
}

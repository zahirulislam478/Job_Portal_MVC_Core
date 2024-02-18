namespace JobPortal.HostedServices
{
    public class IdentityDbSeederHostedService : IHostedService
    {
        private readonly IServiceProvider serviceProvider;
        public IdentityDbSeederHostedService(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<IdentityDbInitializer>();
            await seeder.SeedAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}

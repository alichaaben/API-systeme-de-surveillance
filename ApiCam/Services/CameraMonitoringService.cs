

/*
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiCam.Services
{
    public class CameraMonitoringService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ILogger<CameraMonitoringService> _logger;

        public CameraMonitoringService(IServiceProvider services, ILogger<CameraMonitoringService> logger)
        {
            _services = services;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    
                    await CheckDisconnectedCamerasAsync();

                    
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erreur lors de la vérification des caméras déconnectées");
                }
            }
        }

        private async Task CheckDisconnectedCamerasAsync()
        {
            using var scope = _services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<YourDbContext>(); // Remplacez YourDbContext par votre propre contexte de base de données

            var disconnectedCameras = await dbContext.Cameras.Where(c => c.Status == "déconnectée").ToListAsync();

            if (disconnectedCameras.Any())
            {
                _logger.LogWarning($"{disconnectedCameras.Count} caméra(s) déconnectée(s) détectée(s).");
                
            }
        }
    }
}
*/

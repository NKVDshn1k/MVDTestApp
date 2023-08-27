using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVDTestApp.Data;
using MVDTestApp.Data.Base;
using MVDTestApp.Data.Entityes;
using MVDTestApp.Data.Repositories;
using MVDTestApp.View;
using MVDTestApp.ViewModel;
using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Threading;

namespace MVDTestApp;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private static IHost _host;
    public static IServiceProvider Services => 
        _host.Services;
    public App()
    {
        _host = Host.CreateDefaultBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddSingleton<MainWindow>();

               services.AddDbContext<MVDTestContext>(
                   op => op.UseSqlServer("server=localhost;database=MVDTest;Trusted_Connection=true;TrustServerCertificate=True"));//hostContext.Configuration.GetConnectionString("MsSql")));

               services.AddTransient<MainWindowViewModel>();
               services.AddTransient<WorkTaskCreateOrEditeViewModel>();
               services.AddTransient<WorkTaskDetailsViewModel>();

               services.AddScoped<IRepository<WorkTask>, WorkTaskRepository>();
               services.AddTransient<Logging.ILogger, Logging.FileLogger>();
               services.AddAutoMapper(typeof(App).Assembly);
           })
           .Build();
    }

    private void OnStartup(object sender, StartupEventArgs e)
    {
        var mainWindow = Services.GetService<MainWindow>();
        mainWindow.Show();

    }
    protected override async void OnExit(ExitEventArgs e)
    {
        using (_host)
        {
            await _host.StopAsync();
        }
        base.OnExit(e);
    }
}

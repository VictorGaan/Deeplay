using Deeplay.PersonnelRecords.Persistence.Contexts;
using Deeplay.PersonnelRecords.ViewModels;
using Deeplay.PersonnelRecords.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Deeplay.PersonnelRecords
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();
            using (var serviceScope = this.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
                dbContext.Database.Migrate();
            }
        }

        public new static App Current => (App)Application.Current;

        protected override void OnStartup(StartupEventArgs e)
        {
            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.DataContext = Services.GetService<MainVM>();
            MainWindow.Show();
        }
        public IServiceProvider Services { get; }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<MainVM>();

            services.AddDbContext<DataContext>();

            services.AddSingleton(x => new MainWindow());
            services.AddTransient(x => new EmployeeWindow());

            return services.BuildServiceProvider();
        }
    }
}

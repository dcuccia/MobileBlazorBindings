using System;
using System.Windows;

using Microsoft.MobileBlazorBindings.WebView.Windows;
using Xamarin.Forms.Platform.WPF.Extensions;

// Platform-agnostic dependencies
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.MobileBlazorBindings;
using Microsoft.MobileBlazorBindings.Elements;
using Microsoft.Extensions.Hosting;
using System.CodeDom;
using System.Windows.Controls;

namespace HybridApp.VanillaWpfTest
{
    public class VanillaWpfApplication : Application
    {
        [STAThread]
        public static void Main(string[] args = null)
        {
            VanillaWpfApplication app = new VanillaWpfApplication(args, null);
            app.Run();
        }

        private IHost _host;

        public VanillaWpfApplication(string[] args = null, IFileProvider fileProvider = null)
        {
            // bootstrapping code that would typically be pushed down to the platform-agnostic level (HybridApp.App)
            // leaving it here so as not to touch the rest of the solution for now
            var hostBuilder = MobileBlazorBindingsHost.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();

                    // Register app-specific services
                    services.AddSingleton<CounterState>();
                })
                .UseWebRoot("wwwroot");

            if (fileProvider != null)
            {
                hostBuilder.UseStaticFiles(fileProvider);
            }
            else
            {
                hostBuilder.UseStaticFiles();
            }

            _host = hostBuilder.Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Xamarin.Forms.Forms.Init();
            BlazorHybridWindows.Init();

            // create a Xamarin Forms ContentPage container
            var xfContentPage = new Xamarin.Forms.ContentPage();

            // add MBB Hybrid Blazor/Xamarin components as-needed to that container
            _host.AddComponent<Main>(parent: xfContentPage);

            // optional, create a vanilla WPF container of some sort (here, a Grid)
            var grid = new System.Windows.Controls.Grid();

            // create a top-level vanilla WPF window, with the Grid as it's content
            var window = new Window { Content = grid };

            // add the Xamarin container to the WPF grid (can safely add other WPF elements to the grid as well)
            grid.Children.Add(xfContentPage.ToFrameworkElement());

            // show the top-level vanilla WPF window
            window.Show();
        }
    }
}

﻿using BlinForms.Framework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BlinFormsSample
{
    static class Program
    {
        [STAThread]
        static async Task Main()
        {
            await Host.CreateDefaultBuilder()
                .AddBlinForms()
                .ConfigureServices((hostContext, services) =>
                {
                    // Register app-specific services
                    services.AddSingleton<AppState>();
                    services.AddSingleton<IBlinFormsStartup, TodoAppStartup>();

                    // Register root form content
                    services.AddRootFormContent<TodoApp>();
                })
                .Build()
                .RunAsync();
        }
    }
}

﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LetsDoStuff.Domain;
using LetsDoStuff.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LetsDoStuff.WebApi
{
    [AllowAnonymous]
    public static class Program
    {
        public static void Main()
        {
            var settingsFileName = "appsettings.json";
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var baseUrl = new ConfigurationBuilder().AddJsonFile(settingsFileName).Build().GetValue<string>("BaseUrl");

            var host = new WebHostBuilder()
                .UseKestrel()
                .ConfigureAppConfiguration((host, configurationBuilder) =>
                {
                  configurationBuilder.AddJsonFile("appsettings.json");
                })
                .ConfigureLogging((context, logging) =>
                {
                    var env = context.HostingEnvironment;
                    var config = context.Configuration.GetSection("Logging");
                    logging.AddConfiguration(config);
                    logging.AddConsole();
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Critical)
                    .AddFilter("Default", LogLevel.Trace)
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Information)
                    .AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Warning);
                })
                .UseStartup<Startup>()
                .UseUrls(baseUrl)
                .Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetService<LdsContext>();
                SeedTestData(context);
            }

            host.Run();
        }

        private static void SeedTestData(LdsContext context)
        {
            using (LdsContext context = new LdsContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    context.Users.Add(new User { Name = "Tom", Age = 33 });
                    context.Users.Add(new User { Name = "Alice", Age = 26 });
                    context.SaveChanges();
                }

                if (!context.Tags.Any())
                {
                    context.Tags.Add(new Tag { Name = "Music" });
                    context.Tags.Add(new Tag { Name = "Open-air" });
                    context.Tags.Add(new Tag { Name = "Indoor" });
                    context.Tags.Add(new Tag { Name = "Sport" });
                    context.Tags.Add(new Tag { Name = "Intellectual" });
                    context.SaveChanges();
                }

                if (!context.Activities.Any())
                {
                    context.Activities.Add(new Activity { Name = "Concert", Description = "Classical music", Capacity = 100, Creator = context.Users.Where(u => u.Name.Contains("Tom")).FirstOrDefault(), Tags = new List<Tag>(context.Tags.Where(e => e.Name.Contains("Music"))) });
                    context.Activities.Add(new Activity { Name = "Concert", Description = "Classical music", Capacity = 100, Creator = context.Users.Where(u => u.Name.Contains("Alice")).FirstOrDefault(), Tags = new List<Tag>(context.Tags.Where(e => e.Name.Contains("Music"))) });
                    context.Activities.Add(new Activity { Name = "Concert", Description = "Classical music", Capacity = 100, Creator = context.Users.Where(u => u.Name.Contains("Tom")).FirstOrDefault(), Tags = new List<Tag>(context.Tags.Where(e => e.Name.Contains("Music"))) });
                    context.Activities.Add(new Activity { Name = "Concert", Description = "Classical music", Capacity = 100, Creator = context.Users.Where(u => u.Name.Contains("Tom")).FirstOrDefault(), Tags = new List<Tag>(context.Tags.Where(e => e.Name.Contains("Music"))) });
                    context.Activities.Add(new Activity { Name = "Hicking", Description = "Altai Mountains", Capacity = 10, Creator = context.Users.Where(u => u.Name.Contains("Tom")).FirstOrDefault() });
                    context.Activities.Add(new Activity { Name = "Meeting", Description = "10 am", Capacity = 10 });
                    context.SaveChanges();
                }
            }
        }
    }
}
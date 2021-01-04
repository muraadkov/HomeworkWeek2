using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WorkerFolderService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private FileSystemWatcher watcher;
        private readonly string path = @"C:/Users/murka/Desktop/folder";  

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = path;
            watcher.Created += OnCreate;
            watcher.Changed += OnChange;
            watcher.Deleted += OnDelete;
            watcher.Renamed += OnRename;
            
            return base.StartAsync(cancellationToken);
        }

        private void OnCreate(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning"); 
            _logger.LogInformation("File created at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File changed at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File deleted at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);

        }
        private void OnRename(object sender, FileSystemEventArgs e)
        {
            _logger.LogError("Some error");
            _logger.LogWarning("Some warning");
            _logger.LogInformation("File renamed at: {time}", DateTimeOffset.Now);
            SendMessage(e.FullPath);
        }

        public async Task SendMessage(string fileName)
        {
            var message = new
            {
                Type = "email",
                JsonContent = "File with name " + fileName + "was added!"
            };

            var json = JsonConvert.SerializeObject(fileName);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:65430/api/queue/add", data);
                string result = response.Content.ReadAsStringAsync().Result;
                _logger.LogInformation(result);
            }
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                watcher.EnableRaisingEvents = true;
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}

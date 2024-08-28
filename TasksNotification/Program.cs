using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;

using Microsoft.Extensions.Logging;

var tokenAcquirerFactory = TokenAcquirerFactory.GetDefaultInstance();

tokenAcquirerFactory.Services
    .AddLogging((logBuilder) => logBuilder.SetMinimumLevel(LogLevel.Trace).AddConsole());

tokenAcquirerFactory.Services
    .AddDownstreamApi("TaskApi", tokenAcquirerFactory.Configuration.GetSection("TaskApi"));

var serviceProvider = tokenAcquirerFactory.Build();
var api = serviceProvider.GetRequiredService<IDownstreamApi>();

var tasks = await GetTasks();
Console.WriteLine(string.Join("\n",tasks.Select(t => t.Description).ToArray()));

var newTask = await AddTask();
Console.WriteLine(newTask);

tasks = await GetTasks();
Console.WriteLine(string.Join("\n",tasks.Select(t => t.Description).ToArray()));

return;

async Task<IEnumerable<Task>> GetTasks()
{
    var tasks = await api.GetForAppAsync<IEnumerable<Task>>("TaskApi");
    return tasks!;
}

async Task<Task> AddTask()
{
    var task = await api.PostForAppAsync<Task, Task>("TaskApi", new Task("clean my office"), options =>
    {
        options.RelativePath = "/tasks";
    });

    return task!;
}


internal record Task(string Description);
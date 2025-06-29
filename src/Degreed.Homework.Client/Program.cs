var app = WebApplication
    .CreateBuilder()
    .Build();

app
    .UseDefaultFiles()
    .UseStaticFiles();

app.Run();

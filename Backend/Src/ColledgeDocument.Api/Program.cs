var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var jwtOptions = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>() ?? throw new ArgumentException("Jwt options is missing!");

builder.Services.Configure<JwtOptions>(
    configuration.GetSection(nameof(JwtOptions)));

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    var kestrelSection = configuration.GetSection("Kestrel");
    serverOptions.Configure(kestrelSection);
}).UseKestrel();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddJwtAuthentication(jwtOptions);

builder.Services.AddDatabase(configuration);
builder.Services.AddHelpers();
builder.Services.AddSwagger();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

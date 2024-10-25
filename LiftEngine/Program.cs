using LiftEngine;
using LiftEngine.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");


await Task.Delay(2000);
var elevador = new LiftModule(-2, 10, 0).Start();
//elevador.RequestMoveTo(5);
////await Task.Delay(1200);
//elevador.RequestMoveTo(3);
////await Task.Delay(3000);
//elevador.RequestMoveTo(9);
////await Task.Delay(500);
//elevador.RequestMoveTo(1);
////await Task.Delay(4000);
//elevador.PrintStatus();

bool endApp = false;

while (!endApp)
{
    Console.WriteLine("Type a floor between -2 and 10, or Q do quit");
    var input = Console.ReadLine();

    if (input.ToUpper().Trim() == "Q")
    {
        Console.WriteLine("Quitting");
        endApp = true;
    }
    else
    {
        if (!await RequestLift(input)) 
        {
            Console.WriteLine("Invalid option");
        }
    }
}

async Task<bool> RequestLift(string floor)
{
    if(!Int32.TryParse(floor, out var destination))
    {
        return false;
    }

    if (destination < -2 || destination > 10) 
    {
        Console.WriteLine("Invalid floor");
    }

    await Task.Delay(200);
    elevador.RequestMoveTo(destination);

    return true;
}

//app.Run();
using Grpc.Net.Client;
using LiftEngine;

bool endApp = false;

string address = "https://localhost:7049";
var channel = GrpcChannel.ForAddress(address);
var client = new Greeter.GreeterClient(channel);

while (!endApp)
{
    Console.WriteLine("Type a floor between -2 and 10, or Q do quit");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
        continue;

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
    if (!Int32.TryParse(floor, out var destination))
    {
        return false;
    }

    if (destination < -2 || destination > 10)
    {
        Console.WriteLine("Invalid floor");
    }

    var input = new HelloRequest { Name = "3" };

    await Task.Delay(200);


    var reply = await client.SayHelloAsync(input);
    //elevador.RequestMoveTo(destination);

    return true;
}
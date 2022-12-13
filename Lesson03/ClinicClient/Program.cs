using Grpc.Net.Client;
using ClinicServiceNamespace;

namespace ClinicClient;

internal class Program
{
    static void Main(string[] args)
    {
        AppContext.SetSwitch(
            "System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

        using var channel = GrpcChannel.ForAddress("http://localhost:5001");
        var client = new ClinicService.ClinicServiceClient(channel);

        int count = 10;
        for (int i = 0; i < count; i++)
        {
            CreateClinicClient(client);
        }
        var allClients = GetAllClinicClients(client);
        UpdateClinicClient(client, allClients.First().ClientId);
        GetClinicClientbyId(client, allClients.First().ClientId);
        DeleteClinicClientById(client, allClients.First().ClientId);
        allClients = GetAllClinicClients(client);

        foreach (var clinicClient in allClients)
        {
            DeleteClinicClientById(client, clinicClient.ClientId);
        }

        Console.ReadLine();
    }

    private static void UpdateClinicClient(ClinicService.ClinicServiceClient client, int v)
    {
        Console.WriteLine($"Update client #{v}");
        var response = client.UpdateClient(new UpdateClientRequest
        {
            ClientId = v,
            Document = "new doc",
            Firstname = "Jane",
            Patronymic = "Marta",
            Surname = "Smith"
        });
        Console.WriteLine($"Update result code: {response.ErrCode}\nUpdate message: {response.ErrMessage}");
    }

    private static void DeleteClinicClientById(ClinicService.ClinicServiceClient client, int v)
    {
        Console.WriteLine($"Delete client #{v}");
        var response = client.DeleteClient(new DeleteClientRequest { ClientId = v });
        Console.WriteLine($"delete result code: {response.ErrCode}\ndelete message: {response.ErrMessage}");
    }

    private static void GetClinicClientbyId(ClinicService.ClinicServiceClient client, int v)
    {
        var getClientByIdResponse = client.GetClientById(new GetClientByIdRequest { ClientId = v });
        if (getClientByIdResponse.ErrCode == 0)
        {
            var clinicClient = getClientByIdResponse.Client;
            Console.WriteLine($"Client #{v}: ");
            Console.WriteLine($"|{clinicClient.ClientId,3}|{clinicClient.Surname,10}|{clinicClient.Firstname,10}|{clinicClient.Patronymic,10}|{clinicClient.Document,10}");
        }

    }

    private static ClientResponse[] GetAllClinicClients(ClinicService.ClinicServiceClient client)
    {
        var getClientsResponse = client.GetClients(new GetClientsRequest());
        if (getClientsResponse.ErrCode == 0)
        {
            Console.WriteLine("|{0,3}|{1,10}|{2,10}|{3,10}|{4,10}", "Id", "Surname", "Firstname", "Patronymic", "Document");
            foreach (var clinicClient in getClientsResponse.Clients)
            {
                Console.WriteLine($"|{clinicClient.ClientId,3}|{clinicClient.Surname,10}|{clinicClient.Firstname,10}|{clinicClient.Patronymic,10}|{clinicClient.Document,10}");
            }
            return getClientsResponse.Clients.ToArray();
        }
        else
        {
            Console.WriteLine($"Get clients failed.\nErrorCode: {getClientsResponse.ErrCode}\nError Message: {getClientsResponse.ErrMessage}");
            return Array.Empty<ClientResponse>();
        }
    }

    private static void CreateClinicClient(ClinicService.ClinicServiceClient client)
    {
        var response = client.CreateClient(new CreateClientRequest
        {
            Document = "DOC34 445",
            Firstname = "John",
            Patronymic = "John",
            Surname = "Dowe"
        });

        if (response.ErrCode == 0)
        {
            Console.WriteLine($"Client #{response.ClientId} created successfully.");
        }
        else
        {
            Console.WriteLine($"Create client failed.\nErrorCode: {response.ErrCode}\nError Message: {response.ErrMessage}");
        }
    }
}

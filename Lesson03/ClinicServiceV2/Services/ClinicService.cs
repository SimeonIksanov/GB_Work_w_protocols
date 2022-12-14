using AutoMapper;
using ClinicService.Data;
using ClinicServiceNamespace;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using static ClinicServiceNamespace.ClinicService;

namespace ClinicServiceV2.Services;

public class ClinicService : ClinicServiceBase
{
    private readonly ClinicServiceDbContext _context;
    private readonly IMapper _mapper;

    public ClinicService(ClinicServiceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    #region Create

    public override Task<CreateClientResponse> CreateClient(CreateClientRequest request, ServerCallContext context)
    {
        CreateClientResponse response;
        try
        {
            var client = _mapper.Map<Client>(request);
            _context.Clients.Add(client);
            _context.SaveChanges();

            response = new CreateClientResponse
            {
                ClientId = client.Id,
                ErrCode = 0,
                ErrMessage = string.Empty
            };

        }
        catch (Exception ex)
        {
            response = new CreateClientResponse
            {
                ErrCode = 1001,
                ErrMessage = "Internal server error."
            };
        }
        return Task.FromResult(response);
    }

    #endregion

    #region Read
    public override Task<GetClientsResponse> GetClients(GetClientsRequest request, ServerCallContext context)
    {
        GetClientsResponse response;
        try
        {
            var clientResponses = _context
                .Clients
                .AsNoTracking()
                .Select(client => _mapper.Map<ClientResponse>(client))
                .ToList();
            response = new GetClientsResponse();
            response.Clients.AddRange(clientResponses);
        }
        catch (Exception ex)
        {
            response = new GetClientsResponse
            {
                ErrCode = 1002,
                ErrMessage = "Internal server error."
            };
        }

        return Task.FromResult(response);
    }

    public override async Task<GetClientByIdResponse> GetClientById(GetClientByIdRequest request, ServerCallContext context)
    {
        var client = await _context
            .Clients
            .FindAsync(request.ClientId);

        if (client is null)
        {
            return new GetClientByIdResponse { ErrCode = 1003, ErrMessage = "Client not found" };
        }

        var response = new GetClientByIdResponse
        {
            Client = _mapper.Map<ClientResponse>(client)
        };
        return response;
    }

    #endregion

    #region Delete

    public override async Task<DeleteClientResponse> DeleteClient(DeleteClientRequest request, ServerCallContext context)
    {
        var client = await _context.Clients.FindAsync(request.ClientId);
        if (client is null)
        {
            return new DeleteClientResponse { ErrCode = 1003, ErrMessage = "Client not found" };
        }

        _context.Clients.Remove(client);
        _context.SaveChanges();

        var response = new DeleteClientResponse();
        return response;
    }

    #endregion

    #region Update

    public override async Task<UpdateClientResponse> UpdateClient(UpdateClientRequest request, ServerCallContext context)
    {
        var client = await _context
            .Clients
            .FindAsync(request.ClientId);

        if (client is null)
        {
            return new UpdateClientResponse { ErrCode = 1003, ErrMessage = "Client not found" };
        }

        _mapper.Map<UpdateClientRequest, Client>(request, client);

        try
        {
            _context.SaveChanges();
            return new UpdateClientResponse();
        }
        catch (Exception ex)
        {
            return new UpdateClientResponse { ErrCode = 1004, ErrMessage = $"Failed to update client: {ex}" };
        }
    }

    #endregion

}

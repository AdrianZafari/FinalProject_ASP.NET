using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IClientService
{
    Task<ClientResult<IEnumerable<Client>>> GetAllAsync();
    Task<ClientResult<Client>> GetByIdAsync(string id);
    Task<ClientResult> CreateAsync(AddClientFormData client);
    Task<ClientResult> UpdateAsync(string id, AddClientFormData updatedClient);
    Task<ClientResult> DeleteAsync(string id);
}

public class ClientService(IClientRepository clientRepository) : IClientService
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<ClientResult<IEnumerable<Client>>> GetAllAsync()
    {
        var result = await _clientRepository.GetAllAsync();

        if (!result.Succeeded || result.Result == null)
        {
            return new ClientResult<IEnumerable<Client>>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "No clients found."
            };
        }

        return new ClientResult<IEnumerable<Client>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = result.Result
        };
    }

    public async Task<ClientResult<Client>> GetByIdAsync(string id)
    {
        var result = await _clientRepository.GetAsync(c => c.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new ClientResult<Client>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Client not found."
            };
        }

        return new ClientResult<Client>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = result.Result
        };
    }

    public async Task<ClientResult> CreateAsync(AddClientFormData client)
    {
        var existsResult = await _clientRepository.ExistsAsync(c => c.ClientName == client.ClientName);
        if (existsResult.Succeeded && existsResult.Result)
        {
            return new ClientResult
            {
                Succeeded = false,
                StatusCode = 409,
                Error = "A client with this name already exists."
            };
        }

        var entity = client.MapTo<ClientEntity>();
        entity.Id = Guid.NewGuid().ToString();

        var addResult = await _clientRepository.AddAsync(entity);

        return addResult.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 201 }
            : new ClientResult { Succeeded = false, StatusCode = 500, Error = "Failed to create client." };
    }

    public async Task<ClientResult> UpdateAsync(string id, AddClientFormData updatedClient)
    {
        var result = await _clientRepository.GetEntityAsync(c => c.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new ClientResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Client not found."
            };
        }

        var entity = result.Result;
        entity.ClientName = updatedClient.ClientName;

        var updateResult = await _clientRepository.UpdateAsync(entity);

        return updateResult.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = 500, Error = "Failed to update client." };
    }

    public async Task<ClientResult> DeleteAsync(string id)
    {
        var result = await _clientRepository.GetEntityAsync(c => c.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new ClientResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Client not found."
            };
        }

        var deleteResult = await _clientRepository.DeleteAsync(result.Result);

        return deleteResult.Succeeded
            ? new ClientResult { Succeeded = true, StatusCode = 200 }
            : new ClientResult { Succeeded = false, StatusCode = 500, Error = "Failed to delete client." };
    }


}

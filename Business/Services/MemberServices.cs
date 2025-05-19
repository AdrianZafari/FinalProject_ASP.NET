using Business.DTOs;
using Business.Models;
using Data.Entities;
using Data.Repositories;
using Domain.Extensions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IMemberService
{
    Task<MemberResult> CreateAsync(AddMemberFormData member);
    Task<MemberResult> DeleteAsync(string id);
    Task<MemberResult<IEnumerable<Member>>> GetAllAsync(); 
    Task<MemberResult<Member>> GetByIdAsync(string id);    
    Task<MemberResult> UpdateAsync(string id, AddMemberFormData updatedMember);
}

public class MemberService(IMemberRepository memberRepository) : IMemberService
{
    private readonly IMemberRepository _memberRepository = memberRepository;

    public async Task<MemberResult<IEnumerable<Member>>> GetAllAsync()
    {
        var result = await _memberRepository.GetAllAsync();

        if (!result.Succeeded || result.Result == null)
        {
            return new MemberResult<IEnumerable<Member>>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "No members found."
            };
        }

        var members = result.Result.Select(m => m.MapTo<Member>());
        return new MemberResult<IEnumerable<Member>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = members
        };
    }

    public async Task<MemberResult<Member>> GetByIdAsync(string id)
    {
        var result = await _memberRepository.GetAsync(m => m.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new MemberResult<Member>
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Member not found."
            };
        }

        var member = result.Result.MapTo<Member>();
        return new MemberResult<Member>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = member
        };
    }

    public async Task<MemberResult> CreateAsync(AddMemberFormData member)
    {
        var existsResult = await _memberRepository.ExistsAsync(x => x.Email == member.Email);
        if (existsResult.Succeeded && existsResult.Result)
        {
            return new MemberResult
            {
                Succeeded = false,
                StatusCode = 409,
                Error = "A member with this email already exists."
            };
        }

        var entity = member.MapTo<MemberEntity>();
        var addResult = await _memberRepository.AddAsync(entity);

        return addResult.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 201 }
            : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Failed to create member." };
    }

    public async Task<MemberResult> UpdateAsync(string id, AddMemberFormData updatedMember)
    {
        // GetEntityAsync to get the EF entity, not the domain model. Dear God this is agony.
        var result = await _memberRepository.GetEntityAsync(m => m.Id == id);

        if (!result.Succeeded || result.Result == null)
        {
            return new MemberResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Member not found."
            };
        }

        var entity = result.Result;  // This is MemberEntity, tracked by EF Core. This was not a fun error to debug and the sanity I lost debugging it I shall never recover.

        // Update the entity's properties
        entity.FirstName = updatedMember.FirstName;
        entity.LastName = updatedMember.LastName;
        entity.Email = updatedMember.Email;
        entity.PhoneNumber = updatedMember.PhoneNumber;
        entity.JobTitle = updatedMember.JobTitle;
        entity.Address = updatedMember.Address;
        entity.DateOfBirth = updatedMember.DateOfBirth;
        entity.MemberImage = updatedMember.MemberImage;

        // Call UpdateAsync with the entity
        var updateResult = await _memberRepository.UpdateAsync(entity);

        return updateResult.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Failed to update member." };
    }


    public async Task<MemberResult> DeleteAsync(string id)
    {
        var entityResult = await _memberRepository.GetEntityAsync(m => m.Id == id);

        if (!entityResult.Succeeded || entityResult.Result == null)
        {
            return new MemberResult
            {
                Succeeded = false,
                StatusCode = 404,
                Error = "Member not found."
            };
        }

        var deleteResult = await _memberRepository.DeleteAsync(entityResult.Result);

        return deleteResult.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Failed to delete member." };
    }
}


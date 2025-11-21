using AutoMapper;
using UserManagement.Application.Contracts.Request;
using UserManagement.Application.Contracts.Response;
using UserManagement.Domain.Entities;

namespace UserManagement.Application.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<CreateUserRequests, User>();

        CreateMap<User, UserResponse>();
    }
}

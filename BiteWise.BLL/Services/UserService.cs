using AutoMapper;
using BiteWise.BLL.Models;
using BiteWise.BLL.Services.Interfaces;
using BiteWise.DLL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BiteWise.BLL.Services;

public class UserService(UserManager<UserEntity> userManager, 
    RoleManager<IdentityRole> roleManager, 
    IMapper mapper, 
    SignInManager<UserEntity> signInManager,
    IService<Article> articleService
    ) : IService<User>
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;
    private readonly SignInManager<UserEntity> _signInManager = signInManager;
    private readonly IService<Article> _articleService = articleService;

    public async Task<IdentityResult> CreateUserAsync(User user)
    {
        var userEntity = _mapper.Map<UserEntity>(user);

        if (user.Password is not null)
        {
            var result = await _userManager.CreateAsync(userEntity, user.Password);
            await _userManager.AddToRoleAsync(userEntity, "User");

            return result;
        }

        return new IdentityResult();
    }

    public async Task DeleteAsync(User user)
    {
        await _userManager.DeleteAsync(_mapper.Map<UserEntity>(user));
    }

    public async Task<User?> GetAsync(string email)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);

        if (userEntity is not null)
        {
            return new User()
            {
                UserName = userEntity.UserName ?? string.Empty,
                Id = userEntity.Id,
                Email = userEntity.Email ?? string.Empty,
                Roles = await _userManager.GetRolesAsync(userEntity)
            };
        }
        else { return null; }
    }

    public async Task<User> GetByIdAsync(string id)
    {
        var userEntity = await _userManager.FindByIdAsync(id);

        if (userEntity is not null)
        {
            var user = _mapper.Map<User>(userEntity);
            user.Roles = await _userManager.GetRolesAsync(userEntity);

            return user;
        }
        else { throw new NullReferenceException(); }
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        var users = new List<User>();

        foreach (var userEntity in _userManager.Users)
        {
            var user = _mapper.Map<User>(userEntity);
            user.Roles = await _userManager.GetRolesAsync(userEntity);
            users.Add(user);
        }

        return users;
    }

    public async Task UpdateAsync(User user)
    {
        var userEntity = await _userManager.FindByIdAsync(user.Id?? throw new ArgumentNullException());
        _mapper.Map(user, userEntity);

        await _userManager.UpdateAsync(userEntity?? throw new NullReferenceException());
    }

    public async Task<User> GetByUserAsync(ClaimsPrincipal user)
    {
        var userEntity = await _userManager.GetUserAsync(user);
        var userModel = _mapper.Map<User>(userEntity);
        userModel.Articles = [.. _articleService.GetAllAsync().Result.Where(a => a.UserEntityId == userModel.Id)];
        userModel.Roles = await _userManager.GetRolesAsync(userEntity?? throw new ArgumentNullException());

        return userModel;
    }

    public async Task<bool> CheckPasswordAsync(string email, string password)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);
        if (userEntity == null) return false;

        return await _userManager.CheckPasswordAsync(userEntity, password);
    }

    public async Task SignInAsync(string email, bool isPersistent)
    {
        var userEntity = await _userManager.FindByEmailAsync(email);
        if (userEntity != null)
            await _signInManager.SignInAsync(userEntity, isPersistent);
    }

    public bool IsSignIn(ClaimsPrincipal user)
    {
        return _signInManager.IsSignedIn(user);
    }

    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
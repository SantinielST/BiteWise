using BiteWise.BLL.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BiteWise.BLL.Services.Interfaces;

public interface IService<T>
{
    async Task CreateAsync(T model) { }
    async Task CreateAsyncTagArticleConnections(List<string> tagIds, Article article) { }
    Task UpdateAsync(T model);
    Task DeleteAsync(T model);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetAsync(string item);

    public async Task CreateAsyncTagArticleConnection(string id, Article article) { }
    async Task<bool> UpdateRolesAsync(T model, string roleName) => throw new ArgumentNullException();
    async Task<T> GetByIdAsync(string item) => throw new ArgumentNullException();
    async Task<T> GetByUserAsync(ClaimsPrincipal user) => throw new ArgumentNullException();
    async Task<IdentityResult> CreateUserAsync(T model) => throw new ArgumentNullException();
    async Task<bool> CheckPasswordAsync(string item, string item2) { return false; }
    async Task SignInAsync(string item, bool item2) { }
    bool IsSignIn(ClaimsPrincipal user) { return false; }
    async Task SignOutAsync() { }
}
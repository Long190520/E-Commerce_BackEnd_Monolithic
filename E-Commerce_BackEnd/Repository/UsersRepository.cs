using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using E_Commerce_BackEnd.Models;
using E_Commerce_BackEnd.ViewModels;
using E_Commerce_BackEnd.Utils;
using Microsoft.EntityFrameworkCore;
using E_Commerce_BackEnd.Services;
using System.Net;
using Newtonsoft.Json.Linq;
using E_Commerce_BackEnd.Const;
using Newtonsoft.Json;
using E_Commerce_BackEnd.DAL;

namespace E_Commerce_BackEnd.Repository
{
    public class UsersRepository : Repository<Users>, IUsersRepository
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersRepository(MyDbContext context, IConfiguration configuration) : base(context)
        {
            _context = context;
            _configuration = configuration;
        }
    }

    public interface IUsersRepository : IRepository<Users>
    {
    }
}

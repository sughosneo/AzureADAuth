using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Services
{
    public interface IProductServices
    {
        Task<IEnumerable<Products>> GetAllAsync();
        Task<IEnumerable<Products>> GetSpecificAsync(int id);
    }
}

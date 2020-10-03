using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Repositories
{
    public interface IProductRespository
    {
        Task<IEnumerable<Products>> GetAll();
        Task<IEnumerable<Products>> GetSpecific(int id);
    }
}

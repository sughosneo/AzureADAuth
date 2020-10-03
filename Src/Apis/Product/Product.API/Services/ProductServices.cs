using Product.API.Models;
using Product.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRespository _productRepository;

        public ProductServices(IProductRespository productRespository)
        {
            _productRepository = productRespository;
        }

        public async Task<IEnumerable<Products>> GetAllAsync()
        {
            return await _productRepository.GetAll();
        }

        public async Task<IEnumerable<Products>> GetSpecificAsync(int id)
        {
            return await _productRepository.GetSpecific(id);
        }
    }
}

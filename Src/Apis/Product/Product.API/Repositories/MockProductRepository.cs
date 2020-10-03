using Product.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Product.API.Repositories
{
    public class MockProductRepository : IProductRespository
    {
        private readonly List<Products> _listOfProducts;

        public MockProductRepository()
        {
            _listOfProducts = new List<Products>();
            _listOfProducts.Add(new Products()
            {
                Id = 1,
                Name = "Mug",
                Price = 1.2
            });
            _listOfProducts.Add(new Products()
            {
                Id = 2,
                Name = "Cup",
                Price = 2.1
            });
        }

        public async Task<IEnumerable<Products>> GetAll()
        {
            return _listOfProducts;
        }

        public async Task<IEnumerable<Products>> GetSpecific(int id)
        {
            return _listOfProducts.FindAll((options) => options.Id == id);
        }
    }
}

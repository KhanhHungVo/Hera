using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class OrderService : IOrderService
    {
        public Task<OrderViewModel> Create(OrderViewModel model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OrderViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<OrderViewModel> Update(OrderViewModel mdoel)
        {
            throw new NotImplementedException();
        }
    }
}

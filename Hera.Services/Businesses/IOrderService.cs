using Hera.Common.Core;
using Hera.Data.Entities;
using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public interface IOrderService : IServiceBaseTypeId<OrderEntity, int>
    {
        //Task<OrderViewModel> Create(OrderViewModel model);
        //Task<OrderViewModel> Update(OrderViewModel mdoel);
        //Task<IEnumerable<OrderViewModel>> GetAll();
        //Task<OrderViewModel> GetById(int id);
        //Task<OrderViewModel> DeleteAsync(int id);
        //Task Delete(int id);
    }
}

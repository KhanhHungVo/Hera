using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        public async Task<OrderViewModel> Create(OrderViewModel model)
        {
            var entity = MapToFromViewModel(model);
            return MapToViewModel(await orderRepository.CreateAsync(entity));
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            List<OrderEntity> oders =  await orderRepository.GetAll();
            return oders.ConvertAll(x => MapToViewModel(x));
        }

        public async Task<OrderViewModel> GetById(int id)
        {
            OrderEntity entity = await orderRepository.GetById(id);
            return MapToViewModel(entity);
        }

        public async Task<OrderViewModel> Update(OrderViewModel model)
        {
            var entity = MapToFromViewModel(model);
            return  MapToViewModel(await orderRepository.Update(entity));
        }
        public OrderEntity MapToFromViewModel(OrderViewModel model)
        {
            return new OrderEntity()
            {
                Id = model.Id,
                OrderDate = model.OrderDate,
                Symbol = model.Symbol
            };
        }
        public OrderViewModel MapToViewModel(OrderEntity entity)
        {
            return new OrderViewModel()
            {
                Id = entity.Id,
                OrderDate = entity.OrderDate,
                Symbol = entity.Symbol
            };
        }
    }
}

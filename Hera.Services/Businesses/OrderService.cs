using AutoMapper;
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Common.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class OrderService : ServiceBaseTypeId<OrderEntity, int>, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper) : base(orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<OrderViewModel> CreateAsync(OrderViewModel model)
        {
            var entity = _mapper.Map<OrderEntity>(model);
            return _mapper.Map<OrderViewModel>(await _orderRepository.CreateAsync(entity));
        }

        public async Task<OrderViewModel> DeleteAsync(int id)
        {
            return _mapper.Map<OrderViewModel>(await _orderRepository.DeleteAsync(id)); 
        }

        public async Task<IEnumerable<OrderViewModel>> GetAsync()
        {
            List<OrderEntity> oders =  await _orderRepository.GetAsync();
            return oders.ConvertAll(x => _mapper.Map<OrderViewModel>(x));
        }

        public async Task<OrderViewModel> GetAsync(int id)
        {
            OrderEntity entity = await _orderRepository.GetAsync(id);
            return _mapper.Map<OrderViewModel>(entity);
        }

        public async Task<OrderViewModel> UpdateAsync(OrderViewModel model)
        {
            var entity = _mapper.Map<OrderEntity>(model);
            return _mapper.Map<OrderViewModel>(await _orderRepository.UpdateAsync(entity));
        }
        //public OrderEntity MapToFromViewModel(OrderViewModel model)
        //{
        //    return new OrderEntity()
        //    {
        //        Id = model.Id,
        //        OrderDate = model.OrderDate,
        //        Symbol = model.Symbol
        //    };
        //}
        //public OrderViewModel MapToViewModel(OrderEntity entity)
        //{
        //    return new OrderViewModel()
        //    {
        //        Id = entity.Id,
        //        OrderDate = entity.OrderDate,
        //        Symbol = entity.Symbol
        //    };
        //}
    }
}

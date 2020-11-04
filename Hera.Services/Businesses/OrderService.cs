using AutoMapper;
using Hera.Data.Entities;
using Hera.Data.Infrastructure;
using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    public class OrderService : ServiceBaseTypeId<OrderEntity, int>,IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        public OrderService(IOrderRepository orderRepository, IMapper mapper) : base(orderRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<OrderViewModel> Create(OrderViewModel model)
        {
            var entity = _mapper.Map<OrderEntity>(model);
            return _mapper.Map<OrderViewModel>(await _orderRepository.CreateAsync(entity));
        }

        public async Task<OrderViewModel> Delete(int id)
        {
            return _mapper.Map<OrderViewModel>(await _orderRepository.DeleteAsync(id)); 
        }

        public async Task<IEnumerable<OrderViewModel>> GetAll()
        {
            List<OrderEntity> oders =  await _orderRepository.GetAll();
            return oders.ConvertAll(x => _mapper.Map<OrderViewModel>(x));
        }

        public async Task<OrderViewModel> GetById(int id)
        {
            OrderEntity entity = await _orderRepository.GetById(id);
            return _mapper.Map<OrderViewModel>(entity);
        }

        public async Task<OrderViewModel> Update(OrderViewModel model)
        {
            var entity = _mapper.Map<OrderEntity>(model);
            return _mapper.Map<OrderViewModel>(await _orderRepository.Update(entity));
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

﻿using Hera.Services.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hera.Services.Businesses
{
    interface IOrderService
    {
        Task<OrderViewModel> Create(OrderViewModel model);
        Task<OrderViewModel> Update(OrderViewModel mdoel);
        Task<IEnumerable<OrderViewModel>> GetAll();
        Task Delete(int id);
    }
}
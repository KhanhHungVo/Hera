using System;
using System.Collections.Generic;
using System.Text;

namespace Hera.Services.ViewModels.Order
{
    public class OrderViewModel
    {
        DateTime OrderDate { get; set; }
        string Name { get; set; }
        string Symbol { get; set; }
        int Volume { get; set; }
        string Reason { get; set; }
        double Invesment { get; set; }
        double Cost { get; set; }
        double CurrentPrice { get; set; }
        double MarketPrice { get; set; }
        double GainLossPercentage { get; set; }
        double GainLoss { get; set; }
    }
}

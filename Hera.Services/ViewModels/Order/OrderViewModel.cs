using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hera.Services.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        public int Volume { get; set; }
        string Reason { get; set; }
        [Required]
        public double InvesmentValue { get; set; }
        [Required]
        public double Cost { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketValue { get; set; }
        public double GainLossPercentage { get; set; }
        public double GainLoss { get; set; }
        public Boolean Status { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Hera.Common.ViewModels.Order
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Symbol { get; set; }
        [Required]
        public int Volume { get; set; }
        string Reason { get; set; }
        [Required]
        public double InvestmentValue { get; set; }
        [Required]
        public double OrderPrice { get; set; }
        public double? CurrentPrice { get; set; }
        public double? MarketValue { get; set; }
        public double? GainLossPercentage { get; set; }
        public double? GainLoss { get; set; }
        public Boolean Done { get; set; }
    }
}

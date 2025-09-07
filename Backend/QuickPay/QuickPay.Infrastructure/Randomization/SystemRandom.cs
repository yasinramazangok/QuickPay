using QuickPay.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickPay.Infrastructure.Randomization
{
    public class SystemRandom : IRandom
    {
        private readonly Random _random = new Random();
        public double NextDouble() => _random.NextDouble();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Rolldic.Models
{
    public class DiceRoll
    {
        public int Dice1 { get; set; }
        public int Dice2 { get; set; }
        public int Sum => Dice1 + Dice2;
        public string Result
        {
            get
            {
                if (Sum < 7) return "The sum is less than 7.";
                if (Sum == 7) return "The sum is equal to 7.";
                return "The sum is greater than 7.";
            }
        }

        public string[] BetOptions { get; set; } // Array of user's bet options
        public Dictionary<string, decimal> BetAmounts { get; set; } = new Dictionary<string, decimal>(); // Dictionary to store amounts for each bet option
        public Dictionary<string, bool> BetResults { get; set; } // Dictionary to store results of each bet
        public Dictionary<string, decimal> Payouts { get; set; } // Dictionary to store payouts for each bet
        public decimal TotalBetAmount => BetAmounts.Values.Sum(); // Total bet amount
        public decimal TotalPayout => Payouts?.Values.Sum() ?? 0; // Total payout amount
    }
}

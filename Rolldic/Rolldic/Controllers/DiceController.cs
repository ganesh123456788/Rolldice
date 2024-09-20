using Rolldic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Rolldic.Controllers
{
    public class DiceController : Controller
    {
        private readonly Random _random = new Random();

        // GET: Dice
        public ActionResult Index()
        {
            var model = new DiceRoll();
            return View(model);
        }

        // POST: Dice
        [HttpPost]
        public ActionResult Roll(DiceRoll model, FormCollection form)
        {
            var diceRoll = RollDice();
            model.Dice1 = diceRoll.Dice1;
            model.Dice2 = diceRoll.Dice2;

            // Extract bet amounts from the form
            var betOptions = form.GetValues("BetOptions");
            model.BetOptions = betOptions;

            var betAmounts = new Dictionary<string, decimal>();
            foreach (var option in betOptions)
            {
                var amountStr = form[$"BetAmount_{option}"];
                if (decimal.TryParse(amountStr, out var amount))
                {
                    betAmounts[option] = amount;
                }
            }
            model.BetAmounts = betAmounts;

            model.BetResults = ValidateBets(model.BetOptions, model.Sum);
            model.Payouts = CalculatePayouts(model.BetAmounts, model.Sum);

            return View("Index", model);
        }

        private DiceRoll RollDice()
        {
            return new DiceRoll
            {
                Dice1 = _random.Next(1, 7), // Random number between 1 and 6
                Dice2 = _random.Next(1, 7)  // Random number between 1 and 6
            };
        }

        private Dictionary<string, bool> ValidateBets(string[] betOptions, int sum)
        {
            var results = new Dictionary<string, bool>();

            if (betOptions != null)
            {
                foreach (var option in betOptions)
                {
                    bool isCorrect = (option == "<7" && sum < 7) ||
                                     (option == "=7" && sum == 7) ||
                                     (option == ">7" && sum > 7);

                    results[option] = isCorrect;
                }
            }

            return results;
        }

        private Dictionary<string, decimal> CalculatePayouts(Dictionary<string, decimal> betAmounts, int sum)
        {
            var payouts = new Dictionary<string, decimal>();

            if (betAmounts != null)
            {
                foreach (var bet in betAmounts)
                {
                    string option = bet.Key;
                    decimal amount = bet.Value;
                    decimal multiplier = 0;

                    if (sum < 7)
                    {
                        if (option == "<7") multiplier = 2; // Double the betting amount
                        else if (option == ">7") multiplier = 0; // No payout if the bet is wrong
                    }
                    else if (sum == 7)
                    {
                        if (option == "=7") multiplier = 3; // Triple the betting amount
                        else multiplier = 0; // No payout if the bet is wrong
                    }
                    else
                    {
                        if (option == ">7") multiplier = 3; // Triple the betting amount
                        else if (option == "<7") multiplier = 0; // No payout if the bet is wrong
                    }

                    payouts[option] = multiplier * amount;
                }
            }

            return payouts;
        }
    }
}

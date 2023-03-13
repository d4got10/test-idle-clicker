using UnityEngine;

namespace Core.Services
{
    public class PlayerMoneyService
    {
        public int Amount { get; private set; }

        public bool HasEnough(int amount) => Amount >= amount;

        public void Add(int amount)
        {
            Amount += amount;
        }

        public void Spend(int amount)
        {
            if (!HasEnough(amount))
            {
                Debug.LogError("Not enough money for spending!");
                return;
            }

            Amount -= amount;
        }

        public void Load(int amount) => Amount = amount;
    }
}
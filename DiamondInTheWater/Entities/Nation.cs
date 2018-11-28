using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondInTheWater.Entities
{
    public class Nation
    {
        public readonly string Name;

        public float FactoryAdvantage, ToolAdvantage, TruckAdvantage,
            PhoneAdvantage, ChocolateAdvantage, ShirtAdvantage;
        public float QueuedFactories
        {
            get;
            set;
        }
        public float QueuedTrucks
        {
            get;
            set;
        }
        public float QueuedTools
        {
            get;
            set;
        }

        public float QueuedShirts
        {
            get;
            set;
        }

        public float QueuedPhones
        {
            get;
            set;
        }

        public float QueuedChocolates
        {
            get;
            set;
        }
        public float Factories
        {
            get;
            set;
        }
        public float Trucks
        {
            get;
            set;
        }
        public float Tools
        {
            get;
            set;
        }

        public float Shirts
        {
            get;
            set;
        }

        public float Chocolates
        {
            get;
            set;
        }

        public float Phones
        {
            get;
            set;
        }
        public int Coco
        {
            get { return (int)(ChocolateAdvantage * 33); }
        }

        public int Silk
        {
            get { return (int)(ShirtAdvantage * 31); }
        }

        public int Materials
        {
            get { return (int)(PhoneAdvantage * 34); }
        }

        public float Population
        {
            get { return (int)populationPrecise; }
        }

        public float Production
        {
            get { return Population + (Tools * TOOL_NEEDED + Factories * FACTORY_NEEDED + Trucks * TRUCK_NEEDED) / 200; }
        }

        public float QueuedGoods
        {
            get { return QueuedChocolates + QueuedFactories + QueuedPhones + QueuedShirts + QueuedTools + QueuedTrucks; }
        }
        public const float FACTORY_PRODUCTION = 200f;
        public const int MAX_POPULATION = 100;

        public const float FACTORY_NEEDED = 200;
        public const float TRUCK_NEEDED = 100;
        public const float TOOL_NEEDED = 50;
        public const float SHIRT_NEEDED = 50;
        public const float CHOC_NEEDED = 30;
        public const float PHONE_NEEDED = 100;
        private double populationPrecise;

        /// <summary>
        /// Creates a new <c>Nation</c>.
        /// </summary>
        public Nation(string name)
        {
            Name = name;
            populationPrecise = 16.0;
        }

        public bool IsGoodTrade(float theirChocolate, float theirPhone, float theirShirt,
            float yourChocolate, float yourPhone, float yourShirt)
        {
            float tradeValue = (theirChocolate / ChocolateAdvantage + theirPhone
                / PhoneAdvantage + theirShirt / ShirtAdvantage) - (yourChocolate
                / ChocolateAdvantage + yourPhone / PhoneAdvantage + yourShirt / ShirtAdvantage);
            
            return tradeValue > 0;
        }

        public void DecideProduction()
        {
            QueuedChocolates += ChocolateAdvantage * Production;
            QueuedPhones += PhoneAdvantage * Production;
            QueuedShirts += ShirtAdvantage * Production;
            QueuedFactories += FactoryAdvantage * Production;
            QueuedTools += ToolAdvantage * Production;
            QueuedTrucks += TruckAdvantage * Production;
        }

        public void Progress(int Day)
        {
            float x = Day / 25f;
            populationPrecise += Math.Max(0.2, (MAX_POPULATION * .3 * Math.Exp(3 * x / 10 + 4) / Math.Pow((Math.Exp(3 * x / 10) + Math.Exp(4)), 2)));

            float queuedGoods = (float)(Math.Ceiling(QueuedFactories) / FactoryAdvantage + Math.Ceiling(QueuedPhones)
                + Math.Ceiling(QueuedTrucks) / TruckAdvantage + Math.Ceiling(QueuedChocolates)
                + Math.Ceiling(QueuedShirts) + Math.Ceiling(QueuedTools) / ToolAdvantage);

            float dividedProd = Production / queuedGoods;

            if (QueuedFactories > 0)
            {
                float delta = (float)(dividedProd / FACTORY_NEEDED * Math.Ceiling(QueuedFactories)) * FactoryAdvantage;
                QueuedFactories -= delta;
                Factories += delta;
            }
            if (QueuedFactories < 0)
                QueuedFactories = 0;

            if (QueuedTrucks > 0)
            {
                float delta = (float)(dividedProd / TRUCK_NEEDED * Math.Ceiling(QueuedTrucks)) * TruckAdvantage;
                QueuedTrucks -= delta;
                Trucks += delta;
            }
            if (QueuedTrucks < 0)
                QueuedTrucks = 0;
            if (QueuedTools > 0)
            {
                float delta = (float)(dividedProd / TOOL_NEEDED * Math.Ceiling(QueuedTools)) * ToolAdvantage;

                QueuedTools -= delta;
                Tools += delta;
            }
            if (QueuedTools < 0)
                QueuedTools = 0;

            if (QueuedChocolates > 0)
            {
                float delta = (float)(dividedProd / CHOC_NEEDED * Math.Ceiling(QueuedChocolates)) * ChocolateAdvantage;
                QueuedChocolates -= delta;
                Chocolates += delta;
            }
            if (QueuedChocolates < 0)
                QueuedChocolates = 0;

            if (QueuedPhones > 0)
            {
                float delta = (float)(dividedProd / PHONE_NEEDED * Math.Ceiling(QueuedPhones)) * PhoneAdvantage;
                QueuedPhones -= delta;
                Phones += delta;
            }
            if (QueuedPhones < 0)
                QueuedPhones = 0;

            if (QueuedShirts > 0)
            {
                float delta = (float)(dividedProd / SHIRT_NEEDED * Math.Ceiling(QueuedShirts)) * ShirtAdvantage;

                QueuedShirts -= delta;
                Shirts += delta;
            }
            if (QueuedShirts < 0)
                QueuedShirts = 0;

        }
    }
}

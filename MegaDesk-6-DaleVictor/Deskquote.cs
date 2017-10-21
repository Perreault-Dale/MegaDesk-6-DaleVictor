using Newtonsoft.Json;
using System;
using System.IO;
using Windows.Storage;
using Windows.UI.Popups;

namespace MegaDesk_6_DaleVictor 
{
    class Deskquote
    {
        // class members
        public string custName { get; private set; }
        public Desk custDesk { get; private set; }
        public int prodDays { get; private set; }
        public DateTime orderDate { get; private set; }
        public int price { get; private set; }

        const int BASEPRICE = 200;
        const int BIGAREA = 2000;
        const int SMALLAREA = 1000;

        // constructor methods
        public Deskquote(string name, int days, int wide, int deep, int draw, DesktopMaterial surface)
        {
            custName = name;
            custDesk = new Desk(wide, deep, draw, surface);
            prodDays = days;
            orderDate = DateTime.Now;
            price = BASEPRICE + calcPrice(custDesk, prodDays);
        }
        [JsonConstructor]
        public Deskquote(string custName, Desk custDesk, int prodDays, DateTime orderDate, int price)
        {
            this.custName = custName;
            this.custDesk = custDesk;
            this.prodDays = prodDays;
            this.orderDate = orderDate;
            this.price = price;
        }

        public Deskquote(string name, int days, int wide, int deep, int draw, DateTime date, DesktopMaterial surface)
        {
            custName = name;
            custDesk = new Desk(wide, deep, draw, surface);
            prodDays = days;
            orderDate = date;
            price = BASEPRICE + calcPrice(custDesk, prodDays);
        }

        // add surface charge
        public int addSurface(Desk desk)
        {
            int cost = (int)desk.surface;
            return cost;
        }

        // add size surcharge
        public int addSurcharge(Desk desk)
        {
            int cost = desk.width * desk.depth - SMALLAREA;
            if (cost > 0) { return cost; } else { return 0; }
        }

        // add cost for drawers
        public int addDrawers(Desk desk)
        {
            int cost = desk.drawers * 50;
            return cost;
        }

        public int[,] getRushOrder()
        {
            StreamReader sr = new StreamReader(File.OpenRead("rushOrderPrices.txt"));
            int[,] rushCost = new int[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rushCost[i, j] = int.Parse(sr.ReadLine());
                }
            }
            return rushCost;
        }

        // add upcharge for rush delivery
        public int addRush(Desk desk, int rush)
        {
            int i = 0;
            int j = 0;
            int[,] rushCost = new int[3, 3];
            rushCost = this.getRushOrder();

            // determine upcharge from two-dimsensional array
            if (desk.width * desk.depth < SMALLAREA) { j = 0; }
            else if (desk.width * desk.depth > BIGAREA) { j = 2; }
            else { j = 1; }

            if (rush == 3) { i = 0; }
            else if (rush == 5) { i = 1; }
            else { i = 2; }

            return rushCost[i,j];
        }

        public int calcPrice(Desk desk, int rush)
        {
            int cost = addDrawers(desk) + addRush(desk, rush) + addSurcharge(desk) + addSurface(desk);
            return cost;
        }

        public async void writeQuote()
        {
            StorageFolder path1 = ApplicationData.Current.LocalFolder;
            StorageFile sw = await path1.CreateFileAsync("quotes.json", CreationCollisionOption.OpenIfExists);
            string json = JsonConvert.SerializeObject(this) + Environment.NewLine;
            await FileIO.AppendTextAsync(sw, json);
        }

        public override string ToString()
        {
            var message = custName + ", " + custDesk.width + "x" + custDesk.depth + "x" + custDesk.drawers;
            message = message + ", " + custDesk.surface + ", " + orderDate + ", " + prodDays + ", $" + price;
            return message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace TwsClient.Models
{
    public class RTVolume
    {
        public DateTime Timestamp { get; set; }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private double _prevPrice;
        public double PrevPrice
        {
            get { return _prevPrice; }
            set
            {
                _prevPrice = value;
                CheckDirection();
            }
        }

        private string _direction;
        public string Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }
        public int Size { get; set; }
        public long UnixTime { get; set; }
        public long PrevUnixTime { get; set; }
        public int TotalVolume { get; set; }
        public double Vwap { get; set; }
        public bool IsSingleMarketMaker { get; set; }

        public RTVolume(string tickString)
        {
            if (!String.IsNullOrWhiteSpace(tickString))
            {
                var values = tickString.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length == 6)
                {
                    this.Price = double.Parse(values[0]);
                    this.Size = int.Parse(values[1]);
                    this.UnixTime = long.Parse(values[2]);
                    this.TotalVolume = int.Parse(values[3]);
                    this.Vwap = double.Parse(values[4]);
                    this.IsSingleMarketMaker = bool.Parse(values[5]);
                    this.Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(this.UnixTime).LocalDateTime;
                }
            }
        }

        private void CheckDirection()
        {
            var diff = _price - _prevPrice;
            if (diff != 0)
            {
                _direction = (diff < 0 ? "d" : "u");
            }
        }

    }
}

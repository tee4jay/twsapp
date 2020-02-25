﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TwsClient.Models
{
    public class RTVolume
    {
        public DateTime Timestamp { get; set; }
        public double Price { get; set; }
        public double PrevPrice { get; set; }
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

    }
}

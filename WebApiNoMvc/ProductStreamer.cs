using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using WebApiNoMvc.Classes;
using WebApiNoMvc.Models;

namespace WebApiNoMvc
{
    public static class ProductStreamer
    {
        private static readonly Thread _thread;
        
        private static readonly TimeSpan _updateInterval = TimeSpan.FromMilliseconds(250);

        static ProductStreamer()
        {
            _thread = new Thread(UpdateThreadMethod);
        }

        public static void Start()
        {
            _thread.Start();
        }

        private static void UpdateThreadMethod()
        {
            while (true)
            {
                lock (typeof(DataContainer))
                {
                    foreach (var kvp in DataContainer.Products)
                    {
                        BroadcastProductInfo(kvp.Value);
                        Thread.Sleep(1000);
                    }
                }
            }

        }

        private static void BroadcastProductInfo(Product product)
        {
            GlobalHost.ConnectionManager.GetHubContext<ProductHub>()
                .Clients.All.displayProductInfo(product);
        }

        /*
        private static void BroadcastProductInfo(object state)
        {
            Product product;
            DataContainer.Products.TryGetValue(1, out product);
            GlobalHost.ConnectionManager.GetHubContext<ProductHub>()
                .Clients.All.displayProductInfo(product);
        }
        */
    }
}
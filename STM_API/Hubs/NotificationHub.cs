using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using STM_API.Model;
using STM_API.Model.BreezeAPIModel;
using STM_API.Services;
using System;
using System.Data.Common;
using System.Text.Json;

namespace STM_API.Hubs
{
    public class BreezeOperationHUB : Hub
    {
        public BreezeOperationHUB(BreezapiServices breezapiServices)
        {
            _breezapiServices = breezapiServices;
        }
        private readonly BreezapiServices _breezapiServices;
        public string GetConnectionId() => Context.ConnectionId;

        public async Task GetPortfolioPosition ()
        {
           PortfolioPositions results = _breezapiServices.GetPositions();
            await Clients.Caller.SendAsync("SendPortfolioPosition", results);
        }

        
    }
}

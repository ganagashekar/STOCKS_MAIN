using Microsoft.AspNetCore.SignalR;
using STM_API.Services;

namespace STM_API.Hubs
{


    public class BreezeOrders : Hub
    {
        public BreezeOrders(BreezeOrdersService stockTicker)
        {
            _BreezeOrdersService = stockTicker;
        }
        private readonly BreezeOrdersService _BreezeOrdersService;
        public string GetConnectionId() => Context.ConnectionId;

        private readonly StockTicker _stockTicker;


        public async Task GetPivotData(string Date, string Column, string GroupName, string SubGroup, string CKTNAME = "", string ConditionOperator = "", string dynamicminValue = "", string dynamicmaxValue = "", int IsWatchList = 0)
        {
            if (string.IsNullOrEmpty(Date))
            {
                Date = DateTime.Now.ToString();
            }
            if (string.IsNullOrEmpty(Column))
            {
                Column = "last";
            }
            var results = _stockTicker.GetPivotData(Date, Column, GroupName, SubGroup, CKTNAME, ConditionOperator, dynamicminValue, dynamicmaxValue, IsWatchList == 1);
            await Clients.Caller.SendAsync("SendPivotData", results);
            // return _stockTicker.GetAllStocks();
        }
    }
}

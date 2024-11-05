﻿
using Breeze;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiSignalRChatDemo.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Skender.Stock.Indicators;
using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Timers;

namespace MauiSignalRChatDemo.ViewModels
{

    public partial class BuyStockAlertModel : ObservableObject
    {
        [ObservableProperty]
        public string _symbol;
        [ObservableProperty]
        public string _stockName;
        [ObservableProperty]
        public decimal _buyATPrice;
        [ObservableProperty]
        public decimal? _buyATChange;
        [ObservableProperty]
        public decimal _sellATPrice;
        [ObservableProperty]
        public decimal _currentPrice;

        [ObservableProperty]
        public decimal _currentChange;

        [ObservableProperty]
        public string _stockCode;

        [ObservableProperty]
        public int _qty;

        [ObservableProperty]
        public bool _IsBuy;

        [ObservableProperty]
        public bool _IsSell;

        [ObservableProperty]
        public int _bgcolor;

        [ObservableProperty]
        public string _match;

        [ObservableProperty]
        public decimal _volumeDifferecne;


        [ObservableProperty]
        public int _bullishCount;

        [ObservableProperty]
        public int _bullishCount_100;

        [ObservableProperty]
        public int _bullishCount_95;

        [ObservableProperty]
        public int _bearishCount;

        [ObservableProperty]
        public int _bearishCount_100;

        [ObservableProperty]
        public int _bearishCount_95;

        [ObservableProperty]
        public decimal? _triggredPrice;
        [ObservableProperty]
        public DateTime _triggredLtt;

        [ObservableProperty]
        public DateTime _lttDateTime;

        [ObservableProperty]
        public string _data;
    }



    public partial class EquitiesViewModel : ObservableObject
    {
        static System.Timers.Timer timer;
        private readonly HubConnection _hubConnection;

        [ObservableProperty]
        string _name;

        [ObservableProperty]
        string _message;

        [ObservableProperty]
        ObservableCollection<BuyStockAlertModel> _messages;

        //List<LiveStockData> listofTicks = new List<LiveStockData>();

        Dictionary<string, List<LiveStockData>> dictionary = new Dictionary<string, List<LiveStockData>>();

        [ObservableProperty]
        bool _isConnected;

        [ObservableProperty]
        int _totalcount;

        [ObservableProperty]
        string _latestdateTime;

        [ObservableProperty]
        string _tempurlhub;






        public async Task InitiICICAsync()
        {
            var text = System.IO.File.ReadAllText("C:\\Hosts\\ICICI_Key\\jobskeys.txt");

            string[] lines = text.Split(
    new string[] { Environment.NewLine },
    StringSplitOptions.None
            );


            Random r = new Random();

            // Console.WriteLine(connection.ConnectionId);
            // string HUbUrl = "http://localhost/StockSignalRServer/livefeedhub";
            try
            {
                string APIKEY = string.Empty;
                string APISecret = string.Empty;
                string token = string.Empty;
                //Initialize SDK
                string[] line;
                string arg = "4";



                switch (Convert.ToInt16(arg))
                {
                    case 0:
                        line = lines[0].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 1:
                        line = lines[1].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 2:
                        line = lines[2].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 3:
                        line = lines[3].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                    case 4:
                        line = lines[4].ToString().Split(',');
                        APIKEY = line[0];
                        APISecret = line[1];
                        token = line[2];
                        break;
                }
                // Generate Session
                Console.WriteLine(arg);
                BreezeConnect breeze = new BreezeConnect(APIKEY);
                //Console.WriteLine(args[1].ToString());
                breeze.generateSessionAsPerVersion(APISecret, token);

                // Connect to WebSocket
                var responseObject = breeze.wsConnectAsync();
                Console.WriteLine(JsonSerializer.Serialize(responseObject));

                breeze.subscribeFeedsAsync("4.1!SUZLON");

                breeze.ticker(async (data) =>
                {


                });
            }
            catch
            {

            }
        }


        public DateTime GetParseLTT(string ltt)
        {
            try
            {
                string[] test = ltt.Split(' ');
                string dateformat = string.Format("{0}-{1}-{2} {3}", test.Last(), test[1].ToString(), test[2].ToString(), test[3].ToString());
                var result = DateTime.TryParse(dateformat, out var dt);
                return dt;
            }
            catch (Exception ex)
            {

                return Convert.ToDateTime(ltt);
            }
        }
        public EquitiesViewModel()
        {
            //_hubConnection = new HubConnectionBuilder()
            //    .WithUrl($"http://localhost:90/BreezeOperation").WithAutomaticReconnect()//WithKeepAliveInterval(TimeSpan.FromSeconds(30))
            //    //.WithUrl("https://localhost:7189/BreezeOperation")
            //    .Build();
            //_hubConnection = new HubConnectionBuilder().WithUrl("http://localhost:45/BreezeOperation").WithAutomaticReconnect().Build();


            string HUbUrl = string.Empty;
            int arg =  Convert.ToInt16(Preferences.Get("arg0","").ToString());
            //if (args.Any())
            //    arg = args[0];



            switch (Convert.ToInt16(arg))
            {
                case 0:

                    HUbUrl = "http://localhost:90/BreezeOperation";
                    break;
                case 1:

                    HUbUrl = "http://localhost:91/BreezeOperation";
                    break;
                case 2:

                    HUbUrl = "http://localhost:92/BreezeOperation";
                    break;
                case 3:

                    HUbUrl = "http://localhost:99/BreezeOperation";
                    break;
                case 4:

                    HUbUrl = "http://localhost:49/BreezeOperation";
                    break;
            }

            this.Tempurlhub = HUbUrl;
           // HUbUrl = "https://localhost:7189/breezeOperation";
            _hubConnection = new HubConnectionBuilder().WithUrl(HUbUrl).WithAutomaticReconnect().Build();
           // _hubConnection = new HubConnectionBuilder().WithUrl("https://localhost:7189/BreezeOperation").WithAutomaticReconnect().Build();
            _hubConnection.KeepAliveInterval = TimeSpan.FromSeconds(30);
            Connect(Convert.ToInt16(arg));

            var bullsis = new List<string>() { Match.BullBasis.ToString(), Match.BullConfirmed.ToString(), Match.BullSignal.ToString() };
            var barish = new List<string>() { Match.BearBasis.ToString(), Match.BearConfirmed.ToString(), Match.BearSignal.ToString() };


            _hubConnection.On<BuyStockAlertModel[]>("SendGetBuyStockTriggers", async result =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in result)
                    {
                        _messages.Add(new BuyStockAlertModel()
                        {
                            _bgcolor = 1,
                            _buyATChange = item._buyATChange == Convert.ToDecimal(-9999.00) ? null : item._buyATChange,
                            _stockCode = item._stockCode,
                            _IsBuy = true,
                            _stockName = item._stockName,
                            _buyATPrice = item._buyATPrice,
                            _symbol = item._symbol,
                            _sellATPrice = item._sellATPrice,
                            _currentPrice = 0
                        }
                        );
                       
                    }

                   
                });

                this.Totalcount = result.Count();
                this.LatestdateTime = DateTime.Now.ToShortTimeString();
                
            });


            _hubConnection.On<string>("SendCaptureLiveDataForBuyForAutomation",  param =>
            {
                if (_messages==null || _messages.Count==0) {
                    return;
                }
                
                LiveStockData livedata = JsonSerializer.Deserialize<LiveStockData>(param);
                livedata.LTT_DATE = GetParseLTT(livedata.ltt); //Convert.ToDateTime(livedata.ltt); //
                livedata.ttv = !string.IsNullOrEmpty(livedata.ttv) ? livedata.ttv : "0";
                this.LatestdateTime = livedata.LTT_DATE.ToShortTimeString();
                var dictionaryValue = CollectionsMarshal.GetValueRefOrAddDefault(dictionary, livedata.symbol, out bool exists);
               if(dictionaryValue?.Count > 99)
                {
                    dictionaryValue.RemoveRange(0, 50);
                }
               else if (dictionaryValue == null)
                {
                    dictionaryValue = new List<LiveStockData>();
                   
                }
                
                decimal volumedifference =Convert.ToDecimal(dictionaryValue.LastOrDefault()?.ttv);
                volumedifference= !string.IsNullOrEmpty(livedata.ttv) ?  Convert.ToDecimal(livedata.ttv) - Convert.ToDecimal(volumedifference) : 0;
                dictionaryValue.Add(livedata);
                var findsymbol = _messages.FirstOrDefault(x => x.Symbol == livedata.symbol);
                if (_messages.Count > 0 && findsymbol != null)
                {
                    try
                    {
                        List<Quote> quotesList = dictionaryValue.Where(x => x.symbol == livedata.symbol).ToList().Select(x => new Quote
                        {
                            Close = Convert.ToDecimal(livedata.last),
                            Open = Convert.ToDecimal(livedata.open),
                            Date = Convert.ToDateTime(livedata.LTT_DATE),
                            High = Convert.ToDecimal(livedata.high),
                            Low = Convert.ToDecimal(livedata.low),
                            Volume = !string.IsNullOrEmpty(livedata.ttv) ? Convert.ToDecimal(livedata?.ttv):0
                        }).OrderBy(x => x.Date).ToList();
                        IEnumerable<MacdResult> macdresult = quotesList.GetMacd(12, 26, 9);
                        // var GetWma = quotesList.GetWma(lookbackPeriods);
                        IEnumerable<VolatilityStopResult> Volatilityresults = quotesList.GetVolatilityStop(5, 3);
                        IEnumerable<RsiResult> rsiResults = quotesList.GetObv().GetRsi(5);
                        var candleResult = quotesList.GetMarubozu(85).OrderByDescending(x => x.Date).FirstOrDefault();
                        
                        var candleResult_90 = quotesList.GetMarubozu(90).OrderByDescending(x => x.Date).FirstOrDefault();
                        var candleResult_95 = quotesList.GetMarubozu(95).OrderByDescending(x => x.Date).FirstOrDefault();
                        var candleResult_100 = quotesList.GetMarubozu(100).OrderByDescending(x => x.Date).FirstOrDefault();
                        
                        //IEnumerable<SuperTrendResult> results = quotesList.GetSuperTrend(7,3);
                        
                        

                        dictionary[livedata.symbol] = dictionaryValue.OrderByDescending(x => Convert.ToDateTime(livedata.LTT_DATE)).Take(100).ToList();
                        _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Match = candleResult.Match.ToString();
                        _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).LttDateTime = Convert.ToDateTime(livedata.LTT_DATE);
                        _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).StockName = livedata.stock_name + livedata.expiry_date;

                        var newresul = new
                        {

                            candleResult = candleResult,
                            macdresult = macdresult.LastOrDefault(),
                            rsiResults = rsiResults.LastOrDefault(),
                            Volatilityresults = Volatilityresults.LastOrDefault()
                        };
                        
                        if (candleResult != null && bullsis.Any(x => x.ToString().Contains(candleResult.Match.ToString())))
                        {
                            if (bullsis.Any(x => x.ToString().Contains(candleResult_100.Match.ToString())) && volumedifference > 400000)
                            {
                                _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount_100 = _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount_100 + 1;
                               
                                if(_messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount_100 == 1 )
                                {
                                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredPrice = Convert.ToDecimal(livedata.last);
                                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredLtt = livedata.LTT_DATE;
                                   
                                    _hubConnection.InvokeAsync("SendBullish100", livedata.stock_name,volumedifference.ToString(),livedata.last.ToString());
                                }
                            }

                            else if (bullsis.Any(x => x.ToString().Contains(candleResult_95.Match.ToString())))
                            {
                                _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount_95 = _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount_95 + 1;
                               
                            }



                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredPrice = Convert.ToDecimal(livedata.last);
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredLtt = livedata.LTT_DATE;
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).VolumeDifferecne = (volumedifference / 100000);
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount = _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BullishCount + 1;
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Data = JsonSerializer.Serialize(newresul);
                            _hubConnection.InvokeAsync("ExportBuyStockAlterFromAPP_IND", JsonSerializer.Serialize(_messages.Where(x => x.Symbol == livedata.symbol).ToList()));
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Data = "";
                            // _hubConnection.InvokeAsync("GetTopStockforBuyAutomation");
                        }

                        if (candleResult != null && barish.Any(x => x.ToString().Contains(candleResult.Match.ToString())))
                        {
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredPrice = Convert.ToDecimal(livedata.last);
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).TriggredLtt = livedata.LTT_DATE;
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).VolumeDifferecne = (volumedifference / 100000);
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BearishCount = _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).BearishCount + 1;
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Data = JsonSerializer.Serialize(newresul);
                            _hubConnection.InvokeAsync("ExportBuyStockAlterFromAPP_IND", JsonSerializer.Serialize(_messages.Where(x => x.Symbol == livedata.symbol).ToList()));
                            _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Data = "";
                            // _hubConnection.InvokeAsync("GetTopStockforBuyAutomation");
                        }
                        

                        quotesList.Clear();
                        macdresult = null;
                        Volatilityresults = null;
                        rsiResults = null;
                        Volatilityresults = null;
                        candleResult_90 = null;
                        candleResult_95 = null;
                        candleResult_100 = null;
                    }
                    catch (Exception ex)
                    {

                    }

                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).LttDateTime = livedata.LTT_DATE;
                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).CurrentPrice = Convert.ToDecimal(livedata.last);
                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).CurrentChange = Convert.ToDecimal(livedata.change);
                    _messages.FirstOrDefault(x => x.Symbol == livedata.symbol).Bgcolor = Convert.ToDecimal(livedata.change.Value) > 0 ? 3 : 1;
                    
                   

                }
            });

            


        }



        [RelayCommand]
        async Task Connect(int arg)
        {
            try
            {

                //if (_hubConnection.State == HubConnectionState.Connecting ||
                //    _hubConnection.State == HubConnectionState.Connected) return;

                await _hubConnection.StartAsync();
                Messages ??= new ObservableCollection<BuyStockAlertModel>();
                _hubConnection.SendAsync("GetBuyStockTriggers", Convert.ToInt16(arg));
                IsConnected = true;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [RelayCommand]
        async Task Disconnect()
        {
            _messages.FirstOrDefault().StockName = "ganga";

            if (_hubConnection.State == HubConnectionState.Disconnected) return;

            await _hubConnection.StopAsync();

            IsConnected = false;
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Message)) return;

            await _hubConnection.InvokeAsync("SendMessage", Name, Message);

            Message = string.Empty;
        }

        [RelayCommand]
        async Task ExportJsonData()
        {

            try
            {
                await _hubConnection.InvokeAsync("ExportBuyStockAlterFromAPP", JsonSerializer.Serialize(this.Messages.ToList()));

                Message = string.Empty;
            }
            catch (Exception ex)
            {


            }
        }

        [RelayCommand]
        async Task PerformSearchCommand()
        {

            try
            {
                await _hubConnection.InvokeAsync("ExportBuyStockAlterFromAPP", JsonSerializer.Serialize(this.Messages.ToList()));

                Message = string.Empty;
            }
            catch (Exception ex)
            {


            }
        }
    }
}

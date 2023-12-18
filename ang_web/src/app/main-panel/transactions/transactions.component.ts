import { Component } from '@angular/core';
import { Transactions } from 'src/app/models/transaction.model';
import { accountTransactions } from '../transaction-data/transactions';
import { SignalrService } from 'src/app/services/signalr.service';
import { Stock, StocksService } from 'src/app/services/stocks.service';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription, of } from 'rxjs';
import { Equities } from 'src/app/models/equities.model';
import { RowClassArgs } from '@progress/kendo-angular-grid';
import { SortDescriptor } from '@progress/kendo-data-query';
import { ThemePalette } from '@angular/material/core';
import { ProgressBarMode } from '@angular/material/progress-bar';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';


@Component({
    selector: 'transactions',
    templateUrl: './transactions.component.html',
    styleUrls: ['./transactions.component.css'],
    animations: [
      trigger('positiveState', [
          transition('void => *', []),
          transition('* => void', []),
          transition('* => *', [
              animate(1500, keyframes([style({ backgroundColor: '#32CD32', offset: 0.0 }), style({ backgroundColor: 'inherit', offset: 1.0 })]))
          ])
      ]),
      trigger('negativeState', [
          transition('void => *', []),
          transition('* => void', []),
          transition('* => *', [
              animate(1500, keyframes([style({ backgroundColor: '#FF0000', offset: 0.0 }), style({ backgroundColor: 'inherit', offset: 1.0 })]))
          ])
      ])
  ]


})
export class TransactionsComponent {
    public transactionCards: Transactions[] = accountTransactions;
    public gridData: Equities[] |any;
    color: ThemePalette = 'primary';
    mode: ProgressBarMode = 'determinate';
    value = 0;
    bufferValue = 0;

    public prevDataItem!: Stock;
    private stocksUrl: string = 'assets/data.json';
    private immutableData!: any[];

    public previousData: Equities[] = [];
    private signalRSubscription: Subscription | undefined;
    public updateFreq: number = 2000;


    constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {

      this.signalRService.connection
      .invoke('GetAllStocks')
      .catch((error: any) => {
        console.log(`SGetAllStocks error: ${error}`);
        alert('GetAllStocks error!, see console for details.');
      });

        this.signalRService.connection.on("SendLiveData",(val :string) => {

        const obj = JSON.parse(val);
        let indexToUpdate = [...this.gridData].findIndex((item: { symbol: any; }) => item.symbol===obj.symbol)

        let newRow = {
         indexToUpdate,
          id: obj.symbol,
              currency:"$",
              stockName:obj.stock_name,
              change:obj.change,
              trend:obj.trend,
              volume:obj.ttv,
              open: obj.open,
              low:obj.low,
              high:obj.high,
              last :obj.last,
              bPrice:obj.bPrice,
            sPrice :obj.sPrice,
           // volumeNumber:(((obj.ttv.toString().replace('L',(parseFloat(obj.ttv.toString().replace('L',''))*100000)))).toString().replace('C',(parseFloat(obj.ttv.toString().replace('C',''))*10000000).toString())),


              symbol:obj.symbol,
              currentPrice: obj.last,
              bQty :obj.bQty,
              sQty:obj.sQty,
              value:(parseInt(obj.totalBuyQt)/(parseInt(obj.totalBuyQt) + parseInt(obj.totalSellQ)))*100 ,//-obj.totalBuyQt),
               bufferValue:(parseInt(obj.totalSellQ)/(parseInt(obj.totalBuyQt) + parseInt(obj.totalSellQ)))*100




      };
      this.gridData[indexToUpdate] = newRow;



   this.gridData = Object.assign([], this.gridData);
   this.gridData = this.sortData('stackname');

              })
    }


    sortData(_case: string) {
      switch(_case) {
        case 'stackname':
         return this.gridData.sort((a: { volume: number; }, b: { volume: number; }) => (b.volume - a.volume) )
         //return this.gridData.sort((a: { volumeNumber: number; }, b: { volumeNumber: number; }) => (b.volumeNumber > a.volumeNumber) ? 1 : -1)

      }
    }


  getMostFrequent(arr:any) :any {
    const hashmap = arr.reduce( (acc :any, val:any) => {
     acc[val] = (acc[val] || 0 ) + 1
     return acc
  },{})
 return Object.keys(hashmap).reduce((a, b) => hashmap[a] > hashmap[b] ? a : b)
 }

  updateRandomRowWithData(val :any): any {


  var row = [...this.gridData].find(x=>x.symbol===val.symbol)
  var rowlast=row?.last;
  if(row?.symbol==val.symbol && val.last != rowlast ) {


 var nmostnumber = this.getMostFrequent(row.intraday);



    let newRow = {
        ...row,
        id: val.symbol,
            currency:"$",
            stockName:val.stock_name,
            change:val.change,
            mostnumber:nmostnumber,
            volume:val.ttv,
            open: val.open,
            low:val.low,
            high:val.high,
            last :val.last,

            symbol:val.symbol,
            currentPrice: val.last,
            bQty :val.bQty,
            sQty:val.sQty,
            netQtry : val.bQty-val.sQty,
            avgPrice:val.avgPrice,
           // value:val.totalSellQ,
            bufferValue:val.totalBuyQt,
            bPrice:val.bPrice,
            sPrice :val.sPrice,
           // volumeNumber:parseInt((parseFloat(val.ttv.toString().replace('L',''))*100000).toString().replace('C',(parseFloat(val.ttv.toString().replace('C',''))*100000).toString())),





    };

    // this.previousData = [...this.immutableData];
    let indexToUpdate = [...this.gridData].findIndex(item => item.symbol === val.symbol);

    // this.immutableData[indexToUpdate] = newRow;
    this.gridData[indexToUpdate]== newRow;
    //newRow.dataopen.push(live.open);
   // newRow.dataavgPrice.push(live.avgPrice);


    return newRow;
  }
  else{
    return row;
  }

  }





    ngOnInit() {

;
       this.signalRService.connection.on("SendAllStocks",(data :any) => {

        var  i=0;
        let res = data.map((val: Equities) => {

          return { // Return the new object structure
            id: val.symbol,
            currency:"$",
            stockName:val.stock_name,
            change:val.change,
            volume:val.ttv,
            open: val.open,
            low:val.low,
            high:val.high,
            last :val.last,
            //volume: val.bQty,
            symbol:val.symbol,
            currentPrice: val.last,
            bQty :val.bQty,
            sQty:val.sQty,
            netQtry : val.bQty-val.sQty,
            avgPrice:val.avgPrice,
            //intraday: Observable<number[]>;
            intraday: [val.last],
            dataopen:[val.open],
            dataavgPrice:[val.avgPrice],
            mostnumber:val.last,
            bPrice:val.bPrice,
            sPrice :val.sPrice,

          }
          i++;
        });

        this.gridData = res;// this.getDataObservable(res);



        })


      }


    public sort: SortDescriptor[] = [
      {
        field: "change",
        dir: "desc",
      },
    ];

}

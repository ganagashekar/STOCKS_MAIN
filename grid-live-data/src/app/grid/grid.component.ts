import { Component } from '@angular/core';
import { GridDataResult, RowClassArgs } from '@progress/kendo-angular-grid';
import { Observable, Subscription, interval, of } from 'rxjs';
import { trigger, style, animate, transition, keyframes } from '@angular/animations';
// import { Stock, StocksService } from '../services/stocks.service';
import { SortDescriptor, orderBy } from '@progress/kendo-data-query';

import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { Equities } from '../models/equities.model';
import { StocksService } from '../services/stocks.service';



@Component({
    selector: 'grid',
    templateUrl: './grid.component.html',
    styleUrls: ['./grid.component.css'],
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
export class GridComponent {
    public gridData: Observable<Stock[]> |any;
    public gridDataEquties: Observable<Equities[]> | any;
    public prevDataItem!: Stock;
    private stocksUrl: string = 'assets/data.json';
    private immutableData!: any[];
    private immutableDataEquties!: Stock[];
    public previousData: Stock[] = [];
    private signalRSubscription: Subscription | undefined;
    public updateFreq: number = 2000;

    constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {

      this.signalRService.connection
      .invoke('GetAllStocks')
      .catch((error: any) => {
        console.log(`SGetAllStocks error: ${error}`);
        alert('GetAllStocks error!, see console for details.');
      });
    }



    getDataObservable(equities :Stock[]): Observable<Stock[]> {
      return new Observable<Stock[]>((observer) => {
          this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {

            const obsof1= of(equities);
            obsof1.subscribe((data: Stock[]) => {
              this.immutableData = data ;
              this.previousData = data;
              observer.next(this.immutableData);
            })



            this.signalRService.connection.on("SendLiveData",(val :string) => {

              const obj = JSON.parse(val);
            this.updateRandomRowWithData(obj )

                  observer.next(this.immutableData);
              })


          });
      });


  }

  getMostFrequent(arr:any) :any {
    const hashmap = arr.reduce( (acc :any, val:any) => {
     acc[val] = (acc[val] || 0 ) + 1
     return acc
  },{})
 return Object.keys(hashmap).reduce((a, b) => hashmap[a] > hashmap[b] ? a : b)
 }

  updateRandomRowWithData(val :any): any {


  var row = [...this.immutableData].find(x=>x.symbol===val.symbol)
  var rowlast=row?.last;
  if(row?.symbol==val.symbol && val.last != rowlast ) {


 var nmostnumber = this.getMostFrequent(row.intraday);

   // if( row.change_pct != live.change) {
    // let changePrice = Math.floor(30 * Math.random()) / 10;
    // changePrice *= Math.round(Math.random()) ? 2 : -0.09;

    // let changePercentage = Math.floor(30 * Math.random()) / 10;
    // changePercentage *= Math.round(Math.random()) ? 1 : -1;

    // const percentageValue = row.change_24h + changePercentage;
    // const priceValue = row.currentPrice + changePrice;

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
            //volume: val.bQty,
            symbol:val.symbol,
            currentPrice: val.last,
            bQty :val.bQty,
            sQty:val.sQty,
            netQtry : val.bQty-val.sQty,
            avgPrice:val.avgPrice,
            //intraday: Observable<number[]>;
            // intraday: [],
            // dataopen:[val.open],
            // dataavgPrice:[val.avgPrice],
            // change_24h:val.last,
            // change_pct:val.change,

        // currentPrice: live.last,
        // stockName:live.stock_name,
        //     change:live.change,
        //     volume:live.ttv,
        //     open: live.open,
        //     low:live.low,
        //     high:live.high,

        //     last :live.last,
        //     bQty :live.bQty,
        //     sQty:live.sQty,
        //     netQtry : live.bQty-live.sQty,
        //     avgPrice:live.avgPrice,
        //     intraday:[live.last],
        //     dataopen:[live.open]




    };

    this.previousData = [...this.immutableData];
    let indexToUpdate = this.immutableData.findIndex(item => item.symbol === val.symbol);
    newRow.intraday.push(val.last);
    this.immutableData[indexToUpdate] = newRow;
    //newRow.dataopen.push(live.open);
   // newRow.dataavgPrice.push(live.avgPrice);


    return newRow;
  }
  else{
    return row;
  }

  }





    ngOnInit() {


    //   interval(10000 *6*5).subscribe(x => {

    //     const sub=this.gridDataEquties.subscribe((res: any[]) => {
    //       sub.unsubscribe();
    //       this.stocksService.saveData( { dataSetName :"" ,jsonString :JSON.stringify(res)}).subscribe(() => {

    //       });;
    //       // this.totalDeals = forApprovalResponse  + draftResponse;

    //       //  if (forApprovalResponse > 0) {
    //       //     // ... logic goes here
    //       //  }

    //       //  if (draftResponse > 0) {
    //       //    // ...logic goes here
    //       //  }

    //       //  if (this.totalDeals > 0) {
    //       //     // do something with it
    //       //  }
    //     });

    // });

     // const pairs =  this.signalRService.connection.on("SendAllStocks");
     // console.log(pairs.map((p:Equities[]) => (p)));
       this.signalRService.connection.on("SendAllStocks",(data :any) => {
        debugger;
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
            mostnumber:val.last

          //  label: item.abbreviation, value: item.name
          }
          i++;
        });

        this.gridDataEquties = this.getDataObservable(res);



        })


      }


    public sort: SortDescriptor[] = [
      {
        field: "change",
        dir: "desc",
      },
    ];
    public rowCallback = (context: RowClassArgs) => {
        const previousData = this.previousData;
        const index = previousData.findIndex((item: { id: any; }) => item.id === context.dataItem.id);
        this.prevDataItem = previousData[index];

        if (context.dataItem.change > 0) {
            return { 'price-up': true };
        } else {
            return { 'price-down': true };
        }
    };
}

export interface Stock {
  id: number;
  currency: string;
  symbol: string;

  currentPrice: number;
  change_24h: number;
  intraday: number[];
  dataopen : number[];
  dataavgPrice: number[];
  change_pct:number;
  stockName:number,
  change:number,
  volume:number,
  open: number,
  low:number,
  high:number,
  last :number,
  trend:string,
  bQty :number,
  sQty:number,
  netQtry : number,
  avgPrice:number,

}

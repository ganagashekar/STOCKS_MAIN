import { Component } from '@angular/core';
import {  RowClassArgs } from '@progress/kendo-angular-grid';
import { Observable, Subscription, interval, of } from 'rxjs';
import { trigger, style, animate, transition, keyframes } from '@angular/animations';
// import { Stock, StocksService } from '../services/stocks.service';
import { SortDescriptor, orderBy } from '@progress/kendo-data-query';

import { HttpClient } from '@angular/common/http';
import { SignalrService } from '../services/signalr.service';
import { Equities } from '../models/equities.model';
import { StocksService } from '../services/stocks.service';
import { isNumber } from '@progress/kendo-angular-grid/dist/es2015/utils';



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
    public scrollingData:[] |any ;


    constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {

      this.signalRService.connection
      .invoke('GetAllStocks')
      .catch((error: any) => {
        console.log(`SGetAllStocks error: ${error}`);
        alert('GetAllStocks error!, see console for details.');
      });
    }


    getwma (data: any[], size: any) {
      const length = data.length;

      if (size <= 1) {
        return data.slice();
      }

      if (size > length) {
        return Array(length);
      }

      const ret = [];
      const denominator = size * (size + 1) / 2;
      const prepare = size - 1;
      let sum = 0;
      let numerator = 0;
      let datum = 0;
      let i = 0;
      let real = -1;

      for (; i < prepare; i++) {
        datum = data[i];

        if (isNumber(datum)) {
          sum += datum;
          numerator += (i + 1) * datum;
        }
      }

      for (; i < length; i++, real++) {
        datum = data[i];

        if (isNumber(datum)) {
          sum += datum;
          numerator += size * datum;
        }

        if (real >= 0 && isNumber(data[real])) {
          sum -= data[real];
        }

        ret[i] = numerator / denominator;
        numerator -= sum;
      }

      return ret;
    };

     getma (data: any[], size: any)  {
      const length = data.length;

      if (!size) {
        return data.reduce((a, b) => a + b) / length;
      }

      if (size <= 1) {
        return data.slice();
      }

      if (size > length) {
        return Array(length);
      }

      const prepare = size - 1;
      const ret = [];
      let sum = 0;
      let i = 0;
      let counter = 0;
      let datum;

      for (; i < length && counter < prepare; i++) {
        datum = data[i];

        if (  (datum)) {
          sum += datum;
          counter++;
        }
      }

      for (; i < length; i++) {
        datum = data[i];

        if (isNumber(datum)) {
          sum += datum;
        }

        if (isNumber(data[i - size])) {
          sum -= data[i - size];
        }

        ret[i] = sum / size;
      }

      return ret;
    };
    // const isNumber = (subject: number) => typeof subject === 'number'
    // // is not NaN: `NaN === NaN` => `false`
    // && subject === subject;


     relDiff(a: number, b: number) {
      return  100 * Math.abs( ( a - b ) / ( (a+b)/2 ) );
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
              var row = [...this.immutableData].find(x=>x.symbol===obj.symbol)

              this.updateRandomRowWithData(obj,row )

                  observer.next(this.immutableData);
              })


          });
      });


  }

  updatescrollingtext(obj:any){
    let indexToUpdate = [...this.scrollingData].findIndex((item: { symbol: any; }) => item.symbol===obj.symbol)
    let newRow = {
      indexToUpdate,
       id: obj.symbol,
           stockName:obj.stock_name,
           change:obj.change,
         //  volumeNumber:parseInt((parseFloat(obj.ttv.toString().replace('L',''))*100000).toString().replace('C',(parseFloat(obj.ttv.toString().replace('C',''))*100000).toString())),
          symbol:obj.symbol
   };

    if( indexToUpdate !=null) {

  this.scrollingData[indexToUpdate] = newRow;
  this.scrollingData = Object.assign([], this.scrollingData);
}else{

  this.scrollingData.push(newRow);
}
  }

  sortData(_case: string) {
    switch(_case) {
      case 'stackname':
       return this.gridDataEquties.sort((a: { change: number; }, b: { change: number; }) => (b.change > a.change) ? 1 : -1)

    }
  }

  getMostFrequent(arr:any) :any {
    const hashmap = arr.reduce( (acc :any, val:any) => {
     acc[val] = (acc[val] || 0 ) + 1
     return acc
  },{})
 return Object.keys(hashmap).reduce((a, b) => hashmap[a] > hashmap[b] ? a : b)
 }

  updateRandomRowWithData(val :any,row:any): any {


  // var row = [...this.immutableData].find(x=>x.symbol===val.symbol)
  var rowlast=row?.last;
  if(row?.symbol==val.symbol && val.last != rowlast ) {



 //var nmostnumber = this.getMostFrequent(row.intraday);
    let newRow = {
        ...row,
        id: val.symbol,
            currency:"$",
            stockName:val.stock_name,
            change:val.change,
//mostnumber:nmostnumber,
            volume:val.ttv,
          //  volumeNumber:parseInt((parseFloat(val.ttv.toString().replace('L',''))*100000).toString().replace('C',(parseFloat(val.ttv.toString().replace('C',''))*100000).toString())),
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
            calc_ma: parseFloat(this.getma(row.intraday,null).toString()).toFixed(2),
            calc_wma:parseFloat(this.getwma(row.intraday,null).toString()).toFixed(2),





    };

    this.previousData = [...this.immutableData];
    let indexToUpdate = this.immutableData.findIndex(item => item.symbol === val.symbol);
    newRow.intraday.push(val.last);
    this.immutableData[indexToUpdate] = newRow;
    // if( this.relDiff(row?.volumeNumber,newRow.volumeNumber) > 90){
    //   this.updatescrollingtext(newRow)
    // }
   // this.gridDataEquties = this.sortData('stackname');
    //newRow.dataopen.push(live.open);
   // newRow.dataavgPrice.push(live.avgPrice);


    return newRow;
  }
  else{
    return row;
  }

  }





    ngOnInit() {



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
            calc_ma:this.getma([val.last],null),
            calc_wma:this.getwma([val.last],null)

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

        if (context.dataItem.change > this.prevDataItem.change) {
            return { 'green': true };
        } else {
            return { 'red': true };
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

import { Component, OnInit } from '@angular/core';
import { StocksService } from '../services/stocks.service';
import { SignalrService } from '../services/signalr.service';
import { Observable, Subscription, of } from 'rxjs';
import { Equities } from '../models/equities.model';
import { HttpClient } from '@angular/common/http';
import { SortDescriptor } from '@progress/kendo-data-query';
import { RowClassArgs } from '@progress/kendo-angular-grid';
import {MatChipsModule} from '@angular/material/chips';


@Component({


  selector: 'app-stocklist',
  templateUrl: './stocklist.component.html',
  styleUrls: ['./stocklist.component.css'],


})
export class StocklistComponent implements OnInit {

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
  bgcolor ="#B90143";


  constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {

    // this.signalRService.connection
    // .invoke('GetStocksList',false)
    // .catch((error: any) => {
    //   console.log(`SGetAllStocks error: ${error}`);
    //   alert('GetAllStocks error!, see console for details.');
    // });

    this.signalRService.connection
    .invoke('ExportLiveStocksToJson')
    .catch((error: any) => {
      console.log(`SGetAllStocks error: ${error}`);
      alert('GetAllStocks error!, see console for details.');
    });
  }


  getonlyfavorites(){
    debugger;
    this.signalRService.connection
    .invoke('GetStocksList',true)
    .catch((error: any) => {
      console.log(`SGetAllStocks error: ${error}`);
      alert('GetAllStocks error!, see console for details.');
    });
  }



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
          ltt:val.ltt,
          href : val.href,
          min :val.min,
          max :val.max,
          // calc_ma: parseFloat(this.getma(row.intraday,null).toString()).toFixed(2),
          // calc_wma:parseFloat(this.getwma(row.intraday,null).toString()).toFixed(2)
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

addormidifyfavorite(p :any) {
debugger;

this.signalRService.connection
.invoke('AddOrModifyFavorite',p.msnid,p.selected==true?1 :0)
.catch((error: any) => {
  console.log(`SGetAllStocks error: ${error}`);
  alert('GetAllStocks error!, see console for details.');
});
}




carDateCalculator(value :string ){

  if(value == "")
     return false;

  return value === 'strongBuy' ? true :  (value === 'buy')  ? true: false
 }


  ngOnInit() {

    this.signalRService.connection.on("SendAddOrModifyFavorite",(data :any) => {

     // alert(data)
    })

     this.signalRService.connection.on("SendStocksList",(data :any) => {
debugger;
      var  i=0;
      let res = data.map((val: Equities) => {
        var from =(val.last).toFixed(2);
         var to = ((val.last)-((0.50/100)*val.last)).toFixed(2);

        // var from =61;
        // var to = 63;

        return {
           // Return the new object structure
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
          intraday: val.data,
          dataopen:[val.open],
          dataavgPrice:[val.avgPrice],
          mostnumber:val.last,
          ltt:val.ltt,
          href : val.href,
          min :val.min,
          max :val.max,
          recmdtn : val.recmdtn,
          // int_open: from > to ? to :from,
          // int_last: from < to ? from  :to,

          int_open: from ,
          int_last: to,

          return1w :val.return1w,
           return1m :val.return1m,
          return3m : val.return3m,
          noofrec : val.noofrec,
          beta :val.beta,

          eps:val.eps,
          target:val.target,
          stockdetailshref:val.stockdetailshref,
          secId:val.secId,
          isfavorite : val.isfavorite,

          volumeC:val.volumeC
          // calc_ma:this.getma(+[val.last],null),
          // calc_wma:this.getwma([val.last],null)

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

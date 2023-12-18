import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Transactions } from 'src/app/models/transaction.model';
import { SignalrService } from 'src/app/services/signalr.service';
import { Stock, StocksService } from 'src/app/services/stocks.service';
import { HttpClient } from '@angular/common/http';
import { Observable, Subscription, interval, of } from 'rxjs';
import { Equities } from 'src/app/models/equities.model';
import { RowClassArgs } from '@progress/kendo-angular-grid';
import { SortDescriptor } from '@progress/kendo-data-query';
import { MatOption, ThemePalette } from '@angular/material/core';
import { ProgressBarMode } from '@angular/material/progress-bar';
import { animate, keyframes, style, transition, trigger } from '@angular/animations';
import { accountTransactions } from '../main-panel/transaction-data/transactions';
import { MatSelectChange } from '@angular/material/select';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';
import { formatDate } from '@angular/common';

interface Food {
  value: string;
  viewValue: string;
}

@Component({
    selector: 'app-top-performer',
  templateUrl: './top-performer.component.html',
  styleUrls: ['./top-performer.component.css'],
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
export class TopPerformerComponent {

    public gridData: Equities[] |any;
    public gridData_BuyRunner :any;
    color: ThemePalette = 'primary';

    mode: ProgressBarMode = 'determinate';
    value = 0;
    bufferValue = 0;
    selectedTop : number |any ;
    SelectedDate : string | any ;

    buy_selectedTop : number |any ;
    buy_SelectedDate : string | any ;

    foods: Food[] = [
      {value: '1', viewValue: '1'},
      {value: '5', viewValue: '5'},
      {value: '10', viewValue: '10'},
      {value: '15', viewValue: '15'},
      {value: '20', viewValue: '20'},
      {value: '30', viewValue: '30'},
      {value: '50', viewValue: '50'},
      {value: '70', viewValue: '70'},
      {value: '100', viewValue: '100'},
    ];

    buyRatio: Food[] = [

      {value: '10', viewValue: '10'},
      {value: '15', viewValue: '15'},
      {value: '20', viewValue: '20'},
      {value: '30', viewValue: '30'},
      {value: '50', viewValue: '50'},
      {value: '70', viewValue: '70'},
      {value: '100', viewValue: '100'},
      {value: '120', viewValue: '120'},
      {value: '150', viewValue: '150'},
      {value: '200', viewValue: '200'},
      {value: '300', viewValue: '300'},

      {value: '400', viewValue: '400'},
      {value: '500', viewValue: '500'},
      {value: '600', viewValue: '600'},
      {value: '700', viewValue: '700'},
      {value: '800', viewValue: '800'},
       {value: '900', viewValue: '900'},
       {value: '1000', viewValue: '1000'},


       {value: '-10', viewValue: '-10'},
      {value: '-15', viewValue: '-15'},
      {value: '-20', viewValue: '-20'},
      {value: '-30', viewValue: '-30'},
      {value: '-50', viewValue: '-50'},
      {value: '-70', viewValue: '-70'},
      {value: '-100', viewValue: '-100'},

      {value: '-120', viewValue: '-120'},
      {value: '-150', viewValue: '-150'},
      {value: '-200', viewValue: '-200'},
      {value: '-300', viewValue: '-300'},
      {value: '-400', viewValue: '-400'},
      {value: '-500', viewValue: '-500'},
      {value: '-600', viewValue: '-600'},
      {value: '-700', viewValue: '-700'},
      {value: '-800', viewValue: '-800'},
       {value: '-900', viewValue: '-900'},
       {value: '-1000', viewValue: '-1000'},



    ];

    public prevDataItem!: Stock;
    private stocksUrl: string = 'assets/data.json';
    private immutableData!: any[];

    public previousData: Equities[] = [];
    private signalRSubscription: Subscription | undefined;
    public updateFreq: number = 2000;


    constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient)
    {
      this.buy_SelectedDate = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      this.SelectedDate = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      this.signalRService.connection
      .invoke('FecthTopStocks',this.SelectedDate,this.selectedTop)
      .catch((error: any) => {
        console.log(`SGetAllStocks error: ${error}`);
        alert('GetAllStocks error!, see console for details.');
      });



        this.signalRService.connection.on("SendTopStocks",(val :[any]) => {

          this.gridData = Object.assign([], val);
          this.gridData = this.sortData('stackname');

              })

              this.signalRService.connection.on("SendSTockToBuy",(val :[any]) => {
                debugger;
                 this.gridData_BuyRunner = Object.assign([], val);
                 this.gridData_BuyRunner =  this.gridData_BuyRunner;

                     })

              interval(8000).subscribe(x => {

               debugger;
               var date=formatDate(this.buy_SelectedDate, 'yyyy-MM-dd', 'en');
            this.signalRService.connection
            .invoke('FetchSTockToBuy',date,this.buy_selectedTop)
            .catch((error: any) => {
              console.log(`GetSTockToBuy error: ${error}`);
              alert('GetSTockToBuy error!, see console for details.');
        });
          })


          interval(20000).subscribe(x => {
            this.signalRService.connection
            .invoke('FecthTopStocks',this.SelectedDate,this.selectedTop)
            .catch((error: any) => {
              console.log(`SGetAllStocks error: ${error}`);
              alert('GetAllStocks error!, see console for details.');
        });


      })


    }

    selected(event: MatSelectChange) {


      this.selectedTop=event;

    }

     onDateChange(event: any ): void {

      this.SelectedDate = event.value.toUTCString();
      console.log('Teste', this.SelectedDate);
    }


    buy_selected(event: MatSelectChange) {


      this.buy_selectedTop=event;

    }

     buy_onDateChange(event: any ): void {

      this.buy_SelectedDate = event.value.toUTCString();
      console.log('Teste', this.buy_SelectedDate);
    }

    sortData(_case: string) {
      switch(_case) {
        case 'stackname':
         return this.gridData.sort((a: { volume: number; }, b: { volume: number; }) => (b.volume - a.volume) )

      }
    }




  getMostFrequent(arr:any) :any {
    const hashmap = arr.reduce( (acc :any, val:any) => {
     acc[val] = (acc[val] || 0 ) + 1
     return acc
  },{})
 return Object.keys(hashmap).reduce((a, b) => hashmap[a] > hashmap[b] ? a : b)
 }










    public sort: SortDescriptor[] = [
      {
        field: "change",
        dir: "desc",
      },
    ];

}

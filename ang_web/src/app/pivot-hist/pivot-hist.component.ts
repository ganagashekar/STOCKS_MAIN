import { Component, OnInit } from '@angular/core';
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
import { DomSanitizer,SafeHtml } from '@angular/platform-browser';

interface Food {
  value: string;
  viewValue: string;
}



@Component({
  selector: 'app-pivot-hist',
  templateUrl: './pivot-hist.component.html',
  styleUrls: ['./pivot-hist.component.css'],
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
export class PivotHistComponent {
     html: SafeHtml;
     displayedColumns!: string[];
     dataSource!: any[];
     public transactionCards: Transactions[] = accountTransactions;
      public gridData: Equities[] |any;
      color: ThemePalette = 'primary';

    mode: ProgressBarMode = 'determinate';
    value = 0;
    bufferValue = 0;
    selectedTop : number |any ;
    SelectedDate : string | any ;

    foods: Food[] = [
      {value: 'last', viewValue: 'last'},
      {value: 'close', viewValue: 'close'},
      {value: 'change', viewValue: 'change'},
      {value: 'ttv', viewValue: 'ttv'},
      {value: 'open', viewValue: 'open'},

    ];
    public prevDataItem!: Stock;
    private stocksUrl: string = 'assets/data.json';
    private immutableData!: any[];

    public previousData: Equities[] = [];
    private signalRSubscription: Subscription | undefined;
    public updateFreq: number = 2000;


    constructor(public _sanitizer: DomSanitizer,public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {
      this.html = this._sanitizer.bypassSecurityTrustHtml("");
      // this.signalRService.connection
      // .invoke('GetPivotData',this.SelectedDate,this.selectedTop)
      // .catch((error: any) => {
      //   console.log(`SGetAllStocks error: ${error}`);
      //   alert('GetAllStocks error!, see console for details.');
      // });



      //   this.signalRService.connection.on("SendTopStocks",(val :[any]) => {
      //    debugger;
      //     this.gridData = Object.assign([], val);
      //     this.gridData = this.sortData('stackname');

      //         })

          //     interval(5000).subscribe(x => {
          //       this.signalRService.connection
          //       .invoke('GetPivotData',this.SelectedDate,this.selectedTop)
          //       .catch((error: any) => {
          //         console.log(`SGetAllStocks error: ${error}`);
          //         alert('GetAllStocks error!, see console for details.');
          //   });
          // })
    }

    // selected(event: MatSelectChange) {
    //   debugger;

    //   this.selectedTop=event;
    //   this.signalRService.connection
    //             .invoke('GetPivotData',this.SelectedDate,this.selectedTop)
    //             .catch((error: any) => {
    //               console.log(`SGetAllStocks error: ${error}`);
    //               alert('GetAllStocks error!, see console for details.');
    //         });

    // }

    //  onDateChange(event: any ): void {
    //   debugger;
    //   this.SelectedDate = event.value.toUTCString();
    //   this.signalRService.connection
    //             .invoke('GetPivotData',this.SelectedDate,this.selectedTop)
    //             .catch((error: any) => {
    //               console.log(`SGetAllStocks error: ${error}`);
    //               alert('GetAllStocks error!, see console for details.');
    //         });
    // }
  
    ngOnInit() {


    this.signalRService.connection.on("SendPivotData",(data :any) => {


      //this.displayedColumns= Object.keys(data[0])
      this.html = this._sanitizer.bypassSecurityTrustHtml(data);// this.getDataObservable(res);



      })


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

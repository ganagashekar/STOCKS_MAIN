import { Component, OnInit } from '@angular/core';
import { ThemePalette } from '@angular/material/core';
import { ProgressBarMode } from '@angular/material/progress-bar';
import { Equities } from '../models/equities.model';
import { Subscription, interval } from 'rxjs';
import { Stock, StocksService } from '../services/stocks.service';
import { SignalrService } from '../services/signalr.service';
import { HttpClient } from '@angular/common/http';
import { formatDate } from '@angular/common';
import { MatSelectChange } from '@angular/material/select';
import { SortDescriptor } from '@progress/kendo-data-query';
import { FormControl } from '@angular/forms';
import { dropdownModel } from '../models/transaction.model';


interface Food {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-upperlower-ckt',
  templateUrl: './upperlower-ckt.component.html',
  styleUrls: ['./upperlower-ckt.component.css']
})
export class UpperlowerCKTComponent implements OnInit {
  public gridData: Equities[] |any;
    public gridData_BuyRunner :any;
    color: ThemePalette = 'primary';

    mode: ProgressBarMode = 'determinate';
    value = 0;
    bufferValue = 0;
    selectedTop : number |any ;
    SelectedDate : string | any ;

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

    CKTName = new FormControl();
    CKTNameList :  dropdownModel[] =[{text :"upperCktLm",value :"upperCktLm"},{text :"lowerCktLm",value :"lowerCktLm"}];
    selectedCKTName : any ;

    public prevDataItem!: Stock;
    private stocksUrl: string = 'assets/data.json';
    private immutableData!: any[];

    public previousData: Equities[] = [];
    private signalRSubscription: Subscription | undefined;
    public updateFreq: number = 2000;


    constructor(public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient)
    {

      interval(80000).subscribe(x => {
        console.log('SendAlertsUpperCKT');
        debugger;

     this.signalRService.connection
     .invoke('SendAlertsUpperCKT')
     .catch((error: any) => {
       console.log(`SendAlertsUpperCKT error: ${error}`);
       alert('SendAlertsUpperCKT error!, see console for details.');
 });
   })


      this.SelectedDate = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      this.signalRService.connection
      .invoke('GET_CKT',this.SelectedDate,this.selectedTop,"upperCktLm")
      .catch((error: any) => {
        console.log(`SGetAllStocks error: ${error}`);
        alert('GetAllStocks error!, see console for details.');
      });



        this.signalRService.connection.on("SendGET_CKT",(val :[any]) => {

          this.gridData = Object.assign([], val);
          this.gridData = this.sortData('stackname');

              })




      //     interval(30000).subscribe(x => {
      //       this.signalRService.connection
      //       .invoke('GET_CKT',this.SelectedDate,this.selectedTop,this.selectedCKTName)
      //       .catch((error: any) => {
      //         console.log(`SGetAllStocks error: ${error}`);
      //         alert('GetAllStocks error!, see console for details.');
      //   });


      // })


    }
  ngOnInit(): void {

  }

    selected(event: MatSelectChange) {


      this.selectedTop=event;

    }

    selectedCKTChange(event: any) {
      debugger;

      this.selectedCKTName=event;



    }

     onDateChange(event: any ): void {

      this.SelectedDate = event.value.toUTCString();
      console.log('Teste', this.SelectedDate);
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

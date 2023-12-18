import { Component, OnInit } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { ActivatedRoute, Routes } from '@angular/router';
import { SignalrService } from '../services/signalr.service';
import { StocksService } from '../services/stocks.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-stockdetails',
  templateUrl: './stockdetails.component.html',
  styleUrls: ['./stockdetails.component.css']
})
export class StockdetailsComponent implements OnInit {
  id :string | any;
  html: SafeHtml | any;
  SelectedDate : Date=new Date() ;

    constructor(private route: ActivatedRoute,public _sanitizer: DomSanitizer,public signalRService: SignalrService, public stocksService: StocksService, private http: HttpClient) {
      this.html = this._sanitizer.bypassSecurityTrustHtml("");
    // this.id = this.route.snapshot.paramMap.get('id');

    this.route.queryParams.subscribe(params => {
      this.id = params['id'];

      this.signalRService.connection
      .invoke('GetStockDetailsBySymbol',this.id )
      .catch((error: any) => {
        console.log(`GetStockDetailsBySymbol error: ${error}`);
        alert('GetStockDetailsBySymbol error!, see console for details.');
      });


  });




   }
   onDateChange(event: any ): void {
    debugger;
    this.SelectedDate = event.value;//.toUTCString();
   // this.fetchpivotdata();

  }

  GetPivotData(text:any) {
    //this.fetchpivotdata();
  }

  settomorrow(){
    this.SelectedDate = new Date();
    this.SelectedDate.setDate(this.SelectedDate.getDate() + 1 );

  }
  setweek(){

    this.SelectedDate = new Date();
    this.SelectedDate.setDate(this.SelectedDate.getDate() + 7 );
  }

  setMonth(){
    this.SelectedDate = new Date();
    this.SelectedDate.setDate(this.SelectedDate.getDate() + 29 );
  }
  SaveWatchList() :void {


     this.signalRService.connection
               .invoke('SaveWatchList',this.SelectedDate,this.id
               )
               .catch((error: any) => {
                 console.log(`SaveWatchList error: ${error}`);
                 alert('GetPivotData error!, see console for details.');
           });
   }
  ngOnInit() {


    this.signalRService.connection.on("SendStockDetailsBySymbol",(data :any) => {


      //this.displayedColumns= Object.keys(data[0])
      this.html = this._sanitizer.bypassSecurityTrustHtml(data);// this.getDataObservable(res);



      })

      this.signalRService.connection.on("Send_SaveWatchList",(data :any) => {

        alert('Success');
        //this.displayedColumns= Object.keys(data[0])
        //this.html = this._sanitizer.bypassSecurityTrustHtml(data);// this.getDataObservable(res);



        })

  }

}

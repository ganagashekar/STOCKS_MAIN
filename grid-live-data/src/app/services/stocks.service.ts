
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, Subscription, finalize } from 'rxjs';
import { SignalrService } from './signalr.service';
import { Equities } from '../models/equities.model';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})

export class StocksService {

  constructor(private http: HttpClient) {}
  public saveData(Data: any ): any {
    const url = environment.SignalrAPISaveJson;

    const headers = { 'content-type': 'application/json'}
    const body=JSON.stringify(Data);

    return this.http.post(url, body,{'headers':headers})

  }

}



// import { HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { Observable, Subject, Subscription, finalize } from 'rxjs';
// import { SignalrService } from './signalr.service';
// import { Equities } from '../models/equities.model';

// @Injectable({
//     providedIn: 'root'
// })
// export class StocksService {
//     private stocksUrl: string = 'assets/data.json';
//     private immutableData!: Stock[];
//     private immutableDataEquties!: Equities[];
//     public previousData: Stock[] = [];
//     private signalRSubscription: Subscription | undefined;
//     public updateFreq: number = 2000;

//     constructor(private http: HttpClient,public signalRService: SignalrService) {
//       // this.signalRService.connection
//       // .invoke('GetAllStocks')
//       // .catch((error: any) => {
//       //   console.log(`SGetAllStocks error: ${error}`);
//       //   alert('GetAllStocks error!, see console for details.');
//       // });
//       //this.getAllEmpsStream();

//     }
//     public getAllEmpsStream(): Observable<Equities[]>{

//       const subject = new Subject<Equities[]>();
//       this.signalRService.connection.on("SendLiveData",(data :any) => {
//       this.immutableDataEquties=data;

//       })
//       debugger;
//       return subject.asObservable();

//     }

//     // public getAllEmpsStream(): Observable<Equities[]>{

//     //   const subject = new Subject<Equities[]>();
//     //   this.signalRService.connection.on('SendLiveData', (data: any) => {

//     //   //  subject=data;
//     //           });
//     //   // const subject = new Subject<Equities[]>();
//     //   // this.signalRService.connection.on("SendLiveData").then(subject);

//     //   // return new Observable<Equities[]>((observer) => {subject});
//     //  // return new Observable<Equities[]>(subject)

//     //   // subject.pipe()
//     //   // .subscribe(it  => {

//     //   //   //window.alert(it.length);
//     //   //   this.immutableDataEquties.push(...it);

//     //   // });

//     //   // subject.asObservable().pipe(it  => {

//     //   //   //window.alert(it.length);
//     //   //   this.emp.push(...it);

//     //   // });
//     //   // this.immutableDataEquties = subject.asObservable().pipe(finalize(() => this.immutableData.push()))
//     //   // .subscribe(it  => {

//     //   //   //window.alert(it.length);
//     //   //   this.emp.push(...it);

//     //   // }); ;
//     //   // return subject.asObservable();

//     // }


//     // public loadliveData = () => {

//     //   this.signalRService.connection.on('SendLiveData', (data: any) => {

//     //           console.log(data);
//     //         });


//     //        }


//     //       getDataObservableEquties(): Observable<Equities[]> {
//     //   debugger;



//     //   return new Observable<Equities[]>((observer) => {

//     //     this.signalRService.connection.on('')
//     //     // this.signalRSubscription = this.signalRService.getEquities().subscribe(
//     //     //   (data) => {
//     //     //     this.immutableDataEquties = data ;
//     //     // });
//     //       // this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {


//     //       //     this.immutableData = data ;
//     //       //     this.previousData = data;
//     //       //     observer.next(this.immutableData);

//     //       //     // setInterval(() => {
//     //       //     //     this.immutableData = this.immutableData.map((row: Stock) => this.updateRandomRowWithData(row));

//     //       //     //     observer.next(this.immutableData);
//     //       //     // }, this.updateFreq);
//     //       // });
//     //   });
//     // }

//     // getDataObservable(): Observable<Stock[]> {
//     //     return new Observable<Stock[]>((observer) => {
//     //         this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {


//     //             this.immutableData = data ;
//     //             this.previousData = data;
//     //             observer.next(this.immutableData);

//     //             setInterval(() => {
//     //                 this.immutableData = this.immutableData.map((row: Stock) => this.updateRandomRowWithData(row));

//     //                 observer.next(this.immutableData);
//     //             }, this.updateFreq);
//     //         });
//     //     });
//     // }

//        getDataObservable(): Observable<Stock[]> {
//         return new Observable<Stock[]>((observer) => {
//             this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {


//                 this.immutableData = data ;
//                 this.previousData = data;
//                 observer.next(this.immutableData);

//                 setInterval(() => {
//                     this.immutableData = this.immutableData.map((row: Stock) => this.updateRandomRowWithData(row));

//                     observer.next(this.immutableData);
//                 }, this.updateFreq);
//             });
//         });
//     }



//         // getDataObservableEquties(): Observable<Equities[]> {
//     //   debugger;



//     //   // return new Observable<Equities[]>((observer) => {
//     //   //   this.signalRService.getEquities();
//     //   //   // this.signalRSubscription = this.signalRService.getEquities().subscribe(
//     //   //   //   (data) => {
//     //   //   //     this.immutableDataEquties = data ;
//     //   //   // });
//     //   //     // this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {


//     //   //     //     this.immutableData = data ;
//     //   //     //     this.previousData = data;
//     //   //     //     observer.next(this.immutableData);

//     //   //     //     // setInterval(() => {
//     //   //     //     //     this.immutableData = this.immutableData.map((row: Stock) => this.updateRandomRowWithData(row));

//     //   //     //     //     observer.next(this.immutableData);
//     //   //     //     // }, this.updateFreq);
//     //   //     // });
//     //   // });
//     // }

//   //  public async invokeGetEquties() {
//   //     await this.signalRService.connection.startConnection();
//   //   }
//     // getDataObservableEquties(): Observable<Equities[]> {
//     //   debugger;



//     //   // return new Observable<Equities[]>((observer) => {
//     //   //   this.signalRService.getEquities();
//     //   //   // this.signalRSubscription = this.signalRService.getEquities().subscribe(
//     //   //   //   (data) => {
//     //   //   //     this.immutableDataEquties = data ;
//     //   //   // });
//     //   //     // this.http.get<Stock[]>(this.stocksUrl).subscribe((data: Stock[]) => {


//     //   //     //     this.immutableData = data ;
//     //   //     //     this.previousData = data;
//     //   //     //     observer.next(this.immutableData);

//     //   //     //     // setInterval(() => {
//     //   //     //     //     this.immutableData = this.immutableData.map((row: Stock) => this.updateRandomRowWithData(row));

//     //   //     //     //     observer.next(this.immutableData);
//     //   //     //     // }, this.updateFreq);
//     //   //     // });
//     //   // });
//     // }

//     updateRandomRowWithData(row: Stock): Stock {
//         const shouldUpdateData = Math.random() < 1000;

//         if (shouldUpdateData) {
//             let changePrice = Math.floor(30 * Math.random()) / 10;
//             changePrice *= Math.round(Math.random()) ? 2 : -0.09;

//             let changePercentage = Math.floor(30 * Math.random()) / 10;
//             changePercentage *= Math.round(Math.random()) ? 1 : -1;

//             const percentageValue = row.change_24h + changePercentage;
//             const priceValue = row.currentPrice + changePrice;

//             let newRow = {
//                 ...row,
//                 change_24h: percentageValue,
//                 currentPrice: priceValue,




//             };
//            newRow.intraday.push(Math.round(Math.random()+1));

//             this.previousData = [...this.immutableData];
//             return newRow;
//         } else {
//           row.intraday.push(Math.round(Math.random()+1));
//             return row;
//         }
//     }
// }

export interface Stock {
    id: number;
    currency: string;
    symbol: string;
    volume: number;
    currentPrice: number;
    change_24h: number;
    //intraday: Observable<number[]>;
    intraday: number[];
    change_pct:number;

}

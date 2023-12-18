import { Injectable } from '@angular/core';
import * as signalR from "@microsoft/signalr"
import { Equities } from '../models/equities.model';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  private connection: signalR.HubConnection;
  // private listofEquties$: Subject<Equities[]> | any;
  // private hubConnection: signalR.HubConnection | any
  // private hubConnectionBuilder: signalR.HubConnection | any
  constructor() {

    this.connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7189/livefeedhub", {
      skipNegotiation: true,
      transport: signalR.HttpTransportType.WebSockets
      })
    .build();
    this.startConnection();
  }
  public startConnection = () => {
    this.connection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err: any) => console.log('Error while starting connection: ' + err))
  }

  // public  startConnectionBuilder() :signalR.HubConnectionBuilder  {
  //   this.hubConnectionBuilder = new signalR.HubConnectionBuilder()
  //                           //.withUrl('https://localhost:7189/livefeedhub')
  //                           .withUrl("https://localhost:7189/livefeedhub", {
  //   skipNegotiation: true,
  //   transport: signalR.HttpTransportType.WebSockets
  // });

  //                        return   this.hubConnectionBuilder;

  // }

  //   public async startConnection():Promise<string>  {
  //     this.hubConnection = new signalR.HubConnectionBuilder()
  //                             //.withUrl('https://localhost:7189/livefeedhub')
  //                             .withUrl("https://localhost:7189/livefeedhub", {
  //     skipNegotiation: true,
  //     transport: signalR.HttpTransportType.WebSockets
  //   })
  //      .build();
  //     this.hubConnection
  //       .start()
  //       .then(() => console.log('Connection started'))
  //       .catch((err: any) => console.log('Error while starting connection: ' + err))
  //       //await this.hubConnection.invoke("");
  //       return "";
  //   }


  //   public async getEquities() {
  //     debugger;
  //     const subject = new Subject<Equities[]>();
  //     //this.startConnection()
  //      this.hubConnection.invoke("GetAllStocks") //.subscribe(subject);
  //     // console.log(subject);
  //     //  return subject.asObservable();
  //     //return this.listofEquties$;
  //   }
  //   public addTransferChartDataListener = () => {

  //     this.hubConnection.on('SendLiveData', (data: any) => {

  //       console.log(data);
  //     });


  //   }


}


import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { BehaviorSubject } from 'rxjs';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  connection: any;
  hubHelloMessage: BehaviorSubject<string>;
  progressPercentage: BehaviorSubject<number>;
  progressMessage: BehaviorSubject<string>;

  constructor() {
    this.connection = null;
    this.hubHelloMessage = new BehaviorSubject<string>("");
    this.progressPercentage = new BehaviorSubject<number>(0);
    this.progressMessage = new BehaviorSubject<string>("");
  }

  // Establish a connection to the SignalR server hub
  public initiateSignalrConnection(): Promise<void> {
    return new Promise((resolve, reject) => {
      this.connection = new signalR.HubConnectionBuilder()
        .withUrl(environment.signalrHubUrl,{
          skipNegotiation: true,
         transport: signalR.HttpTransportType.WebSockets
        })
        .withAutomaticReconnect()
        .build();

      this.connection.serverTimeoutInMilliseconds = 3600000;

      this.setSignalrClientMethods();

      this.connection
        .start()
        .then(() => {

          console.log(`SignalR connection success! connectionId: ${this.connection.connectionId}`);
          resolve();
        })
        .catch((error: any) => {
          console.log(`SignalR connection error: ${error}`);
          reject();
        });
    });
  }

  // This method will implement the methods defined in the ISignalrDemoHub interface in the SignalrDemo.Server .NET solution
  private setSignalrClientMethods(): void {
    this.connection.on('DisplayMessage', (message: string) => {
      this.hubHelloMessage.next(message);
    });

    this.connection.on('UpdateProgressBar', (percentage: number) => {
      this.progressPercentage.next(percentage);
    });

    this.connection.on('DisplayProgressMessage', (message: string) => {
      this.progressMessage.next(message);
    });
  }
}

// import { Injectable } from '@angular/core';
// import * as signalR from "@microsoft/signalr"
// import { Equities } from '../models/equities.model';
// import { Subject, Observable } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class SignalrService {
//   private connection: signalR.HubConnection;
//   // private listofEquties$: Subject<Equities[]> | any;
//   // private hubConnection: signalR.HubConnection | any
//   // private hubConnectionBuilder: signalR.HubConnection | any
//   constructor() {

//     this.connection = new signalR.HubConnectionBuilder()
//     .withUrl("https://localhost:7189/livefeedhub", {
//       skipNegotiation: true,
//       transport: signalR.HttpTransportType.WebSockets
//       })
//     .build();
//     this.startConnection();
//   }
//   public startConnection = () => {
//     this.connection
//       .start()
//       .then(() => console.log('Connection started'))
//       .catch((err: any) => console.log('Error while starting connection: ' + err))
//   }

//   // public  startConnectionBuilder() :signalR.HubConnectionBuilder  {
//   //   this.hubConnectionBuilder = new signalR.HubConnectionBuilder()
//   //                           //.withUrl('https://localhost:7189/livefeedhub')
//   //                           .withUrl("https://localhost:7189/livefeedhub", {
//   //   skipNegotiation: true,
//   //   transport: signalR.HttpTransportType.WebSockets
//   // });

//   //                        return   this.hubConnectionBuilder;

//   // }

//   //   public async startConnection():Promise<string>  {
//   //     this.hubConnection = new signalR.HubConnectionBuilder()
//   //                             //.withUrl('https://localhost:7189/livefeedhub')
//   //                             .withUrl("https://localhost:7189/livefeedhub", {
//   //     skipNegotiation: true,
//   //     transport: signalR.HttpTransportType.WebSockets
//   //   })
//   //      .build();
//   //     this.hubConnection
//   //       .start()
//   //       .then(() => console.log('Connection started'))
//   //       .catch((err: any) => console.log('Error while starting connection: ' + err))
//   //       //await this.hubConnection.invoke("");
//   //       return "";
//   //   }


//   //   public async getEquities() {
//   //     debugger;
//   //     const subject = new Subject<Equities[]>();
//   //     //this.startConnection()
//   //      this.hubConnection.invoke("GetAllStocks") //.subscribe(subject);
//   //     // console.log(subject);
//   //     //  return subject.asObservable();
//   //     //return this.listofEquties$;
//   //   }
//   //   public addTransferChartDataListener = () => {

//   //     this.hubConnection.on('SendLiveData', (data: any) => {

//   //       console.log(data);
//   //     });


//   //   }


// }

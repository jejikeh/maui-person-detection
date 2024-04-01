import { Injectable, inject } from '@angular/core';
import { ApiRoutesService } from './api-routes.service';
import * as signalR from '@microsoft/signalr';
import { Observable, Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  subject = new Subject<string>();
  hubConnection: signalR.HubConnection;
  api = inject(ApiRoutesService);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.api.VideoHub())
      .build();
  }

  startConnection(): Observable<void> {
    if (this.hubConnection.state === signalR.HubConnectionState.Connected) {
      return new Observable<void>((observer) => {
        observer.next();
        observer.complete();
      });
    }

    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          observer.error(error);
        });
    });
  }

  stopConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .stop()
        .then(() => {
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          observer.error(error);
        });
    });
  }

  receivePhoto(): Observable<string> {
    return new Observable<string>((observer) => {
      this.hubConnection.on('ProcessPhotoOutput', (data) => {
        observer.next('data:image/png;base64, ' + data);
      });
    });
  }

  createSubject<T>(name: string): signalR.Subject<T> {
    const subject = new signalR.Subject<T>();

    this.hubConnection.send(name, subject);

    return subject;
  }
}

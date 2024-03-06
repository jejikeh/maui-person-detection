import { Injectable, inject } from '@angular/core';
import { ApiRoutesService } from './api-routes.service';
import * as signalR from '@microsoft/signalr';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  hubConnection: signalR.HubConnection;
  api = inject(ApiRoutesService);

  constructor() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.api.VideoHub())
      .build();
  }

  startConnection(): Observable<void> {
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

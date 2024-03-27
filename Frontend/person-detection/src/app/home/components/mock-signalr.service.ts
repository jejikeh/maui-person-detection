import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

@Injectable()
export class MockSignalRService {
  subject = new Subject<string>();

  startConnection(): Observable<void> {
    return new Observable<void>();
  }

  createSubject<T>(eventName: string): signalR.Subject<T> {
    return this.subject as any;
  }

  stopConnection(): void {}

  receivePhoto(): Observable<string> {
    return this.subject.asObservable();
  }
}

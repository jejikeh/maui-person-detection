import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class ApiRoutesService {
  public Register(): string {
    return environment.api + 'register';
  }

  public Login(): string {
    return environment.api + 'login';
  }
}
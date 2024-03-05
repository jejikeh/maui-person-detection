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

  public User(): string {
    return environment.api + 'user';
  }

  public Logout(): string {
    return environment.api + 'logout';
  }

  public VideoHub(): string {
    return environment.api + 'video';
  }
}

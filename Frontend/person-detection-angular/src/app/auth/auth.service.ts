import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { User } from './common/models/user.model';
import { environment } from '../../environments/environment';
import { Claims } from './common/responses/claims.response';
import { ClaimNames } from './common/claim.names';
import { LoginRequest } from './common/requests/login.request';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public http: HttpClient = inject(HttpClient);

  public user: User = {
    id: '',
    email: '',
  };

  public register() {}

  public login(request: LoginRequest) {
    return this.http
      .post<any>(environment.backend + '/login', request, {
        withCredentials: true,
      })
      .subscribe((_) => {
        this.identify();
      });
  }

  public identify() {
    return this.http
      .get<any>(environment.backend + '/identify', {
        withCredentials: true,
      })
      .subscribe((data) => {
        let id = data[ClaimNames.Id];
        let email = data[ClaimNames.Email];
        this.user = new User(id, email);
        console.log(this.user);
      });
  }

  public logout() {
    return this.http
      .post(environment.backend + '/logout', {}, { withCredentials: true })
      .subscribe((data) => {
        console.log(data);
      });
  }
}

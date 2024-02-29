import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { User } from './common/models/user.model';
import { environment } from '../../environments/environment';
import { Claims } from './common/responses/claims.response';
import { ClaimNames } from './common/claim.names';
import { LoginRequest } from './common/requests/login.request';
import { RegisterRequest } from './common/requests/register.request';
import { catchError, map, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  public http: HttpClient = inject(HttpClient);

  public user: User = {
    id: '',
    userName: '',
  };

  public register(request: RegisterRequest) {
    return this.http
      .post<any>(environment.backend + '/register', request, {
        withCredentials: true,
      })
      .pipe(catchError(this.handleError))
      .subscribe((_) => {
        this.identify();
      });
  }

  public login(request: LoginRequest) {
    return this.http
      .post<any>(environment.backend + '/login', request, {
        withCredentials: true,
      })
      .pipe(catchError(this.handleError))
      .subscribe((_) => {
        this.identify();
      });
  }

  public identify() {
    return this.http
      .get<any>(environment.backend + '/identify', {
        withCredentials: true,
      })
      .pipe(catchError(this.handleError))
      .subscribe((data) => {
        console.log(data);
        let id = data[ClaimNames.Id];
        let userName = data[ClaimNames.UserName];
        this.user = new User(id, userName);
        console.log(this.user);
      });
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status === 0) {
      console.error('An error occurred:', error.error);
    } else {
      console.error(
        `Backend returned code ${error.status}, body was: `,
        error.error
      );
    }
    return throwError(() => {
      alert(error.error.detail);
      new Error('Something bad happened; please try again later.');
    });
  }

  public logout() {
    return this.http
      .post(environment.backend + '/logout', {}, { withCredentials: true })
      .pipe(catchError(this.handleError))
      .subscribe((data) => {
        this.identify();
      });
  }
}

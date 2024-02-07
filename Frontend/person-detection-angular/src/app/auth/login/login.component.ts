import { Component, Inject, inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { LoginRequest } from '../common/requests/login.request';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  template:
  `
  <section>
      <form>
        <label for="email">Email</label>
        <input id="email" type="text" name="email" [formControl]="email" />
        <label for="password">Password</label>
        <input
          id="password"
          type="password"
          name="password"
          [formControl]="password"
        />
      </form>
      <button (click)="login()">Login</button>
    </section>
  `,
  styles: ``
})
export class LoginComponent {
  private _authService: AuthService = inject(AuthService);

  public email: FormControl = new FormControl('');
  public password: FormControl = new FormControl('');

  public login() {
    return this._authService.login(new LoginRequest(this.email.value, this.password.value));
  }
}

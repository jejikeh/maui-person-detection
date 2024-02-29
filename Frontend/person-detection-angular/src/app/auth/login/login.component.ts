import { Component, Inject, inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { LoginRequest } from '../common/requests/login.request';
import { FormControl, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <section>
      <form>
        <label for="username">Username</label>
        <input
          id="username"
          type="text"
          name="username"
          [formControl]="userName"
        />
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
  styles: ``,
})
export class LoginComponent {
  private _authService: AuthService = inject(AuthService);

  public password: FormControl = new FormControl('');
  public userName: FormControl = new FormControl('');

  ngOnInit() {
    this._authService.identify().add(() => {
      if (this._authService.user.userName !== undefined) {
        location.replace('/');
      }
    });
  }

  public login() {
    return this._authService
      .login(new LoginRequest(this.userName.value, this.password.value))
      .add(() => {
        location.reload();
      });
  }
}

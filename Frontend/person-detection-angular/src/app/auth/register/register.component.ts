import { Component, inject } from '@angular/core';
import { AuthService } from '../auth.service';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { RegisterRequest } from '../common/requests/register.request';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <section>
      <form>
        <label for="email">Email</label>
        <input id="email" type="text" name="email" [formControl]="email" />
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
      <button (click)="register()">Login</button>
    </section>
  `,
  styles: ``,
})
export class RegisterComponent {
  private _authService: AuthService = inject(AuthService);

  public email: FormControl = new FormControl('');
  public password: FormControl = new FormControl('');
  public userName: FormControl = new FormControl('');

  ngOnInit() {
    this._authService.identify().add(() => {
      if (this._authService.user.userName !== undefined) {
        location.replace('/');
      }
    });
  }

  public register() {
    this._authService
      .register(
        new RegisterRequest(
          this.email.value,
          this.password.value,
          this.userName.value
        )
      )
      .add(() => {
        location.reload();
      });
  }
}

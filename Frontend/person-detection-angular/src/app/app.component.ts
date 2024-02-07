import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <section>
      <div class="navbar">
        <p>{{ email }}</p>
        <button (click)="logout()">Logout</button>
      </div>
      <div class="container">
        <router-outlet></router-outlet>
      </div>
    </section>
  `,
})
export class AppComponent {
  title = 'person-detection-angular';
  public email = '';

  private _authService: AuthService = inject(AuthService);

  ngOnInit() {
    this._authService.identify().add(() => {
      this.email = this._authService.user.email;
      console.log(this.email);
    });
  }

  public logout() {
    return this._authService.logout();
  }
}

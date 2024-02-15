import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  template: `
    <header>
      <nav>
        <ul>
          <li>
            <p>{{ userName }}</p>
          </li>
          <li><button (click)="logout()">Logout</button></li>
        </ul>
      </nav>
      <div class="container">
        <router-outlet></router-outlet>
      </div>
    </header>
  `,
})
export class AppComponent {
  title = 'person-detection-angular';
  public userName = '';

  private _authService: AuthService = inject(AuthService);

  ngOnInit() {
    this._authService.identify().add(() => {
      this.userName = this._authService.user.userName;
      console.log(this.userName);
    });
  }

  public logout() {
    this._authService.logout().add(() => {
      location.reload();
    });
  }
}

import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from './auth/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  template: `
    <header>
      <nav>
        <ul>
          <li>
            <a>{{ userName }}</a>
          </li>
          <li><a routerLink="stream">Stream</a></li>
          <li><a routerLink="home">Home</a></li>
          <li><a (click)="logout()">Logout</a></li>
        </ul>
      </nav>
    </header>
    <article>
      <router-outlet></router-outlet>
    </article>
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

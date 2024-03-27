import { Component, inject } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/services/auth.service';
import { NavbarComponent } from './components/navbar.component';
import { LoginCardComponent } from './auth/components/login-card.components';

@Component({
  selector: 'app-root',
  standalone: true,
  template: ` <navbar /><router-outlet></router-outlet> `,
  imports: [
    CommonModule,
    RouterOutlet,
    NavbarComponent,
    NgIf,
    LoginCardComponent,
  ],
})
export class AppComponent {
  auth = inject(AuthService);
}

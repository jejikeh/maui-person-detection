import { Component, inject } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgIf } from '@angular/common';
import { LoginCardComponent } from '../auth/components/login-card.components';

@Component({
  selector: 'app-home',
  standalone: true,
  template: `
    <div *ngIf="auth.currentUser() === null">
      <login-card />
    </div>
  `,
  styles: ``,
  imports: [NgIf, LoginCardComponent],
})
export class HomeComponent {
  auth = inject(AuthService);
}

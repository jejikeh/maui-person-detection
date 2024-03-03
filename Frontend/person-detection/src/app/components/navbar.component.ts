import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';

@Component({
  selector: 'navbar',
  standalone: true,
  imports: [NgIf, HlmButtonDirective],
  template: `
    <div
      class="flex items-center justify-center"
      *ngIf="auth.currentUser() === null"
    >
      <button
        hlmBtn
        variant="link"
        (click)="router.navigateByUrl('/login')"
        brnHoverCardTrigger
      >
        Login
      </button>

      <button
        hlmBtn
        variant="link"
        (click)="router.navigateByUrl('/register')"
        brnHoverCardTrigger
      >
        Register
      </button>
    </div>

    <div *ngIf="auth.currentUser() !== null">
      {{ auth.currentUser()?.userName }}
      <a routerLink="/logout">logout</a>,
    </div>
  `,
})
export class NavbarComponent {
  auth = inject(AuthService);
  router = inject(Router);
}

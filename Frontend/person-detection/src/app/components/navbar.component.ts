import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgIf } from '@angular/common';
import { Router } from '@angular/router';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HttpClient } from '@angular/common/http';
import { ApiRoutesService } from '../services/api-routes.service';
import { UserInterface } from '../auth/interfaces/user.interface';

@Component({
  selector: 'navbar',
  standalone: true,
  imports: [NgIf, HlmButtonDirective],
  template: `
    <div class="flex items-center justify-center">
      <div *ngIf="auth.currentUser() === null">
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
        <button class="ml-2" (click)="logout()" hlmBtn>Logout</button>
      </div>
    </div>
  `,
})
export class NavbarComponent {
  api = inject(ApiRoutesService);
  auth = inject(AuthService);
  http = inject(HttpClient);
  router = inject(Router);

  ngOnInit() {
    this.http
      .get<UserInterface>(this.api.User(), { withCredentials: true })
      .subscribe({
        next: (data) => {
          this.auth.currentUser.set(data);
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {},
      });
  }

  logout() {
    this.http.post(this.api.Logout(), {}, { withCredentials: true }).subscribe({
      next: () => {
        this.auth.currentUser.set(null);
        this.router.navigateByUrl('/login');
      },
      error: (error) => {
        console.log(error);
      },
      complete: () => {},
    });
  }
}

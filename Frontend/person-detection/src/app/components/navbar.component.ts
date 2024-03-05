import { Component, inject, signal } from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgIf } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { HlmButtonDirective } from '@spartan-ng/ui-button-helm';
import { HttpClient } from '@angular/common/http';
import { ApiRoutesService } from '../services/api-routes.service';
import { UserInterface } from '../auth/interfaces/user.interface';
import { HlmIconComponent } from '../../../components/ui-icon-helm/src/lib/hlm-icon.component';

@Component({
  selector: 'navbar',
  standalone: true,
  imports: [
    NgIf,
    HlmButtonDirective,
    HlmButtonDirective,
    RouterLink,
    HlmIconComponent,
  ],
  host: {
    class:
      'block sticky w-full top-0 z-40 bg-background/95 bg-blur-lg sm:px-4 border-b border-border',
  },
  template: `
    <div
      class="mx-auto flex w-full max-w-screen-xl items-center justify-between"
    >
      <nav class="flex items-center">
        <div *ngIf="auth.currentUser() !== null">
          <div class="hidden sm:flex sm:space-x-2">
            <a routerLink="/" hlmBtn variant="link">Home</a>
            <a routerLink="/Gallery" hlmBtn variant="link">Gallery</a>
          </div>
        </div>
      </nav>

      <div class="flex space-x-2" *ngIf="auth.currentUser() !== null">
        <a hlmBtn variant="link">{{ auth.currentUser()?.userName }}</a>
        <a routerLink="/" hlmBtn variant="link" (click)="logout()">Logout</a>
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

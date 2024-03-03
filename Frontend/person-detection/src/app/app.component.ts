import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AuthService } from './auth/services/auth.service';
import { NavbarComponent } from './components/navbar.component';

@Component({
  selector: 'app-root',
  standalone: true,
  host: {
    class: 'block p-10',
  },
  template: `
    <navbar />
    <router-outlet></router-outlet>
  `,
  imports: [CommonModule, RouterOutlet, NavbarComponent],
})
export class AppComponent {}

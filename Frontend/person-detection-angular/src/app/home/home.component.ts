import { Component, inject } from '@angular/core';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  template: `
    <p>
      home works!
    </p>
  `,
  styles: ``
})
export class HomeComponent {
}
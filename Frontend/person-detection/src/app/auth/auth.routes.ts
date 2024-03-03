import { Routes } from '@angular/router';
import { RegisterCardComponent } from './components/register-card.components';
import { LoginCardComponent } from './components/login-card.components';

export const auth_routes: Routes = [
  {
    path: 'register',
    component: RegisterCardComponent,
  },
  {
    path: 'login',
    component: LoginCardComponent,
  },
];

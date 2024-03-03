import { Routes } from '@angular/router';
import { auth_routes } from './auth/auth.routes';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  ...auth_routes,
];

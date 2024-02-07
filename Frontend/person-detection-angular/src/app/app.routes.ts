import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authRoutes } from './auth/auth.routes';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },
  {
    path: 'home',
    component: HomeComponent
  },
  ...authRoutes
];

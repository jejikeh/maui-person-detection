import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authRoutes } from './auth/auth.routes';
import { StreamComponent } from './stream/stream.component';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: 'stream',
    component: StreamComponent,
  },
  ...authRoutes,
];

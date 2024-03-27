import { Routes } from '@angular/router';
import { auth_routes } from './auth/auth.routes';
import { HomeComponent } from './home/home.component';
import { GalleryComponent } from './gallery/gallery.component';

export const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
  },
  {
    path: 'gallery',
    component: GalleryComponent,
  },
  ...auth_routes,
];

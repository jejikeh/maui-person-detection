import {
  Component,
  ElementRef,
  ViewChild,
  effect,
  inject,
  signal,
} from '@angular/core';
import { AuthService } from '../auth/services/auth.service';
import { NgFor, NgIf, NgSwitch, NgSwitchCase } from '@angular/common';
import { LoginCardComponent } from '../auth/components/login-card.components';
import {
  hlmH1,
  hlmH2,
  hlmH3,
  hlmLead,
  hlmP,
} from '@spartan-ng/ui-typography-helm';
import { HlmAspectRatioDirective } from '@spartan-ng/ui-aspectratio-helm';
import { HlmSeparatorDirective } from '@spartan-ng/ui-separator-helm';
import { ModelType } from '../common/models.types';
import { HttpClient } from '@angular/common/http';
import { ApiRoutesService } from '../services/api-routes.service';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { DropdownPhotoMenuComponent } from './ui/dropdown-photo-menu.component';
import { GalleryPhotoInterface } from '../auth/interfaces/gallery-photo.interface';

@Component({
  selector: 'gallery',
  standalone: true,
  imports: [
    NgIf,
    LoginCardComponent,
    HlmAspectRatioDirective,
    HlmSeparatorDirective,
    NgSwitchCase,
    NgFor,
    NgSwitch,
    InfiniteScrollModule,
    DropdownPhotoMenuComponent,
  ],
  host: {
    class: 'block p-10 mb-10',
  },
  template: `
    <div *ngIf="auth.currentUser() === null">
      <login-card />
    </div>

    <div *ngIf="auth.currentUser() !== null" class="w-full">
      <h1 class="${hlmH1}">
        Gallery of {{ auth.currentUser()?.userName }} photos.
      </h1>
      <p class="${hlmLead} mt-4">Here you can see your saved photos.</p>
      <brn-separator hlmSeparator />
    </div>

    <div class="w-full" style="padding-bottom: 320px">
      <div
        class="grid grid-cols-1 gap-4 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4"
        infiniteScroll
        [infiniteScrollDistance]="2"
        [infiniteScrollThrottle]="50"
        (scrolled)="onScroll()"
      >
        <ng-container *ngFor="let photo of photos">
          <!-- <img class="rounded shadow" [src]="photo" alt="photo" /> -->
          <dropdown-photo-menu [photo]="photo.content" [index]="photo.id" />
        </ng-container>
      </div>
    </div>
  `,
})
export class GalleryComponent {
  auth = inject(AuthService);
  api = inject(ApiRoutesService);
  http = inject(HttpClient);

  page = 0;
  allPhotosLoaded = false;
  photos: GalleryPhotoInterface[] = [];

  loadMorePhotos() {
    if (this.allPhotosLoaded) {
      return;
    }

    this.http
      .get<GalleryPhotoInterface[]>(this.api.GetGallery(this.page), {
        withCredentials: true,
      })
      .subscribe({
        next: (data) => {
          if (data.length === 0) {
            this.allPhotosLoaded = true;
          } else {
            for (let photo of data) {
              photo.content = 'data:image/jpeg;base64,' + photo.content;
              this.photos.push(photo);
            }
            this.page++;
          }
        },
        error: (error) => {
          console.log(error);
        },
        complete: () => {},
      });
  }

  onScroll() {
    console.log('scrolled');
    this.loadMorePhotos();
  }

  ngOnInit() {
    this.loadMorePhotos();
  }
}

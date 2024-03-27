import {
  TestBed,
  ComponentFixture,
  fakeAsync,
  tick,
  waitForAsync,
} from '@angular/core/testing';
import { GalleryComponent } from './gallery.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core'; // Import NO_ERRORS_SCHEMA
import { AuthService } from '../auth/services/auth.service';
import { of } from 'rxjs';
import { ApiRoutesService } from '../services/api-routes.service';
import { HttpClient } from '@angular/common/http';

describe('GalleryComponent', () => {
  let component: GalleryComponent;
  let fixture: ComponentFixture<GalleryComponent>;
  let authService: AuthService;
  let httpClient: HttpClient;
  let apiRoutesService: ApiRoutesService;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      schemas: [NO_ERRORS_SCHEMA],
      providers: [AuthService, ApiRoutesService],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GalleryComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    httpClient = TestBed.inject(HttpClient);
    apiRoutesService = TestBed.inject(ApiRoutesService);
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load more photos on scroll', fakeAsync(() => {
    const mockPhotos = [
      { id: 1, content: 'MockBase64String1' },
      { id: 2, content: 'MockBase64String2' },
    ];

    spyOn(httpClient, 'get').and.returnValue(of(mockPhotos));

    component.onScroll();
    tick(100);

    expect(component.photos.length).toBe(2);
    expect(component.page).toBe(1);
  }));

  it('should stop loading more photos if all photos are loaded', fakeAsync(() => {
    component.allPhotosLoaded = true;

    spyOn(httpClient, 'get').and.returnValue(of([]));

    component.onScroll();
    tick(100);

    expect(component.photos.length).toBe(0);
    expect(component.page).toBe(0);
  }));

  it('should load more photos on initialization', fakeAsync(() => {
    const mockPhotos = [
      { id: 1, content: 'MockBase64String1' },
      { id: 2, content: 'MockBase64String2' },
    ];

    spyOn(httpClient, 'get').and.returnValue(of(mockPhotos));

    component.ngOnInit();
    tick(100);

    expect(component.photos.length).toBe(2);
    expect(component.page).toBe(1);
  }));
});

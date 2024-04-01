import { TestBed, ComponentFixture, waitForAsync } from '@angular/core/testing';
import { NavbarComponent } from './navbar.component';
import { RouterTestingModule } from '@angular/router/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { AuthService } from '../auth/services/auth.service';
import { ApiRoutesService } from '../services/api-routes.service';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs';

describe('NavbarComponent', () => {
  let component: NavbarComponent;
  let fixture: ComponentFixture<NavbarComponent>;
  let authService: AuthService;
  let httpClient: HttpClient;
  let apiRoutesService: ApiRoutesService;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, HttpClientTestingModule],
      declarations: [],
      providers: [AuthService, ApiRoutesService],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NavbarComponent);
    component = fixture.componentInstance;
    authService = TestBed.inject(AuthService);
    httpClient = TestBed.inject(HttpClient);
    apiRoutesService = TestBed.inject(ApiRoutesService);

    spyOn(httpClient, 'get').and.returnValue(of({ userName: 'testUser' }));

    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch user data on initialization', () => {
    expect(httpClient.get).toHaveBeenCalledWith(apiRoutesService.User(), {
      withCredentials: true,
    });
    expect(authService.currentUser()?.userName).toEqual('testUser');
  });
});

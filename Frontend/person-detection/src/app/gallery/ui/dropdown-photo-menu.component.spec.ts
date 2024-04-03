import { TestBed, ComponentFixture } from '@angular/core/testing';
import { DropdownPhotoMenuComponent } from './dropdown-photo-menu.component';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { HttpClient } from '@angular/common/http';
import { ApiRoutesService } from '../../services/api-routes.service';
import { of } from 'rxjs';

describe('DropdownPhotoMenuComponent', () => {
  let component: DropdownPhotoMenuComponent;
  let fixture: ComponentFixture<DropdownPhotoMenuComponent>;
  let httpClient: HttpClient;
  let apiRoutesService: ApiRoutesService;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ApiRoutesService],
    }).compileComponents();

    httpClient = TestBed.inject(HttpClient);
    apiRoutesService = TestBed.inject(ApiRoutesService);
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DropdownPhotoMenuComponent);
    component = fixture.componentInstance;
    component.photo = 'data:image/jpeg;base64,PHOTO_BASE64_STRING';
    component.index = 1;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should delete the photo when delete method is called', () => {
    spyOn(httpClient, 'delete').and.returnValue(of('success'));
    component.delete();

    expect(component.photo).toEqual('');
    expect(httpClient.delete).toHaveBeenCalledWith(
      apiRoutesService.DeletePhoto(1),
      {
        withCredentials: true,
      }
    );
  });
});

import { TestBed, inject, fakeAsync, tick } from '@angular/core/testing';
import { SignalRService } from './signal-r.service';
import { ApiRoutesService } from './api-routes.service';
import { HttpClientTestingModule } from '@angular/common/http/testing';

describe('SignalRService', () => {
  let service: SignalRService;
  let apiRoutesService: ApiRoutesService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SignalRService, ApiRoutesService],
    });
    service = TestBed.inject(SignalRService);
    apiRoutesService = TestBed.inject(ApiRoutesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should start connection successfully', fakeAsync(() => {
    spyOn(service.hubConnection, 'start').and.returnValue(Promise.resolve());

    const startConnectionObs = service.startConnection();
    tick();

    startConnectionObs.subscribe(() => {
      expect(service.hubConnection.start).toHaveBeenCalled();
    });
  }));

  it('should stop connection successfully', fakeAsync(() => {
    spyOn(service.hubConnection, 'stop').and.returnValue(Promise.resolve());

    const stopConnectionObs = service.stopConnection();
    tick();

    stopConnectionObs.subscribe(() => {
      expect(service.hubConnection.stop).toHaveBeenCalled();
    });
  }));
});

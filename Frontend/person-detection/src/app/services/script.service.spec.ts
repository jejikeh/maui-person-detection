import { TestBed } from '@angular/core/testing';
import { ScriptService } from './script.service';

describe('ScriptService', () => {
  let service: ScriptService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ScriptService],
    });
    service = TestBed.inject(ScriptService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});

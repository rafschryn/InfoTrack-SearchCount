import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { SearchCountHistoryService } from './search-count-history.service';
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ErrorHandler } from '../interceptor/error-handler';

class MockErrorHandler {
  handleError(err: HttpErrorResponse) {
    throwError(() => new Error ("error"));
  }
}

describe('SearchCountHistoryService', () => {
  let service: SearchCountHistoryService;
  let httpTestingController: HttpTestingController;
  let mockErrorHandler: MockErrorHandler;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [SearchCountHistoryService, 
        { provide: ErrorHandler, useClass: MockErrorHandler },
      ]
    });

    service = TestBed.inject(SearchCountHistoryService);
    httpTestingController = TestBed.inject(HttpTestingController);
    mockErrorHandler = TestBed.inject(ErrorHandler);
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should retrieve all search count history successfully', () => {
    const mockHistoryData = [{}];

    service.getAllSearchCountHistory().subscribe(data => {
      expect(data).toEqual(mockHistoryData);
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/history/all`);
    expect(req.request.method).toBe('GET');
    req.flush(mockHistoryData);
  });

  it('should handle errors using the errorHandler', () => {
    const errorResponse = new HttpErrorResponse({
      status: 404, statusText: 'Not Found'
    });
    spyOn(mockErrorHandler, 'handleError').and.returnValue();

    service.getAllSearchCountHistory().subscribe({
      next: () => fail('should have failed with 404 error'),
      error: (error) => {
        expect(mockErrorHandler.handleError).toHaveBeenCalled();
      }
    });

    const req = httpTestingController.expectOne(`${environment.apiUrl}/history/all`);
    req.flush('Something went wrong', errorResponse);
  });
});
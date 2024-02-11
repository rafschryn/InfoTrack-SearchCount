import { TestBed } from '@angular/core/testing';
import { ToastrService } from 'ngx-toastr';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorHandler } from './error-handler';

describe('ErrorHandler', () => {
  let service: ErrorHandler;
  let toastrServiceSpy: jasmine.SpyObj<ToastrService>;

  beforeEach(() => {
    const spy = jasmine.createSpyObj('ToastrService', ['error']);

    TestBed.configureTestingModule({
      providers: [
        ErrorHandler,
        { provide: ToastrService, useValue: spy }
      ]
    });
    service = TestBed.inject(ErrorHandler);
    toastrServiceSpy = TestBed.inject(ToastrService) as jasmine.SpyObj<ToastrService>;

    spyOn(service, 'handleError').and.callThrough();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return error for client side error', () => {
    const clientError = new HttpErrorResponse({
      error: new ErrorEvent('ClientError'),
      status: 0
    });

    service.handleError(clientError).subscribe({
      error: (error: any) => {
        expect(error).toBeTruthy();
        expect(error.message).toContain('Oops, something happened');
      }
    });
  });

  it('should call toaster for client side error', () => {
    const clientError = new HttpErrorResponse({
      error: new ErrorEvent('ClientError'),
      status: 0
    });

    service.handleError(clientError).subscribe({
      error: () => {
        expect(toastrServiceSpy.error).toHaveBeenCalledWith('Oops, something happened', 'Error', { closeButton: true });
      }
    });
  });

  it('should return error for API error', () => {
    const apiError = new HttpErrorResponse({
      error: 'API Error',
      status: 404
    });

    service.handleError(apiError).subscribe({
      error: (error: any) => {
        expect(error).toBeTruthy();
        expect(error.message).toContain('API error, code: 404, body: Oops, something happened');
      }
    });
  });

  it('should call toaster for API error', () => {
    const apiError = new HttpErrorResponse({
      error: 'API Error',
      status: 404
    });

    service.handleError(apiError).subscribe({
      error: () => {
        expect(toastrServiceSpy.error).toHaveBeenCalledWith('Oops, something happened', 'Error', { closeButton: true });
      }
    });
  });
});
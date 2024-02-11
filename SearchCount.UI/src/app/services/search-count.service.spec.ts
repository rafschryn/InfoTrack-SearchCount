import { HttpClientTestingModule, HttpTestingController } from "@angular/common/http/testing";
import { SearchCountService } from "./search-count.service";
import { TestBed } from "@angular/core/testing";
import { environment } from "../../environments/environment.development";
import { HttpErrorResponse } from "@angular/common/http";
import { SearchEngine } from "../models/search-engine";
import { ToastrModule } from "ngx-toastr";
import { ErrorHandler } from "../interceptor/error-handler";
import { throwError } from "rxjs";

class MockErrorHandler {
    handleError(err: HttpErrorResponse) {
      throwError(() => new Error ("error"));
    }
  }

describe('SearchCountService', () => {
    let service: SearchCountService;
    let httpTestingController: HttpTestingController;
  let mockErrorHandler: MockErrorHandler;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [HttpClientTestingModule, ToastrModule],
            providers: [SearchCountService,
                { provide: ErrorHandler, useClass: MockErrorHandler },
            ]
        });

        service = TestBed.inject(SearchCountService);
        httpTestingController = TestBed.inject(HttpTestingController);
        mockErrorHandler = TestBed.inject(ErrorHandler);
    });

    afterEach(() => {
        httpTestingController.verify();
    });

    it('should send a post request and return expected data', () => {
        const mockRequestData = { searchTerm: 'example', url: 'https://example.com', searchEngine: SearchEngine.Google };
        const mockResponseData = { count: 100 };

        service.getSearchCount(mockRequestData).subscribe(data => {
            expect(data).toEqual(mockResponseData);
        });

        const req = httpTestingController.expectOne(`${environment.apiUrl}/search/count`);
        expect(req.request.method).toEqual('POST');
        expect(req.request.body).toEqual(mockRequestData);
        req.flush(mockResponseData);
    });

    it('should handle HTTP errors through errorHandler', () => {
        const mockRequestData = { searchTerm: 'example', url: 'https://example.com', searchEngine: SearchEngine.Google };
        const errorResponse = new HttpErrorResponse({
            status: 500, statusText: 'Internal Server Error'
        });

        spyOn(service['errorHandler'], 'handleError').and.callThrough();

        service.getSearchCount(mockRequestData).subscribe({
            next: () => fail('Expected an error, not search counts'),
            error: error => {
                expect(service['errorHandler'].handleError).toHaveBeenCalled();
            }
        });

        const req = httpTestingController.expectOne(`${environment.apiUrl}/search/count`);
        req.flush('Something went wrong', errorResponse);
    });
});

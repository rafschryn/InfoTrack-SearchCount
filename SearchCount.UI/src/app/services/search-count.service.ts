import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { SearchCountRequest } from '../models/search-count-request';
import { ErrorHandler } from '../interceptor/error-handler';

@Injectable({
  providedIn: 'root'
})
export class SearchCountService {
  errorHandler = inject(ErrorHandler);

  constructor(private httpClient: HttpClient) { }

  getSearchCount(searchCountRequest: SearchCountRequest) : Observable<any> {

    return this.httpClient.post(`${environment.apiUrl}/search/count`, searchCountRequest).pipe(
      catchError((response: HttpErrorResponse) => {
        return this.errorHandler.handleError(response);
      })
    );
  }
}
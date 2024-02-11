import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { ErrorHandler } from '../interceptor/error-handler';

@Injectable({
  providedIn: 'root'
})
export class SearchCountHistoryService {
  errorHandler = inject(ErrorHandler);

  constructor(private httpClient: HttpClient) { }

  getAllSearchCountHistory() : Observable<any> {

    return this.httpClient.get(`${environment.apiUrl}/history/all`).pipe(
      catchError((response: HttpErrorResponse) => {
        return this.errorHandler.handleError(response);
      })
    );
  }
}
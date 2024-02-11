import { inject, Injectable } from "@angular/core";
import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from "rxjs";
import { ToastrService } from "ngx-toastr";

@Injectable({ providedIn: 'root' })
export class ErrorHandler {
  toastr = inject(ToastrService);

  handleError(error: HttpErrorResponse) {
    let errorMessage = '';

    if(error.status === 0) {
      console.error("Client side error: ", error.error);
    } else {
      console.error(`API error, code: ${error.status}, body: `, error.error);
      errorMessage = `API error, code: ${error.status}, body: `, error.error;
    }

    errorMessage += 'Oops, something happened';
    this.toastr.error("Oops, something happened", "Error", {closeButton: true});

    return throwError(() => new Error (errorMessage));
  }
}
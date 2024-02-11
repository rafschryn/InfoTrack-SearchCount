import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { SearchCountComponent } from './search-count.component';
import { SearchCountService } from '../services/search-count.service';
import { ErrorHandler } from '../interceptor/error-handler';
import { SearchCountRequest } from '../models/search-count-request';
import { SearchEngine } from '../models/search-engine';

class MockSearchCountService {
  getSearchCount(searchCountRequest: SearchCountRequest) {
    return of({});
  }
}

class MockErrorHandler {
  handleError() {}
}

describe('SearchCountComponent', () => {
  let component: SearchCountComponent;
  let fixture: ComponentFixture<SearchCountComponent>;
  let mockSearchCountService: MockSearchCountService;
  let mockErrorHandler: MockErrorHandler;
  let toastrService: ToastrService;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchCountComponent ],
      imports: [ ReactiveFormsModule ],
      providers: [
        { provide: SearchCountService, useClass: MockSearchCountService },
        { provide: ErrorHandler, useClass: MockErrorHandler },
        { provide: ToastrService, useValue: jasmine.createSpyObj('ToastrService', ['success', 'error']) },
        FormBuilder
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SearchCountComponent);
    component = fixture.componentInstance;
    mockSearchCountService = TestBed.inject(SearchCountService);
    mockErrorHandler = TestBed.inject(ErrorHandler) as any;
    toastrService = TestBed.inject(ToastrService);

    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should handle form invalid when empty', () => {
    expect(component.searchForm.valid).toBeFalsy();
  });

  it('should handle searchTerm field validity', () => {
    let searchTerm = component.searchForm.controls['searchTerm'];
    expect(searchTerm.valid).toBeFalsy();

    let errors = searchTerm.errors || {};
    expect(errors['required']).toBeTruthy();

    searchTerm.setValue("a".repeat(151));
    errors = searchTerm.errors || {};
    expect(errors['maxlength']).toBeTruthy();
  });

  it('should handle url field validity', () => {
    let url = component.searchForm.controls['url'];
    expect(url.valid).toBeFalsy();

    let errors = url.errors || {};
    expect(errors['required']).toBeTruthy();

    url.setValue("a".repeat(3));
    errors = url.errors || {};
    expect(errors['pattern']).toBeTruthy();
  });

  it('submitting a form fetch the result', waitForAsync(() => {
    expect(component.searchForm.valid).toBeFalsy();
    component.searchForm.controls['searchTerm'].setValue("test");
    component.searchForm.controls['url'].setValue("http://example.com");
    component.searchForm.controls['searchEngine'].setValue(SearchEngine.Google);

    expect(component.searchForm.valid).toBeTruthy();

    spyOn(mockSearchCountService, 'getSearchCount').and.returnValue(of({
      indices: [1,2] }));

    component.onSubmit();

    fixture.whenStable().then(() => {
      component.result = { indices: [1,2] }
    });
  }));

  it('submitting a form show toaster', waitForAsync(() => {
    expect(component.searchForm.valid).toBeFalsy();
    component.searchForm.controls['searchTerm'].setValue("test");
    component.searchForm.controls['url'].setValue("http://example.com");
    component.searchForm.controls['searchEngine'].setValue(SearchEngine.Google);
    expect(component.searchForm.valid).toBeTruthy();

    spyOn(mockSearchCountService, 'getSearchCount').and.returnValue(of({}));

    component.onSubmit();

    fixture.whenStable().then(() => {
      expect(toastrService.success).toHaveBeenCalled();
    });
  }));

  it('submitting a form, error, show toaster', waitForAsync(() => {
    expect(component.searchForm.valid).toBeFalsy();
    component.searchForm.controls['searchTerm'].setValue("test");
    component.searchForm.controls['url'].setValue("http://example.com");
    component.searchForm.controls['searchEngine'].setValue(SearchEngine.Google);
    expect(component.searchForm.valid).toBeTruthy();

    spyOn(mockSearchCountService, 'getSearchCount').and.returnValue(throwError(() => new Error()));

    component.onSubmit();

    fixture.whenStable().then(() => {
      expect(toastrService.error).toHaveBeenCalled();
    });
  }));
});
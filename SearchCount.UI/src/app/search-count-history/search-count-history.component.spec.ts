import { ToastrService } from 'ngx-toastr';
import { SearchCountHistoryService } from '../services/search-count-history.service';
import { SearchCountHistoryComponent } from './search-count-history.component';
import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { of, throwError } from 'rxjs';
import { SearchCountHistory } from '../models/search-count-history';
import { SearchEngine } from '../models/search-engine';
import { NgbDate, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms';

class MockSearchCountHistoryService {
  getAllSearchCountHistory() {
    return of([{ id: 'id', searchEngine: SearchEngine.Google, url: 'https://www.example.com', searchTerm: 'test', indices: [1,2], dateOfExcecution: new Date() }]);
  }
}

describe('SearchCountHistoryComponent', () => {
  let component: SearchCountHistoryComponent;
  let fixture: ComponentFixture<SearchCountHistoryComponent>;
  let mockSearchCountHistoryService: MockSearchCountHistoryService;
  let toastrService: jasmine.SpyObj<ToastrService>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SearchCountHistoryComponent ],
      providers: [
        { provide: SearchCountHistoryService, useClass: MockSearchCountHistoryService },
        { provide: ToastrService, useValue: jasmine.createSpyObj('ToastrService', ['error']) }
      ],
      imports: [NgbModule, FormsModule],

    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SearchCountHistoryComponent);
    component = fixture.componentInstance;
    mockSearchCountHistoryService = TestBed.inject(SearchCountHistoryService) as any;
    toastrService = TestBed.inject(ToastrService) as any;

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call getAllSearchCountHistory on init', () => {
    spyOn(component, 'getAllSearchCountHistory');
    component.ngOnInit();
    expect(component.getAllSearchCountHistory).toHaveBeenCalled();
  });

  it('should set loading to false and call filter after successful fetch', waitForAsync(() => {
    const testData: SearchCountHistory[] = [{ id: 'id', searchEngine: SearchEngine.Google, url: 'https://www.example.com', searchTerm: 'test', indices: [1,2], dateOfExcecution: new Date() }];
    spyOn(mockSearchCountHistoryService, 'getAllSearchCountHistory').and.returnValue(of(testData));
    spyOn(component, 'filter').and.callThrough();

    component.getAllSearchCountHistory();

    fixture.whenStable().then(() => {
      expect(component.loading).toBeFalse();
      expect(component.searchCountHistories).toEqual(testData);
      expect(component.filteredSearchCountHistories).toEqual(testData);
      expect(component.filter).toHaveBeenCalled();
    });
  }));

  it('should handle error when fetching search count history fails', waitForAsync(() => {
    spyOn(mockSearchCountHistoryService, 'getAllSearchCountHistory').and.returnValue(throwError(() => new Error()));
    component.getAllSearchCountHistory();

    fixture.whenStable().then(() => {
      expect(component.loading).toBeFalse();
      expect(toastrService.error).toHaveBeenCalledWith('Something went wrong while fetching the history', 'Error', { closeButton: true });
    });
  }));

  describe('filter - ', () => {
    beforeEach(() => {
      component.dateRangeSelected = {
        startDate: { year: 2000, month: 1, day: 1 },
        endDate: { year: 2030, month: 1, day: 1 }
      };
      component.searchEngineSelected = null;
      component.urlSelected = null;
      component.searchTermSelected = null;

      component.searchCountHistories = [
        { id: 'id', indices: [1], searchEngine: SearchEngine.Google, url: 'https://example.com', searchTerm: 'test1', dateOfExcecution: new Date('2023-01-01') },
        { id: 'id', indices: [1],searchEngine: SearchEngine.Bing, url: 'https://example.org', searchTerm: 'test2', dateOfExcecution: new Date('2023-01-02') },
      ];
    });
  
    it('should return all histories when no filters are applied', () => {
      component.filter();
      expect(component.filteredSearchCountHistories.length).toBe(component.searchCountHistories.length);
    });
  
    it('should filter by search engine', () => {
      component.searchEngineSelected = SearchEngine.Google;
      component.filter();
      expect(component.filteredSearchCountHistories.every(h => h.searchEngine === 'Google')).toBeTrue();
      expect(component.filteredSearchCountHistories.length).toBe(1); 
    });

    it('should filter by URL substring', () => {
      component.urlSelected = 'example.com';

      component.filter();
      expect(component.filteredSearchCountHistories.every(h => h.url.includes('example.com'))).toBeTrue();
      expect(component.filteredSearchCountHistories.length).toBe(1);
    });
  
    it('should filter by search term substring', () => {
      component.searchTermSelected = 'test1';
      component.filter();
      expect(component.filteredSearchCountHistories.every(h => h.searchTerm.includes('test1'))).toBeTrue();
      expect(component.filteredSearchCountHistories.length).toBe(1);
    });
  
    it('should filter by date range', () => {
      component.dateRangeSelected = {
        startDate: { year: 2023, month: 1, day: 1 },
        endDate: { year: 2023, month: 1, day: 1 }
      };
      component.filter();

      expect(component.filteredSearchCountHistories.length).toBeGreaterThan(0);
      expect(component.filteredSearchCountHistories.every(h => 
        new Date(h.dateOfExcecution) >= new Date(2023, 0, 1) &&
        new Date(h.dateOfExcecution) <= new Date(2023, 0, 2)
      )).toBeTrue();
    });
  });
});
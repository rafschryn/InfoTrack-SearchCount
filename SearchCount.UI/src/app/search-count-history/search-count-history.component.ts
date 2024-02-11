import { Component, inject, OnInit } from '@angular/core';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandler } from '../interceptor/error-handler';
import { SearchCountHistory } from '../models/search-count-history';
import { SearchEngine } from '../models/search-engine';
import { SearchCountHistoryService } from '../services/search-count-history.service';

@Component({
  selector: 'app-search-count-history',
  templateUrl: './search-count-history.component.html',
  styleUrls: ['../app.component.css'],
})

export class SearchCountHistoryComponent implements OnInit {
  title = 'Search Count History';
  searchCountHistories: SearchCountHistory[] = [];
  filteredSearchCountHistories: SearchCountHistory[] = [];

  loading = false;
  searchEngines = Object.values(SearchEngine);

  searchEngineSelected: SearchEngine | null = null;
  urlSelected: string | null = null;
  searchTermSelected: string | null = null;
  dateRangeSelected: {startDate: NgbDateStruct, endDate: NgbDateStruct};
  nowDate: NgbDateStruct;
  errorHandler = inject(ErrorHandler);

  constructor(private searchCountHistoryService: SearchCountHistoryService,
    private toastr: ToastrService) {
    const now = new Date();
    this.nowDate = {
      year: now.getFullYear(),
      month: now.getMonth() + 1,
      day: now.getDate()
    };

    this.dateRangeSelected = {
      startDate: {
        year: now.getFullYear() - 1,
        month: now.getMonth() + 1,
        day: now.getDate()
      },
      endDate: this.nowDate
    };
  }

  ngOnInit(): void {
    this.getAllSearchCountHistory();
  }

  getAllSearchCountHistory(): void {
    this.loading = true;

    this.searchCountHistoryService.getAllSearchCountHistory().subscribe({
      next: (data) => {
        this.searchCountHistories = data;
        this.filteredSearchCountHistories = data;
        this.filter();

        this.loading = false;
      },
      error: (e) => {
        this.loading = false;
        this.toastr.error("Something went wrong while fetching the history", "Error", {closeButton: true});
      } 
    });
  }

  filter(): void {
    this.filteredSearchCountHistories = this.searchCountHistories.filter(h => 
      (this.searchEngineSelected == null ?
        true :
        h.searchEngine == this.searchEngineSelected) &&
      (this.urlSelected == null ? 
        true :
        h.url.includes(this.urlSelected)) &&
      (this.searchTermSelected == null ?
        true :
        h.searchTerm.includes(this.searchTermSelected)) &&
      (this.dateRangeSelected?.startDate == null ?
        true :
        new Date(h.dateOfExcecution) >= this.convertNgbDateStructToDate(this.dateRangeSelected?.startDate)) &&
      (this.dateRangeSelected?.endDate == null ?
        true :
        new Date(h.dateOfExcecution) <= this.convertNgbDateStructToDate(
          { year: this.dateRangeSelected?.endDate.year, month: this.dateRangeSelected?.endDate.month, day: this.dateRangeSelected?.endDate.day + 1 }
        )));
  }

  private convertNgbDateStructToDate(ngbDate: NgbDateStruct): Date {
    return new Date(ngbDate.year, ngbDate.month - 1, ngbDate.day);
  }
}
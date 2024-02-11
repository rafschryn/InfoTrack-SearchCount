import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { ErrorHandler } from '../interceptor/error-handler';
import { SearchCountRequest } from '../models/search-count-request';
import { SearchCountResponse } from '../models/search-count-response';
import { SearchEngine } from '../models/search-engine';
import { SearchCountService } from '../services/search-count.service';

@Component({
  selector: 'app-search-count',
  templateUrl: './search-count.component.html',
  styleUrls: ['../app.component.css']
})

export class SearchCountComponent implements OnInit {
  title = 'Search Count';
  searchForm!: FormGroup;
  searchEngines = Object.values(SearchEngine);
  result: SearchCountResponse = new SearchCountResponse;
  loading = false;
  errorHandler = inject(ErrorHandler);

  constructor(private searchCountService: SearchCountService,
    private formBuilder: FormBuilder,
    private toastr: ToastrService) {}


  ngOnInit(): void {
    this.searchForm = this.formBuilder.group({
      searchTerm: ['', [Validators.required, Validators.maxLength(150)]],
      url: ['', [Validators.required, Validators.pattern("^(?:(ftp|http|https)?:\/\/)?(?:[\\w-]+\\.)+([a-z]|[A-Z]|[0-9]){2,6}$")]],
      searchEngine: [SearchEngine.Google, Validators.required]
    });
  }

  onSubmit() {
    this.loading = true;

    const searchCountRequest: SearchCountRequest = {
      searchTerm: this.searchForm.value.searchTerm,
      url: this.searchForm.value.url,
      searchEngine: this.searchForm.value.searchEngine
    }

    this.searchCountService.getSearchCount(searchCountRequest).subscribe({
      next: (data) => {
      this.result = data;
      this.loading = false;

      this.toastr.success("Success", `${searchCountRequest.searchEngine} search successful`, {closeButton: true});
      },
      error: (e) => {
        this.loading = false;

        this.toastr.error("Error", `Something went wrong while searching ${searchCountRequest.searchEngine}`, {closeButton: true});
      } 
    });
  }
}
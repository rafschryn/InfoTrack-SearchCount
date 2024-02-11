import {  NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SearchCountHistoryComponent } from './search-count-history/search-count-history.component';
import { SearchCountComponent } from './search-count/search-count.component';

const routes: Routes = [
  { path: '', component: SearchCountComponent , pathMatch: 'full'},
  { path: 'search-count-history', component: SearchCountHistoryComponent , pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
  providers: [],
})
export class AppRoutingModule { }

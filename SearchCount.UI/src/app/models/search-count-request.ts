import { SearchEngine } from "./search-engine";

export class SearchCountRequest {
    searchTerm: string = '';
    url: string = '';
    searchEngine: SearchEngine = SearchEngine.Google;
}
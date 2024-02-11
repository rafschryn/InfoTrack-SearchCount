import { SearchEngine } from "./search-engine";

export class SearchCountHistory {
    id!: string;
    searchTerm!: string;
    url!: string;
    searchEngine!: SearchEngine;
    indices!: number[];
    dateOfExcecution!: Date;
}
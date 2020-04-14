import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../services/dataService';
import { IFeedItem } from '../../data/entities/IFeedItem';


@Component({
  selector: 'search',
  templateUrl: './search.component.html',
})
export class SearchComponent  {

  constructor(private data: DataService) { }

  private feedItemResults: IFeedItem[];
  private errorMessage: string;
  private searchCriteria: string;
 

  searchFeedItems() {

    this.data
      .searchFeedItems(this.searchCriteria)
      .subscribe(data => {
        if (data) {
          this.feedItemResults = data;
        }
      }, err => this.errorMessage = "An error happened while searching :(");


  }
  




}


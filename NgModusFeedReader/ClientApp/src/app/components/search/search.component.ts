import { Component, OnDestroy, Input } from '@angular/core';
import { DataService } from '../../services/dataService';
import { IFeedItem } from '../../data/entities/IFeedItem';
import { Subscription } from 'rxjs';

@Component({
  selector: 'search',
  templateUrl: './search.component.html',
})
export class SearchComponent implements OnDestroy {

  constructor(private data: DataService) { }

  private feedItemResults: IFeedItem[];
  private errorMessage: string;
  private searchCriteria: string = "";

  private searchSubsciption: Subscription;


  ngOnDestroy() {
    if (this.searchSubsciption)
      this.searchSubsciption.unsubscribe();

  }
 

  searchFeedItems() {

    this.searchSubsciption = this.data
      .searchFeedItems(this.searchCriteria)
      .subscribe(data => {
        if (data) {
          this.feedItemResults = data;
        }
      }, err => this.errorMessage = "An error happened while searching :(");


  }
  




}


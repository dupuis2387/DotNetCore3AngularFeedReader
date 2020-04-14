import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { DataService } from '../../services/dataService';
import { IFeedItem } from '../../data/entities/IFeedItem';
import { ActivatedRoute } from "@angular/router";
import { Subscription } from 'rxjs';


@Component({
  selector: 'feed-items',
  templateUrl: './feed-items.component.html',
})
export class FeedItemsComponent implements OnInit, OnDestroy {

  constructor(private data: DataService, private route: ActivatedRoute) { }

  private feedItems: IFeedItem[];
  private errorMessage: string = "Loading feeds...";

  private feedItemSubsciption: Subscription;

  //good cleanup, regardless of this not still being an ongoing stream of data
  ngOnDestroy() {
    
    if (this.feedItemSubsciption)
      this.feedItemSubsciption.unsubscribe();
        

  }


  ngOnInit(): void {   

    switch (this.route.snapshot.routeConfig.path) {
      case "all-feed-items":
        this.feedItemSubsciption = this.data
          .getAllFeedItems()
          .subscribe(data => {
            if (data) {
              this.errorMessage = "";
              this.feedItems = data;
            }
            
          }, err => this.errorMessage = "Failed to get feed items :(");
        break;
      default:
        this.feedItemSubsciption = this.data
          .getSubscribedFeeds()
          .subscribe(data => {
            if (data) {
              this.errorMessage = "";
              this.feedItems = data;
            }
            else {
              this.errorMessage = "Looks like you're not subscribed to any feeds :("
            }
          }, err => this.errorMessage = "Failed to get feed items :(");
        break;
    }

    
  }

  


}


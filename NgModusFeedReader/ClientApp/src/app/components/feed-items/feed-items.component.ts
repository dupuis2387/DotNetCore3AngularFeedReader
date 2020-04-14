import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../services/dataService';
import { IFeedItem } from '../../data/entities/IFeedItem';
import { ActivatedRoute } from "@angular/router";


@Component({
  selector: 'feed-items',
  templateUrl: './feed-items.component.html',
})
export class FeedItemsComponent implements OnInit {

  constructor(private data: DataService, private route: ActivatedRoute) { }

  private feedItems: IFeedItem[];
  private errorMessage: string = "Loading feeds...";



  ngOnInit(): void {

   

    switch (this.route.snapshot.routeConfig.path) {
      case "all-feed-items":
        this.data
          .getAllFeedItems()
          .subscribe(data => {
            if (data) {
              this.errorMessage = "";
              this.feedItems = data;
            }
            
          }, err => this.errorMessage = "Failed to get feed items :(");
        break;
      default:
        this.data
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


import { Component, OnInit, Input } from '@angular/core';
import { DataService } from '../../services/dataService';
import { Router, ActivatedRoute } from '@angular/router';
import { IFeed } from '../../data/entities/IFeed';


@Component({
  selector: 'feed',
  templateUrl: './feed.component.html',
})
export class FeedComponent implements OnInit {

  constructor(private data: DataService, private route: ActivatedRoute) { }

  private feeds: IFeed[];
  private errorMessage: string = "";

  @Input() id:any;

  ngOnInit(): void {

    //if we dont have a parent category param, load all feeds
    const feedId = this.route.snapshot.paramMap.get("id") || this.id;

    this.data
      .getFeeds(feedId)
      .subscribe(data => {
        if (data) {
          this.feeds = data;
        }
      }, err => { console.log(err); this.errorMessage = "Failed to get feed(s) :(" });
  }

  subscribeToFeed(feed): void {
    this.data
      .subscribeToFeed(feed.id)
      .subscribe(data => {
        console.log("subscribe", data);
        feed.subscribed = true;
        if (data) {
          //this.feeds = data;
          console.log(feed);
          
        }
      }, err => this.errorMessage = "Failed to subscribe to feed :(");
  }
  unsubscribeFromFeed(feed): void {
    this.data
      .unSubscribeFromFeed(feed.id)
      .subscribe(data => {
        console.log("unsubscribe", data);
        feed.subscribed = false;
        if (data) {
          //this.feeds = data;
          console.log(feed);

        }
      }, err => this.errorMessage = "Failed to unsubscribe from feed :(");
  }


}


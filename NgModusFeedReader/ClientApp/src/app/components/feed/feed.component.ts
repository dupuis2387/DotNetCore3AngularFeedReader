import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { DataService } from '../../services/dataService';
import { Router, ActivatedRoute } from '@angular/router';
import { IFeed } from '../../data/entities/IFeed';
import { Subscription } from 'rxjs';


@Component({
  selector: 'feed',
  templateUrl: './feed.component.html',
})
export class FeedComponent implements OnInit, OnDestroy {

  constructor(private data: DataService, private route: ActivatedRoute) { }

  private feeds: IFeed[];
  private errorMessage: string = "";

  private feedListingSubscription: Subscription;
  private subscribeAttempt: Subscription;
  private unsubscribeAttempt: Subscription;

  ngOnDestroy() {
    
    if (this.feedListingSubscription)
      this.feedListingSubscription.unsubscribe();

    if (this.subscribeAttempt)
      this.subscribeAttempt.unsubscribe();

    if (this.unsubscribeAttempt)
      this.unsubscribeAttempt.unsubscribe();
    
  }

  ngOnInit(): void {   

    this.feedListingSubscription = this.data
      .getFeeds()
      .subscribe(data => {
        if (data) {
          this.feeds = data;
        }
      }, err => { console.log(err); this.errorMessage = "Failed to get feed(s) :(" });
  }

  subscribeToFeed(feed): void {
    this.subscribeAttempt = this.data
      .subscribeToFeed(feed.id)
      .subscribe(data => {
        
        feed.subscribed = true;
        if (data) {
        
        }
      }, err => this.errorMessage = "Failed to subscribe to feed :(");
  }
  unsubscribeFromFeed(feed): void {
    this.unsubscribeAttempt = this.data
      .unSubscribeFromFeed(feed.id)
      .subscribe(data => {
        
        feed.subscribed = false;
        if (data) {
          

        }
      }, err => this.errorMessage = "Failed to unsubscribe from feed :(");
  }


}


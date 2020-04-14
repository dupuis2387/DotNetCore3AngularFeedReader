import { Injectable, Inject } from "@angular/core";
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, map } from 'rxjs/operators';
import { IFeedCategory } from "../data/entities/IFeedCategory";
import { IFeed } from "../data/entities/IFeed";
import { IFeedItem } from "../data/entities/IFeedItem";


@Injectable()
export class DataService {

  private token: string = "";
  private tokenExpiration: Date;

  /**
   * use this property to determine if we're logged in or not
   */
  public get loginRequired(): boolean {

    if (!localStorage.getItem("jwtToken"))
      return true;
    else
      return false;

    //const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));
    //return (this.token.length == 0 || this.tokenExpiration > new Date());
  }


  getFeedCategories(): Observable<IFeedCategory[]> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));

    return this.http
      .get("/api/Feed/FeedCategories",
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: IFeedCategory[]) => {
          return data;
        })
      );

  }

  getFeeds(feedId?): Observable<IFeed[]> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));


    return this.http
      .get((feedId ? "/api/Feed/Feeds/" + feedId : "/api/Feed/Feeds"),
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: IFeed[]) => {
          return data;
        })
      );

  }

  subscribeToFeed(feedId): Observable<boolean> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));

    return this.http
      .post("/api/feed/subscribe",
        feedId,
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: boolean) => {
          return data;
        })
      );
  }

  unSubscribeFromFeed(feedId): Observable<boolean> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));

    return this.http
      .post("/api/feed/unsubscribe",
        feedId,
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: boolean) => {
          return data;
        })
      );
  }


  /**
   * 
   * @param creds
   */
  login(loginViewModel): Observable<boolean> {
    return this.http
      .post("/api/account/login", loginViewModel)
      .pipe(
        map((data: any) => {
          console.log("login response:", data);
          //this.token = data.token;
          //this.tokenExpiration = data.expiration;


          //cheating here...ran out of time
          const jwtToken = {
            token: data.token,
            tokenExpiration: data.expiration
          };
          localStorage.setItem("jwtToken", JSON.stringify(jwtToken));

          return true;
        })
      );
  }

  /**
   * 
   * @param registerViewModel
   */
  register(registerViewModel): Observable<boolean> {
    return this.http
      .post("/api/account/register", registerViewModel)
      .pipe(
        map((data: any) => {
          console.log("register response:", data);
          //this.token = data.token;
          //this.tokenExpiration = data.expiration;

          const jwtToken = {
            token: data.token,
            tokenExpiration: data.expiration
          };
          localStorage.setItem("jwtToken", JSON.stringify(jwtToken));

          return true;
        }));
  }

  getSubscribedFeeds(): Observable<IFeedItem[]> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));


    return this.http
      .get("/api/Feed/GetFeedStream",
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: IFeedItem[]) => {
          return data;
        })
      );

  }

  searchFeedItems(searchCriteria): Observable<IFeedItem[]> {
    //get our token from our secret hiding place
    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));


    return this.http
      .get("/api/Feed/search?searchString=" + searchCriteria,
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: IFeedItem[]) => {
          return data;
        })
      );

  }

  getAllFeedItems(): Observable<IFeedItem[]> {
    //get our token from our secret hiding place

    const jwtToken = JSON.parse(localStorage.getItem("jwtToken"));


    return this.http
      .get("/api/Feed/FeedItems" ,
        {
          headers:
            new HttpHeaders().set("Authorization", "Bearer " + jwtToken.token)
        })
      .pipe(
        map((data: IFeedItem[]) => {
          return data;
        })
      );

  }


  constructor(private http: HttpClient) {

  }


}
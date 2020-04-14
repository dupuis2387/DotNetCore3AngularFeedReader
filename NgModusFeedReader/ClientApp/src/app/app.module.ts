import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { FeedComponent } from './components/feed/feed.component';
import { FeedItemsComponent } from './components/feed-items/feed-items.component';
import { SearchComponent } from './components/search/search.component';
import { DataService } from './services/dataService';
import { VideoComponent } from './components/youtube-video/youtube-video-component';



const routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'feeds', component: FeedComponent },
  { path: 'my-feed-items', component: FeedItemsComponent },
  { path: 'all-feed-items', component: FeedItemsComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'search', component: SearchComponent }
];

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FeedComponent,
    FeedItemsComponent,
    LoginComponent,
    RegisterComponent,
    SearchComponent,
    VideoComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot(routes, {
      useHash: true,
      //enableTracing:true 
    })
  ],
  providers: [DataService],
  bootstrap: [AppComponent]
})
export class AppModule { }

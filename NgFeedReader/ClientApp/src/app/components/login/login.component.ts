import { Component, Inject, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from "../../services/dataService";
import { Router } from "@angular/router";
import { Subscription } from 'rxjs';

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnDestroy {

  private loginViewModel = {
    Email: "",
    Password: ""
  }

  private errorMessage: string = "";

  private loginSubsciption: Subscription;


  ngOnDestroy() {
    if (this.loginSubsciption)
      this.loginSubsciption.unsubscribe();

  }

  


  constructor(private data: DataService, private router: Router) { }


  onLogin() {
    //Call login service
    //console.table(this.creds);

    this.loginSubsciption = this.data
      .login(this.loginViewModel)
      .subscribe(success => {
        if (success) {
          this.router.navigate(["/"]);
        }
      }, err=>this.errorMessage = "Failed to login");
  }

  

}
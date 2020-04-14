import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from "../../services/dataService";
import { Router } from "@angular/router";

@Component({
  selector: 'login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  private loginViewModel = {
    Email: "",
    Password: ""
  }

  private errorMessage: string = "";

  constructor(private data: DataService, private router: Router) { }


  onLogin() {
    //Call login service
    //console.table(this.creds);
    this.data
      .login(this.loginViewModel)
      .subscribe(success => {
        if (success) {
          this.router.navigate(["/"]);
        }
      }, err=>this.errorMessage = "Failed to login");
  }

  

}
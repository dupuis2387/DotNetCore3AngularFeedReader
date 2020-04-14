import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from '../../services/dataService';
import { Router } from '@angular/router';

@Component({
  selector: 'register',
  templateUrl: './register.component.html'
})
export class RegisterComponent {

  public registerViewModel = {
    Email: "",
    Password: "",
    FirstName: "",
    LastName:""
  }

  private errorMessage: string = "";

  constructor(private data: DataService, private router: Router) { }


  onRegister() {
    //Call login service
    //console.table(this.creds);

    this.data
      .register(this.registerViewModel)
      .subscribe(success => {
        if (success) {
          console.log("successfully registered");
          this.router.navigate(["/"]);
        }
      }, err => this.errorMessage = "Failed to register");
  }
}
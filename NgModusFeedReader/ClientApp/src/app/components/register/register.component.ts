import { Component, Inject, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from '../../services/dataService';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'register',
  templateUrl: './register.component.html'
})
export class RegisterComponent implements OnDestroy {

  public registerViewModel = {
    Email: "",
    Password: "",
    FirstName: "",
    LastName:""
  }

  private errorMessage: string = "";

  constructor(private data: DataService, private router: Router) { }

  private registerSubsciption: Subscription;


  ngOnDestroy() {
    if (this.registerSubsciption)
      this.registerSubsciption.unsubscribe();

  }


  onRegister() {
    //Call login service
    //console.table(this.creds);

    this.registerSubsciption = this.data
      .register(this.registerViewModel)
      .subscribe(success => {
        if (success) {
          console.log("successfully registered");
          this.router.navigate(["/"]);
        }
      }, err => {
          this.errorMessage = err.error ? err.error : "Failed to register";
      });
  }
}
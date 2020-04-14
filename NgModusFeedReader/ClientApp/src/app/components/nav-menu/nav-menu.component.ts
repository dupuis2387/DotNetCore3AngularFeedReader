import { Component } from '@angular/core';
import { DataService } from "../../services/dataService";
import { Router } from '@angular/router';


@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    //cheating....
    localStorage.removeItem("jwtToken");
    this.router.navigate([""]);

  }
  constructor(private dataService: DataService, private router: Router) { }
}

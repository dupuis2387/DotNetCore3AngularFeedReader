import { Component } from '@angular/core';
import { DataService } from '../../services/dataService';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private dataService: DataService) { }


}

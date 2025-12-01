import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SearchCityComponent } from './cities/search-city/search-city';
import { DestinationListComponent } from './Destinos/destination-list';
import { LoginComponent } from './auth/login.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    SearchCityComponent,
    DestinationListComponent,
    LoginComponent  
  ],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class App {
  title = 'Mi App de Ciudades';
}

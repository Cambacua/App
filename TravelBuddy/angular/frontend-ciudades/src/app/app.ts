import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SearchCityComponent } from './cities/search-city/search-city';
import { HttpClientModule } from '@angular/common/http'; 

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet, 
    SearchCityComponent,
    HttpClientModule 
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'Mi App de Ciudades';
}
import { Routes } from '@angular/router';
import { SearchCityComponent } from './cities/search-city/search-city';

export const routes: Routes = [
  { path: 'buscar-ciudades', component: SearchCityComponent },
  { path: '', redirectTo: 'buscar-ciudades', pathMatch: 'full' }
]; 
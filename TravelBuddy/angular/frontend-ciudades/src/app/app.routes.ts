import { Routes } from '@angular/router';
import { SearchCityComponent } from './cities/search-city/search-city';
import { DestinationListComponent } from './Destinos/destination-list';
import { LoginComponent } from './auth/login.component';

export const appRoutes: Routes = [
  { path: '', component: LoginComponent },
  { path: 'search', component: SearchCityComponent },
  { path: 'destinations', component: DestinationListComponent }
];

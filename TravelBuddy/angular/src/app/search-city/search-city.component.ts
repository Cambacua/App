import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { DestinationService } from '../proxy/destinations/destination.service';
import { CityDto } from '../proxy/cities/models';
import { debounceTime, distinctUntilChanged, switchMap, catchError, finalize, tap } from 'rxjs/operators';
import { of } from 'rxjs';

@Component({
  selector: 'app-search-city',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './search-city.component.html',
  styleUrls: ['./search-city.component.css'],
})
export class SearchCityComponent {
  private destinationService = inject(DestinationService);

  searchControl = new FormControl('');
  results: CityDto[] = [];
  isLoading = false;
  errorMessage = '';

  constructor() {
    this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      tap(() => {
        this.isLoading = true;
        this.errorMessage = '';
        this.results = [];
      }),
      switchMap(query => {
        if (!query || query.trim().length === 0) {
          this.isLoading = false;
          return of(null);
        }
        
        return this.destinationService.searchCitiesByName({ partialName: query }).pipe(
          catchError(err => {
            this.errorMessage = 'Ocurrió un error al buscar ciudades. Por favor intente nuevamente.';
            console.error('Error searching cities:', err);
            return of(null);
          }),
          finalize(() => {
            this.isLoading = false;
          })
        );
      })
    ).subscribe(response => {
      if (response && response.cities) {
        this.results = response.cities;
      }
    });
  }
}

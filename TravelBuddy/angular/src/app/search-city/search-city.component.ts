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

  // Mock Data for the Body (Destinos Recomendados) - "Cargalos vos"
  readonly featuredDestinations = [
    {
      nombre: 'Paris',
      descripcion: 'Ciudad del amor y las luces.',
      ubicacion: 'Francia',
      precio: 1500,
      imagenUrl: 'https://images.unsplash.com/photo-1502602898657-3e91760cbb34?auto=format&fit=crop&q=80&w=1000',
    },
    {
      nombre: 'Londres',
      descripcion: 'Capital del Reino Unido.',
      ubicacion: 'Inglaterra',
      precio: 1800,
      imagenUrl: 'https://images.unsplash.com/photo-1513635269975-59663e0ac1ad?auto=format&fit=crop&q=80&w=1000',
    },
    {
      nombre: 'Buenos Aires',
      descripcion: 'Capital de Argentina.',
      ubicacion: 'Argentina',
      precio: 900,
      imagenUrl: 'https://images.unsplash.com/photo-1589909202802-8f4aadce1849?auto=format&fit=crop&q=80&w=1000',
    },
    {
      nombre: 'Nueva York',
      descripcion: 'La ciudad que nunca duerme.',
      ubicacion: 'Estados Unidos',
      precio: 2000,
      imagenUrl: 'https://images.unsplash.com/photo-1496442226666-8d4a0e62e6e9?auto=format&fit=crop&q=80&w=1000',
    },
    {
      nombre: 'Tokio',
      descripcion: 'Mezcla de tradición y tecnología.',
      ubicacion: 'Japón',
      precio: 2200,
      imagenUrl: 'https://images.unsplash.com/photo-1540959733332-eab4deabeeaf?auto=format&fit=crop&q=80&w=1000',
    }
  ];

  searchControl = new FormControl('');

  // Search Results from API (Cities)
  cityResults: CityDto[] = [];

  isSearching = false;
  searchError = '';

  constructor() {
    this.searchControl.valueChanges.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      tap(() => {
        this.isSearching = true;
        this.searchError = '';
        this.cityResults = [];
      }),
      switchMap(query => {
        if (!query || query.trim().length === 0) {
          this.isSearching = false;
          return of(null);
        }

        // Restore API call to GeoDb (via Backend proxy)
        return this.destinationService.searchCitiesByName({ partialName: query }).pipe(
          catchError(err => {
            this.searchError = 'Error al buscar ciudades.';
            console.error(err);
            return of(null);
          }),
          finalize(() => {
            this.isSearching = false;
          })
        );
      })
    ).subscribe(response => {
      if (response && response.cities) {
        this.cityResults = response.cities;
      }
    });
  }
}

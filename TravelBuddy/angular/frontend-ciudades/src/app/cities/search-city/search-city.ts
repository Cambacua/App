import { Component, OnDestroy } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CityService } from '../city.service';
import { CommonModule } from '@angular/common';
import { City } from '../city'; 
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-search-city',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './search-city.html',
  styleUrls: ['./search-city.css'],
})
export class SearchCityComponent implements OnDestroy {

  search = new FormControl('');
  cities: City[] = [];
  loading = false;
  error = '';

  private subscription!: Subscription;

  constructor(private cityService: CityService, public auth: AuthService) {
    this.setupSearchSubscription();
  }

  // Configuracion del debounce
  setupSearchSubscription() {
    this.subscription = this.search.valueChanges
      .pipe(
        debounceTime(500),          // espera 500ms sin escribir
        distinctUntilChanged(),     // evita repetir la misma búsqueda
        switchMap((value) => {      // cancela si llega una búsqueda nueva
          const name = value?.trim() ?? '';

          if (!name) {
            this.cities = [];
            return []; // devuelve observable vacío
          }

          this.loading = true;
          this.error = '';

          return this.cityService.search(name);
        })
      )
      .subscribe({
        next: (response: any) => {
          this.cities = response.cities ?? [];
          this.loading = false;
        },
        error: () => {
          this.error = 'Error consultando el servidor.';
          this.loading = false;
        }
      });
  }

  // Para evitar fugas de memoria
  ngOnDestroy() {
    if (this.subscription) this.subscription.unsubscribe();
  }

  // Si querés seguir usando el botón Buscar
  onSearch() {
    const value = this.search.value ?? '';
    this.search.setValue(value); // dispara manualmente el flujo del debounce
  }
}

import { Component } from '@angular/core';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { CityService } from '../city.service';
import { CommonModule } from '@angular/common';
import { City } from '../city';

@Component({
  selector: 'app-search-city',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './search-city.html',
  styleUrls: ['./search-city.css'],
})
export class SearchCityComponent {

  search = new FormControl('');
  cities: City[] = [];
  loading = false;
  error = '';

  constructor(private cityService: CityService) {}

  onSearch() {
    const value = this.search.value ?? '';

    if (!value.trim()) {
      this.cities = [];
      return;
    }

    this.loading = true;
    this.error = '';

    this.cityService.search(value).subscribe({
      next: (response) => {
        this.cities = response.cities;
        this.loading = false;
      },
      error: () => {
        this.error = 'Error consultando el servidor.';
        this.loading = false;
      }
    });
  }
}

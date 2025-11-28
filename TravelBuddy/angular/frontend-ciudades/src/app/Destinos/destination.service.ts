import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Destination } from './destination';

@Injectable({
  providedIn: 'root'
})
export class DestinationService {

private apiUrl = 'https://localhost:44388/api/app/destination';

  constructor(private http: HttpClient) {}

getDestinations(): Observable<Destination[]> {
  return this.http.get<Destination[]>(this.apiUrl);
}
}

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:44388/api/account/login';

  constructor(private http: HttpClient) {}

  login(username: string, password: string, rememberMe: boolean): Observable<any> {
    const body = {
      userNameOrEmailAddress: username,
      password: password,
      rememberMe: rememberMe
    };

    return this.http.post(this.apiUrl, body);
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  clear() {
    localStorage.removeItem('token');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}

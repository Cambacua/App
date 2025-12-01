import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../core/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
  styleUrls: ['./login.css']
})
export class LoginComponent {

  username = '';
  password = '';
  rememberMe = true;
  error = '';

  constructor(private auth: AuthService) {}

  login() {
    this.error = '';

    this.auth.login(this.username, this.password, this.rememberMe).subscribe({
      next: (result) => {
        console.log("LOGIN RESULT:", result);

        this.auth.setToken(result.accessToken);
        alert("Login exitoso. Token guardado.");
      },
      error: () => {
        this.error = 'Usuario o contraseña incorrectos';
      }
    });
  }
}

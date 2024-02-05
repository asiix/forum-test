import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { User } from '../../Models/User';
import { AuthenticationService } from '../../Services/authentication.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, MatButtonModule, MatIconModule, FormsModule, ReactiveFormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  constructor(private auth: AuthenticationService) { }
  hide = true;
  email = new FormControl('', [Validators.required, Validators.email]);
  username = new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(16)]);
  password = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(24)]);
  user: User = {} as User;

  //REMINDER: to access FormControl variables like email, username and password you MUST do .value
  async Login() {
    this.user.Username = this.username.value!;
    this.user.EmailAddress = this.email.value!;
    this.user.PasswordSalt = this.password.value!;
    await this.auth.Login(this.user);
  }
}

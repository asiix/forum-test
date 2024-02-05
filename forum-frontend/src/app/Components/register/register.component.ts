import { Component } from '@angular/core';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormControl, Validators, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthenticationService } from '../../Services/authentication.service';
import { User } from '../../Models/User';

/**
 * @title Inputs in a form
 */

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [MatFormFieldModule, MatInputModule, FormsModule, ReactiveFormsModule, MatSelectModule, MatIconModule, MatButtonModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})

export class RegisterComponent {
  constructor(private auth: AuthenticationService) { }
  hide: boolean = true;
  username = new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(16)]);
  email = new FormControl('', [Validators.required, Validators.email]);
  password = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(24)]);
  confirmedPassword = new FormControl('', [Validators.required, Validators.minLength(8), Validators.maxLength(24)]);
  user: User = {} as User;

  async Register() {
    this.user.Username = this.username.value!;
    this.user.EmailAddress = this.email.value!;
    this.user.PasswordSalt = this.password.value!;
    this.user.PasswordHash = this.confirmedPassword.value!;

    await this.auth.Registration(this.user);
  }


  getErrorMessage() {
    if (this.email.hasError('required')) {
      return 'You must enter a value';
    }

    return this.email.hasError('email') ? 'Not a valid email' : '';
  }
}
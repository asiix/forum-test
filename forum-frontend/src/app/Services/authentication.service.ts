import { Injectable } from '@angular/core';
import { HttpRequestService } from './http-requests.service';
import { User } from '../Models/User';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  constructor(private httpSvc: HttpRequestService) { }

  //TO FINISH
  Registration(user: User) {
    this.httpSvc.Create('https://localhost:7241/api/Users', user).subscribe(data => {
      return data;
    })
  }

  //TO FINISH
  //POST to BE the email and psw, check if they're equal and get Token
  Login(user: User) {
    this.httpSvc.Create('https://localhost:7241/api/Users/Login', user).subscribe(response => {
      return response;
    })
  }
}

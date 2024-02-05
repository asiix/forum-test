import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class HttpRequestService {
  _id: string = '';

  constructor(private http: HttpClient) { }

  setTokenHttpHeader() {
    let btoa = localStorage.getItem('token');

    let headerOptions = new HttpHeaders({
      contentType: 'application/json',
      responseType: 'text',
      authorization: 'Basic ' + btoa,
    });

    return headerOptions;
  }

  Create(path: string, data: any) {
    return this.http.post(path, data, {
      //headers: this.setTokenHttpHeader(),
      observe: 'response',
    });
  }

  Read(path: string): Observable<any> {
    return this.http.get(path, {
      //headers: this.setTokenHttpHeader(),
      observe: 'response',
    });
  }

  ReadById(path: string): Observable<any> {
    return this.http.get(`${path + this._id}`);
  }

  Update(path: string, data: any) {
    return this.http.put(path, data, {
      //headers: this.setTokenHttpHeader(),
      observe: 'response',
    });
  }

  Delete(path: string) {
    return this.http.delete(path, {
      //headers: this.setTokenHttpHeader(),
      observe: 'response',
    });
  }
}

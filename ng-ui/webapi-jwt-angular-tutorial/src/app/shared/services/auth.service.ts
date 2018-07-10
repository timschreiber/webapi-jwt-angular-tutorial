import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { BaseService } from './base.service';
import { BehaviorSubject, Observable, pipe } from 'rxjs';
import { map, catchError } from 'rxjs/operators'
import { environment } from '../../../environments/environment';

@Injectable()

export class AuthService extends BaseService {

  private _authNavItemSource = new BehaviorSubject<boolean>(false);
  private _loggedIn = false;
  
  authNavStatus$ = this._authNavItemSource.asObservable();

  constructor(private http: Http) {
    super();
    this._loggedIn = !!localStorage.getItem('access_token');
    this._authNavItemSource.next(this._loggedIn);
  }

  register(email: string, password: string, confirmPassword: string) : Observable<boolean> {
    let body = JSON.stringify({
      email, 
      password,
      confirmPassword
    });

    let headers = new Headers({
      'Content-Type': 'application/json'
    });

    let options = new RequestOptions({
      headers: headers
    });
    
    return this.http.post(environment.apiBaseUrl + 'api/account/register', body, options)
      .pipe(map(res => true))
      .pipe(catchError(this.handleError));
  }

  login(email: string, password: string) {
    let body = JSON.stringify({
      grant_type: 'password',
      userName: email,
      password: password,
    });
    
    let headers = new Headers({
      'Content-Type': 'application/json'
    });

    let options = new RequestOptions({
      headers: headers
    });

    return this.http.post(environment.apiBaseUrl + 'token', options)
      .pipe(map(res => res.json()))
      .pipe(map(res => {
        localStorage.setItem('access_token', res.access_token);
        this._loggedIn = true;
        this._authNavItemSource.next(true);
        return true;
      }))
      .pipe(catchError(this.handleError));
  }
}

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/internal/operators/map';
import { User } from '../_models/user';
import { ReplaySubject } from 'rxjs/internal/ReplaySubject';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();
  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model, {
      headers : {
        'Access-Control-Allow-Origin' : '*'
      }
    }).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  register(model:any) {
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  setCurrentUser(user: User){
    this.currentUserSource.next(user);  
  }

  logout(){
    localStorage.removeItem('user');
    this.currentUserSource.next(null!); 
  } 
}

import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Member } from '../_models/member';

let token = "";

const userItem = localStorage.getItem('user');
if (userItem) {
  const user = JSON.parse(userItem);
  if (user && user.token) {
    token = user.token;
  }
}

const httpOptions = {
  headers: new HttpHeaders({
    Authorization: 'Bearer ' + token,
  })
}

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getMembers() {
    return this.http.get<Member[]>(this.baseUrl + 'users', httpOptions);
  }

  getMember(username: string) {
    return this.http.get<Member>(this.baseUrl + 'users' + username, httpOptions);
  }

}

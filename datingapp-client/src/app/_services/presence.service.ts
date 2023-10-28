import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment.development';
import { BehaviorSubject, take } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class PresenceService {
  private hubConnection!: HubConnection;
  private onlineUsersSource = new BehaviorSubject<string[]>([]);
  onlineUsers$ = this.onlineUsersSource.asObservable();
  hubUrl = environment.hubUrl;

  constructor(private toastr: ToastrService, private router: Router) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder().withUrl(this.hubUrl + 'presence', {
      accessTokenFactory: () => user.token
    }).withAutomaticReconnect().build();

    this.hubConnection.start().catch(error => console.log(error));

    this.hubConnection.on('UserIsOnline', (username: string) => {
      this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
        this.onlineUsersSource.next([...usernames, username]);
      });
    });

    this.hubConnection.on('UserIsOffline', (username: string) => {
      this.onlineUsers$.pipe(take(1)).subscribe(usernames => {
        this.onlineUsersSource.next([...usernames.filter(name => name !== username)]);
      });
    });

    this.hubConnection.on('GetOnlineUsers', (username: string[]) => {
      this.onlineUsersSource.next(username);
    });

    this.hubConnection.on("NewMessageReceived", ({ username, knownAs }) => {
      this.toastr.info(knownAs + ' has sent you a new message!').onTap.pipe(take(1)).subscribe(() => {
        this.router.navigateByUrl('/members/' + username + '?tab=3');
      }); 
    });
  }

  stopHubConnection() {
    this.hubConnection.stop().catch(error => console.log(error));
  }
}

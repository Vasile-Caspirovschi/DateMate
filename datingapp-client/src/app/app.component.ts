import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AccountService } from './_services/account.service';
import { User } from './_models/user';
import { PresenceService } from './_services/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'The datting app';
  users: any;

  constructor(private accountService: AccountService, private presence: PresenceService) { }

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
    const item = localStorage.getItem('user');
    if (item) {
      const user: User = JSON.parse(item);
      this.accountService.setCurrentUser(user);
      this.presence.createHubConnection(user);
    }
  }
}

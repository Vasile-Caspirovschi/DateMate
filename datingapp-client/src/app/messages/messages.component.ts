import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/messages';
import { Pagination } from '../_models/pagination';
import { MessageService } from '../_services/message.service';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {
  messages!: Message[];
  pagination!: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 6;
  isLoading = false;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.isLoading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe(response => {
      this.messages = response.result;
      this.pagination = response.pagination;
      this.isLoading = false;
    });
  }

  pageChanged(event: any) {
    this.pageChanged = event.page;
    this.loadMessages();
  }
}

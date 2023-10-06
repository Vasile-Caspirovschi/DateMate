import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('formEdit') editForm!: NgForm;
  member!: Member;
  user!: User;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
  }

  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    this.memberService.getMember(this.user.username).subscribe(member => this.member = member);
  }

  updateMember() {
    this.toastr.success("Profile updated successfully!");
    console.log(this.editForm);
    this.editForm.reset(this.member);
  }
}

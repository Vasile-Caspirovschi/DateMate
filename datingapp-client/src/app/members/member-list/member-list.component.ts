import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members!: Member[];
  pagination!: Pagination;
  userParams!: UserParams;
  genderList = [{ value: 'male', display: "Males" }, { value: 'female', display: 'Females' }];

  constructor(private memberService: MembersService, private route: ActivatedRoute) {
    this.userParams = memberService.getUserParams();
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.members = data['paginatedResult'].result;
      this.pagination = data['paginatedResult'].pagination;
    });
  }

  loadMembers() {
    this.memberService.getMembers(this.userParams).subscribe((response: {result: Member[], pagination: Pagination}) => {
      this.members = response.result;
      this.pagination = response.pagination;
    });
  }

  resetFilters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.userParams.pageNumber = event.page;
    this.memberService.setUserParams(this.userParams);
    this.loadMembers();
  }
}

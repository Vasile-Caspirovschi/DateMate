import { ResolveFn } from '@angular/router';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../_models/pagination';
import { Member } from '../_models/member';
import { inject } from '@angular/core';
import { MembersService } from '../_services/members.service';

export const memberListResolver: ResolveFn<PaginatedResult<Member[]>> = (route) : Observable<PaginatedResult<Member[]>> => {
  const memberService = inject(MembersService);
  const userParams = memberService.getUserParams();
  return memberService.getMembers(userParams);
};

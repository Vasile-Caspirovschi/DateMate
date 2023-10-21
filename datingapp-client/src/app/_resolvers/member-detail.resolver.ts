import { ResolveFn } from '@angular/router';
import { Member } from '../_models/member';
import { inject } from '@angular/core';
import { MembersService } from '../_services/members.service';
import { Observable } from 'rxjs';

export const memberDetailResolver: ResolveFn<Member> = (route): Observable<Member> => {
  const memberService = inject(MembersService);
  const username = route.paramMap.get('username') ?? "";
  return memberService.getMember(username);
};

import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';

export const adminGuard: CanActivateFn = (): Observable<boolean> => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  return accountService.currentUser$.pipe(
    map(user => {
      if (user.roles.includes('Admin') || user.roles.includes('Moderator')) return true;
      toastr.error('You cannot access this area');
      return false;
    })
  );
};

import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { Observable} from 'rxjs';
import { map } from 'rxjs/operators';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (): Observable<boolean> => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map(user => {
      let isAnonymous = !!user;
      isAnonymous = !isAnonymous;
      if (isAnonymous) {
        toastr.error('You shall no pass!');
        return false;
      } else
        return true;
    })
  );
};

// export const authGuard: CanActivateFn = (): Observable<boolean> => {
//   const currentUser = inject(AccountService).currentUser$;
//   const toastr = inject(ToastrService);

//   let isAnonymous = true;
//   currentUser.pipe(
//     map(user => {
//       isAnonymous = !(user == null);
//     })
//   );

//   if (isAnonymous) {
//     toastr.error('You shall no pass!');
//     return of(false);
//   }

//   return of(true);
// };

import { CanActivateFn } from '@angular/router';

export const preventUnsavedChangesGuard: CanActivateFn = (route, state) => {
  return true;
};

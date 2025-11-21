import { Routes } from '@angular/router';
import { User } from './user/user';
import { Dashboard } from './pages/dashboard/dashboard';

export const routes: Routes = [
  { path: '', component: Dashboard },
  { path: 'users', component: User },
];

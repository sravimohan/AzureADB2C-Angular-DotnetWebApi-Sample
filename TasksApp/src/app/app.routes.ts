import { Routes } from '@angular/router';
import { MsalGuard } from '@azure/msal-angular';
import { HomeComponent } from './home/home.component';

export const routes: Routes = [{ path: '', component: HomeComponent, canActivate: [MsalGuard] }];

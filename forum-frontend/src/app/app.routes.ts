import { Routes } from '@angular/router';
import { HomepageComponent } from './Components/homepage/homepage.component';
import { ProfileComponent } from './Components/profile/profile.component';
import { ThreadComponent } from './Components/thread/thread.component';
import { LoginComponent } from './Components/login/login.component';
import { RegisterComponent } from './Components/register/register.component';

export const routes: Routes = [
    { path: 'home', component: HomepageComponent },
    { path: 'profile', component: ProfileComponent },
    { path: 'thread', component: ThreadComponent },
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
];

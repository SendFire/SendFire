import { Routes } from '@angular/router';
import { ErrorComponent } from './error/error.component';

export const ROUTES: Routes = [
    {
        path: '', redirectTo: 'queues', pathMatch: 'full'
    },
    {
        path: 'queues', loadChildren: './queues/queues.module#QueuesModule'
    },
    {
        path: 'error', component: ErrorComponent
    },
    {
        path: '**', component: ErrorComponent
    }
];

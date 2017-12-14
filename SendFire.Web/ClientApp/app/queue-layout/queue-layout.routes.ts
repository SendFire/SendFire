import { Routes } from '@angular/router';

import { QueueLayoutComponent } from './queue-layout.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetchdata/fetchdata.component';
import { HomeComponent } from './home/home.component';

export const ROUTES: Routes = [
    {
        path: '', component: QueueLayoutComponent, children: [
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'counter', component: CounterComponent },
            { path: 'fetchdata', component: FetchDataComponent },
            { path: 'home', component: HomeComponent }
            //{ path: 'widgets', loadChildren: '../widgets/widgets.module#WidgetsModule' },
            //{ path: 'special', loadChildren: '../special/special.module#SpecialModule' }
        ]
    }
];
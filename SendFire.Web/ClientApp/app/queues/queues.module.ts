import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { ROUTES } from './queues.routes';
import { QueuesLayoutComponent } from './queues-layout/queues-layout.component';
import { QueuesMenuComponent } from './queues-menu/queues-menu.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetchdata/fetchdata.component';
import { HomeComponent } from './home/home.component';

@NgModule({
  imports: [
      CommonModule,
      FormsModule,
      RouterModule.forChild(ROUTES)
  ],
  declarations: [
      QueuesLayoutComponent,
      QueuesMenuComponent,
      CounterComponent,
      FetchDataComponent,
      HomeComponent
  ]
})
export class QueuesModule { }

import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { ROUTES } from './queue-layout.routes';
import { QueueLayoutComponent } from './queue-layout.component';
import { QueueMenuComponent } from './queue-menu/queue-menu.component';
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
      QueueLayoutComponent,
      QueueMenuComponent,
      CounterComponent,
      FetchDataComponent,
      HomeComponent
  ]
})
export class QueueLayoutModule { }

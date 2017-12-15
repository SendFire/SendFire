import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, PreloadAllModules } from '@angular/router';

import { ROUTES } from './app.routes';
import { AppComponent } from './app.component';
import { ErrorComponent } from "./error/error.component";
import { TopnavComponent } from './components/topnav/topnav.component';
import { JobService } from './services/job.service';
import { QueuesService } from './services/queues.service';

@NgModule({
    declarations: [
        AppComponent,
        ErrorComponent,
        TopnavComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        RouterModule.forRoot(ROUTES, {
            useHash: true,
            preloadingStrategy: PreloadAllModules
        })
    ],
    providers: [JobService, QueuesService]
})
export class AppModuleShared {
}

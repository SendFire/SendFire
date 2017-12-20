import { Component } from '@angular/core';
import { JobService } from '../../services/job.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent {
    constructor(private _jobService: JobService){
    }
    command:string = '';
    jobId:string = '';
    counts: any[] = [];
    dashboardInterval:any = null;
    result:string = '';
    showTerminal: boolean = false;
    ngOnInit() {
        this.updateDashboardCounts()
        this.dashboardInterval = window.setInterval(() => {
            this.updateDashboardCounts();
        }, 2000); 
    }

    updateDashboardCounts() {
        return this._jobService.getDashboardCounts().subscribe(data => {
            this.counts = data;
        });
    }

    ngOnDestroy() {
        window.clearInterval(this.dashboardInterval);
    }

    enqueueCommand() {
        this.result = '';
        this._jobService.runCommand(this.command).subscribe(data => {
            this.command = '';
            if (!data && !data.id) {
                return;
            }  

            this.jobId = data.id;
            this.fetchResult();
                    
        });
    }

    fetchResult() {
        this.showTerminal = true;
    }

    
    
}

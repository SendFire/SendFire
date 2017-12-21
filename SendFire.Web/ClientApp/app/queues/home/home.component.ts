import { Component } from '@angular/core';
import { JobService } from '../../services/job.service';
import { QueuesService } from '../../services/queues.service';

@Component({
    selector: 'home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.scss']
})
export class HomeComponent {
    constructor(private _jobService: JobService, private _queuesService: QueuesService){
    }
    command:string = '';
    queue:string = 'default';
    jobId:string = '';
    counts: any[] = [];
    dashboardInterval:any = null;
    result:string = '';
    showTerminal: boolean = false;
    queues: any[] = [];
    ngOnInit() {
        this.updateDashboardCounts()
        this.dashboardInterval = window.setInterval(() => {
            this.updateDashboardCounts();
        }, 2000); 

        this._queuesService.getQueues().subscribe(data => {
            this.queues = data;
        });
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
        if(!this.command) {
            return;
        }
        this._jobService.runCommand(this.queue, this.command).subscribe(data => {
            if (!data && !data.id) {
                return;
            }  

            this.jobId = data.id;
            this.fetchResult();
                    
        });
    }

    clear() {
        this.command = '';
        this.jobId = '0';
    }

    fetchResult() {
        this.showTerminal = true;
    }

    
    
}

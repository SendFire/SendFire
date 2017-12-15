import { Component } from '@angular/core';
import { QueuesService } from '../../services/queues.service';

@Component({
    selector: 'queues-menu',
    templateUrl: './queues-menu.component.html',
    styleUrls: ['./queues-menu.component.css']
})
export class QueuesMenuComponent {

    constructor(private _queuesService: QueuesService){
    }
    queues = [];

    ngOnInit() {
        this._queuesService.getQueues().subscribe(data => {
            this.queues = data.map((q: any) => { q.heading = `${q.name} (${q.jobCount})`; return q; });
        });
    }
}

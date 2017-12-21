import { Component } from '@angular/core';
import { QueuesService } from '../../services/queues.service';
import { Router } from '@angular/router';

@Component({
    selector: 'queues-menu',
    templateUrl: './queues-menu.component.html',
    styleUrls: ['./queues-menu.component.scss']
})
export class QueuesMenuComponent {

    constructor(private _queuesService: QueuesService, private _router: Router){
    }
    queues = [];

    ngOnInit() {
        this._queuesService.getQueues().subscribe(data => {
            this.queues = data.map((q: any) => { q.heading = `${q.name} (${q.jobCount})`; return q; });
        });
    }

    goToDetails(event: any, id: string) {
        event.preventDefault();
        this._router.navigate([`/queues/details`, id]);
    }
}

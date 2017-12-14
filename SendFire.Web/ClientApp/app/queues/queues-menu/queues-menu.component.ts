import { Component } from '@angular/core';

@Component({
    selector: 'queues-menu',
    templateUrl: './queues-menu.component.html',
    styleUrls: ['./queues-menu.component.css']
})
export class QueuesMenuComponent {
    queues = [
        {
            name: 'Server 1',
            list: ['queue 1', 'queue 2']
        },
        {
            name: 'Server 2',
            list: ['queue 1', 'queue 2']
        },
        {
            name: 'Server 3',
            list: ['queue 1', 'queue 2']
        }
    ];
}

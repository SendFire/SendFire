import { Component } from '@angular/core';

@Component({
    selector: 'app',
    template: `<sf-topnav></sf-topnav>
                <router-outlet></router-outlet>`
})
export class AppComponent {
}

import { Component, OnInit } from '@angular/core';
import { JobService } from '../../services/job.service';
import { ActivatedRoute, ParamMap, Params } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';

@Component({
  selector: 'sf-details',
  templateUrl: './details.component.html',
  styleUrls: ['./details.component.scss']
})
export class DetailsComponent {

  constructor(
    private service: JobService,
    private route: ActivatedRoute
  ) {}
  details: any;
  private id: string;
  ngOnInit() {
    console.log("hi")
    this.route.params
      .subscribe((params: Params) => {
        this.service.getDetails(params['id']).subscribe(details => this.details = details);
      });
  }

}

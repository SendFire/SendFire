import { Injectable, Inject } from '@angular/core';
import { PlatformLocation } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';



@Injectable()
export class JobService {

  private _platformLocation: any;
  constructor(private _http: Http, private platformLocation: PlatformLocation) { 
    this._platformLocation = platformLocation as any;
  }

  getDashboardCounts():Observable<any> {
    return this._http.get(`${this._platformLocation.location.origin}/api/dashboard/counts`)
      .map((res:Response) => res.json())
  }

  runCommand(cmd: string):Observable<any> {
    return this._http.post(`${this._platformLocation.location.origin}/api/jobs/enqueue`, {command: cmd})
      .map((res:Response) => res.json())
  }

  getDetails(id: string) {
    return this._http.get(`${this._platformLocation.location.origin}/api/job/details/${id}`)
      .map((res:Response) => res.json())
  }

  getResults(id: string) {
    return this._http.get(`${this._platformLocation.location.origin}/api/job/results/${id}`)
      .map((res:Response) => res.json())
  }

}

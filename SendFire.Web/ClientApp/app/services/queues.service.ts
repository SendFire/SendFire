import { Injectable, Inject } from '@angular/core';
import { PlatformLocation } from '@angular/common';
import { Observable } from 'rxjs/Observable';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import 'rxjs/add/operator/map';

@Injectable()
export class QueuesService {

  private _platformLocation: any;
  constructor(private _http: Http, private platformLocation: PlatformLocation) { 
    this._platformLocation = platformLocation as any;
  }

  getQueues():Observable<any> {
    return this._http.get(`${this._platformLocation.location.origin}/api/queues/list`)
      .map((res:Response) => res.json())
  }
}

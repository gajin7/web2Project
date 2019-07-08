import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class BusMapsService {

  constructor(private http: HttpClient) { }

  GetAllStations() : Observable<any>
  {
   return this.http.get<any>('http://localhost:52295/api/BusStations/GetAllStations', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  GetAllLines() : Observable<any>
  {
   return this.http.get<any>('http://localhost:52295/api/BusStations/GetAllLines', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}

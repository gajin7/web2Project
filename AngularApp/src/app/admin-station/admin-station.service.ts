import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Station } from './station';

@Injectable({
  providedIn: 'root'
})
export class AdminStationService {

  constructor(private http: HttpClient) { }

  AddStation(st : Station) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/AddStation',st).pipe(
      catchError(this.handle)
    );
  }

  DeleteStation(station: any) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/DeleteStation',`id=`+station.Station + `&version=`+station.Version,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  GetStationInfo(id :string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetStationInfo', `id= ` + id, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
    catchError(this.handle)
    );
  }

  ChangeStation(id :string, station: Station, version:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/ChangeStation', `id= ` + id + `&name=`+station.name + `&address=`+station.address + `&longitude=`+station.longitude + `&latitude=`+station.latitude +`&version=` + version, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
    catchError(this.handle)
    );
  }

  
  GetStations() : Observable<any>
  {
   return this.http.get<any>('http://localhost:52295/api/Admin/GetStations').pipe(
      catchError(this.handle)
    );
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}

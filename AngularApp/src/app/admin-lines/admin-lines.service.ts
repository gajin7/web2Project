import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminLinesService {

  constructor(private http: HttpClient) { }

  GetStations() : Observable<any>
  {
   return this.http.get<any>('http://localhost:52295/api/Admin/GetStations').pipe(
      catchError(this.handle)
    );
  }

  AddLine(name: string, stations: string, type: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/AddLine',`name=`+name + `&stations=`+stations + `&type=`+type,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  GetLines() : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetLines', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  RemoveLine(line:any) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/DeleteLine',`id=`+line.Line + `&version=`+line.Version, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  ShowLine(id:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetLineInfo',`id=`+id, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  AddStation(line:any, station:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/AddStationToLine',`id=`+line.Line +`&version=` + line.Version + `&station=`+station, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    ); 
  }

  DeleteStation(line:any, station:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/DeleteStationToLine',`id=`+line.Line +`&version=` + line.Version +  `&station=`+station, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}

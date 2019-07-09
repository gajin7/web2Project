import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminScheduleService {

  constructor(private http: HttpClient) { }

  GetLines() : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetLines', { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  GetDepatures(type: string, line:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetDepatures',`type=`+type + `&line=`+line, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  

  AddDepature(type: string, line:string, depature:string, version:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/AddDepature',`type=`+type + `&line=`+line + `&depature=`+depature + `&version=`+version, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  RemoveDepature(type: string, line:string, depature:string, version:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/RemoveDepature',`type=`+type + `&line=`+line + `&depature=`+depature + `&version=`+version, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }

  private handle(error: any) {
    return of (error.error.Message);
}
}

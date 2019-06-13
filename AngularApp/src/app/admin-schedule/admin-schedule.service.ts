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
      catchError(this.handleError<any>('GetLines'))
    );
  }

  GetDepatures(type: string, line:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetDepatures',`type=`+type + `&line=`+line, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetLines'))
    );
  }

  ChangeSchedule(type: string, line:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Admin/GetLines',`type=`+type + `&line=`+line, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetLines'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}

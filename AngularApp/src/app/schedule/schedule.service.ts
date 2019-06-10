import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  constructor(private http: HttpClient) { }

  GetLines(type: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Schedule/GetLines', `type=`+type, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetLines'))
    );
  }

  GetSchedule(day: string,line : string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Schedule/GetSchedule', `day=`+day + `&line=`+line, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetSchedule'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}

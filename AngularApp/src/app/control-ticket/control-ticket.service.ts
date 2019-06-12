import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ControlTicketService {

  constructor(private http: HttpClient) { }

  public ControlTicket(Id : string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/CheckIn/ControlTicket', `Id=` + Id,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('CheckInTcket'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}

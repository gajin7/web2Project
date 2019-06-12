import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CheckInTicketService {

  constructor(private http: HttpClient) { }

  public CheckInTcket(Id : string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/CheckIn/CheckTicket', `Id=` + Id,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('CheckInTcket'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}

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

  
  public GetUserToApprove() : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/CheckIn/GetUsersToApprove',{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetUsersToApprove'))
    );
  }

  public AproveUser(email: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/CheckIn/ApproveUser',`email=`+email,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetUsersToApprove'))
    );
  }

  public DeclineUser(email: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/CheckIn/DeclineUser',`email=`+email,{ 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetUsersToApprove'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}

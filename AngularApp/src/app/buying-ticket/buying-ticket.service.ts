import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class BuyingTicketService {




  constructor(private http: HttpClient) { }

  BuyTimeTicket(mail: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Ticket/ByTimeTicket', `email=`+mail, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('BuyTimeTicket'))
    );
  }

  GetTicketPrice(type :string, lineType:string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Ticket/GetTicketPrice', `type= ` + type + `&linetype=`+lineType, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handleError<any>('GetTicketPrice'))
    );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }
}



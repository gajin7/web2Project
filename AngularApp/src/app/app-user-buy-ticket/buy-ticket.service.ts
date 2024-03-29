import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Validators } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class BuyTicketService {

  constructor(private http: HttpClient) { }

  BuyTicket(type: string,data : any) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Ticket/BuyTicket', `type=`+type +  `&id=` + data.id +  `&status=` + data.status + `&payer_email=` + data.payer.email_address  + `&payer_id=` + data.payer.payer_id  + `&create_time=` + data.create_time + `&update_time=` + data.update_time, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
    );
  }

  GetTicketForSale(type: string) : Observable<any>
  {
   return this.http.post<any>('http://localhost:52295/api/Ticket/SetTicketForSale', `type=`+type, { 'headers': { 'Content-type': 'application/x-www-form-urlencoded' } }).pipe(
      catchError(this.handle)
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

  private handle(error: any) {
    return of (error.error.Message);
}
}
